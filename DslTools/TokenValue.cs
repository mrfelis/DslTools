using System;
using System.Text.RegularExpressions;

namespace DslTools
{

    /// <summary>
    /// Holds specific values related to a string of characters of a
    /// stream that a Lexer has identified as a specific token
    /// </summary>
    /// <typeparam name="T">Enumeration providing identifing types for tokens</typeparam>
    public class TokenValue<T>
    {
        /// <summary>
        /// Constructs <see cref="TokenValue{T}"/> specified with all properties manually
        /// </summary>
        /// <param name="id">Identifier for the type token</param>
        /// <param name="original">the original string of characters identified by the Lexer as this token</param>
        /// <param name="value">the string of characters after being processed by any rewriters</param>
        /// <param name="start">the starting position of the token in the stream</param>
        /// <param name="length">the length of the string of tokens that
        /// represent the token in the original stream</param>
        public TokenValue(T id, string original, string value, int start, int length)
        {
            Id = id;
            Original = original;
            Value = value;
            Start = start;
            Length = length;
        }

        /// <summary>
        /// Constructs a <see cref="TokenValue{T}"/> using a stringified version of the identity and 
        /// the <see cref="Group"/> from the regular expression <see cref="Match"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        public TokenValue(string name, Group group)
        {
            Id = (T)Enum.Parse(typeof(T), name);
            Value = Original = group.Value;
            Start = group.Index;
            Length = group.Length;
        }

        /// <summary>
        /// Reserved for <see cref="Clone(T, string)"/>
        /// </summary>
        protected TokenValue() { }

        /// <summary>
        /// Clones a copy of the <see cref="TokenValue{T}"/> allowing
        /// replacement of <see cref="Id"/> and the <see cref="Value"/> properties.
        /// </summary>
        /// <param name="newId">the cloned <see cref="TokenValue{T}"/> will have this <see cref="Id"/></param>
        /// <param name="newValue">the cloned <see cref="TokenValue{T}"/> will have this <see cref="Id"/></param>
        /// <returns></returns>
        public TokenValue<T> Clone(T newId, string newValue)
        {
            return new TokenValue<T>()
            {
                Id = newId,
                Original = this.Original,
                Value = newValue,
                Start = this.Start,
                Length = this.Length
            };
        }

        /// <summary>
        /// Identifier for the type of <see cref="TokenValue{T}"/>
        /// </summary>
        public T Id { get; private set; }

        /// <summary>
        /// The final string of characters that provide the value for the 
        /// <see cref="TokenValue{T}"/> to be used by the parser
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// The original string of characters captured from the stream 
        /// being lexed to be used for reporting errors
        /// </summary>
        public string Original { get; private set; }

        /// <summary>
        /// The 0 based index of the first character in the match
        /// to be used for reporting errors
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// The number of characters captured form the string
        /// to be used for reporting errors
        /// </summary>
        public int Length { get; private set; }
    }
}