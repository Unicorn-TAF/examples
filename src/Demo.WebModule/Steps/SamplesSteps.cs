using Demo.Commons;
using Demo.Commons.BO;
using Demo.WebModule.Gui;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.WebModule.Steps
{
    [StepsClass]
    public class SamplesSteps
    {
        private readonly SamplesPage _page;

        public SamplesSteps(SamplesPage page)
        {
            _page = page;
        }

        public void LoginWith(User user)
        {
            InputEmail(user.Email);
            InputPassword(user.Password);
            SignIn();
        }

        /// <summary>
        /// Example of step with description and parameters (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// With placeholders parameters could be substituted into description.
        /// </summary>
        [Step("Input '{0}' email")]
        public void InputEmail(string email) =>
            _page.EmailInput.SetValue(email);

        // If it's necessary to NOT to log sensitive info, it's better to call Instance from an element when sending keys.
        [Step("Input password")]
        public void InputPassword(string password) =>
            _page.PasswordInput.Instance.SendKeys(password);

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
