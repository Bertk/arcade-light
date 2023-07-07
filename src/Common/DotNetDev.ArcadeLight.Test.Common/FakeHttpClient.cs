// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDev.ArcadeLight.Test.Common
{
  public static class FakeHttpClient
  {
    public static HttpClient WithResponses(params HttpResponseMessage[] responses)
    {
      if (responses == null || responses.Length == 0)
      {
        throw new ArgumentNullException(nameof(responses));
      }
#pragma warning disable CA2000 // Dispose objects before losing scope
      return new(new FakeHttpMessageHandler(responses)); // lgtm [cs/httpclient-checkcertrevlist-disabled] This is used for unit tests
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
      private readonly IEnumerator<HttpResponseMessage> _responseEnumerator;

      public FakeHttpMessageHandler(IEnumerable<HttpResponseMessage> responses)
      {
        _responseEnumerator = responses.GetEnumerator();
      }

      protected override void Dispose(bool disposing)
      {
        if (disposing)
        {
          _responseEnumerator?.Dispose();
        }
        base.Dispose(disposing);
      }

      protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
      {
        if (!_responseEnumerator.MoveNext())
          throw new InvalidOperationException($"Unexpected end of response sequence. Number of predefined responses should be at least equal to number of requests invoked by HttpClient.");

        return Task.FromResult(_responseEnumerator.Current);
      }
    }
  }
}
