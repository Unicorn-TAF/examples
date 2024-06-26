﻿using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Core.Matchers;
using Demo.Charmap;
using Demo.Tests.Base;
using Demo.Tests.Metadata;

namespace Demo.Tests.Scenarios.Desktop
{
    /// <summary>
    /// Desktop application test suite example.
    /// The test suite should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Suite("Charmap Fonts dropdown")]
    [Tag(Platforms.Win), Tag(Platforms.Apps.Charmap), Tag(Features.CharmapFonts)]
    [Metadata("Description", "Tests for Charmap fonts dropdown functionality")]
    [Metadata("Specs link",
        "https://support.microsoft.com/en-us/help/315684/how-to-use-special-characters-in-windows-documents")]
    public class SuiteCharmapFonts : BaseTestSuite
    {
        private CharmapApp Charmap => CharmapApp.Charmap;

        /// <summary>
        /// Data for parameterized test. First parameter is <see cref="DataSet"/> name 
        /// and is not considered in parameterization.
        /// The method should be static.
        /// </summary>
        /// <returns></returns>
        public static List<DataSet> GetFontsData() =>
            new List<DataSet>
            {
                new DataSet("Calibri font", "Calibri"),
                new DataSet("Consolas font", "Consolas"),
                new DataSet("Courier font", "Courier"),
                new DataSet("Wingdings font", "Wingdings")
            };

        /// <summary>
        /// Actions before whole suite execution.
        /// </summary>
        [BeforeSuite]
        public void ClassInit() =>
            Do.CharMap.StartApplication();

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author(Authors.JBloggs)]
        [Test("Check 'Font' dropdown default state")]
        public void TestFontDropdownDefaultState()
        {
            Do.Assertion.AssertThat(Charmap.Window.DropdownFonts, UI.Control.Visible());
            Do.Assertion.AssertThat(Charmap.Window.DropdownFonts, UI.Control.Enabled());
        }

        /// <summary>
        /// Example of parameterized test.
        /// Number of parameters should be the same as number of items in <see cref="DataSet"/> (DataSet name is not considered)
        /// </summary>
        /// <param name="font">1st DataSet parameter</param>
        [Author(Authors.JBloggs)]
        [Category(Categories.Smoke)]
        [Test("Check 'Font' dropdown ability to select")]
        [TestData(nameof(GetFontsData))]
        public void TestFontDropdownAbilityToSelect(string font)
        {
            Do.CharMap.SelectFont(font);
            Do.Assertion.AssertThat(Charmap.Window.DropdownFonts, UI.Dropdown.HasSelectedValue(font));
        }

        /// <summary>
        /// Actions after whole suite execution.
        /// </summary>
        [AfterSuite]
        public void ClassTearDown() =>
            Do.CharMap.CloseApplication();
    }
}
