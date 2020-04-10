using DslTools.Tests.StockObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTools.Tests
{

    [TestClass]
    public class TokenValueTests
    {
        private enum TestToken { DoNotUse, One, Two }

        [TestMethod]
        public void ConstructFromMatch()
        {
            const TestToken expectedToken = TestToken.Two;
            const string expectedValue = "Value";

            var capture = Stock.Tokens.CaptureAll(expectedValue);

            // Sanity checks: if these asserts fail, The GenerateCapture function
            // did not set up the test correctly
            Assert.AreEqual(expectedValue, capture.Value);
            Assert.AreEqual(0, capture.Index);
            Assert.AreEqual(expectedValue.Length, capture.Length);


            var to = new TokenValue<TestToken>(expectedToken.ToString(), capture);

            Assert.AreEqual(expectedToken, to.Id, "Token Id should be taken from name argument");
            Assert.AreEqual(expectedValue, to.Value, "Value should come from the capture");
            Assert.AreEqual(expectedValue, to.Original, "Original should come from the capture");

            Assert.AreEqual(0, to.Start, "Start should be the value of Index from the capture");
            Assert.AreEqual(expectedValue.Length, to.Length, "Length should be the length of the Capture");
        }

        [TestMethod]
        public void CorrectStartWhenCaptureDoesNotIncludeFirstCharacter()
        {
            const int expectedStart = 2;

            var capture = Stock.Tokens.CaptureAfter("a", expectedStart);

            // Sanity checks: if these asserts fail, The GenerateCapture function
            // did not set up the test correctly
            Assert.AreEqual(expectedStart, capture.Index);

            var to = new TokenValue<TestToken>(TestToken.One.ToString(), capture);

            Assert.AreEqual(expectedStart, to.Start, "Start should be the value of Index from the capture");
        }

        [TestMethod]
        public void ConstructorAssignsValues()
        {
            const TestToken expectedToken = TestToken.Two;
            const string expectedValue = "Value";
            const string expectedOriginal = "Original";
            const int expectedStart = 10;
            const int expectedLength = 20;

            var to = new TokenValue<TestToken>(expectedToken, expectedOriginal, expectedValue, expectedStart, expectedLength);

            Assert.AreEqual(expectedToken, to.Id, "Token Id should be assigned from parameter");
            Assert.AreEqual(expectedOriginal, to.Original, "Original should be assigned from parameter");
            Assert.AreEqual(expectedValue, to.Value, "Value should be assigned from parameter");
            Assert.AreEqual(expectedStart, to.Start, "Start should be assigned from parameter");
            Assert.AreEqual(expectedLength, to.Length, "Length should be assigned from parameter");
        }

        [TestMethod]
        public void ClonesWithNewValue()
        {
            const TestToken expectedToken = TestToken.Two;
            const string expectedValue = "Value";
            const string expectedOriginal = "Original";
            const int expectedStart = 10;
            const int expectedLength = 20;

            var source = new TokenValue<TestToken>(TestToken.DoNotUse, expectedOriginal, "this is not right", expectedStart, expectedLength);

            var to = source.Clone(expectedToken, expectedValue);

            Assert.AreEqual(expectedToken, to.Id, "Token Id should be assigned from source");
            Assert.AreEqual(expectedOriginal, to.Original, "Original should be assigned from source");
            Assert.AreEqual(expectedValue, to.Value, "Value should be assigned from parameter");
            Assert.AreEqual(expectedStart, to.Start, "Start should be assigned from source");
            Assert.AreEqual(expectedLength, to.Length, "Length should be assigned from source");
        }
    }
}
