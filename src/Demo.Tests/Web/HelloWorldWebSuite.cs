using Demo.Tests.Base;
using Demo.Tests.Metadata;
using Demo.WebModule;
using Demo.WebModule.Ui;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Web;

namespace Demo.Tests.Web
{
    /// <summary>
    /// Web test suite example. The class should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>.
    /// <br/>
    /// It's possible to specify any number of suite tags and metadata.
    /// Suite tags allow to use parameterized targeted runs: suites are selected based on specific tags presence.
    /// </summary>
    [Suite("Hello world web tests")]
    [Tag(Features.Web), Tag(Features.HelloWorld)]
    [Metadata("Description", 
        "Example of test suite with parameterized test. Suite checks functionality of hello world app")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class HelloWorldWebSuite : BaseTestSuite
    {
        private TestWebsite website;

        private HelloWorldPage HelloWorld => website.GetPage<HelloWorldPage>();

        /// <summary>
        /// Actions executed before all tests in current suite.
        /// </summary>
        [BeforeSuite]
        public void ClassInit() =>
            website = Do.Website.Open(BrowserType.Chrome, Config.Instance.WebsiteUrl);

        /// <summary>
        /// Data for parameterized test. The method should return a list of <see cref="DataSet"/>.<br/>
        /// First parameter of <see cref="DataSet"/> is data set name and it is not a part of test data. <br/>
        /// For test parameterization the method could be static or non-static.<br/>
        /// For whole suite parametherization th method should have <see cref="SuiteDataAttribute"/> 
        /// and should be <c>static</c>.
        /// </summary>
        public List<DataSet> TestParameters() =>
            new List<DataSet>
            {
                new DataSet("Nothing selected", string.Empty, string.Empty, "Error: name is empty",
                    UI.Control.HasAttributeContains("class", "error")),

                new DataSet("Only title", "Mr", string.Empty, "Error: name is empty",
                    UI.Control.HasAttributeContains("class", "error")),

                new DataSet("Only name", string.Empty, "John Doe", "John Doe said: 'Hello World!'",
                    Is.Not(UI.Control.HasAttributeContains("class", "error"))),

                new DataSet("Title and name", "Mr", "John Doe", "Mr John Doe said: 'Hello World!'",
                    Is.Not(UI.Control.HasAttributeContains("class", "error"))),
            };

        /// <summary>
        /// Example of parameterized test. The test should have <see cref="TestDataAttribute"/> 
        /// and the same number of parameters as in DataSets in test data.
        /// </summary>
        [Author(Authors.JDoe)]
        [Category(Categories.Smoke)]
        [Test("Test 'Say' button functionality")]
        [TestData(nameof(TestParameters))]
        public void TestSaying(string title, string name, string expectedText,
            TypeSafeMatcher<IControl> dialogMatcher) // It's possible to parameterize even matchers
        {
            if (!string.IsNullOrEmpty(title))
            {
                Do.Website.HelloWorld.SelectTitle(title);
            }

            Do.Website.HelloWorld.InputName(name);
            Do.Website.HelloWorld.ClickSay();

            Do.Assertion.AssertThat(HelloWorld.Modal, dialogMatcher);
            Do.Assertion.AssertThat(HelloWorld.Modal.TextContent, UI.Control.HasText(expectedText));
        }

        /// <summary>
        /// Example of parameterized test. The test should have <see cref="TestDataAttribute"/> 
        /// and the same number of parameters as in DataSets in test data.
        /// </summary>
        [Author(Authors.JDoe)]
        [Test("Test which always fails")]
        public void FailingTest()
        {
            Do.Assertion.AssertThat(HelloWorld.NameInput, UI.TextInput.HasValue("Some name"));
        }

        /// <summary>
        /// Actions executed after each test.
        /// It's possible to specify:<br/>
        ///  - whether it needs to be run in case of test fail or not<br/>
        ///  - whether need to skip all next tests if AfterTest is failed or not
        /// </summary>
        [AfterTest]
        public void RefreshPage()
        {
            website.Driver.SeleniumDriver.Navigate().Refresh();
            HelloWorld.WaitForLoading();
        }

        /// <summary>
        /// Actions executed after all tests in current suite.
        /// </summary>
        [AfterSuite]
        public void ClassTearDown() =>
            Do.Website.CloseBrowser();
    }
}
