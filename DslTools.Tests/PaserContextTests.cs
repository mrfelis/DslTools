using DslTools.Tests.StockObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using static DslTools.Tests.StockObjects.Stock.Tokens;

namespace DslTools.Tests
{
    [TestClass]
    public class PaserContextTests
    {
        [TestMethod]
        public void CurrentInitializedOnConstructions()
        {
            var to = new ParseContext<Numbers>(Stock.Tokens.Stream(Numbers.Three), Numbers.EOF);

            Assert.IsNotNull(to.Current);
            Assert.AreEqual(Numbers.Three, to.Current.Id);
        }

        [TestMethod]
        public void Advances()
        {
            var to = new ParseContext<Numbers>(Stock.Tokens.Stream(Numbers.Three, Numbers.Two, Numbers.One),
                Numbers.EOF);

            Assert.AreEqual(Numbers.Three, to.Current.Id);

            Assert.IsTrue(to.Advance());

            Assert.AreEqual(Numbers.Two, to.Current.Id);

            Assert.IsTrue(to.Advance());

            Assert.AreEqual(Numbers.One, to.Current.Id);
        }

        [TestMethod]
        public void AdancedPastEndBecomesEOF()
        {
            var to = new ParseContext<Numbers>(Stock.Tokens.Stream(Numbers.One).ToList(),
                Numbers.EOF);

            Assert.IsFalse(to.Advance());

            Assert.AreEqual(Numbers.EOF, to.Current.Id);
        }

        [TestMethod]
        public void AdanceWellBeyondEnd()
        {
            var to = new ParseContext<Numbers>(Stock.Tokens.Stream(Numbers.One),
                Numbers.EOF);

            Assert.IsFalse(to.Advance());
            Assert.IsFalse(to.Advance());
            Assert.IsFalse(to.Advance());
            Assert.IsFalse(to.Advance());
            Assert.IsFalse(to.Advance());
            Assert.IsFalse(to.Advance());

            Assert.AreEqual(Numbers.EOF, to.Current.Id);
        }

        [TestMethod]
        public void EofIsAtEndOfSource()
        {
            const string source = "OneTwoThree";

            var to = new ParseContext<Numbers>(Stock.Lexers.GetOneTwoThree().Tokenize(source),
                Numbers.EOF);

            to.Advance(); // Two
            to.Advance(); // Three
            to.Advance(); // EOF

            Assert.AreEqual(Numbers.EOF, to.Current.Id);
            Assert.AreEqual(source.Length, to.Current.Start);
            Assert.AreEqual(0, to.Current.Length);
        }
    }
}
