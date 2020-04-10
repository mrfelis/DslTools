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
    public class LexerTests
    {
        private Lexer _lexer;

        [TestInitialize]
        public void SetUp()
        {
            _lexer = new Lexer(new LexerOptions
            {
                RemoveDelimeters = false,
                InjectNullValues = false
            });
        }

        [TestMethod]
        public void LexEolUnix()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                //.Expect(Tokens.EOL, "\n")
                .Expect(t => t.Id(Tokens.EOL)
                              .Value("\n")
                )
                .Verify(_lexer.Tokenize("\n"));
        }

        [TestMethod]
        public void LexEolWindows()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                //.Expect(Tokens.EOL, "\r\n")
                .Expect(t => t.Id(Tokens.EOL)
                              .Value("\r\n")
                ).Verify(_lexer.Tokenize("\r\n"));
        }

        [TestMethod]
        public void LexCommaDelimiter()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                //.Expect(Tokens.Delimiter, ",")
                .Expect(t => t.Id(Tokens.Delimiter)
                                .Value(",")
                )
                .Verify(_lexer.Tokenize(","));
        }


        [TestMethod]
        public void LexTabDelimiter()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                //.Expect(Tokens.Delimiter, "\t")
                .Expect(t => t.Id(Tokens.Delimiter)
                              .Value("\t")
                )
                .Verify(_lexer.Tokenize("\t"));
        }

        [TestMethod]
        public void LexInteger()
        {
            const string value = "123";
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                //.Expect(Tokens.Value, "123")
                .Expect(t => t.Id(Tokens.Value)
                              .Value (value)
                )
                .Verify(_lexer.Tokenize(value));
        }

        [TestMethod]
        public void LexNegative()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                .Expect(Tokens.Value, "-123")
                .Verify(_lexer.Tokenize("-123"));
        }

        [TestMethod]
        public void LexDouble()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                 .Expect(Tokens.Value, "-123.456")
                 .Verify(_lexer.Tokenize("-123.456"));
        }

        [TestMethod]
        public void LexString()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
                 .Expect(Tokens.Value, "test")
                 .Verify(_lexer.Tokenize("test"));
        }

        [TestMethod]
        public void LexQuotedString()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(Tokens.Value, "\"test\"", "test")
             .Verify(_lexer.Tokenize("\"test\""));
        }

        [TestMethod]
        public void LexMultipleQuotedStrings()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(Tokens.Value, "\"test\"", "test")
             .Expect(Tokens.Delimiter, ",")
             .Expect(Tokens.Value, "\"123\"", "123")
             .Verify(_lexer.Tokenize("\"test\",\"123\""));
        }

        [TestMethod]
        public void LexQuotedIncludeComma()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(Tokens.Value, "\"1,2\"", "1,2")
             .Verify(_lexer.Tokenize("\"1,2\""));
        }

        [TestMethod]
        public void LexQuotedStringExcapedDoubleQuote()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(Tokens.Value, "\"test\"\"123\"", "test\"123")
             .Verify(_lexer.Tokenize("\"test\"\"123\""));
        }

        [TestMethod]
        public void LexUnterminatedString()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(Tokens.Unterminated, "\"test")
             .Expect(Tokens.EOL, "\n")
             .Verify(_lexer.Tokenize("\"test\n"));
        }

        [TestMethod]
        public void LexUnterminatedStringAtEof()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(Tokens.Unterminated, "\"test")
             .Verify(_lexer.Tokenize("\"test"));
        }

        [TestMethod]
        public void TwoUnterminatedStrings()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(Tokens.Unterminated, "\"test,123")
             .Expect(Tokens.EOL, "\n")
             .Expect(Tokens.Unterminated, "\"row2,456")
             .Verify(_lexer.Tokenize("\"test,123\n\"row2,456"));
        }

        [TestMethod]
        public void ReportNullsBetweenCommas()
        {
            _lexer.Options.InjectNullValues = true;

            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             //.Expect(Tokens.Delimiter, ",")
             .Expect(t => t.Id(Tokens.Delimiter)
                           .Value(","))
             //.Expect(Tokens.Value, string.Empty)
             .Expect(t => t.Id(Tokens.Value))
             //.Expect(Tokens.Delimiter, ",")
             .Expect(t => t.Id(Tokens.Delimiter)
                           .Value(","))
             .Expect(t => t.Id(Tokens.Value).Value("a"))
             .Verify(_lexer.Tokenize(",,a"));
        }

        [TestMethod]
        public void ReportNullAtEndOfLine()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             //.Expect(Tokens.Delimiter, ",")
             .Expect(t => t.Id(Tokens.Value).Value("a"))
             .Expect(t => t.Id(Tokens.Value).Position(2))
             .Expect(t => t.Id(Tokens.EOL)
                           .Value("\n").Position(2))
             //.Expect(Tokens.Value, string.Empty)
             //.Expect(t => t.Id(Tokens.Value))
             //.Expect(Tokens.Delimiter, ",")
             //.Expect(t => t.Id(Tokens.Delimiter)
             //              .Value(","))
             .Verify(Lexer.Default.Tokenize("a,\n"));
        }


        [TestMethod]
        public void ReportNullAtEndOfFile()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             //.Expect(Tokens.Delimiter, ",")
             .Expect(t => t.Id(Tokens.Value).Value("a"))
             .Expect(t => t.Id(Tokens.Value))
             //.Expect(t => t.Id(Tokens.EOL)
             //              .Value("\n").Position(2))
             //.Expect(Tokens.Value, string.Empty)
             //.Expect(t => t.Id(Tokens.Value))
             //.Expect(Tokens.Delimiter, ",")
             //.Expect(t => t.Id(Tokens.Delimiter)
             //              .Value(","))
             .Verify(Lexer.Default.Tokenize("a,"));
        }

        [TestMethod]
        public void OptionsSetByDefault()
        {
            var to = new Lexer();

            Assert.IsNotNull(to.Options);
            Assert.IsTrue(to.Options.RemoveDelimeters);
            Assert.IsTrue(to.Options.InjectNullValues);
        }

        [TestMethod]
        public void DefaultLexerRemovesDelemiters()
        {
            Assert.IsTrue(Lexer.Default.Options.RemoveDelimeters);
        }

        [TestMethod]
        public void RemovesDelimiters()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>()
             .Expect(t => t.Id(Tokens.Value).Value("a"))
             .Expect(t => t.Id(Tokens.Value).Value("b").Position(2))
             .Expect(t => t.Id(Tokens.Value).Value("c").Position(4))
             .Verify(Lexer.Default.Tokenize("a,b,c"));
        }

        [TestMethod]
        public void DoNotInjectNullTokens()
        {
            Stock.Verify.CreateTokenStreamExpectation<Tokens>(StockObjects.Verifiers.Options.IgnorePositions)
             .Expect(t => t.Id(Tokens.Value).Value("a"))
             .Expect(t => t.Id(Tokens.Delimiter).Value(","))
             .Expect(t => t.Id(Tokens.Delimiter).Value(","))
             .Expect(t => t.Id(Tokens.EOL).Value("\n"))
             .Expect(t => t.Id(Tokens.Value).Value("d"))
             .Expect(t => t.Id(Tokens.Delimiter).Value(","))
             //.Expect(t => t.Id(Tokens.EOL).Value("\n"))
             .Verify(_lexer.Tokenize("a,,\nd,"));
        }
    }
}
