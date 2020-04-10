using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DslTools.Languages.Csv;
using DslTools.Tests.StockObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTools.Tests.Languages.Csv
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void CanSetHeader()
        {
            const string h1 = "First";
            const string h2 = "Second";
            const string h3 = "Third";

            var to = new Data();
            to.SetHeaders(new List<string> { h1, h2, h3 });

            var actual = to.Headers.ToList();

            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual(h1, actual[0]);
            Assert.AreEqual(h2, actual[1]);
            Assert.AreEqual(h3, actual[2]);
        }

        [TestMethod]
        public void CanAddRow()
        {
            const string h1 = "First";
            const string h2 = "Second";
            const string h3 = "Third";

            var to = new Data();
            to.AddRow(new List<string> { h1, h2, h3 });

            var actual = to[0].ToList();

            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual(h1, actual[0]);
            Assert.AreEqual(h2, actual[1]);
            Assert.AreEqual(h3, actual[2]);
        }

        [TestMethod]
        public void RowCountUpdates()
        {
            var row = new List<string> { "a", "b", "c" };

            var to = new Data();

            Assert.AreEqual(0, to.Rows);

            to.AddRow(row);

            Assert.AreEqual(1, to.Rows);

            to.AddRow(row);

            Assert.AreEqual(2, to.Rows);
        }

        [TestMethod]
        public void HeadersInitializedToEmptyList()
        {
            var to = new Data();

            Assert.IsNotNull(to.Headers);
            Assert.AreEqual(0, to.Headers.Count());
        }

        [TestMethod]
        public void CanNotModifyHeadersThroughList()
        {
            var row = new List<string> { "a", "b", "c" };

            var to = new Data();

            to.SetHeaders(row);
            row.Add("d");

            Assert.AreEqual(3, to.Headers.Count());
        }

        //[TestMethod]
        //public void CanNotCastHeadersToList()
        //{
        //    var row = new List<string> { "a", "b", "c" };

        //    var to = new Data();

        //    to.SetHeaders(row);

        //    Assert.IsNotInstanceOfType(to.Headers, typeof(List<string>));
        //}

        [TestMethod]
        public void CanNotModifyRowsThroughList()
        {
            var row = new List<string> { "a", "b", "c" };

            var to = new Data();

            to.AddRow(row);
            row.Add("d");

            Assert.AreEqual(3, to[0].Count());
        }

        #region Parse tests

        [TestMethod]
        public void ParseData()
        {
            var to = Data.Parse("a,b\n1,2");

            Assert.IsNotNull(to);

            var h = to.Headers.ToList();

            Assert.AreEqual(2, h.Count);
            Assert.AreEqual("a", h[0]);
            Assert.AreEqual("b", h[1]);

            Assert.AreEqual(1, to.Rows);

            var r = to[0].ToList();

            Assert.AreEqual(2, r.Count);
            Assert.AreEqual("1", r[0]);
            Assert.AreEqual("2", r[1]);
        }

        [TestMethod]
        public void HeadersAreOptional()
        {
            var to = Data.Parse("a,b\n1,2", new ParseOptions { HasHeaders = false });

            Assert.AreEqual(2, to.Rows);

            Assert.AreEqual("a", to[0, 0]);
            Assert.AreEqual("1", to[1, 0]);
        }

        [TestMethod]
        public void HeadersAreProvided()
        {
            var to = Data.Parse("a,b\n1,2", new ParseOptions { HasHeaders = false });

            var h = to.Headers.ToList();

            Assert.AreEqual(2, h.Count);
            Assert.AreEqual("Column 0", h[0]);
            Assert.AreEqual("Column 1", h[1]);
        }

        #endregion
    }
}
