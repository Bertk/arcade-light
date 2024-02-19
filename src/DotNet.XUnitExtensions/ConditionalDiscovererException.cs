using System;

namespace DotNet.XUnitExtensions
{
#pragma warning disable CA1064 // Exceptions should be public
  internal sealed class ConditionalDiscovererException : Exception
#pragma warning restore CA1064 // Exceptions should be public
  {
    public ConditionalDiscovererException(string message) : base(message) { }

    public ConditionalDiscovererException()
    {
    }

    public ConditionalDiscovererException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}
