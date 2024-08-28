using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Demo.WebModule.Ui
{
    /// <summary>
    /// Example of web page (see also <seealso cref="BasePage"/>).<br/>
    /// Page properties like relative url and title could be specified using <see cref="PageInfoAttribute"/>.<br/>
    /// <c>Url</c> property is implicitly used to navigate to the page from <see cref="WebSite"/> instance.<br/>
    /// <c>Title</c> property is implicitly used in page <c>Opened</c> property indicating 
    /// whether the page is currently opened or not.
    /// <br/>
    /// Any page should be derived from <see cref="WebPage"/>
    /// </summary>
    [PageInfo("test-ui-apps.html", "Test UI apps | Unicorn.TAF")]
    public class HelloWorldPage : BasePage
    {
        /// <summary>
        /// Creating page instance with <see cref="WebDriver"/> context.
        /// </summary>
        /// <param name="driver">Selenium WebDriver instance</param>
        public HelloWorldPage(WebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Example of templated control: the generic locator template (common for different places/ pages) 
        /// is specified for the control type via <see cref="FindTemplateAttribute"/>, 
        /// and for each specific place only unique part (label text for example) 
        /// should be specified via <see cref="FindParamAttribute"/>
        /// </summary>
        [Name("'Title' dropdown")]
        [FindParam("Title")]
        public Controls.Dropdown TitleDropdown { get; set; }

        /// <summary>
        /// Control of any type derived from <see cref="WebControl"/> could be a part of page object.<br/>
        /// Controls implementing predefined controls interfaces allow to apply type specific matchers 
        /// to make tests and assertions easier and more readable.
        /// </summary>
        [Name("'Name' input"), ById("name")]
        public TextInput NameInput { get; set; }

        /// <summary>
        /// Controls could be located using generic <see cref="FindAttribute"/>.
        /// </summary>
        [Name("'Say' button")]
        [Find(Using.WebCss, "[href = '#helloWorld']")]
        public WebControl SayButton { get; set; }

        [Find(Using.WebCss, "a[href *= HelloWorldGui]")]
        public WebControl DownloadWinAppLink { get; set; }
    }
}
