using Demo.Commons;
using Demo.DesktopModule;
using Demo.Tests.Base;
using Demo.WebModule.Api;
using System.IO;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Demo.Tests.Scenarios.Desktop
{
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
            if (!File.Exists(TafConfig.Get.DesktopAppName))
            {
                new TestApplsClient().DownloadDesktopApplication();
            }
        }
    }
}
