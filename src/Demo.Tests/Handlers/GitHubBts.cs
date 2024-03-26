using System;
using System.Reflection;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Demo.Tests.Handlers
{
    internal class GitHubBts : IDisposable
    {
        /// <summary>
        /// Initialize github issues handler (suppose our example product issues are stored there).
        /// Consider scenario, that one wants to attach link to defect in case test is failed and linked defect is opened.
        /// </summary>
        public GitHubBts() 
        {
            // Subscribe to test fail events
            Test.OnTestFail += HandleOpenIssues;
        }

        /// <summary>
        /// Issues handler method. In example it operates by <see cref="BugAttribute"/>.
        /// The method attaches a defect to test outcome if test has the attribute and the issue by provided ID is opened.
        /// </summary>
        /// <param name="test"></param>
        public void HandleOpenIssues(Test test)
        {
            BugAttribute bugAttribute = test.TestMethod.GetCustomAttribute<BugAttribute>();

            if (bugAttribute != null)
            {
                // check if the bug falls into consideration conditions
                bool validBug = test.Outcome.FailMessage.Contains(bugAttribute.OnError);

                // also one can check if the bug is not closed in BTS.

                // if the bug meets all conditions, adding it to test outcome
                if (validBug)
                {
                    // using Defect object here, need to specify mandatory ID and defect type
                    // (for example ReportPortal considers type field in defects categorization),
                    // also some comment could be assigned.
                    // Here is used "AB" type which is default type for automation bugs in ReportPortal
                    test.Outcome.Defect = new Defect(bugAttribute.Bug, "ab001", "The fail is expected");
                }
            }
        }

        /// <summary>
        /// Unsubscribe from test events.
        /// </summary>
        public void Dispose()
        {
            Test.OnTestFail -= HandleOpenIssues;
        }
    }
}
