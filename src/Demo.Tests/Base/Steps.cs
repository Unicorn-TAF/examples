using Demo.AndroidDialer.Steps;
using Demo.DesktopModule.Steps;
using Demo.DummyRestApi;
using Demo.WebModule.Steps;
using System;
using Unicorn.Taf.Core.Steps;

namespace Demo.Tests.Base
{
    public class Steps
    {
        private readonly Lazy<AssertionSteps> _assertion = new Lazy<AssertionSteps>();
        private readonly Lazy<DummyRestApiSteps> _dummyRestApi = new Lazy<DummyRestApiSteps>();
        private readonly Lazy<StepsTestWindowsApp> _windowsApp = new Lazy<StepsTestWindowsApp>();
        private readonly Lazy<TestWebsiteSteps> _website = new Lazy<TestWebsiteSteps>();
        private readonly Lazy<StepsAndroidDialer> _android = new Lazy<StepsAndroidDialer>();

        public AssertionSteps Assertion => _assertion.Value;

        public DummyRestApiSteps DummyRestApi => _dummyRestApi.Value;

        public StepsTestWindowsApp DesktopApp => _windowsApp.Value;

        public TestWebsiteSteps Website => _website.Value;

        public StepsAndroidDialer Android => _android.Value;
    }
}
