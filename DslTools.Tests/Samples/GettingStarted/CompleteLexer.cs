using System.Collections.Generic;

namespace DslTools.Tests.Samples.GettingStarted
{

    public class CompleteLexer : LexerBaseWithUnkown<Tokens>
    {
        protected override Dictionary<Tokens, string> TokenDefinitions =>
            new Dictionary<Tokens, string>
            {
                [Tokens.EOL] = "\r\n|\n",
                [Tokens.FirstTwo] = @"(?<=\s)\d{2}(?=\d{2}[\s:])",
                [Tokens.LastTwo] = @"(?<=\s\d{2})\d{2}(?=[\s:])",
            };

        protected override Tokens Unknown => Tokens.Unknown;
    }
}
