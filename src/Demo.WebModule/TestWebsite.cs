using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject;

namespace Demo.WebModule
{
    /// <summary>
    /// Describes specific website implementation.<br/>
    /// The base website gives access to underlying <see cref="WebDriver"/> instance, provides with pages cache 
    /// mechanism (to avoid page creation each time) and eases pages navigation. 
    /// Some site specific actions and helpers could be placed here.
    /// <br/>
    /// The website should inherit <see cref="WebSite"/>
    /// </summary>
    public class TestWebsite : WebSite
    {
        /// <summary>
        /// Website creation based on existing explicitly created WebDriver instance.
        /// 
        /// Should call base constructor.
        /// </summary>
        /// <param name="driver"><see cref="WebDriver"/> instance</param>
        /// <param name="siteUrl">site base url</param>
        public TestWebsite(WebDriver driver, string siteUrl) : base(driver, siteUrl)
        {
        }

        /// <summary>
        /// Website creation based on retrieved type of browser (WebDriver in this case is created automatically).
        /// 
        /// Should call base constructor.
        /// </summary>
        /// <param name="browser">type of browser</param>
        /// <param name="siteUrl">site base url</param>
        public TestWebsite(BrowserType browser, string siteUrl) : base(browser, siteUrl)
        {
        }
    }
}
