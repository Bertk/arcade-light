// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.Build.Framework;
using Tasks = System.Threading.Tasks;

namespace DotNetDev.ArcadeLight.Sdk
{
  public class DownloadFile : Microsoft.Build.Utilities.Task, ICancelableTask, IDisposable
  {
    /// <summary>
    /// List of URls to attempt download from. Accepted metadata are:
    ///     - Token: Base64 encoded token to be appended to base URL for accessing private locations.
    /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
    public ITaskItem[] Uris { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

#pragma warning disable CA1056 // URI-like properties should not be strings
    public string Uri { get; set; }
#pragma warning restore CA1056 // URI-like properties should not be strings

    [Required]
    public string DestinationPath { get; set; }

    public bool Overwrite { get; set; }

    /// <summary>
    /// Delay between any necessary retries.
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 1000;

    public int Retries { get; set; } = 3;

    public int TimeoutInSeconds { get; set; } = 100;

    private readonly CancellationTokenSource _cancellationSource = new();

    public void Cancel() => _cancellationSource.Cancel();

    private readonly string FileUriProtocol = "file://";


    public override bool Execute()
    {
      if (Retries < 0)
      {
        Log.LogError($"Invalid task parameter value: Retries={Retries}");
        return false;
      }

      if (RetryDelayMilliseconds < 0)
      {
        Log.LogError($"Invalid task parameter value: RetryDelayMilliseconds={RetryDelayMilliseconds}");
        return false;
      }

      if (File.Exists(DestinationPath) && !Overwrite)
      {
        return true;
      }

      if (string.IsNullOrWhiteSpace(Uri) && (Uris == null || Uris.Length == 0))
      {
        Log.LogError($"Invalid task parameter value: {nameof(Uri)} and {nameof(Uris)} are empty.");
        return false;
      }

      Directory.CreateDirectory(Path.GetDirectoryName(DestinationPath));

      if (!string.IsNullOrWhiteSpace(Uri))
      {
#pragma warning disable S4462 // Replace this use of 'Task.Result' with 'await'
        return DownloadFromUriAsync(Uri).Result;
#pragma warning restore S4462 // Replace this use of 'Task.Result' with 'await'
      }

      if (Uris != null)
      {
        foreach (ITaskItem uriConfig in Uris)
        {
          string uri = uriConfig.ItemSpec;
          string encodedToken = uriConfig.GetMetadata("token");

          if (!string.IsNullOrWhiteSpace(encodedToken))
          {
            byte[] encodedTokenBytes = System.Convert.FromBase64String(encodedToken);
            string decodedToken = System.Text.Encoding.UTF8.GetString(encodedTokenBytes);
            uri = $"{uri}{decodedToken}";
          }

#pragma warning disable S4462 // Replace this use of 'Task.Result' with 'await'
          if (DownloadFromUriAsync(uri).Result)
#pragma warning restore S4462 // Replace this use of 'Task.Result' with 'await'
          {
            return true;
          }
        }

        Log.LogError($"Download from all targets failed. List of attempted targets: {string.Join(", ", Uris.Select(m => m.ItemSpec))}");
      }

      Log.LogError($"Failed to download file using addresses in {nameof(Uri)} and/or {nameof(Uris)}.");

      return false;
    }

    private async Tasks.Task<bool> DownloadFromUriAsync(string uri)
    {
      if (uri.StartsWith(FileUriProtocol, StringComparison.Ordinal))
      {
        string filePath = uri.Substring(FileUriProtocol.Length);
        Log.LogMessage($"Copying '{filePath}' to '{DestinationPath}'");
        File.Copy(filePath, DestinationPath, overwrite: true);
        return true;
      }

      Log.LogMessage($"Downloading '{uri}' to '{DestinationPath}'");

      using HttpClientHandler handler = new() { CheckCertificateRevocationList = true };
      using HttpClient httpClient = new(handler);
      httpClient.Timeout = TimeSpan.FromSeconds(TimeoutInSeconds);
      try
      {
        return await DownloadWithRetriesAsync(httpClient, uri).ConfigureAwait(false);
      }
      catch (AggregateException e)
      {
        if (e.InnerException is OperationCanceledException)
        {
          Log.LogMessage($"Download of '{uri}' to '{DestinationPath}' has been cancelled.");
          return false;
        }

        throw e.InnerException;
      }
    }

    private async Tasks.Task<bool> DownloadWithRetriesAsync(HttpClient httpClient, string uri)
    {
      int attempt = 0;

      while (true)
      {
#pragma warning disable S1067 // Expressions should not be too complex
        try
        {
#pragma warning disable S4005 // "System.Uri" arguments should be used instead of strings
#pragma warning disable CA2234 // Pass system uri objects instead of strings
          HttpResponseMessage httpResponse = await httpClient.GetAsync(uri, _cancellationSource.Token).ConfigureAwait(false);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
#pragma warning restore S4005 // "System.Uri" arguments should be used instead of strings

          // The Azure Storage REST API returns '400 - Bad Request' in some cases
          // where the resource is not found on the storage.
          // https://docs.microsoft.com/en-us/rest/api/storageservices/common-rest-api-error-codes
          if (httpResponse.StatusCode == HttpStatusCode.NotFound ||
              httpResponse.ReasonPhrase.StartsWith("The requested URI does not represent any resource on the server.", StringComparison.OrdinalIgnoreCase))
          {
            Log.LogMessage($"Problems downloading file from '{uri}'. Does the resource exist on the storage? {httpResponse.StatusCode} : {httpResponse.ReasonPhrase}");
            return false;
          }

          httpResponse.EnsureSuccessStatusCode();

          using FileStream outStream = File.Create(DestinationPath);
          await httpResponse.Content.CopyToAsync(outStream).ConfigureAwait(false);

          return true;
        }
        // Retry cases:
        // 1. Plain Http error
        // 2. IOExceptions that aren't definitely deterministic (such as antivirus was scanning the file)
        // 3. HttpClient Timeouts - these surface as TaskCanceledExceptions that don't match our cancellation token source
        catch (Exception e) when (e is HttpRequestException ||
                                  e is IOException && !(e is DirectoryNotFoundException || e is PathTooLongException) ||
                                  e is Tasks.TaskCanceledException && ((Tasks.TaskCanceledException)e).CancellationToken != _cancellationSource.Token)
        {
          attempt++;

          if (attempt > Retries)
          {
            Log.LogMessage($"Failed to download '{uri}' to '{DestinationPath}': {e.Message}");
            return false;
          }

          Log.LogMessage($"Retrying download of '{uri}' to '{DestinationPath}' due to failure: '{e.Message}' ({attempt}/{Retries})");

          await Tasks.Task.Delay(RetryDelayMilliseconds).ConfigureAwait(false);
          continue;
        }
#pragma warning restore S1067 // Expressions should not be too complex
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      // Cleanup
      _cancellationSource?.Dispose();

    }
  }
}
