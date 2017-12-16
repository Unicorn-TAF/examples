﻿using Unicorn.Core.Testing.Assertions.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeIsEqualToMatcher : Matcher
    {
        private string attribute, value;

        public AttributeIsEqualToMatcher(string attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }

        public override string CheckDescription => $"Attribute '{this.attribute}' is equal to '{this.value}'";

        public override bool Matches(object obj)
        {
            return this.IsNotNull(obj) && this.Assertion(obj);
        }

        protected bool Assertion(object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(IControl)))
            {
                MatcherOutput.Append($"was not an instance of IControl");
                return false;
            }

            IControl element = (IControl)obj;
            string actualValue = element.GetAttribute(this.attribute);

            bool equals = actualValue.Equals(this.value);

            if (!equals)
            {
                this.MatcherOutput.Append("was ").Append(actualValue);
            }

            return equals;
        }
    }
}
