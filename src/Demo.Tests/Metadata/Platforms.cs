namespace Demo.Tests.Metadata
{
    internal static class Platforms
    {
        private const string Prefix = "platform:";

        public const string Api = Prefix + "api";
        public const string Web = Prefix + "web";
        public const string Win = Prefix + "win";
        public const string Android = Prefix + "android";

        internal static class Apps
        {
            private const string Prefix = "app:";

            public const string DummyApi = Prefix + "dummy-api";

            public const string HelloWorld = Prefix + "hello-world";
            public const string Samples = Prefix + "samples";

            public const string Charmap = Prefix + "charmap";

            public const string Dialer = Prefix + "dialer";
        }
    }
}
