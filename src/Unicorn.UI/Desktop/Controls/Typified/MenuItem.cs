﻿using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class MenuItem : GuiControl
    {
        public MenuItem()
        {
        }

        public MenuItem(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.MenuItem;

        public void Select()
        {
            object pattern;

            if (Instance.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out pattern))
            {
                ((ExpandCollapsePattern)pattern).Expand();
            }
            else if (Instance.TryGetCurrentPattern(TogglePattern.Pattern, out pattern))
            {
                ((TogglePattern)pattern).Toggle();
            }
            else
            {
                ((InvokePattern)Instance.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
            }
        }
    }
}