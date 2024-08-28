﻿using Demo.DesktopModule.Gui;
using Demo.Tests.BO;
using Demo.Tests.Metadata;
using Demo.Tests.TestData;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Scenarios.Desktop
{
    /// <summary>
    /// Web test suite example. The class should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>.
    /// <br/>
    /// It's possible to specify any number of suite tags and metadata.
    /// Suite tags allow to use parameterized targeted runs: suites are selected based on specific tags presence.
    /// </summary>
    [Suite("Hello World desktop app")]
    [Tag(Platforms.Win), Tag(Apps.HelloWorld)]
    [Metadata("Description", "Example of test suite with parameterized test. Сhecks functionality of hello world app")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class HelloWorldDesktopSuite : BaseDesktopSuite
    {
        private HelloWorldView HelloWorld => application.Window.HelloWorldView;

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
                new DataSet("Nothing selected", 
                    new User("", "", "", "", ""), "Name is empty", 
                    UI.Control.HasAttributeContains("name", "Error")),

                new DataSet("Only title", 
                    UsersFactory.GetUser(Users.NoGivenName), "Name is empty",
                    UI.Control.HasAttributeContains("name", "Error")),

                new DataSet("Title and name", 
                    UsersFactory.GetUser(Users.JDoe), "Mr John said: 'Hello World!'", 
                    Is.Not(UI.Control.HasAttributeContains("name", "Error"))),
            };

        /// <summary>
        /// Example of simple test with specified category.
        /// It's possible to specify tests execution order within a test suite using <see cref="OrderAttribute"/>.
        /// Tests with higher order will be executed later.
        /// </summary>
        [Author(Authors.JBloggs)]
        [Category(Categories.Smoke)]
        [Order(1)]
        [Test("Hello World page default layout")]
        public void TestHelloWorldDefaultLayout() =>
            Do.Assertion.StartAssertionsChain()
                .VerifyThat(application.Window, UI.Window.HasTitle("\"Hello World\" app"))
                .VerifyThat(HelloWorld.TitleDropdown, UI.Dropdown.HasSelectedValue(""))
                .VerifyThat(HelloWorld.NameInput, UI.TextInput.HasValue(string.Empty))
                .AssertChain();

        /// <summary>
        /// Example of parameterized test. The test should have <see cref="TestDataAttribute"/> 
        /// and the same number of parameters as in DataSets in test data.
        /// </summary>
        [Author(Authors.JBloggs)]
        [Category(Categories.Smoke)]
        [Test("'Say' button functionality")]
        [Order(2)]
        [TestData(nameof(TestParameters))]
        public void TestSaying(User user, string expectedText,
            TypeSafeMatcher<IControl> dialogMatcher) // It's possible to parameterize even matchers
        {
            if (!string.IsNullOrEmpty(user.Title))
            {
                Do.DesktopApp.HelloWorld.SelectTitle(user.Title);
            }

            Do.DesktopApp.HelloWorld.InputName(user.GivenName);
            Do.DesktopApp.HelloWorld.ClickSay();

            Do.Assertion.AssertThat(application.Window.Modal, dialogMatcher);
            Do.Assertion.AssertThat(application.Window.Modal.TextContent, UI.Control.HasText(expectedText));
        }

        /// <summary>
        /// Actions executed after each test.
        /// It's possible to specify:<br/>
        ///  - whether it needs to be run in case of test fail or not<br/>
        ///  - whether need to skip all next tests if AfterTest is failed or not
        /// </summary>
        [AfterTest]
        public void CloseDialog() =>
            Do.DesktopApp.CloseDialogIfAny();
    }
}