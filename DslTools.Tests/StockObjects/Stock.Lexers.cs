using DslTools.Tests.StockObjects.Lexers;

namespace DslTools.Tests.StockObjects
{
    public static partial class Stock
    {
        public static class Lexers
        {
            public static OneTwoThreeLexer GetOneTwoThree() { return new OneTwoThreeLexer(); }

            public static OneTwoThreeAndUnknownLexer OneTwoThreeAndUnknown => new OneTwoThreeAndUnknownLexer();
        }
    }
}
