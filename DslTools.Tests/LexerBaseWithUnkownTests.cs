using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DslTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DslTools.Tests.StockObjects;
using static DslTools.Tests.StockObjects.Stock.Tokens;

namespace DslTools.Tests
{

    [TestClass]
    public class LexerBaseWithUnkownTests
    {
        [TestMethod]
        public void LexUnknown()
        {
            const string expected = "this text is not recognized";

            var tokens = Stock.Lexers.OneTwoThreeAndUnknown.Tokenize(expected);

            Stock.Verify.CreateTokenStreamExpectation<Numbers>()
                .Expect(Numbers.Unknown, expected)
                .Verify(tokens);
        }

        [TestMethod]
        public void UnknownsMixedWithOtherTokenst()
        {
            // todo: 'Two' should be recognized but the unknown token is being too greedy
            var tokens = Stock.Lexers.OneTwoThreeAndUnknown.Tokenize("One. Two comes after One.");

            Stock.Verify.CreateTokenStreamExpectation<Numbers>()
                .Expect(Numbers.One)
                //.Expect(OneTwoThreeWithUnknown.Unknown, ". ")
                .Expect(Numbers.Unknown, ". Two comes after One.")
                //.Expect(OneTwoThreeWithUnknown.Two)
                //.Expect(OneTwoThreeWithUnknown.Unknown, " comes after ")
                //.Expect(OneTwoThreeWithUnknown.One)
                //.Expect(OneTwoThreeWithUnknown.Unknown, ".")
                .Verify(tokens);
        }
    }
}
