using Demo.WebModule;
using Demo.WebModule.Ui;
using Demo.Tests.Base;
using Demo.Tests.Metadata;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Web;

namespace Demo.Tests.Web
{
    /// <summary>
    /// Example of parameterized test suite (the whole suite is run for all entries in data set), 
    /// the suite should be marked with <see cref="ParameterizedAttribute"/>.
    /// The class should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>.
    /// </summary>
    [Parameterized]
    [Suite("Test website default state tests")]
    [Tag(Features.Web), Tag(Features.HelloWorld), Tag(Features.Samples)]
    [Metadata("Description", 
        "Example of parameterized test suite with ordered tests. Suite checks default state of controls of the test website")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class TestWebsiteDefaultsSuite : BaseTestSuite
    {
        private readonly BrowserType _browser;
        private TestWebsite website;

        private HelloWorldPage HelloWorld => website.GetPage<HelloWorldPage>();

        private SamplesPage Samples => website.GetPage<SamplesPage>();

        /// <summary>
        /// Constructor for parameterized suite. It should contain the same number of parameters as suite DataSet.
        /// </summary>
        /// <param name="browser">browser type to run suite on (corresponds to same parameter of suite DataSet)</param>
        public TestWebsiteDefaultsSuite(BrowserType browser)
        {
            _browser = browser;
        }

        /// <summary>
        /// Data for parameterized suite. First parameter is <see cref="DataSet"/> name 
        /// and is not considered in parameterization.
        /// Data for parameterized suite should have <see cref="SuiteDataAttribute"/> and the method should be static.
        /// </summary>
        /// <returns></returns>
        [SuiteData]
        public static List<DataSet> GetBrowsers() =>
            new List<DataSet>
            {
                new DataSet("Google Chrome", BrowserType.Chrome),
                //new DataSet("Edge", BrowserType.Edge),
            };

        /// <summary>
        /// Actions executed before whole tests in current suite.
        /// </summary>
        [BeforeSuite]
        public void ClassInit() =>
            website = Do.Website.Open(_browser, Config.Instance.WebsiteUrl);

        /// <summary>
        /// Example of simple test with specified category.
        /// It's possible to specify tests execution order within a test suite using <see cref="OrderAttribute"/>.
        /// Tests with higher order will be executed later.
        /// </summary>
        [Author(Authors.JDoe)]
        [Category(Categories.Smoke)]
        [Test("Check hello world page elements")]
        [Order(0)]
        public void TestHelloWorldElements() =>
            Do.Assertion.StartAssertionsChain()
                .VerifyThat(HelloWorld.Opened, Is.EqualTo(true), "Check Hello World page opened")
                .VerifyThat(HelloWorld.MainTitle, UI.Control.HasText("Sample \"Hello World\" app"))
                .VerifyThat(HelloWorld.TitleDropdown, UI.Dropdown.HasSelectedValue("Nothing selected"))
                .VerifyThat(HelloWorld.NameInput, UI.TextInput.HasValue(string.Empty))
                .AssertChain();

        [Author(Authors.JDoe)]
        [Test("Check samples page elements")]
        [Order(1)]
        public void TestSamplesElements()
        {
            Do.Website.SwitchApp();
            Do.Assertion.StartAssertionsChain()
                .VerifyThat(Samples.EmailInput, UI.TextInput.HasValue(string.Empty))
                .VerifyThat(Samples.PasswordInput, UI.TextInput.HasValue(string.Empty))
                .AssertChain();
        }

        /// <summary>
        /// Actions executed after each test.
        /// </summary>
        [AfterSuite]
        public void ClassTearDown() =>
            Do.Website.CloseBrowser();
    }
}
