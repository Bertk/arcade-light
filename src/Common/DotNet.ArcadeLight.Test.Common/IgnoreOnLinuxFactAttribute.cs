using System.Runtime.InteropServices;
using Xunit;

namespace DotNet.ArcadeLight.Test.Common
{
  public sealed class IgnoreOnLinuxFactAttribute : FactAttribute
  {
    public void IgnoreOnLinuxFact()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) )
      {
        Skip = "Ignore on Linux platform";
      }
    }
  }
}
