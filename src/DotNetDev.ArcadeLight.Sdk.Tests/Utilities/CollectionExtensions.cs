// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetDev.ArcadeLight.Sdk.Tests.Utilities
{
  public static class CollectionExtensions
  {
    public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
    {
      ArgumentNullException.ThrowIfNull(items);
      foreach (T item in items)
      {
        collection?.Add(item);
      }
      return collection;
    }
  }
}
