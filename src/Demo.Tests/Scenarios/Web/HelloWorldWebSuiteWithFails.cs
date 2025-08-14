using Demo.Commons.BO;
using Demo.Tests.Metadata;
using Demo.Tests.TestData;
using Demo.WebModule.Gui;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Scenarios.Web
{
    /// <summary>
    /// Web test suite example. The class should have <see cref="SuiteAttribute"/>.
    /// <br/>
    /// It's possible to specify any number of suite tags and metadata.
    /// Suite tags allow to use parameterized targeted runs: suites are selected based on specific tags presence.
    /// </summary>
    [Suite("Hello World web app modal window")]
    [Tag(Platforms.Web), Tag(Apps.HelloWorld)]
    [Metadata("Description", "Example of test suite with dependent tests where main test is failing")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class HelloWorldWebSuiteWithFails : BaseWebSuite
    {
        private HelloWorldPage HelloWorld => website.GetPage<HelloWorldPage>();

        /// <summary>
        /// Example of test predecessor for other tests (which fails and should affect dependent tests execution).
        /// The test is marked with <see cref="BugAttribute"/>. Besides mandatory ID it's possible to specify also
        /// some specific conditions when one wants to consider the bug (exception Type or part of error message)
        /// </summary>
        [Author(Authors.JDoe)]
        [TestCaseId("1")]
        [Test("'Say' button functionality without title (intended to fail)")]
        [Bug("1", typeof(AssertionException), "was having text = ' Rudolf said")]
        public void SayingWithoutTitleFailingTest()
        {
            User user = UsersFactory.GetUser(Users.NoTitle);

            string expectedMessage = $"{user.GivenName} said: 'Hello World!'";

            Do.Website.HelloWorld.InputName(user.GivenName);
            Do.Website.HelloWorld.ClickSay();

            Do.Assertion.AssertThat(HelloWorld.Modal, UI.Control.Visible());
            Do.Assertion.AssertThat(HelloWorld.Modal, Is.Not(UI.Control.HasAttributeContains("class", "error")));
            Do.Assertion.AssertThat(HelloWorld.Modal.TextContent, UI.Control.HasText(expectedMessage));
        }

        /// <summary>
        /// Example of test with dependency on another test, it could be done via <see cref="DependsOnAttribute"/>.
        /// It's possible to configure behavior of dependent test in case when referenced test failed 
        /// (fail, skip, do not run). See <see cref="Unicorn.Taf.Core.Config.DependentTests"/>.
        /// </summary>
        [Author(Authors.JDoe)]
        [DependsOn(nameof(SayingWithoutTitleFailingTest))]
        [Test("Test with dependency on failed test")]
        public void TestWithDependencyOnFailedTest()
        {
            Do.Website.HelloWorld.CloseModalWindow();
            Do.Assertion.AssertThat(HelloWorld.Modal, Is.Not(UI.Control.Visible()));
        }
    }
}
