using System;

namespace DotNet.XUnitExtensions
{
    internal class ConditionalDiscovererException : Exception
    {
        public ConditionalDiscovererException(string message) : base(message) { }
    }
}
