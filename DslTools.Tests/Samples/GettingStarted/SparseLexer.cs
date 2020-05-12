using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DslTools.Tests.Samples.GettingStarted
{

    public class SparseLexer : LexerBase<Tokens>
    {
        protected override Dictionary<Tokens, string> TokenDefinitions =>
            new Dictionary<Tokens, string>
            {
                [Tokens.FirstTwo] = @"(?<=\s)\d{2}(?=\d{2}[\s:])",
                [Tokens.LastTwo] = @"(?<=\s\d{2})\d{2}(?=[\s:])",
                [Tokens.EOL] = "\r?\n"
            };
    }
}
