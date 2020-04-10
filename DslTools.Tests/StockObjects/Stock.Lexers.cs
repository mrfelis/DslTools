using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
