using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;
using static Unicorn.Taf.Core.Testing.Test;

namespace Unicorn.UnitTests.Core.Testing
{
    [TestFixture]
    public class ParallelizationTests : NUnitTestRunner
    {
        private static TestsRunner runner;
        private static List<Test> excutedTestsOrder = new List<Test>();

        [OneTimeSetUp]
        public static void SetConfig()
        {
            Config.TestsExecutionOrder = TestsOrder.Random;
            Config.SetSuiteTags("parallelizaton");
            Config.ParallelBy = Parallelization.Suite;
            Config.Threads = 3;

            Test.OnTestFinish += OnTestFinish;

            var runFilter = new Dictionary<string, string>
            {
                { "Suite that tests parallelization by suite 1", "" },
                { "Suite that tests parallelization by suite 2", "" },
            };

            runner = new PlaylistRunner(Assembly.GetExecutingAssembly(), runFilter);
        }

        public static void OnTestFinish(Test test)
        {
            excutedTestsOrder.Add(test);
        }

        [OneTimeTearDown]
        public static void ResetConfig()
        {
            Config.Reset();
        }

        [Author("Evgeniy Voronyuk")]
        [Test(Description = "Check Parallelization by suites")]
        public void TestStepEvents()
        {
            var outcome = runner.RunTests();

            // TODO: make tests end in this order:
            // suite 1 - test 1
            // suite 2 - test 2
            // suite 2 - tset 3
            // suite 2 - test 1
            // suite 1 - test 3
            // suite 1 - test 2
            // basically they are sorted by duration (descending)

            // TODO: create asserts for desired list
            Assert.IsTrue(excutedTestsOrder.Count == 6);
        }
    }
}
