using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DslTools.Languages.Csv
{

    public class LexerOptions
    {

        /// <summary>
        /// If true (default), the Lexer will not emit <see cref="Tokens.Delimiter"/> tokens
        /// </summary>
        public bool RemoveDelimeters { get; set; }

        /// <summary>
        /// If true (default), the Lexer will emmit a null <see cref="Tokens.Value"/> token
        /// if the last emitted token is a delimiter and the token that is about to be emitted
        /// is also a delimiter
        /// </summary>
        public bool InjectNullValues { get; set; }
    }
}
