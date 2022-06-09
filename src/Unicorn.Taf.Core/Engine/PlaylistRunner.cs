using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicorn.Taf.Api;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides with ability to run tests in specified order and with specified per suite categories.
    /// It is parameterized by dictionary where key is suite name and value is category
    /// </summary>
    

    // TODO: think about cases where suite parallel runs have suites with dependent suites

    public class PlaylistRunner : TestsRunner
    {
        private const string DataSetDelimiter = "::";

        private readonly Assembly _testAssembly;
        private readonly Dictionary<string, string> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistRunner"/> class 
        /// for specified assembly and with specified filters.
        /// </summary>
        /// <param name="testAssembly">assembly with tests</param>
        /// <param name="filters">filters (key: suite name, value: tests categories to run within the suite)</param>
        /// <exception cref="FileNotFoundException">is thrown when tests assembly was not found</exception>
        public PlaylistRunner(Assembly testAssembly, Dictionary<string, string> filters)
        {
            _testAssembly = testAssembly ?? throw new ArgumentNullException(nameof(testAssembly));
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
            Outcome = new LaunchOutcome();
        }

        /// <summary>
        /// Run all observed tests matching selection criteria
        /// </summary>
        /// <exception cref="TypeLoadException">is thrown when suite class was not 
        /// found for specified suite name in run filters</exception>
        public override IOutcome RunTests()
        {
            var runnableSuites = CollectSuitesToRun(_testAssembly);

            if (!runnableSuites.Any())
            {
                return null;
            }

            Outcome.StartTime = DateTime.Now;

            // Execute run init action if exists in assembly.
            try
            {
                GetRunInitCleanupMethod(_testAssembly, typeof(RunInitializeAttribute))?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Error, "Run initialization failed:\n" + ex);
                Outcome.RunInitialized = false;
                Outcome.RunnerException = ex.InnerException;
            }

            if (Outcome.RunInitialized)
            {
                // Compute runnable suites
                var suiteToRun = new Dictionary<Type, string>();

                foreach (var suiteEntry in runnableSuites.Keys)
                {
                    string[] suiteAndDataSet = Regex.Split(suiteEntry, DataSetDelimiter);
                    string dataSet = suiteAndDataSet.Length == 2 ? suiteAndDataSet[1] : string.Empty;

                    // Commented for now.
                    // This action removes auto-population of config(and therefor report) with executed suites that are selected implicitly.
                    // To restore this functionality, need to figure something else. Config should be unidirectional.
                    //// Config.SetTestCategories(_filters[suiteEntry]);

                    suiteToRun.Add(runnableSuites[suiteEntry], dataSet);
                }

                // Execute in parallel runnable steps
                Parallel.ForEach(
                    suiteToRun,
                    new ParallelOptions { MaxDegreeOfParallelism = Config.Threads },
                    suiteAndDataSetPair => RunTestSuite(suiteAndDataSetPair.Key, suiteAndDataSetPair.Value));

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(_testAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }

            return Outcome;
        }

        private void RunTestSuite(Type type, string dataSet)
        {
            if (AdapterUtilities.IsSuiteParameterized(type))
            {
                var dataSetsToRun = string.IsNullOrEmpty(dataSet) ?
                    AdapterUtilities.GetSuiteData(type) :
                    AdapterUtilities.GetSuiteData(type).Where(ds => ds.Name.Equals(dataSet));

                foreach (var parametersSet in dataSetsToRun)
                {
                    var parameterizedSuite = Activator.CreateInstance(type, parametersSet.Parameters.ToArray()) as TestSuite;
                    parameterizedSuite.Metadata.Add("Data Set", parametersSet.Name);
                    parameterizedSuite.Outcome.DataSetName = parametersSet.Name;
                    ExecuteSuiteIteration(parameterizedSuite);
                }
            }
            else
            {
                var suite = Activator.CreateInstance(type) as TestSuite;
                ExecuteSuiteIteration(suite);
            }
        }

        private Dictionary<string, Type> CollectSuitesToRun(Assembly assembly)
        {
            var suitesToRun = new Dictionary<string, Type>();

            //As suite entry in filter can contain data set name need to extract pure suites names
            //to filter assembly types by them 
            var suiteNames = _filters.Keys.Select(k => GetSuiteNameFromFilter(k));

            var filteredSuites = TestsObserver.ObserveTestSuites(assembly)
                .Where(s => suiteNames.Contains(AdapterUtilities.GetSuiteName(s)));

            foreach (var filterSuiteName in _filters.Keys)
            {
                var suiteName = GetSuiteNameFromFilter(filterSuiteName);

                var suite = filteredSuites.FirstOrDefault(s =>
                    AdapterUtilities.GetSuiteName(s).Equals(suiteName, StringComparison.InvariantCultureIgnoreCase));

                if (suite == null)
                {
                    throw new TypeLoadException($"Suite with name '{suiteName}' was not found in tests assembly.");
                }

                if (suite.IsDefined(typeof(DisabledAttribute)))
                {
                    continue;
                }

                if (suite.GetRuntimeMethods().Any(t => AdapterUtilities.IsTestRunnable(t, _filters[filterSuiteName])))
                {
                    suitesToRun.Add(filterSuiteName, suite);
                }
            }

            return suitesToRun;
        }

        private static string GetSuiteNameFromFilter(string filterSuiteName) =>
            Regex.Split(filterSuiteName, DataSetDelimiter)[0];
    }
}
