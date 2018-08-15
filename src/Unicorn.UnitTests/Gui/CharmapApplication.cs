﻿using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.PageObject;

namespace Unicorn.UnitTests.Gui
{
    public class CharmapApplication : Application
    {
        [Find(Using.Name, "Character Map")]
        public WindowCharMap Window;

        [Find(Using.Name, "asdlkjfghsdhjkfgdsfkjhfg")]
        public WindowCharMap FakeWindow;

        public CharmapApplication(string path, string exeName) : base(path, exeName)
        {
        }
    }
}
