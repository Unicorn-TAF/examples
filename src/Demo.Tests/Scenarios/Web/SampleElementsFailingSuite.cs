using Demo.Tests.Base;
using Demo.Tests.Metadata;
using Demo.WebModule;
using Demo.WebModule.Ui;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Web;

namespace Demo.Tests.Scenarios.Web
{
    [Suite("Sample elements failing suite")]
    [Tag(Platforms.Web), Tag(Platforms.Apps.Samples)]
    [Metadata("Description",
        "Example of test suite with dependent tests where main test is failing")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class SampleElementsFailingSuite : BaseTestSuite
    {
        private TestWebsite website;

        private SamplesPage Samples => website.GetPage<SamplesPage>();

        [BeforeSuite]
        public void ClassInit()
        {
            website = Do.Website.Open(BrowserType.Chrome, Config.Instance.WebsiteUrl);
            Do.Website.SwitchApp();
        }

        /// <summary>
        /// Example of test predecessor for other tests (which fails and should affect dependent tests execution).
        /// The test is marked with <see cref="BugAttribute"/>. Besides mandatory ID it's possible to specify also
        /// some specific conditions when one wants to consider the bug (typer of exception or part of error message)
        /// </summary>
        [Author(Authors.JDoe)]
        [Bug("1", typeof(AssertionException), "But: was not visible")]
        [Test("Test which always fails")]
        public void FailingTest()
        {
            Do.Website.Samples.InputEmail("incorrect email");
            Do.Website.Samples.InputPassword("some password");
            Do.Website.Samples.SignIn();
            Do.Assertion.AssertThat(Samples.Accordion, UI.Control.Visible());
        }

        /// <summary>
        /// Example of test with dependency on another test, it could be done via <see cref="DependsOnAttribute"/>.
        /// It's possible to configure behavior of dependent test in case when referenced test failed 
        /// (fail, skip, do not run). See <see cref="TestsAssembly.InitRun"/>.
        /// </summary>
        [Author(Authors.JDoe)]
        [DependsOn(nameof(FailingTest))]
        [Test("Accordion control default selection")]
        public void AccordionDependentTest() =>
            Do.Assertion.AssertThat(Samples.Accordion.SelectedValue, Is.EqualTo("Windows GUI"));

        /// <summary>
        /// Example of test with dependency on another test, it could be done via <see cref="DependsOnAttribute"/>.
        /// It's possible to configure behavior of dependent test in case when referenced test failed 
        /// (fail, skip, do not run). See <see cref="TestsAssembly.InitRun"/>.
        /// </summary>
        [Author(Authors.JDoe)]
        [DependsOn(nameof(FailingTest))]
        [Test("Tabs control control default selection")]
        public void TabsControlDependentTest() =>
            Do.Assertion.AssertThat(Samples.TabsControl.SelectedValue, Is.EqualTo("Unit Tests"));

        /// <summary>
        /// Actions executed after all tests in current suite.
        /// </summary>
        [AfterSuite]
        public void ClassTearDown() =>
            Do.Website.CloseBrowser();
    }
}
