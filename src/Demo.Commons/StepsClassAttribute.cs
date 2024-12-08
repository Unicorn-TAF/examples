using AspectInjector.Broker;
using System;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.Commons
{
    /// <summary>
    /// Using aspectinjector to implement code injections during build phase.
    /// Custom attribute which will be used to inject steps functionality into classes with steps methods.
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(StepsClassAttribute))]
    [AttributeUsage(AttributeTargets.Class)]
    public class StepsClassAttribute : Attribute
    {
        [Advice(Kind.Around, Targets = Target.Method)]
        public object HandleMethod(
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> method,
            [Argument(Source.Metadata)] MethodBase methodBase)
        {
            bool isStep = methodBase.IsDefined(typeof(StepAttribute), true);

            if (isStep)
            {
                // calling OnStepStart event before step method execution.
                TafEvents.CallOnStepStartEvent(methodBase, arguments);
            }

            // calling the step itself.
            var result = method(arguments);

            if (isStep)
            {
                //calling OnStepFinish event after step method execution.
                TafEvents.CallOnStepFinishEvent(methodBase, arguments);
            }

            return result;
        }
    }
}
