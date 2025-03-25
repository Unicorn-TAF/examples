using Demo.Commons;
using Demo.Tests.Handlers;
using System;
using Unicorn.Reporting.Allure;
using Unicorn.Reporting.ReportPortal;
using Unicorn.Reporting.TestIt;
using Unicorn.Taf.Api;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Core;
using Unicorn.UI.Win;

namespace Demo.Tests
{
    /// <summary>
    /// Actions performed before and/or after all tests execution.
    /// </summary>
    [TestAssembly]
    public class TestsAssembly
    {
        private static ITestReporter reporter;
        private static GitHubBts bts;
        private static ScreenshotTakerBase screenshotter;

        /// <summary>
        /// Actions before all tests execution.
        /// The method should be static.
        /// </summary>
        [RunInitialize]
        public static void InitRun()
        {
            // Use of custom logger instead of default Console logger.
            ULog.SetLogger(new CustomLogger());

            // Set trace logging level.
            ULog.SetLevel(LogLevel.Trace);

            // It's possible to customize TAF configuration in assembly init. 
            // Current setting controls behavior of dependent tests in case when referenced test is failed
            // (tests could be failed, skipped or not run)
            Config.DependentTests = TestsDependency.Skip;

            // Initialize built-in screenshotter with automatic subscription to test fail event.
            // By default the screenshotter will be initialized in ./Screenshots directory in test binaries dir.
            screenshotter = new WinScreenshotTaker();
            screenshotter.SubscribeToTafEvents();

            bts = new GitHubBts();

            // It's possible to report to several supported reporting systems
            Type reporterType = typeof(AllureReporter);

            switch (reporterType.Name)
            {
                case "AllureReporter":
                    // Initialize built-in Allure reporter with automatic subscription to all testing events.
                    // allureConfig.json should exist in binaries directory.
                    reporter = new AllureReporter();
                    break;
                case "ReportPortalReporter":
                    // Initialize built-in Report Portal reporter with automatic subscription to all testing events.
                    // ReportPortal.config.json should exist in binaries directory.
                    reporter = new ReportPortalReporter();
                    break;
                case "TestItReporter":
                    // Initialize built-in TestIT reporter with automatic subscription to all testing events.
                    // Tms.config.json should exist in binaries directory.
                    reporter = new TestItReporter();
                    break;
            }
        }

        /// <summary>
        /// Actions after all tests execution.
        /// The method should be static.
        /// </summary>
        [RunFinalize]
        public static void FinalizeRun()
        {
            // Unsubscribe reporter from unicorn events.
            reporter?.Dispose();
            reporter = null;

            // Unsubscribe screenshotter from unicorn events.
            screenshotter.UnsubscribeFromTafEvents();
            screenshotter = null;

            bts.Dispose();
            bts = null;
        }
    }
}
