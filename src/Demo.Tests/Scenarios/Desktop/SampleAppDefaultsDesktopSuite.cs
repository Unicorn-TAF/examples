using Demo.Charmap.Gui;
using Demo.Tests.BO;
using Demo.Tests.Metadata;
using Demo.Tests.TestData;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Scenarios.Desktop
{
    /// <summary>
    /// Example of parameterized test suite (the whole suite is run for all entries in data set), 
    /// the suite should be marked with <see cref="ParameterizedAttribute"/>.
    /// The class should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>.
    /// </summary>
    [Suite("Sample app defaults")]
    [Tag(Platforms.Win), Tag(Apps.Samples)]
    [Metadata("Description",
        "Example of parameterized test suite with ordered tests. Suite checks default state of sample app")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class SampleAppDefaultsDesktopSuite : BaseDesktopSuite
    {
        private SamplesView Samples => application.Window.SamplesView;

        [Author(Authors.JBloggs)]
        [Test("Sample app authentication default layout")]
        [Order(1)]
        public void TestSamplesAuthDefaultLayout()
        {
            Do.DesktopApp.SwitchApp();
            Do.Assertion.StartAssertionsChain()
                .VerifyThat(Samples.EmailInput, UI.TextInput.HasValue(string.Empty))
                .VerifyThat(Samples.PasswordInput, UI.TextInput.HasValue(string.Empty))
                .AssertChain();
        }

        [Author(Authors.JBloggs)]
        [Test("Sample app page default layout")]
        [Order(2)]
        public void TestSamplesDefaultLayout()
        {
            User user = UsersFactory.GetDefaultUser();

            Do.DesktopApp.Samples.InputEmail(user.Email);
            Do.DesktopApp.Samples.InputPassword(user.Password);
            Do.DesktopApp.Samples.SignIn();

            Do.Assertion.StartAssertionsChain()
                .VerifyThat(Samples.WelcomeTitle, UI.Control.HasText("Welcome!"))
                .VerifyThat(Samples.ConsoleRunnerCheckbox, UI.Checkbox.Checked())
                .VerifyThat(Samples.TestAdapterCheckbox, UI.Checkbox.HasCheckState(false))
                .VerifyThat(Samples.ReportPortalRadio, Is.Not(UI.Control.Selected()))
                .VerifyThat(Samples.AllureReportlRadio, UI.Control.Selected())
                .VerifyThat(Samples.RuntimesDropdown, UI.Dropdown.HasSelectedValue(ConfigData.Runtimes.Net))
                .VerifyThat(Samples.ShowConfigButton, UI.Control.Visible())
                .VerifyThat(Samples.TabsControl.ActiveTab, UI.Control.HasText("Unit Tests"))
                .AssertChain();
        }
    }
}
