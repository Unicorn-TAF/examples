using Demo.Tests.Base;
using Demo.Tests.BO;
using Demo.Tests.Metadata;
using Demo.Tests.TestData;
using Demo.WebModule;
using Demo.WebModule.Ui;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Core.PageObject;

namespace Demo.Tests.Scenarios.Web
{
    [Suite("Sample app configurations display")]
    [Tag(Platforms.Web), Tag(Apps.Samples), Tag(Features.ConfigSetup)]
    [Metadata("Description", "Example of test suite with dependent tests where main test is failing")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class SampleAppConfigDisplayWebSuite : BaseTestSuite
    {
        private TestWebsite website;
        private SamplesPage Samples => website.GetPage<SamplesPage>();

        /// <summary>
        /// Actions executed before whole tests in current suite.
        /// </summary>
        [BeforeSuite]
        public void ClassInit()
        {
            User user = UsersFactory.GetDefaultUser();
            website = Do.Website.Open(Config.Instance.WebsiteUrl);
            Do.Website.SwitchApp();
            Do.Website.Samples.InputEmail(user.Email);
            Do.Website.Samples.InputPassword(user.Password);
            Do.Website.Samples.SignIn();
        }

        public List<DataSet> ConfigurationsData() =>
            new List<DataSet>
            {
                new DataSet("Configuration #1", ConfigData.Runtimes.Net, ConfigData.Reports.ReportPortal, 
                    new string[] { }),

                new DataSet("Configuration #2", ConfigData.Runtimes.NetCore, ConfigData.Reports.Allure,
                    new string[] { ConfigData.Runners.ConsoleRunner }),

                new DataSet("Configuration #3", ConfigData.Runtimes.NetFramework, ConfigData.Reports.Allure,
                    new string[] { ConfigData.Runners.ConsoleRunner, ConfigData.Runners.TestAdapter })
            };

        [Author(Authors.JDoe)]
        [Test("Show configuration functionality")]
        [TestData(nameof(ConfigurationsData))]
        public void CheckShowConfigurationFunctionality(string runtime, string report, string[] runners)
        {
            List<string> runnersToDisplay = new List<string>();

            HandleRunner(runners, ConfigData.Runners.ConsoleRunner, runnersToDisplay, "Console");
            HandleRunner(runners, ConfigData.Runners.TestAdapter, runnersToDisplay, "Adapter");

            Do.Website.Samples.SelectReport(report);
            Do.Website.Samples.SelectRuntime(runtime);
            Do.Website.Samples.ShowConfiguration();

            string message = GenerateMessageFrom(runtime, report, runnersToDisplay);

            Do.Assertion.AssertThat(Samples.Modal, UI.Control.Visible());
            Do.Assertion.AssertThat(Samples.Modal.TextContent, UI.Control.HasText(message));
        }

        private void HandleRunner(string[] runners, string runner, List<string> runnersToDisplay, string displayName)
        {
            if (runners.Contains(runner))
            {
                Do.Website.Samples.SelectRunner(runner);
                runnersToDisplay.Add(displayName);
            }
            else
            {
                Do.Website.Samples.DeselectRunner(runner);
            }
        }

        [AfterTest]
        public void CloseModal()
        {
            if (Samples.Modal.ExistsInPageObject())
            {
                Samples.Modal.Close();
            }
        }

        [AfterSuite]
        public void ClassTearDown() =>
            Do.Website.CloseBrowser();

        private static string GenerateMessageFrom(string runtime, string report, List<string> runners)
        {
            string runnersValue = runners.Count > 0 ? string.Join(", ", runners) : "-";
            return $"Configuration\r\nRuntime: {runtime}\r\nReport: {report}\r\nRunners: {runnersValue}";
        }
    }
}
