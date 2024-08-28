using Demo.DesktopModule;
using Demo.DesktopModule.Gui;
using Demo.Tests.Base;
using Demo.Tests.BO;
using Demo.Tests.Metadata;
using Demo.Tests.TestData;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Scenarios.Desktop
{
    [Suite("Sample app authentication")]
    [Tag(Platforms.Win), Tag(Apps.Samples), Tag(Features.Auth)]
    [Metadata("Description", "The suite checks auth process for Sample app including positive and negative cases")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class SampleAppAuthDesktopSuite : BaseTestSuite
    {
        protected TestWindowsApp application;

        private SamplesView Samples => application.Window.SamplesView;

        /// <summary>
        /// Actions executed before all tests in current suite.
        /// </summary>
        [BeforeTest]
        public void ClassInit()
        {
            application = Do.DesktopApp.StartApplication();
            Do.DesktopApp.SwitchApp();
        }

        /// <summary>
        /// Actions executed after all tests in current suite.
        /// </summary>
        [AfterTest]
        public void ClassTearDown()
        {
            Do.DesktopApp.CloseDialogIfAny();
            Do.DesktopApp.CloseApplication();
        }

        public List<DataSet> IncorrectUsersData() =>
            new List<DataSet>
            {
                new DataSet("Email is empty", UsersFactory.GetUser(Users.NoEmail), "Email is empty"),
                new DataSet("Incorrect email", UsersFactory.GetUser(Users.IncorrectEmail), "Email is not valid"),
                new DataSet("Password is empty", UsersFactory.GetUser(Users.NoPassword), "Password is empty"),
            };

        [Author(Authors.JBloggs)]
        [Test("Successful authorization")]
        public void CheckAuthenticationPositiveTest()
        {
            User user = UsersFactory.GetUser(Users.JDoe);
            Do.DesktopApp.Samples.InputEmail(user.Email);
            Do.DesktopApp.Samples.InputPassword(user.Password);
            Do.DesktopApp.Samples.SignIn();
            Do.Assertion.AssertThat(Samples.WelcomeTitle, UI.Control.Visible());
        }

        [Author(Authors.JBloggs)]
        [Test("Authorization fail for incorrect data")]
        [TestData(nameof(IncorrectUsersData))]
        public void CheckAuthenticationNegativeTest(User user, string errorMessage)
        {
            Do.DesktopApp.Samples.InputEmail(user.Email);
            Do.DesktopApp.Samples.InputPassword(user.Password);
            Do.DesktopApp.Samples.SignIn();

            Do.Assertion.AssertThat(application.Window.Modal, UI.Control.Visible());
            Do.Assertion.AssertThat(application.Window.Modal, UI.Control.HasAttributeContains("name", "Error"));
            Do.Assertion.AssertThat(application.Window.Modal.TextContent, UI.Control.HasText(errorMessage));
            Do.Assertion.AssertThat(Samples.WelcomeTitle, Is.Not(UI.Control.Visible()));
        }
    }
}
