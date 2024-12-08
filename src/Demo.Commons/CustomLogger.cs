using System;
using System.IO;
using System.Reflection;
using Unicorn.Taf.Api;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Steps;

namespace Demo.Commons
{
    /// <summary>
    /// Custom implementation of logger which will be assigned to <see cref="Logger.Instance"/>
    /// </summary>
    public sealed class CustomLogger : ILogger
    {
        private readonly string _logFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLogger"/> class.
        /// Logger writes all messages into text file.
        /// </summary>
        public CustomLogger()
        {
            // Subscribe to step start event to log step.
            TafEvents.OnStepStart += ReportStepInfo;

            var logsDirectory = Path.Combine(TafConfig.Get.TestsDir, "Logs");

            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            _logFile = Path.Combine(logsDirectory, "tests-execution.log");
        }

        public void Debug(string message, params object[] parameters) =>
            Log("  [Debug]:   ", message, parameters);

        public void Error(string message, params object[] parameters) =>
            Log("  [Error]: ", message, parameters);

        public void Info(string message, params object[] parameters) =>
            Log("   [Info]: ", message, parameters);

        public void Trace(string message, params object[] parameters) =>
            Log("  [Trace]:     ", message, parameters);

        public void Warn(string message, params object[] parameters) =>
            Log("[Warning]: ", message, parameters);

        private void Log(string prefix, string message, object[] parameters)
        {
            var logString = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss.ff} {prefix}{string.Format(message, parameters)}";

#if DEBUG
            System.Diagnostics.Debug.WriteLine(prefix + message);
#else
            WriteToFile(logString);
#endif
        }

        private void ReportStepInfo(MethodBase method, object[] arguments)
        {
            var info = StepsUtilities.GetStepInfo(method, arguments);
            Info("STEP: " + info);
        }

        /// <summary>
        /// Log info to the file
        /// </summary>
        /// <param name="text">text to log</param>
        private void WriteToFile(string text)
        {
            try
            {
                using (var file = new StreamWriter(_logFile, true))
                {
                    file.WriteLine(text);
                }
            }
            catch
            {
                // Just skip, if unable to write to file.
            }
        }
    }
}
