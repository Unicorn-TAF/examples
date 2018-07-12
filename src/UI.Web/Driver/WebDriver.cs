﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Web.Driver
{
    public class WebDriver : WebSearchContext, IDriver
    {
        private static bool needInit = false;
        private static DriverOptions options = null;

        private static WebDriver instance = null;

        private WebDriver(bool maximize = true)
        {
            Driver = options == null ? GetInstance() : GetInstance(options);

            if (maximize)
            {
                Driver.Manage().Window.Maximize();
            }

            this.ImplicitlyWait = this.TimeoutDefault;
        }

        public static WebDriver Instance
        {
            get
            {
                if (instance == null || needInit)
                {
                    instance = new WebDriver();
                    instance.SearchContext = Driver;
                    needInit = false;
                    Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, $"{Browser} WebDriver initialized");
                }

                return instance;
            }
        }

        public static IWebDriver Driver { get; set; }

        public static Browser Browser { get; set; } = Browser.Chrome;

        public string Url => Driver.Url;

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return WebSearchContext.ImplicitlyWaitTimeout;
            }

            set
            {
                Driver.Manage().Timeouts().ImplicitWait = value;
                WebSearchContext.ImplicitlyWaitTimeout = value;
            }
        }

        public static void Init(Browser browser, DriverOptions driverPptions = null)
        {
            needInit = true;
            Browser = browser;
            options = driverPptions;
        }

        public void Get(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public object ExecuteJS(string script, params object[] parameters)
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            return js.ExecuteScript(script, parameters);
        }

        public static void Close()
        {
            if (instance != null)
            {
                Driver.Quit();
                instance = null;

                options = null;
            }
        }

        private IWebDriver GetInstance()
        {
            switch (Browser)
            {
                case Browser.Chrome:
                    return new ChromeDriver();
                case Browser.IE:
                    return new InternetExplorerDriver();
                case Browser.Firefox:
                    return new FirefoxDriver();
                default:
                    return null;
            }
        }

        private IWebDriver GetInstance(DriverOptions options)
        {
            switch (Browser)
            {
                case Browser.Chrome:
                    return new ChromeDriver((ChromeOptions)options);
                case Browser.IE:
                    return new InternetExplorerDriver((InternetExplorerOptions)options);
                case Browser.Firefox:
                    return new FirefoxDriver((FirefoxOptions)options);
                default:
                    return null;
            }
        }
    }
}
