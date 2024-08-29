using Demo.Commons.BO;
using Demo.Tests.Metadata;
using Demo.Tests.TestData;
using Demo.WebModule.Gui;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Scenarios.Web
{
    [Suite("Sample app authentication")]
    [Tag(Platforms.Web), Tag(Apps.Samples), Tag(Features.Auth)]
    [Metadata("Description", "The suite checks auth process for Sample app including positive and negative cases")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class SampleAppAuthWebSuite : BaseWebSuite
    {
        private SamplesPage Samples => website.GetPage<SamplesPage>();

        [BeforeTest]
        public void RefreshApp() =>
            Do.Website.SwitchApp();

        public List<DataSet> IncorrectUsersData() =>
            new List<DataSet>
            {
                new DataSet("Email is empty", UsersFactory.GetUser(Users.NoEmail), "Email is empty"),
                new DataSet("Incorrect email", UsersFactory.GetUser(Users.IncorrectEmail), "Email is not valid"),
                new DataSet("Password is empty", UsersFactory.GetUser(Users.NoPassword), "Password is empty"),
            };

        [Author(Authors.JDoe)]
        [Test("Successful authorization")]
        public void CheckAuthenticationPositiveTest()
        {
            User user = UsersFactory.GetUser(Users.JDoe);
            Do.Website.Samples.LoginWith(user);
            Do.Assertion.AssertThat(Samples.WelcomeTitle, UI.Control.Visible());
        }

        [Author(Authors.JDoe)]
        [Test("Authorization fail for incorrect data")]
        [TestData(nameof(IncorrectUsersData))]
        public void CheckAuthenticationNegativeTest(User user, string errorMessage)
        {
            Do.Website.Samples.LoginWith(user);

            Do.Assertion.AssertThat(Samples.Modal, UI.Control.Visible());
            Do.Assertion.AssertThat(Samples.Modal, UI.Control.HasAttributeContains("class", "error"));
            Do.Assertion.AssertThat(Samples.Modal.TextContent, UI.Control.HasText(errorMessage));
            Do.Assertion.AssertThat(Samples.WelcomeTitle, Is.Not(UI.Control.Visible()));
        }

        [AfterTest]
        public void RefreshPage() =>
            Do.Website.RefreshPage();
    }
}
