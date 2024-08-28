using System.Collections.Generic;
using System.Linq;
using UIAutomationClient;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Controls.Typified;

namespace Demo.DesktopModule.Gui.Controls
{
    /// <summary>
    /// Another complex control example (see also <seealso cref="Accordion"/>).<br/>
    /// 
    /// Controls implementing predefined controls interfaces allow to apply type specific matchers 
    /// to make tests and assertions easier and more readable.
    /// </summary>
    public class TabsControl : WinControl, IItemSelectable
    {
        public override int UiaType => UIA_ControlTypeIds.UIA_TabControlTypeId;

        public override string ClassName => "TabControl";

        /// <summary>
        /// <see cref="IList{T}"/> with controls is also could be a part of page object and is located 
        /// by the same attributes as single controls
        /// </summary>
        [Name("Tabs list"), ByClass("TabItem")]
        public IList<TabItem> TabsList { get; set; }

        public TabItem ActiveTab => TabsList.First(tab => tab.Selected);

        public string SelectedValue => ActiveTab.Text;

        public bool Select(string tabName)
        {
            TabItem tab = TabsList.FirstOrDefault(x => x.Text == tabName);

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
    }
}
