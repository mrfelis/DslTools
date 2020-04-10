using DslTools.Tests.StockObjects;
using DslTools.Tests.StockObjects.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using static DslTools.Tests.StockObjects.Stock.Tokens;

namespace DslTools.Tests
{

    [TestClass]
    public class LexerBaseTests
    {
        [TestMethod]
        public void ReturnsStringOfTokens()
        {
            var tokens = Stock.Lexers.GetOneTwoThree().Tokenize("ThreeOneThree");

            Stock.Verify.CreateTokenStreamExpectation<Numbers>()
                .Expect(Numbers.Three)
                .Expect(Numbers.One)
                .Expect(Numbers.Three)
                .Verify(tokens);
        }

        [TestMethod]
        public void LexError()
        {
            var tokens = Stock.Lexers.GetOneTwoThree().Tokenize("I don't know what this is");

            Assert.IsFalse(tokens.Any(), "No tokens should be returned from this string");
        }

        [TestMethod]
        public void TokenStreamCanBeRewritten()
        {
            const string code = "TwoOneTwoThreeTwo";
            var lexer = Stock.Lexers.GetOneTwoThree();

            Assert.AreEqual(5, lexer.Tokenize(code).Count(), "Sanity Check: Test results depend on there being 5 tokens initially");

            lexer.RemoveTwos = true;

            Stock.Verify.CreateTokenStreamExpectation<Numbers>(Options.IgnorePositions, "Something wrong in Rewrite: ")
                .Expect(Numbers.One)
                .Expect(Numbers.Three)
                .Verify(lexer.Tokenize(code));
        }
    }
}
