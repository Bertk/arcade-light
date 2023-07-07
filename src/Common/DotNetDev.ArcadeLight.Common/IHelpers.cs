using System;
using System.Threading.Tasks;

namespace DotNetDev.ArcadeLight.Common
{
  public interface IHelpers
  {
    string ComputeSha256Hash(string normalizedPath);

    T MutexExec<T>(Func<T> func, string mutexName);
    T MutexExec<T>(Func<Task<T>> func, string mutexName);
    void MutexExec(Func<Task> func, string mutexName);

    T DirectoryMutexExec<T>(Func<T> func, string path);
    T DirectoryMutexExec<T>(Func<Task<T>> func, string path);
    void DirectoryMutexExec(Func<Task> func, string path);

    string RemoveTrailingSlash(string directoryPath);
  }
}
