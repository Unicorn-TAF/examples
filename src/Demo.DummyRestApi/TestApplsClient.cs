using Unicorn.Backend.Services.RestService;

namespace Demo.DummyRestApi
{
    /// <summary>
    /// Implementation of some dummy api client (should inherit <see cref="RestClient"/>)
    /// </summary>
    public class TestApplsClient : RestClient
    {
        private const string ExeUrl = "/assets/TestWindowsApp.exe";

        /// <summary>
        /// Initializing instance of <see cref="DummyApiClient"/> calling base constructor with api base url.
        /// </summary>
        public TestApplsClient() : base("https://unicorn-taf.github.io")
        {
        }

        public void DownloadExe() =>
            DownloadFile(ExeUrl, "");
    }
}
