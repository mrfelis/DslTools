using System.Collections.Generic;
using static DslTools.Tests.StockObjects.Stock.Tokens;

namespace DslTools.Tests.StockObjects.Lexers
{

    public class OneTwoThreeAndUnknownLexer : LexerBaseWithUnkown<Numbers>
    {
        protected override Dictionary<Numbers, string> TokenDefinitions => Stock.Tokens.OneTwoThree;

        protected override Numbers Unknown => Numbers.Unknown;
    }

}
