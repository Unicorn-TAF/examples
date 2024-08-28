using UIAutomationClient;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Win.Controls.Typified;

namespace Demo.DesktopModule.Gui
{
    public class TestAppWindow : Window
    {
        public TestAppWindow()
        {
        }

        public TestAppWindow(IUIAutomationElement instance)
            : base(instance)
        {
        }

        [Name("'Switch app' button"), ById("switchAppButton")]
        public Button SwitchAppToggle { get; set; }

        [Name("'Hello World' view"), ById("HelloWorldView")]
        public HelloWorldView HelloWorldView { get; set; }

        [Name("'Hello World' view"), ById("SampleView")]
        public SamplesView SamplesView { get; set; }

        [Name("Modal window"), ByClass("#32770")]
        public ModalWindow Modal {  get; set; }
    }
}
