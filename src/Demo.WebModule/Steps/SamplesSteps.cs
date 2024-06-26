﻿using Demo.WebModule.Ui;
using Demo.StepsInjection;
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

        /// <summary>
        /// Example of step with description and parmeters (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// With placeholders parameters could be substitured into description.
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
    }
}
