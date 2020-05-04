﻿using System;
using System.IO;

namespace Demo.Specifics.Environment
{
    public class Config
    {
        private static Config _instance = null;

        protected Config()
        {
        }

        public static Config Instance => _instance ?? (_instance = new Config());

        public string TestsDir { get; } =  Path.GetDirectoryName(new Uri(typeof(Config).Assembly.CodeBase).LocalPath);
    }
}
