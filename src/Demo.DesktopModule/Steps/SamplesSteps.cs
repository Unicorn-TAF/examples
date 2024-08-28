using Demo.DesktopModule.Gui;
using Demo.StepsInjection;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.DesktopModule.Steps
{
    [StepsClass]
    public class SamplesSteps
    {
        private readonly SamplesView _page;

        public SamplesSteps(SamplesView page) 
        { 
            _page = page;
        }

        /// <summary>
        /// Example of step with description and parmeters (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// With placeholders parameters could be substitured into description.
        /// </summary>
        [Step("Input '{0}' email")]
        public void InputEmail(string email) =>
            _page.EmailInput.SetValue(email);

        // If it's necessary to NOT to log sensitive info, it's better to call custom method.
        [Step("Input password")]
        public void InputPassword(string password) =>
            _page.SetPassword(password); 

        [Step("Sign in")]
        public void SignIn() =>
            _page.SignInButton.Click();

        [Step("Select '{0}' report")]
        public void SelectReport(string reportName) =>
            _page.SelectRadio(reportName);

        [Step("Select '{0}' runner")]
        public void SelectRunner(string runnerName) =>
            _page.SetCheckbox(runnerName, true);

        [Step("Deselect '{0}' runner")]
        public void DeselectRunner(string runnerName) =>
            _page.SetCheckbox(runnerName, false);

        [Step("Select '{0}' runtime")]
        public void SelectRuntime(string runtime) =>
            _page.RuntimesDropdown.Select(runtime);

        [Step("Show configuration")]
        public void ShowConfiguration() =>
            _page.ShowConfigButton.Click();
    }
}
