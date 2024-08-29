using Demo.Commons;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.UI.Core.PageObject;

namespace Demo.DesktopModule.Steps
{
    /// <summary>
    /// Represents high-level steps for application.
    /// To make steps be able to use events subscriptions it's necessary to add <see cref="StepsClassAttribute"/>.
    /// </summary>
    [StepsClass]
    public class StepsTestWindowsApp
    {
        private readonly TestWindowsApp _testApp = new TestWindowsApp();
        private HelloWorldSteps helloWorld;
        private SamplesSteps samples;

        /// <summary>
        /// It makes sense to init child steps only when the are called. It's better to take care of resources :)
        /// </summary>
        public HelloWorldSteps HelloWorld => helloWorld ??
            (helloWorld = new HelloWorldSteps(_testApp.Window.HelloWorldView));

        public SamplesSteps Samples => samples ??
            (samples = new SamplesSteps(_testApp.Window.SamplesView));

        /// <summary>
        /// Example of step with description (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// </summary>
        [Step("Start test application")]
        public TestWindowsApp StartApplication()
        {
            _testApp.Start();
            helloWorld = null;
            samples = null;

            return _testApp;
        }

        [Step("Switch app")]
        public void SwitchApp() =>
            _testApp.Window.SwitchAppToggle.Click();

        [Step("Close application")]
        public void CloseApplication() =>
            _testApp.Close();

        [Step("Close dialog if exists")]
        public void CloseDialogIfAny()
        {
            if (_testApp.Window.Modal.ExistsInPageObject())
            {
                _testApp.Window.Modal.Close();
            }
        }
    }
}
