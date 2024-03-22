using System;

namespace Demo.WebModule
{
    /// <summary>
    /// Config with website timeouts.
    /// </summary>
    internal static class Timeouts
    {
        /// <summary>
        /// Gets timeout for pages load.
        /// </summary>
        internal static TimeSpan PageLoadTimeout { get; } = TimeSpan.FromSeconds(10);
    }
}
