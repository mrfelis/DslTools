using DslTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DslTools.Tests.StockObjects.Stock.Tokens;

namespace DslTools.Tests.StockObjects.Lexers
{
    public class OneTwoThreeLexer : LexerBase<Numbers>
    {
        protected override Dictionary<Numbers, string> TokenDefinitions => Stock.Tokens.OneTwoThree;

        public bool RemoveTwos { get; set; }

        protected override IEnumerable<TokenValue<Numbers>> Rewrite(IEnumerable<TokenValue<Numbers>> tokens)
        {
            return base.Rewrite(RemoveTwos ? tokens.Where(t => t.Id != Numbers.Two) : tokens);
        }
    }

}
