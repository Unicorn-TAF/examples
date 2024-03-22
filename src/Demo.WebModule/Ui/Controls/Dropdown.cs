using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace Demo.WebModule.Ui.Controls
{
    /// <summary>
    /// Example of templated control: the generic locator template (common for different places/ pages) 
    /// is specified for the control type via <see cref="FindTemplateAttribute"/>, 
    /// and for each specific place only unique part (label text for example) 
    /// should be specified via <see cref="FindParamAttribute"/>
    /// </summary>
    [FindTemplate(Using.WebXpath, "//div[contains(@class, 'bootstrap-select') and ../label[. = '{0}:']]")]
    public class Dropdown : WebControl, IDropdown
    {
        [Name("Dropdown toggle")]
        [Find(Using.WebCss, "button.dropdown-toggle")]
        public WebControl Toggle { get; set; }

        [Name("Selected value")]
        [Find(Using.WebCss, "span.filter-option")]
        private WebControl selectedValue;

        public bool Expanded => GetAttribute("class").Contains("open");

        public string SelectedValue => selectedValue.Text;

        public bool Select(string itemName)
        {
            if (SelectedValue == itemName)
            {
                return false;
            }

            Expand();

            WebControl item = GetItem(itemName);
            string classValue = item.GetAttribute("class");
            bool needToSelect = classValue != null && !classValue.Contains("selected");

            if (needToSelect)
            {
                item.Click();
            }
            else
            {
                Collapse();
            }

            return needToSelect;
        }

        public bool Expand()
        {
            if (!Expanded)
            {
                Toggle.JsClick();
            }

            return !Expanded;
        }

        public bool Collapse()
        {
            if (Expanded)
            {
                Toggle.JsClick();
            }

            return Expanded;
        }

        private WebControl GetItem(string itemName) =>
            Find<WebControl>(ByLocator.Xpath(string.Format(".//ul/li[. = '{0}']", itemName)));
    }
}
