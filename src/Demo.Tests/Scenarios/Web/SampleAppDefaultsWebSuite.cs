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
using Demo.Tests.BO;
using Demo.Tests.TestData;

namespace Demo.Tests.Scenarios.Web
{
    /// <summary>
    /// Example of parameterized test suite (the whole suite is run for all entries in data set), 
    /// the suite should be marked with <see cref="ParameterizedAttribute"/>.
    /// The class should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>.
    /// </summary>
    [Parameterized]
    [Suite("Sample app defaults")]
    [Tag(Platforms.Web), Tag(Apps.Samples)]
    [Metadata("Description",
        "Example of parameterized test suite with ordered tests. Suite checks default state of sample app")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class SampleAppDefaultsWebSuite : BaseTestSuite
    {
        private readonly BrowserType _browser;
        private TestWebsite website;

        private SamplesPage Samples => website.GetPage<SamplesPage>();

        /// <summary>
        /// Constructor for parameterized suite. It should contain the same number of parameters as suite DataSet.
        /// </summary>
        /// <param name="browser">browser type to run suite on (corresponds to same parameter of suite DataSet)</param>
        public SampleAppDefaultsWebSuite(BrowserType browser)
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

        [Author(Authors.JDoe)]
        [Test("Sample app authentication default layout")]
        [Order(1)]
        public void TestSamplesAuthDefaultLayout()
        {
            Do.Website.SwitchApp();
            Do.Assertion.StartAssertionsChain()
                .VerifyThat(Samples.Opened, Is.EqualTo(true), "Check Hello World page opened")
                .VerifyThat(Samples.EmailInput, UI.TextInput.HasValue(string.Empty))
                .VerifyThat(Samples.PasswordInput, UI.TextInput.HasValue(string.Empty))
                .AssertChain();
        }

        [Author(Authors.JDoe)]
        [Test("Sample app page default layout")]
        [Order(2)]
        public void TestSamplesDefaultLayout()
        {
            User user = UsersFactory.GetDefaultUser();

            // Precondition to open main page

            Do.Website.Samples.InputEmail(user.Email);
            Do.Website.Samples.InputPassword(user.Password);
            Do.Website.Samples.SignIn();

            Do.Assertion.StartAssertionsChain()
                .VerifyThat(Samples.WelcomeTitle, UI.Control.HasText("Welcome!"))
                .VerifyThat(Samples.ConsoleRunnerCheckbox, UI.Checkbox.Checked())
                .VerifyThat(Samples.TestAdapterCheckbox, UI.Checkbox.HasCheckState(false))
                .VerifyThat(Samples.ReportPortalRadio, Is.Not(UI.Control.Selected()))
                .VerifyThat(Samples.AllureReportlRadio, UI.Control.Selected())
                .VerifyThat(Samples.RuntimesDropdown, UI.Dropdown.HasSelectedValue(ConfigData.Runtimes.Net))
                .VerifyThat(Samples.ShowConfigButton, UI.Control.Visible())
                .VerifyThat(Samples.TabsControl.ActiveTab, UI.Control.HasText("Unit Tests"))
                .VerifyThat(Samples.Accordion.ActivePanel.PanelTitle, UI.Control.HasText("Windows GUI"))
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
