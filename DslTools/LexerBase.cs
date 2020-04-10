using DslTools.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DslTools
{
    /// <summary>
    /// Base class allowing the definition of a lexer as a set of tokens identified
    /// with .Net regular expressions.
    /// </summary>
    /// <typeparam name="T">Enumeration providing ids for tokens</typeparam>
    /// <remarks>
    /// Core concept inspired by:
    /// http://yourdotnetdesignteam.blogspot.com/2007/04/tokenize-string-with-c-regular.html
    /// </remarks>
    public abstract class LexerBase<T>
    {
        /// <summary>
        /// A dicitionary pairing the token id and the .Net regular expression
        /// that recognizes characters as a token.
        /// </summary>
        protected abstract Dictionary<T, string> TokenDefinitions { get; }

        protected virtual IEnumerable<KeyValuePair<T, string>> GetAdditionalTokens()
        {
            return Enumerable.Empty<KeyValuePair<T, string>>();
        }

        /// <summary>
        /// Override to alter a stream of tokens in a way to make parsing
        /// the stream easier
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TokenValue<T>> Rewrite(IEnumerable<TokenValue<T>> tokens)
        {
            return tokens;
        }

        /// <summary>
        /// Converts string into an stream of <see cref="TokenValue{T}"/> objects defined within
        /// <see cref="TokenDefinitions"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerable<TokenValue<T>> Tokenize(string value)
        {
            if (_regEx == null)
            {
                _regEx = new Regex("(?:" + string.Join("|", TokenDefinitions.Concat(GetAdditionalTokens())
                    .Select(p => $"(?<{p.Key}>{p.Value})")) + ")?");
            }

            var names = _regEx.GetGroupNames();

            return Rewrite(_regEx.Matches(value)
                .OfType<Match>()
                .SelectMany(g => g
                    .Groups
                    .OfType<Group>()
                    .Skip(1)
                    .WithIndex(1)
                    .Where(x => x.Value.Success))
                .Select(x => new TokenValue<T>(names[x.Key], x.Value)));
        }

        private Regex _regEx = null;
    }
}