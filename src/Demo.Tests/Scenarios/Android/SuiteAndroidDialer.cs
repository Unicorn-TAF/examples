﻿using Demo.AndroidDialer;
using Demo.Tests.Base;
using Demo.Tests.Metadata;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Core.PageObject;

namespace Demo.Tests.Scenarios.Android
{
    /// <summary>
    /// Example of disabled test suite, it could be done using <see cref="DisabledAttribute"/>. 
    /// (Disabled suite means that all it's tests will not be executed, but they're still discoverable and can be collected in stats)
    /// </summary>
    [Disabled("Android emulator is not configured")]
    [Suite("Tests Android dialer application")]
    [Tag(Platforms.Android), Tag(Apps.Dialer)]
    [Metadata("Description", "Suite with tests for android Dialer app")]
    [Metadata("Target device", "Android emulator")]
    [Metadata("API version", "v25")]
    public class SuiteAndroidDialer : BaseTestSuite
    {
        private AndroidDialerApi25 dialer;

        [BeforeTest]
        public void TestInit() =>
            dialer = Do.Android.OpenDialer();

        [Author(Authors.RLingens)]
        [Category(Categories.Smoke)]
        [Test("Check dialpad button")]
        public void TestDialpadButton()
        {
            Do.Assertion.AssertThat(dialer.AppFrame.DialPadButton, UI.Control.Visible());
        }

        [Author(Authors.RLingens)]
        [Category(Categories.Smoke)]
        [Test("Check ability to type call number")]
        public void TestAbilityToTypeCallNumber()
        {
            Do.Android.OpenDialpad();
            Do.Android.TapNumber("#");
            Do.Android.TapNumber("1");
            Do.Android.TapNumber("2");
            Do.Android.TapNumber("3");
            Do.Assertion.AssertThat(dialer.AppFrame.DialPad.NumberText, UI.Control.HasText("#123"));
        }

        /// <summary>
        /// Example of disabled test, it could be done using <see cref="DisabledAttribute"/>. 
        /// (The test will not be executed, but it's still discoverable and can be collected in stats)
        /// </summary>
        [Disabled("Disable reason")]
        [Author(Authors.RLingens)]
        [Test("Check calls history")]
        public void TestCallsHistory()
        {
            Do.Android.OpenCallsHistory();
            Do.Assertion.AssertThat(
                dialer.AppFrame.CallsHistory.EmptyListIcon.ExistsInPageObject(),
                Is.EqualTo(true));
        }

        [AfterTest]
        public void TestCleanup() =>
            Do.Android.CloseDialer();
    }
}
