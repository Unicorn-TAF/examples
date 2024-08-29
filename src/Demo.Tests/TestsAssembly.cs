using Demo.Commons;
using Demo.Tests.Handlers;
using System.Drawing.Imaging;
using System.IO;
using Unicorn.AllureAgent;
using Unicorn.ReportPortalAgent;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Win;

namespace Demo.Tests
{
    /// <summary>
    /// Actions performed before and/or after all tests execution.
    /// </summary>
    [TestAssembly]
    public class TestsAssembly
    {
        private static AllureReporterInstance reporter;
        //private static ReportPortalReporterInstance reporter;
        private static GitHubBts bts;
        private static WinScreenshotTaker screenshotter;

        /// <summary>
        /// Actions before all tests execution.
        /// The method should be static.
        /// </summary>
        [RunInitialize]
        public static void InitRun()
        {
            // Use of custom logger instead of default Console logger.
            Logger.Instance = new CustomLogger();

            // Set trace logging level.
            Logger.Level = LogLevel.Trace;

            // It's possible to customize TAF configuration in assembly init. 
            // Current setting controls behavior of dependent tests in case when referenced test is failed
            // (tests could be failed, skipped or not run)
            Unicorn.Taf.Core.Config.DependentTests = Unicorn.Taf.Core.TestsDependency.Skip;

#if NETFRAMEWORK
            // Initialize built-in screenshotter with automatic subscription to test fail event.
            var screenshotsDir = Path.Combine(TafConfig.Get.TestsDir, "Screenshots");
            screenshotter = new WinScreenshotTaker(screenshotsDir, ImageFormat.Png);
            screenshotter.ScribeToTafEvents();
#endif

            bts = new GitHubBts();

            // Initialize built-in allure reporter with automatic subscription to all testing events.
            // allureConfig.json should exist in binaries directory.
            reporter = new AllureReporterInstance();

            // Initialize built-in report portal reporter with automatic subscription to all testing events.
            // ReportPortal.config.json should exist in binaries directory.
            //reporter = new ReportPortalReporterInstance();
        }

        /// <summary>
        /// Actions after all tests execution.
        /// The method should be static.
        /// </summary>
        [RunFinalize]
        public static void FinalizeRun()
        {
            // Unsubscribe reporter from unicorn events.
            reporter.Dispose();
            reporter = null;

#if NETFRAMEWORK
            // unsubscribing screenshotter from unicorn events.
            screenshotter.UnsubscribeFromTafEvents();
            screenshotter = null;
#endif

            bts.Dispose();
            bts = null;
        }
    }
}
