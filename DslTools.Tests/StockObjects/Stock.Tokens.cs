using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DslTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTools.Tests.StockObjects
{
    /// <summary>
    /// Provides a set of objects and factories intended to
    /// make writing and reading unit tests easier
    /// </summary>
    public static partial class Stock
    {
        /// <summary>
        /// Provides factory methods for generating RegEx capture Groups
        /// </summary>
        public static class Tokens
        {
            public enum Numbers { DoNotUse, Four, Three, Two, One, Unknown, EOF }

            public static Dictionary<Numbers, string> OneTwoThree = new Dictionary<Numbers, string>()
            {
                [Numbers.One] = "One",
                [Numbers.Two] = "Two",
                [Numbers.Three] = "Three"
            };

            /// <summary>
            /// Returns a capture group for the entire value
            /// </summary>
            /// <param name="value">the value to capture</param>
            /// <returns></returns>
            public static Group CaptureAll(string value)
            {
                return Capture(value, $"({value})");
            }

            /// <summary>
            /// Returns a capture group for using a specified pattern
            /// </summary>
            /// <param name="value">the value to capture</param>
            /// <param name="index">the index where the the value will be captured</param>
            /// <returns></returns>
            public static Group CaptureAfter(string value, int index)
            {
                return Capture($"{new string(' ',index)}{value}", $@"\w*({value})");

            }

            private static Group Capture(string value, string pattern)
            {
                return Regex.Match(value, pattern).Groups[1];
            }

            public static IEnumerable<TokenValue<T>> Stream<T>(params T[] items)
            {
                var f = new TokenFactory<T>();
                return items.Select(t => f.Id(t).Create());
            }

            public static IEnumerable<TokenValue<T>> Stream<T>(params Action<TokenFactory<T>>[] items)
            {
                var f = new TokenFactory<T>();
                return items.Select(a =>
                {
                    a(f);
                    var r = f.Create()
                    ; f.Reset();
                    return r;
                });
            }
        }
    }
}
