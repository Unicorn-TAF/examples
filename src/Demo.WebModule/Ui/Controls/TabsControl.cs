using System.Collections.Generic;
using System.Linq;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls;

namespace Demo.WebModule.Ui.Controls
{
    /// <summary>
    /// Another complex control example (see also <seealso cref="Accordion"/>).<br/>
    /// 
    /// Controls implementing predefined controls interfaces allow to apply type specific matchers 
    /// to make tests and assertions easier and more readable.
    /// </summary>
    public class TabsControl : WebControl, IItemSelectable
    {
        /// <summary>
        /// <see cref="IList{T}"/> with controls is also could be a part of page object and is located 
        /// by the same attributes as single controls
        /// </summary>
        [Name("Tabs list")]
        [Find(Using.WebCss, "ul > li")]
        public IList<WebControl> TabsList { get; set; }

        [Name("Tab content")]
        [Find(Using.WebCss, ".tab-content")]
        public WebControl TabContent { get; set; }

        public WebControl ActiveTab => TabsList.First(tab => tab.GetAttribute("class") == "active");

        public string SelectedValue => ActiveTab.Text;

        public bool Select(string tabName)
        {
            WebControl tab = TabsList.FirstOrDefault(x => x.Text == tabName);

            if (tab == null)
            {
                throw new ControlNotFoundException($"Unable to find tab with name '{tabName}'");
            }

            bool needToSelect = tab.GetAttribute("class") != "active";

            if (needToSelect)
            {
                tab.Click();
            }

            return needToSelect;
        }

        public string GetContent() =>
            TabContent.GetAttribute("innerText");
    }
}
