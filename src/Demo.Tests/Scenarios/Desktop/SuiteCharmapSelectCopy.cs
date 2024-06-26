﻿using Demo.Charmap;
using Demo.Tests.Base;
using Demo.Tests.Metadata;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Scenarios.Desktop
{
    /// <summary>
    /// Desktop application test suite example.
    /// The test suite should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Suite("Charmap Select/Copy")]
    [Tag(Platforms.Win), Tag(Platforms.Apps.Charmap), Tag(Features.CharmapSelectCopy)]
    [Metadata("Description", "Tests for Charmap select/copy functionality")]
    [Metadata("Specs link",
        "https://support.microsoft.com/en-us/help/315684/how-to-use-special-characters-in-windows-documents")]
    public class SuiteCharmapSelectCopy : BaseTestSuite
    {
        private CharmapApp Charmap => CharmapApp.Charmap;

        /// <summary>
        /// Actions executed before each test.
        /// </summary>
        [BeforeTest]
        public void TestInit() =>
            Do.CharMap.StartApplication();

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author(Authors.JBloggs)]
        [Category(Categories.Smoke)]
        public void TestSelectCopyButtonsDefalutState()
        {
            Do.Assertion.AssertThat(Charmap.Window.ButtonSelect, UI.Control.Visible());
            Do.Assertion.AssertThat(Charmap.Window.ButtonSelect, UI.Control.Enabled());

            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, UI.Control.Visible());
            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, Is.Not(UI.Control.Enabled()));
        }

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author(Authors.JBloggs)]
        [Category(Categories.Smoke)]
        [Test("Check 'Copy' button is enabled after selection")]
        public void TestCopyIsEnabledAfterSelection()
        {
            Do.CharMap.SelectCurrentSymbol();
            Do.Assertion.AssertThat(Charmap.Window.ButtonCopy, UI.Control.Enabled());
        }

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author(Authors.JBloggs)]
        [Category(Categories.Smoke)]
        [Test("Check ability to select chars")]
        public void TestAbilityToSelectChars()
        {
            Do.CharMap.SelectCurrentSymbol();
            Do.Assertion.AssertThat(Charmap.Window.InputCharactersToCopy, Is.Not(UI.Control.HasText(string.Empty)));
        }

        /// <summary>
        /// Actions executed after each test.
        /// </summary>
        [AfterTest]
        public void TestTearDown() =>
            Do.CharMap.CloseApplication();
    }
}
