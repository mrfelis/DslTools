using DslTools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DslTools.Tests.Extensions
{

    [TestClass]
    public class LinqTests
    {
        [TestMethod]
        public void Zip()
        {
            var x = new List<string> { "a", "b", "c" };
            var y = new List<string> { "d", "e", "f" };
            var z = new List<string> { "x", "y", "z" };

            var equal = x.ZipAll(y, Tuple.Create).ToList();
            var less = x.ZipAll(y.Concat(z), Tuple.Create).ToList();
            var more = x.Concat(z).ZipAll(y, Tuple.Create).ToList();
            var leftOnly = x.ZipAll(Enumerable.Empty<int>(), Tuple.Create).ToList();
            var rightOnly = Enumerable.Empty<int>().ZipAll(y, Tuple.Create).ToList();

            //Assert.Fail("add asserts");
            var expectedEqual = new List<Tuple<string, string>>
            {
                Tuple.Create("a", "d" ),
                Tuple.Create("b", "e" ),
                Tuple.Create("c", "f" ),
            };

            CollectionAssert.AreEqual(expectedEqual, equal);

            var expectedLess = new List<Tuple<string, string>>
            {
                Tuple.Create("a", "d" ),
                Tuple.Create("b", "e" ),
                Tuple.Create("c", "f" ),
                Tuple.Create((string)null, "x" ),
                Tuple.Create((string)null, "y" ),
                Tuple.Create((string)null, "z" ),
            };

            CollectionAssert.AreEqual(expectedLess, less);

            var expectedMore = new List<Tuple<string, string>>
            {
                Tuple.Create("a", "d" ),
                Tuple.Create("b", "e" ),
                Tuple.Create("c", "f" ),
                Tuple.Create("x", (string)null),
                Tuple.Create("y", (string)null ),
                Tuple.Create("z", (string)null ),
            };

            CollectionAssert.AreEqual(expectedMore, more);

            var expectedLeftOnly = new List<Tuple<string, int>>
            {
                Tuple.Create("a",0 ),
                Tuple.Create("b",0 ),
                Tuple.Create("c",0 ),
            };

            CollectionAssert.AreEqual(expectedLeftOnly, leftOnly);

            var expectedRightOnly = new List<Tuple<int, string>>
            {
                Tuple.Create(0,"d" ),
                Tuple.Create(0,"e" ),
                Tuple.Create(0,"f" ),
            };

            CollectionAssert.AreEqual(expectedRightOnly, rightOnly);
        }
    }
}
