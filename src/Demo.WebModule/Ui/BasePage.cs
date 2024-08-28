using Demo.WebModule.Ui.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject;

namespace Demo.WebModule.Ui
{
    /// <summary>
    /// Example of base web page.<br/>
    /// Page object supports inheritance, so all derived classes initialize controls described in base class also.
    /// <br/>
    /// Any page should be derived from <see cref="WebPage"/>
    /// </summary>
    public abstract class BasePage : WebPage
    {
        /// <summary>
        /// Creating page instance with <see cref="WebDriver"/> context and specified relative url and title.
        /// </summary>
        /// <param name="driver">Selenium WebDriver instance</param>
        /// <param name="subUrl">page relative url</param>
        /// <param name="title">page title</param>
        public BasePage(WebDriver driver, string subUrl, string title) : base(driver, subUrl, title)
        {
        }

        /// <summary>
        /// Creating page instance with <see cref="WebDriver"/> context.
        /// </summary>
        /// <param name="driver">Selenium WebDriver instance</param>
        public BasePage(WebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Page object controls could either class properties or class fields (properties should have a setter).
        /// </summary>
        [Name("Page title")]
        [Find(Using.WebCss, "section[style *= block] h1.heading-separator")]
        public WebControl MainTitle { get; set; }

        [Name("'Switch app' toggle")]
        [Find(Using.WebCss, "label.switch:has([onclick *= onAppSwitch])")]
        public Checkbox SwitchAppToggle { get; set; }

        /// <summary>
        /// Each page object control could have readable name specified through <see cref="NameAttribute"/>. 
        /// This generate human friendly <c>ToString</c> for the control and makes reports and logs more readable.
        /// Besides generic <see cref="FindAttribute"/> there are simlified shortcuts for locators:
        /// <see cref="ByIdAttribute"/>, <see cref="ByClassAttribute"/>, <see cref="ByNameAttribute"/>
        /// </summary>
        [Name("Page header")]
        [ById("hero")]
        public WebControl Header { get; set; }

        /// <summary>
        /// If a control has the same locator across all the places, then the locator and the name could be 
        /// specified for the control type using the same <see cref="FindAttribute"/> and <see cref="NameAttribute"/>. 
        /// In such case the control also will be initialized with page object.
        /// </summary>
        public ModalWindow Modal { get; set; }

        public void WaitForLoading() =>
            Header.Wait(Until.Visible, Timeouts.PageLoadTimeout);

        public override bool SetCheckbox(string label, bool state)
        {
            Checkbox checkbox = Find<Checkbox>(ByLocator.Xpath(".//input[@type='checkbox' and ..//*[. = '" + label + "']]"));
            bool needToClick = checkbox.Checked != state;

            if (needToClick)
            {
                checkbox.JsClick();
            }

            return needToClick;
        }

        public override bool SelectRadio(string label)
        {
            Radio radio = Find<Radio>(ByLocator.Xpath(".//input[@type='radio' and ..//*[. = '" + label + "']]"));

            if (radio.Selected)
            {
                return false;
            }
            else
            {
                radio.JsClick();
                return true;
            }
        }
    }
}
