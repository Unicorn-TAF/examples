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
    /// Example of complex UI control block which could be reused across different places.<br/>
    /// Any block could be a PageObject for other child controls.<br/>
    /// <br/>
    /// Typified or complex UI control should inherit <see cref="WebControl"/>
    /// </summary>
    public class Accordion : WebControl, IItemSelectable
    {
        /// <summary>
        /// <see cref="IList{T}"/> with controls is also could be a part of page object and is located 
        /// by the same attributes as single controls
        /// </summary>
        [Name("Accordion panels")]
        [Find(Using.WebCss, "div.panel")]
        private IList<Panel> panelsList;

        public Panel ActivePanel => panelsList.First(p => p.Expanded);

        public string SelectedValue => ActivePanel.PanelTitle.Text;

        public bool Select(string panelName)
        {
            Panel panel = panelsList.FirstOrDefault(x => x.PanelTitle.Text == panelName);

            if (panel == null)
            {
                throw new ControlNotFoundException($"Unable to find panel with name '{panelName}'");
            }

            bool needToSelect = !panel.Expanded;

            if (needToSelect)
            {
                panel.PanelTitle.Click();
            }

            return needToSelect;
        }

        public string GetContent() =>
            ActivePanel.PanelContent.GetAttribute("innerText");

        public class Panel : WebControl
        {
            [Name("Accordion title")]
            [Find(Using.WebTag, "h4")]
            public WebControl PanelTitle { get; set; }

            [Name("Accordion content")]
            [Find(Using.WebCss, ".panel-collapse")]
            public WebControl PanelContent { get; set; }

            public bool Expanded => PanelContent.GetAttribute("aria-expanded") == "true";
        }
    }
}
