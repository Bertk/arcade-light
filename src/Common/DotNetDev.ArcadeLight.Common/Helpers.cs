using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDev.ArcadeLight.Common
{
  public class Helpers : IHelpers
  {
    public string RemoveTrailingSlash([NotNull] string directoryPath)
    {
      return directoryPath.TrimEnd('/', '\\');
    }

    public string ComputeSha256Hash(string normalizedPath)
    {
      string dirHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(normalizedPath)));
      dirHash = dirHash.TrimEnd('='); // base64 url encode it.
      dirHash = dirHash.Replace('+', '-');
      dirHash = dirHash.Replace('/', '_');
      return dirHash;
    }

    public T MutexExec<T>(Func<T> func, string mutexName)
    {
      using Mutex mutex = new(false, mutexName);
      bool hasMutex = false;
      if (func == null)
      {
        throw new ArgumentNullException(nameof(func));
      }

      try
      {
        try
        {
          mutex.WaitOne();
        }
        catch (AbandonedMutexException)
        {
          // ignore exception
        }

        hasMutex = true;
        return func();
      }
      finally
      {
        if (hasMutex)
        {
          mutex.ReleaseMutex();
        }
      }
    }

#pragma warning disable S4462 // Calls to "async" methods should not be blocking
    public T MutexExec<T>(Func<Task<T>> func, string mutexName) =>
            MutexExec(() => func().GetAwaiter().GetResult(), mutexName); // Can't await because of mutex


    // This overload is here so that async Actions don't get routed to MutexExec<T>(Func<T> function, string path)
    public void MutexExec(Func<Task> func, string mutexName) =>
            MutexExec(() => { func().GetAwaiter().GetResult(); return true; }, mutexName);

#pragma warning restore S4462 // Calls to "async" methods should not be blocking

    public T DirectoryMutexExec<T>(Func<T> func, string path) =>
            MutexExec(func, $"Global\\{ComputeSha256Hash(path)}");

#pragma warning disable S4462 // Calls to "async" methods should not be blocking
    public T DirectoryMutexExec<T>(Func<Task<T>> func, string path) =>
        DirectoryMutexExec(() => func().GetAwaiter().GetResult(), path); // Can't await because of mutex

    // This overload is here so that async Actions don't get routed to DirectoryMutexExec<T>(Func<T> function, string path)
    public void DirectoryMutexExec(Func<Task> func, string path) =>
        DirectoryMutexExec(() => { func().GetAwaiter().GetResult(); return true; }, path);
#pragma warning restore S4462 // Calls to "async" methods should not be blocking
  }

  public static class KeyValuePairExtensions
  {
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value) { key = pair.Key; value = pair.Value; }
  }
}
