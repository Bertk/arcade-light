// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace DotNetDev.ArcadeLight.Test.Common
{
  /// <summary>
  /// Assert style type to deal with the lack of features in xUnit's Assert type
  /// </summary>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
  public static class AssertEx
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
  {
    #region AssertEqualityComparer<T>

    private sealed class AssertEqualityComparer<T> : IEqualityComparer<T>
    {
      private static readonly IEqualityComparer<T> s_instance = new AssertEqualityComparer<T>();

      private static bool CanBeNull()
      {
        Type type = typeof(T);
        return !type.GetTypeInfo().IsValueType ||
            (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
      }

      public static bool Equals(T left, T right)
      {
        return s_instance.Equals(left, right);
      }

      bool IEqualityComparer<T>.Equals(T x, T y)
      {
        if (CanBeNull())
        {
          if (object.Equals(x, default(T)))
          {
            return object.Equals(y, default(T));
          }

          if (object.Equals(y, default(T)))
          {
            return false;
          }
        }

        if (x.GetType() != y.GetType())
        {
          return false;
        }

        IEquatable<T> equatable = x as IEquatable<T>;
        if (equatable != null)
        {
          return equatable.Equals(y);
        }

        IComparable<T> comparableT = x as IComparable<T>;
        if (comparableT != null)
        {
          return comparableT.CompareTo(y) == 0;
        }

        IComparable comparable = x as IComparable;
        if (comparable != null)
        {
          return comparable.CompareTo(y) == 0;
        }

        IEnumerable enumerableX = x as IEnumerable;
        IEnumerable enumerableY = y as IEnumerable;

        if (enumerableX != null && enumerableY != null)
        {
          IEnumerator enumeratorX = enumerableX.GetEnumerator();
          IEnumerator enumeratorY = enumerableY.GetEnumerator();

          while (true)
          {
            bool hasNextX = enumeratorX.MoveNext();
            bool hasNextY = enumeratorY.MoveNext();

            if (!hasNextX || !hasNextY)
            {
              return hasNextX == hasNextY;
            }

            if (!Equals(enumeratorX.Current, enumeratorY.Current))
            {
              return false;
            }
          }
        }

        return object.Equals(x, y);
      }

      int IEqualityComparer<T>.GetHashCode(T obj)
      {
        throw new NotImplementedException();
      }
    }

    #endregion

    public static void Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual, Func<T, T, bool> comparer = null, string message = null,
        string itemSeparator = null, Func<T, string> itemInspector = null)
    {
      if (ReferenceEquals(expected, actual))
      {
        return;
      }

      if (expected == null)
      {
        Fail("expected was null, but actual wasn't\r\n" + message);
      }
      else if (actual == null)
      {
        Fail("actual was null, but expected wasn't\r\n" + message);
      }
      else if (!SequenceEqual(expected, actual, comparer))
      {
        string assertMessage = GetAssertMessage(expected, actual, comparer, itemInspector, itemSeparator);

        if (message != null)
        {
          assertMessage = message + "\r\n" + assertMessage;
        }

        Assert.Fail(assertMessage);
      }
    }

    private static bool SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, Func<T, T, bool> comparer = null)
    {
      IEnumerator<T> enumerator1 = expected.GetEnumerator();
      IEnumerator<T> enumerator2 = actual.GetEnumerator();

      while (true)
      {
        bool hasNext1 = enumerator1.MoveNext();
        bool hasNext2 = enumerator2.MoveNext();

        if (hasNext1 != hasNext2)
        {
          return false;
        }

        if (!hasNext1)
        {
          break;
        }

        T value1 = enumerator1.Current;
        T value2 = enumerator2.Current;

        if (!(comparer != null ? comparer(value1, value2) : AssertEqualityComparer<T>.Equals(value1, value2)))
        {
          return false;
        }
      }

      return true;
    }

    public static void Fail(string message)
    {
      Assert.Fail(message);
    }

    // compares against a baseline
    public static void AssertEqualToleratingWhitespaceDifferences(
        string expected,
        string actual,
        bool escapeQuotes = true,
        [CallerFilePath] string expectedValueSourcePath = null,
        [CallerLineNumber] int expectedValueSourceLine = 0)
    {
      if (!EqualIgnoringWhitespace(expected, actual))
      {
        Assert.Fail(GetAssertMessage(expected, actual, escapeQuotes, expectedValueSourcePath, expectedValueSourceLine));
      }
    }

    public static bool EqualIgnoringWhitespace(string left, string right)
        => NormalizeWhitespace(left) == NormalizeWhitespace(right);

#pragma warning disable CA1062 // Validate arguments of public methods

    public static string NormalizeWhitespace(string input)
    {
      StringBuilder output = new();
      ArgumentNullException.ThrowIfNull(input);
      string[] inputLines = input.Split('\n', '\r');
      foreach (string line in inputLines)
      {
        string trimmedLine = line.Trim();
        if (trimmedLine.Length > 0)
        {
          if (!(trimmedLine[0] == '{' || trimmedLine[0] == '}'))
          {
            output.Append("  ");
          }

          output.AppendLine(trimmedLine);
        }
      }

      return output.ToString();
    }

    public static string GetAssertMessage(string expected, string actual, bool escapeQuotes = false, string expectedValueSourcePath = null, int expectedValueSourceLine = 0)
    {
      return GetAssertMessage(DiffUtil.Lines(expected), DiffUtil.Lines(actual), escapeQuotes, expectedValueSourcePath, expectedValueSourceLine);
    }

    public static string GetAssertMessage<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool escapeQuotes, string expectedValueSourcePath = null, int expectedValueSourceLine = 0)
    {
      Func<T, string> itemInspector = escapeQuotes ? new Func<T, string>(t => t.ToString().Replace("\"", "\"\"")) : null;
      return GetAssertMessage(expected, actual, itemInspector: itemInspector, itemSeparator: "\r\n", expectedValueSourcePath: expectedValueSourcePath, expectedValueSourceLine: expectedValueSourceLine);
    }

    public static string GetAssertMessage<T>(
        IEnumerable<T> expected,
        IEnumerable<T> actual,
        Func<T, T, bool> comparer = null,
        Func<T, string> itemInspector = null,
        string itemSeparator = null,
        string expectedValueSourcePath = null,
        int expectedValueSourceLine = 0)
    {
      if (itemInspector == null)
      {
        if (expected is IEnumerable<byte>)
        {
          itemInspector = b => $"0x{b:X2}";
        }
        else
        {
          itemInspector = new Func<T, string>(obj => (obj != null) ? obj.ToString() : "<null>");
        }
      }

      if (itemSeparator == null)
      {
        if (expected is IEnumerable<byte>)
        {
          itemSeparator = ", ";
        }
        else
        {
          itemSeparator = ",\r\n";
        }
      }

      StringBuilder message = new();
      message.AppendLine();
      message.AppendLine("Actual:");
      message.AppendLine(string.Join(itemSeparator, actual.Select(itemInspector)));

      message.AppendLine();
      message.AppendLine("Expected:");
      message.AppendLine(string.Join(itemSeparator, expected.Select(itemInspector)));

      message.AppendLine();
      message.AppendLine("Diff:");
      message.Append(DiffUtil.DiffReport(expected, actual, comparer, itemInspector, itemSeparator));

      return message.ToString();
    }
  }
}
