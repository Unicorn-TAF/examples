using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls.Typified;

namespace Demo.Charmap.Gui
{
    public class ModalWindow : Window
    {
        [Name("Text content"), ByClass("Static")]
        public Text TextContent { get; set; }
    }
}
