using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls;

namespace Demo.WebModule.Ui.Controls
{
    /// <summary>
    /// Example of complex UI control block which could be reused across different places.<br/>
    /// Any block could be a PageObject for other child controls.<br/>
    /// Any block could have default locator and name which are used if no direct locator/name is specified in PageObject.
    /// <br/>
    /// Typified or complex UI control should inherit <see cref="WebControl"/>
    /// </summary>
    [Name("Modal window")]
    [Find(Using.Id, "modalWindow")]
    public class ModalWindow : WebControl
    {
        [Name("Close button"), Find(Using.WebCss, "a.onclick")]
        private WebControl closeButton;

        [Name("Text content"), ById("caption")]
        public WebControl TextContent { get; set; }

        public bool IsError => GetAttribute("class").Contains("error");

        public void Close() =>
            closeButton.Click();
    }
}
