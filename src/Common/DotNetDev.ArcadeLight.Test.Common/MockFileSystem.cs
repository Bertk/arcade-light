// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using DotNetDev.ArcadeLight.Common;

#nullable enable
namespace Microsoft.Arcade.Test.Common
{
  public class MockFileSystem : IFileSystem
  {
    #region File system state

    public HashSet<string> Directories { get; }

    public Dictionary<string, string> Files { get; }

#pragma warning disable CA1002 // Do not expose generic lists
    public List<string> RemovedFiles { get; } = new();
#pragma warning restore CA1002 // Do not expose generic lists

    #endregion

    public MockFileSystem(
        Dictionary<string, string>? files = null,
        IEnumerable<string>? directories = null)
    {
      Directories = new(directories ?? System.Array.Empty<string>());
      Files = files ?? new();
    }

    #region IFileSystem implementation

    public void CreateDirectory(string path) => Directories.Add(path);

    public bool DirectoryExists(string path) => Directories.Contains(path);

    public bool FileExists(string path) => Files.ContainsKey(path);

    public void DeleteFile(string path)
    {
      Files.Remove(path);
      RemovedFiles.Add(path);
    }

    public string? GetDirectoryName(string? path) => Path.GetDirectoryName(path);

    public string? GetFileName(string? path) => Path.GetFileName(path);

    public string? GetFileNameWithoutExtension(string? path) => Path.GetFileNameWithoutExtension(path);

    public string? GetExtension(string? path) => Path.GetExtension(path);

    public string PathCombine(string path1, string path2) => path1 + "/" + path2;

    public void WriteToFile(string path, string content) => Files[path] = content;

    public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite = false) => Files[destinationFileName] = Files[sourceFileName];

    public Stream GetFileStream(string path, FileMode mode, FileAccess access)
        => FileExists(path) ? new MemoryStream() : new MockFileStream(this, path);

    public FileAttributes GetAttributes(string path)
    {
      FileAttributes attributes = FileAttributes.Normal;

      if (Directories.Contains(path))
      {
        attributes |= FileAttributes.Directory;
      }

      return attributes;
    }

    #endregion

    /// <summary>
    /// Allows to write to a stream that will end up in the MockFileSystem.
    /// </summary>
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    private sealed class MockFileStream : MemoryStream
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
      private readonly MockFileSystem _fileSystem;
      private readonly string _path;
      private bool _disposed;

      public MockFileStream(MockFileSystem fileSystem, string path)
          : base(fileSystem.FileExists(path) ? System.Text.Encoding.UTF8.GetBytes(fileSystem.Files[path]) : new byte[2048])
      {
        _fileSystem = fileSystem;
        _path = path;
      }

#pragma warning disable CA2215 // Dispose methods should call base class dispose
      protected override void Dispose(bool disposing)
#pragma warning restore CA2215 // Dispose methods should call base class dispose
      {
        // flush file to our system
        if (!_disposed)
        {
          _disposed = true;
          using StreamReader sr = new(this);
          Seek(0, SeekOrigin.Begin);
          _fileSystem.WriteToFile(_path, sr.ReadToEnd().Replace("\0", ""));
        }
      }
    }
  }
}
