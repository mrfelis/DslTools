using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DslTools
{
    /// <summary>
    /// Lexer base class that automatically matches everything not already 
    /// identified as a specific token as unknown. 
    /// The id of the unkown enum member must be provided.
    /// </summary>
    /// <typeparam name="T">Enumeration providing ids for tokens</typeparam>
    public abstract class LexerBaseWithUnkown<T> : LexerBase<T>
    {
        /// <summary>
        /// Identifies the token enumeration member that will be used
        /// as the id for the undentified token
        /// </summary>
        protected abstract T Unknown { get; }

        protected override IEnumerable<KeyValuePair<T, string>> GetAdditionalTokens()
        {
            // Match everything that doesn't already match as Unknown
            yield return new KeyValuePair<T, string>(Unknown, ".+");
        }
    }
}