using DslTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DslTools.Tests.StockObjects.Stock.Tokens;

namespace DslTools.Tests.StockObjects.Lexers
{

    public class OneTwoThreeAndUnknownLexer : LexerBaseWithUnkown<Numbers>
    {
        protected override Dictionary<Numbers, string> TokenDefinitions => Stock.Tokens.OneTwoThree;

        protected override Numbers Unknown => Numbers.Unknown;
    }

}
