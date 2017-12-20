﻿using Unicorn.Core.Testing.Assertions.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeContainsMatcher : Matcher
    {
        private string attribute, value;

        public AttributeContainsMatcher(string attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }

        public override string CheckDescription => $"Attribute '{this.attribute}' contains '{this.value}'";

        public override bool Matches(object obj)
        {
            return IsNotNull(obj) && Assertion(obj);
        }

        protected bool Assertion(object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(IControl)))
            {
                DescribeMismatch("not an instance of IControl");
                return false;
            }

            string actualValue = (obj as IControl).GetAttribute(this.attribute);
            bool contains = actualValue.Contains(this.value);

            if (contains != this.Reverse)
            {
                DescribeMismatch(actualValue);
            }
                
            return contains;
        }
    }
}
