using System.Collections.Generic;

namespace DslTools
{
    /// <summary>
    /// Context object tracking state for a parser
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ParseContext<T>
    {
        /// <summary>
        /// Constructs a <see cref="ParseContext{T}"/> using an enumerable of
        /// <see cref="TokenValue{T}"/> and the memeber of <see cref="T"/> that
        /// represents the End of File token
        /// </summary>
        /// <param name="tokens">Stream of tokens to be parsed</param>
        /// <param name="EOF">enumeration memeber that identifies the end of
        /// the stream of tokens being parsed. This <see cref="TokenValue{T}"/>
        /// element will be automatically generated and need not be included
        /// in the <see cref="tokens"/> parameter nor lexed.</param>
        public ParseContext(IEnumerable<TokenValue<T>> tokens, T EOF)
        {
            _eof = EOF;
            _notDone = true;
            _enumerator = tokens.GetEnumerator();
            _enumerator.MoveNext();
        }

        /// <summary>
        /// Moved to the next token in the stream
        /// </summary>
        /// <returns>true if there are additional <see cref="TokenValue{T}"/> in the stream</returns>
        public bool Advance()
        {
            _position = Current.Start + Current.Length;
            _notDone = _enumerator.MoveNext();
            return _notDone;
        }

        /// <summary>
        /// Provides the current <see cref="TokenValue{T}"/>. Once all elements are exhausted
        /// the automatic End Of File <see cref="TokenValue{T}"/> will be returned.
        /// </summary>
        public TokenValue<T> Current => _notDone ? _enumerator.Current : GetEof();

        private TokenValue<T> GetEof()
        {
            return new TokenValue<T>(_eof, string.Empty, string.Empty, _position, 0);
        }

        private readonly IEnumerator<TokenValue<T>> _enumerator;
        private readonly T _eof;
        private int _position;
        private bool _notDone;
    }
}