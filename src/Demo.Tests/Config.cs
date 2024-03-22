using System;
using System.IO;

namespace Demo.Tests
{
    public class Config
    {
        private static Config instance = null;

        private Config()
        {
            
        }

        public static Config Instance => instance ?? (instance = new Config());

        public string TestsDir { get; } =  Path.GetDirectoryName(new Uri(typeof(Config).Assembly.Location).LocalPath);

        public string WebsiteUrl { get; } = "https://unicorn-taf.github.io/test-ui-apps.html";
    }
}
