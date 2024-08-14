using Demo.StepsInjection;
using Demo.WebModule.Ui;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.UI.Web;

namespace Demo.WebModule.Steps
{
    /// <summary>
    /// Represents high-level steps for website.
    /// To make steps be able to use events subscriptions it's necessary to add <see cref="StepsClassAttribute"/>.
    /// </summary>
    [StepsClass]
    public class TestWebsiteSteps
    {
        private TestWebsite website;
        private HelloWorldSteps _helloWorld;
        private SamplesSteps _samples;

        /// <summary>
        /// Gets web page from website cache.
        /// If page was already initializaed and called before the same instance is used further.
        /// </summary>
        private HelloWorldPage Home => website.GetPage<HelloWorldPage>();

        /// <summary>
        /// It makes sense to init child steps only when the are called. It's better to take care of resources :)
        /// </summary>
        public HelloWorldSteps HelloWorld => _helloWorld ?? 
            (_helloWorld = new HelloWorldSteps(website.GetPage<HelloWorldPage>()));

        public SamplesSteps Samples => _samples ??
            (_samples = new SamplesSteps(website.GetPage<SamplesPage>()));

        [Step("Open Test website in {0} browser")]
        public TestWebsite Open(BrowserType browser, string siteUrl)
        {
            /*
             * Example of how to attach WebDriver to existing opened browser (browser should be started in debug mode)
             */
            //var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            //options.DebuggerAddress = "127.0.0.1:9222"; //real debugging port should be specified
            //var driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            //var webDriver = new Unicorn.UI.Web.Driver.DesktopWebDriver(driver);
            //website = new TestWebsite(webDriver, siteUrl);

            website = new TestWebsite(browser, siteUrl);

            website.Open();
            Home.WaitForLoading();

            return website;
        }

        [Step("Switch app")]
        public void SwitchApp() =>
            Home.SwitchAppButton.JsClick();

        /// <summary>
        /// Example of step with description (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// </summary>
        [Step("Close Browser")]
        public void CloseBrowser() =>
            website.Driver.Close();
    }
}
