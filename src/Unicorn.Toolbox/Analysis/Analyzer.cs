﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Adapter;

namespace Unicorn.Toolbox.Analysis
{
    public class Analyzer
    {
        private readonly string assemblyDirectory;
        private readonly Type baseSuiteType = typeof(TestSuite);

        public Analyzer(Assembly assembly, string fileName)
        {
            this.assemblyDirectory = Path.GetDirectoryName(fileName);
            this.AssemblyFile = Path.GetFileName(fileName);
            this.AssemblyName = assembly.FullName;

            this.Data = new AutomationData();
        }

        public AutomationData Data { get; protected set; }

        public string AssemblyFile { get; protected set; }

        public string AssemblyName { get; protected set; }

        public void GetTestsStatistics()
        {
            LoadDependenciesToCurrentAppDomain();

            var testsAssembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName.Equals(AssemblyName));
            var allSuites = TestsObserver.ObserveTestSuites(testsAssembly);

            foreach (var suiteType in allSuites)
            {
                if (AdapterUtilities.IsSuiteParameterized(suiteType))
                {
                    foreach (var parametersSet in AdapterUtilities.GetSuiteData(suiteType))
                    {
                        var parameterizedSuite = testsAssembly.CreateInstance(suiteType.FullName, true, BindingFlags.Default, null, parametersSet.Parameters.ToArray(), null, null);
                        ((TestSuite)parameterizedSuite).Name += $" [{parametersSet.Name}]";
                        this.Data.AddSuiteData(GetSuiteInfo(parameterizedSuite));
                    }
                }
                else
                {
                    var nonParameterizedSuite = testsAssembly.CreateInstance(suiteType.FullName);
                    this.Data.AddSuiteData(GetSuiteInfo(nonParameterizedSuite));
                }
            }
        }

        private void LoadDependenciesToCurrentAppDomain()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(assemblyDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(System.Reflection.AssemblyName.GetAssemblyName(path))));
        }

        private SuiteInfo GetSuiteInfo(object suiteInstance)
        {
            int inheritanceCounter = 0;
            var currentType = suiteInstance.GetType();

            while (!currentType.Equals(baseSuiteType) && inheritanceCounter++ < 50)
            {
                currentType = currentType.BaseType;
            }

            var testSuite = suiteInstance as TestSuite;
            var suiteInfo = new SuiteInfo(testSuite.Name, testSuite.Features, testSuite.Metadata);

            var fieldInfo = currentType.GetField("tests", BindingFlags.NonPublic | BindingFlags.Instance);
            var tests = fieldInfo.GetValue(suiteInstance) as Test[];

            foreach (var test in tests)
            {
                suiteInfo.TestsInfos.Add(GetTestInfo(test));
            }

            return suiteInfo;
        }

        private TestInfo GetTestInfo(Test test)
        {
            var testInfo = new TestInfo(test.Description, test.Author, test.Categories);
            return testInfo;
        }
    }
}