using Demo.Commons;
using Demo.DesktopModule.Gui;
using System;
using System.Runtime.InteropServices;
using UIAutomationClient;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Win.Driver;
using Unicorn.UI.Win.PageObject;

namespace Demo.DesktopModule
{
    /// <summary>
    /// Describes desktop application (TestWindowsApp application).
    /// should inherit <see cref="Application"/>.
    /// </summary>
    public class TestWindowsApp : Application
    {
        /// <summary>
        /// Application constructor. Calls base constructor with path to application and application executable name.
        /// </summary>
        public TestWindowsApp() 
            : base("", TafConfig.Get.DesktopAppName) 
        { 
        }

        public TestAppWindow Window { get; private set; }

        public override void Start()
        {
            base.Start();

            IUIAutomationElement element = null;

            DefaultWait wait = new DefaultWait(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(250));
            wait.IgnoreExceptionTypes(typeof(COMException));

            wait.Until(() =>
            {
                element = WinDriver.Instance.Driver.ElementFromHandle(Process.MainWindowHandle);
                return true;
            });

            Window = new TestAppWindow(element);
        }
    }
}
