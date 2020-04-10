using DslTools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DslTools.Tests.StockObjects.Verifiers
{
    public enum Options { None, IgnorePositions }

    public class TokenExpectation<T>
    {
        public TokenExpectation(Options options = Options.None, string messagePrefix = null)
        {
            MessagePrefix = messagePrefix;
            Options = options;
        }

        public string MessagePrefix { get; }
        public Options Options { get; }

        private readonly List<TokenValue<T>> expected = new List<TokenValue<T>>();
        private int position = 0;

        public TokenExpectation<T> Expect(T id)
        {
            return Expect(id, id.ToString());
        }

        public TokenExpectation<T> Expect(T id, string value)
        {
            return Expect(id, value, value);
        }

        public TokenExpectation<T> Expect(T id, string original, string value)
        {
            expected.Add(new TokenValue<T>(id, original, value, position, original.Length));
            position += original.Length;

            return this;
        }

        public TokenExpectation<T> Expect(Action<TokenFactory<T>> configure)
        {
            var factory = new TokenFactory<T>(position);
            configure(factory);
            position = factory.NextTokenStart;
            expected.Add(factory.Create());

            return this;
        }

        public void Verify(IEnumerable<TokenValue<T>> actual)
        {
            var combined = expected.ZipAll(actual, Tuple.Create).WithIndex(0).ToList();

            foreach (var compare in combined)
            {
                var x = compare.Value.Item1;
                var y = compare.Value.Item2;

                if (x == null)
                {
                    var missing = GetDiff(combined.Select(p => p.Value), compare.Key, t => t.Item2);
                    Assert.Fail($"{MessagePrefix} Actual: additional unexpected tokens starting at index {compare.Key}: {missing}");
                }
                if (y == null)
                {
                    var missing = GetDiff(combined.Select(p => p.Value), compare.Key, t => t.Item1);
                    Assert.Fail($"{MessagePrefix} Expected tokens starting at index {compare.Key}: {missing}");
                }

                var sameValue = x.Id.Equals(y.Id) && x.Original == y.Original && x.Value == y.Value;

                Assert.IsTrue(sameValue, $"Expected: <{x.Id}: {Escape(x.Value)} ({Escape(x.Original)})>" +
                    $" Actual: <{y.Id}: {Escape(y.Value)} ({Escape(y.Original)})> at index {compare.Key}");

                //Assert.AreEqual(x.Value, y.Value, $"{MessagePrefix} {nameof(x.Value)} differs at index {compare.Key}");
                //Assert.AreEqual(x.Id, y.Id, $"{MessagePrefix} {nameof(x.Id)} differs at index {compare.Key}");

                if (Options == Options.IgnorePositions) continue;
                var sameLocation = x.Start == y.Start && x.Length == y.Length;

                Assert.IsTrue(sameLocation, $"Expected: <{x.Id} of {x.Length} characters at position {x.Start}>" +
                    $" Actual: <{y.Id} of {y.Length} characters at position {y.Start}>");

                //Assert.AreEqual(x.Start, y.Start, $"{MessagePrefix} {nameof(x.Start)} differs at index {compare.Key}");
                //Assert.AreEqual(x.Length, y.Length, $"{MessagePrefix} {nameof(x.Length)} differs at index {compare.Key}");
            }
        }
        private string Escape(string value)
        {
            if (value == null) return "<null>";
            if (value.Length == 0) return "<empty>";

            return value.Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }

        private string GetDiff(IEnumerable<Tuple<TokenValue<T>, TokenValue<T>>> combined, int starting,
            Func<Tuple<TokenValue<T>, TokenValue<T>>, TokenValue<T>> selector)
        {
            return string.Join(", ", combined.Skip(starting)
                .Select(x => $"<Token: {selector(x).Id}, \"{Escape(selector(x).Value)}\">"));

        }

    }

}
