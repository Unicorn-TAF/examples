using Demo.WebModule.Gui.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Demo.WebModule.Gui
{
    /// <summary>
    /// Another web page example (see also <seealso cref="HelloWorldPage"/>).
    /// </summary>
    [PageInfo("test-ui-apps.html", "Test UI apps | Unicorn.TAF")]
    public class SamplesPage : BasePage
    {
        public SamplesPage(WebDriver driver) : base(driver)
        {
        }

        [Name("email input"), ById("email")]
        public TextInput EmailInput { get; set; }

        [Name("password input"), ById("password")]
        public TextInput PasswordInput { get; set; }

        [Name("'Sign in' button")]
        [Find(Using.WebCss, "[href = '#signin']")]
        public WebControl SignInButton { get; set; }

        [Name("'Welcome' title")]
        [Find(Using.WebCss, "#contentSection > .text-center > h2")]
        public WebControl WelcomeTitle { get; set; }

        [Name("'ConsoleRunner' checkbox"), ById("checkbox1")]
        public Checkbox ConsoleRunnerCheckbox { get; set; }

        [Name("'TestAdapter' checkbox"), ById("checkbox2")]
        public Checkbox TestAdapterCheckbox { get; set; }

        [Name("'Report Portal' radio"), ById("radio-btn1")]
        public Radio ReportPortalRadio { get; set; }

        [Name("'Allure Report' radio"), ById("radio-btn2")]
        public Radio AllureReportlRadio { get; set; }

        [Name("'Supported runtimes' dropdown")]
        [FindParam("Supported runtimes")]
        public Controls.Dropdown RuntimesDropdown { get; set; }

        [Name("'Show configuration' button")]
        [Find(Using.WebCss, "[href= '#displayConfig']")]
        public WebControl ShowConfigButton { get; set; }

        [Name("tabs control")]
        [Find(Using.WebXpath, "//ul[@role = 'tablist']/..")]
        public TabsControl TabsControl { get; set; }

        [Name("accordion control"), ById("accordion")]
        public Accordion Accordion { get; set; }
    }
}
