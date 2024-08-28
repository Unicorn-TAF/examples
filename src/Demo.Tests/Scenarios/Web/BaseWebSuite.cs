using Demo.Tests.Base;
using Demo.WebModule;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Web;

namespace Demo.Tests.Scenarios.Web
{
    /// <summary>
    /// Web test suite example. The class should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>.
    /// <br/>
    /// It's possible to specify any number of suite tags and metadata.
    /// Suite tags allow to use parameterized targeted runs: suites are selected based on specific tags presence.
    /// </summary>
    public abstract class BaseWebSuite : BaseTestSuite
    {
        protected TestWebsite website;

        /// <summary>
        /// Actions executed before all tests in current suite.
        /// </summary>
        [BeforeSuite]
        public void ClassInit() =>
            website = Do.Website.Open(BrowserType.Chrome, Config.Instance.WebsiteUrl);

        /// <summary>
        /// Actions executed after all tests in current suite.
        /// </summary>
        [AfterSuite]
        public void ClassTearDown() =>
            Do.Website.CloseBrowser();
    }
}
