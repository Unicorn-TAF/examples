using Demo.DesktopModule.Gui.Controls;
using UIAutomationClient;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Controls.Typified;

namespace Demo.DesktopModule.Gui
{
    /// <summary>
    /// Another web page example (see also <seealso cref="HelloWorldPage"/>).
    /// </summary>
    public class SamplesView : Custom
    {
        [Name("email input"), ById("emailInput")]
        public TextInput EmailInput { get; set; }

        [Name("password input"), ById("passwordInput")]
        public TextInput PasswordInput { get; set; }

        [Name("'Sign in' button"), ByName("Sign in")]
        public Button SignInButton { get; set; }

        [Name("'Welcome' title"), ByName("Welcome!")]
        public Text WelcomeTitle { get; set; }

        [Name("'ConsoleRunner' checkbox"), ById("runnerCheckbox")]
        public Checkbox ConsoleRunnerCheckbox { get; set; }

        [Name("'TestAdapter' checkbox"), ById("adapterCheckbox")]
        public Checkbox TestAdapterCheckbox { get; set; }

        [Name("'Report Portal' radio"), ById("rpRadio")]
        public Radio ReportPortalRadio { get; set; }

        [Name("'Allure Report' radio"), ById("alRadio")]
        public Radio AllureReportlRadio { get; set; }

        [Name("'Supported runtimes' dropdown")]
        [ById("runtimeCbox")]
        public Dropdown RuntimesDropdown { get; set; }

        [Name("'Show configuration' button"), ByName("Show configuration")]
        public WinControl ShowConfigButton { get; set; }

        [Name("tabs control"), ByClass("TabControl")]
        public TabsControl TabsControl { get; set; }

        /// <summary>
        /// Separate method to avoid password logging.
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password) =>
            PasswordInput.Instance.GetPattern<IUIAutomationValuePattern>().SetValue(password);
    }
}
