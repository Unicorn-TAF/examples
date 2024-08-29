using Demo.Commons;
using Unicorn.Backend.Services.RestService;

namespace Demo.WebModule.Api
{
    /// <summary>
    /// Implementation of api client (should inherit <see cref="RestClient"/>)
    /// The client is used to download desktop application from test website.
    /// </summary>
    public class TestApplsClient : RestClient
    {
        private readonly string _desktopAppUrl = "/assets/" + TafConfig.Get.DesktopAppName;

        /// <summary>
        /// Initializing instance of <see cref="TestApplsClient"/> calling base constructor with api base url.
        /// </summary>
        public TestApplsClient() : base(TafConfig.Get.WebsiteUrl)
        {
        }

        public void DownloadDesktopApplication() =>
            DownloadFile(_desktopAppUrl, "");
    }
}
