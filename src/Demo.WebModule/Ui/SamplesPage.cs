using Demo.WebModule.Ui.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Demo.WebModule.Ui
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

        [Name("Email input"), ById("email")]
        public TextInput EmailInput { get; set; }

        [Name("Password input"), ById("password")]
        public TextInput PasswordInput { get; set; }

        [Name("'Sign in' button")]
        [Find(Using.WebCss, "[href = '#signin']")]
        public WebControl SignInButton { get; set; }

        [Name("Tabs control")]
        [Find(Using.WebXpath, "//ul[@role = 'tablist']/..")]
        public TabsControl TabsControl { get; set; }

        [Name("Accordion control"), ById("accordion")]
        public Accordion Accordion { get; set; }
    }
}
