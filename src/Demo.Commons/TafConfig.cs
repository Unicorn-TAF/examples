using System;
using System.IO;

namespace Demo.Commons
{
    public class TafConfig
    {
        private static TafConfig instance = null;

        private TafConfig() { }

        public static TafConfig Get => instance ?? (instance = new TafConfig());

        public string TestsDir { get; } = Path.GetDirectoryName(new Uri(typeof(TafConfig).Assembly.Location).LocalPath);

        public string BtsIssueUrl { get; } = "https://github.com/Unicorn-TAF/examples/issues/";

        public string WebsiteUrl { get; } = "https://unicorn-taf.github.io";

        public string TestUiAppsUrl => WebsiteUrl + "/test-ui-apps.html";

        public string DesktopAppName { get; } = "TestWindowsApp.exe";
    }
}
