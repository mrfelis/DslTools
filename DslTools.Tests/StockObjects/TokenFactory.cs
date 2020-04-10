using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DslTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTools.Tests.StockObjects
{
    public class TokenFactory<T>
    {
        private int _position;
        private bool _hasLength;
        private T _id;
        private int _length;
        private string _value;
        private string _original;
        private bool _hasOriginal;


        public int TokenLength => _hasLength ? _length : _hasOriginal ? _original.Length : _value.Length;

        /// <summary>
        /// Predicts the position of the next token in a stream. This is only valid between
        /// calls to Value and Create (in this order).
        /// </summary>
        public int NextTokenStart => _position + TokenLength;

        public TokenFactory(int position = 0)
        {
            _position = position;
            Reset();
        }

        public void Reset()
        {
            _hasLength = false;
            _hasOriginal = false;
            _id = default(T);
            _value = string.Empty;
        }

        public TokenValue<T> Create()
        {
            var x = TokenLength;
            var r = new TokenValue<T>(_id, _hasOriginal ? _original : _value, _value, _position, TokenLength);

            Reset();
            var y = TokenLength;

            
            return r;
        }

        public TokenFactory<T> Position(int newPosition)
        {
            _position = newPosition;
            return this;
        }

        public TokenFactory<T> Id(T id)
        {
            _id = id;
            return this;
        }

        public TokenFactory<T> Original(string original)
        {
            _original = original;
            _hasOriginal = true;
            return this;
        }

        public TokenFactory<T> Value(string value)
        {
            _value = value;
            return this;
        }

        public TokenFactory<T> Length(int length)
        {
            _length = length;
            _hasLength = true;
            return this;
        }
    }
}
