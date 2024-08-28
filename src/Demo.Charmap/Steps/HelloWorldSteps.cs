using Demo.Charmap.Gui;
using Demo.StepsInjection;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.Charmap.Steps
{
    /// <summary>
    /// Example of class with steps methods. The class uses custom steps implementation (<see cref="Demo.StepsInjection"/>)
    /// based on AOP.
    /// </summary>
    [StepsClass]
    public class HelloWorldSteps
    {
        private readonly HelloWorldView _view;

        public HelloWorldSteps(HelloWorldView view) 
        { 
            _view = view;
        }

        /// <summary>
        /// Example of step with description through <see cref="StepAttribute"/>. 
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// Through placeholders parameters of step method could be substitured into description.
        /// </summary>
        [Step("Select '{0}' title")]
        public void SelectTitle(string title) =>
            _view.TitleDropdown.Select(title);

        [Step("Input '{0}' name")]
        public void InputName(string name) =>
            _view.NameInput.SetValue(name);

        /// <summary>
        /// Example of step without placeholders in description. 
        /// </summary>
        [Step("Click 'Say'")]
        public void ClickSay() =>
            _view.SayButton.Click();
    }
}
