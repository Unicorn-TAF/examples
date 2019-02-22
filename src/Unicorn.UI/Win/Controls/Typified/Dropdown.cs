﻿using System.Threading;
using UIAutomationClient;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Dropdown : WinControl, IDropdown
    {
        public Dropdown()
        {
        }

        public Dropdown(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_ComboBoxControlTypeId;

        public bool Expanded => this.ExpandCollapsePattern.CurrentExpandCollapseState.Equals(ExpandCollapseState.ExpandCollapseState_Expanded);

        public string SelectedValue
        {
            get
            {
                var selection = this.SelectionPattern;
                if (selection != null)
                {
                    var items = selection.GetCurrentSelection();

                    if (items.Length > 1)
                    {
                        return items.GetElement(0).GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId) as string;
                    }
                }

                var value = this.ValuePattern;

                if (value != null)
                {
                    return value.CurrentValue;
                }

                return string.Empty;
            }
        }

        protected IUIAutomationExpandCollapsePattern ExpandCollapsePattern => this.GetPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) as IUIAutomationExpandCollapsePattern;

        protected IUIAutomationSelectionPattern SelectionPattern => this.GetPattern(UIA_PatternIds.UIA_SelectionPatternId) as IUIAutomationSelectionPattern;

        protected IUIAutomationValuePattern ValuePattern => this.GetPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;

        public bool Select(string itemName)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {this.ToString()}");

            if (itemName.Equals(this.SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (the item is selected by default)");
                return false;
            }

            var value = this.ValuePattern;

            if (value != null)
            {
                value.SetValue(itemName);
            }
            else
            {
                Expand();
                Thread.Sleep(500);
                var itemEl = Find<ListItem>(ByLocator.Name(itemName));

                if (itemEl != null)
                {
                    Logger.Instance.Log(LogLevel.Trace, "Item was found. Selecting...");
                    itemEl.Select();
                }
                    
                Collapse();
                Thread.Sleep(500);
            }

            Logger.Instance.Log(LogLevel.Trace, "Item was selected");

            return true;
        }

        public bool Expand()
        {
            Logger.Instance.Log(LogLevel.Trace, "Expanding dropdown");
            if (this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to expand (expanded by default)");
                return false;
            }

            this.ExpandCollapsePattern.Expand();
            Logger.Instance.Log(LogLevel.Trace, "Expanded");
            return true;
        }

        public bool Collapse()
        {
            Logger.Instance.Log(LogLevel.Trace, "Collapsing dropdown");
            if (!this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to collapse (collapsed by default)");
                return false;
            }

            this.ExpandCollapsePattern.Collapse();
            Logger.Instance.Log(LogLevel.Trace, "Collapsed");
            return true;
        }
    }
}
