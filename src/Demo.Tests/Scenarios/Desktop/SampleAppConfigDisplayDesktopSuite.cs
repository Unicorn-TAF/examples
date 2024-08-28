using Demo.Charmap.Gui;
using Demo.Tests.BO;
using Demo.Tests.Metadata;
using Demo.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Core.Matchers;

namespace Demo.Tests.Scenarios.Desktop
{
    [Suite("Sample app configurations display")]
    [Tag(Platforms.Win), Tag(Apps.Samples), Tag(Features.ConfigSetup)]
    [Metadata("Description", "Example of test suite with dependent tests where main test is failing")]
    [Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
    public class SampleAppConfigDisplayDesktopSuite : BaseDesktopSuite
    {
        private SamplesView Samples => application.Window.SamplesView;

        /// <summary>
        /// Actions executed before whole tests in current suite.
        /// </summary>
        [BeforeSuite]
        public override void ClassInit()
        {
            base.ClassInit();

            User user = UsersFactory.GetDefaultUser();
            Do.DesktopApp.SwitchApp();
            Do.DesktopApp.Samples.InputEmail(user.Email);
            Do.DesktopApp.Samples.InputPassword(user.Password);
            Do.DesktopApp.Samples.SignIn();
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

        [Author(Authors.JBloggs)]
        [Test("Show configuration functionality")]
        [TestData(nameof(ConfigurationsData))]
        public void CheckShowConfigurationFunctionality(string runtime, string report, string[] runners)
        {
            List<string> runnersToDisplay = new List<string>();

            HandleRunner(runners, ConfigData.Runners.ConsoleRunner, runnersToDisplay, "Console");
            HandleRunner(runners, ConfigData.Runners.TestAdapter, runnersToDisplay, "Adapter");

            Do.DesktopApp.Samples.SelectReport(report);
            Do.DesktopApp.Samples.SelectRuntime(runtime);
            Do.DesktopApp.Samples.ShowConfiguration();

            string message = GenerateMessageFrom(runtime, report, runnersToDisplay);

            Do.Assertion.AssertThat(application.Window.Modal, UI.Control.Visible());
            Do.Assertion.AssertThat(application.Window.Modal.TextContent, UI.Control.HasText(message));
        }

        private void HandleRunner(string[] runners, string runner, List<string> runnersToDisplay, string displayName)
        {
            if (runners.Contains(runner))
            {
                Do.DesktopApp.Samples.SelectRunner(runner);
                runnersToDisplay.Add(displayName);
            }
            else
            {
                Do.DesktopApp.Samples.DeselectRunner(runner);
            }
        }

        [AfterTest]
        public void CloseModal()
        {
            Do.DesktopApp.CloseDialogIfAny();
        }

        private static string GenerateMessageFrom(string runtime, string report, List<string> runners)
        {
            string runnersValue = runners.Count > 0 ? string.Join(", ", runners) : "-";
            return $"Configuration\nRuntime: {runtime}\nReport: {report}\nRunners: {runnersValue}";
        }
    }
}
