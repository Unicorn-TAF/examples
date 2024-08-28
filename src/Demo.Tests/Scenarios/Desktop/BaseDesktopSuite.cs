using Demo.Charmap;
using Demo.DummyRestApi;
using Demo.Tests.Base;
using System.IO;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Demo.Tests.Scenarios.Desktop
{
    /// <summary>
    /// Web test suite example. The class should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>.
    /// <br/>
    /// It's possible to specify any number of suite tags and metadata.
    /// Suite tags allow to use parameterized targeted runs: suites are selected based on specific tags presence.
    /// </summary>
    public abstract class BaseDesktopSuite : BaseTestSuite
    {
        protected TestWindowsApp application;

        /// <summary>
        /// Actions executed before all tests in current suite.
        /// </summary>
        [BeforeSuite]
        public virtual void ClassInit()
        {
            DownloadAppIfAbsent();
            application = Do.DesktopApp.StartApplication();
        }

        /// <summary>
        /// Actions executed after all tests in current suite.
        /// </summary>
        [AfterSuite]
        public virtual void ClassTearDown() =>
            Do.DesktopApp.CloseApplication();

        private void DownloadAppIfAbsent()
        {
            if (!File.Exists(TestWindowsApp.AppExeName))
            {
                new TestApplsClient().DownloadExe();
            }
        }
    }
}
