using System.Collections.Generic;

namespace DslTools.Languages.Csv
{
    /// <summary>
    /// Converts CSV data into a series of <see cref="TokenValue{T}"/>
    /// that can be used to parse the content into <see cref="Data"/>
    /// </summary>
    public class Lexer : LexerBaseWithUnkown<Tokens>
    {
        /// <summary>
        /// A preconstructed lexer ready to process a stream
        /// </summary>
        public static Lexer Default = new Lexer();

        /// <summary>
        /// Options used by the Lexer
        /// </summary>
        public LexerOptions Options { get; }

        /// <summary>
        /// Constructs a Lexer with an optional set of options
        /// </summary>
        /// <param name="options"></param>
        public Lexer(LexerOptions options = null)
        {
            Options = options ?? new LexerOptions { RemoveDelimeters = true, InjectNullValues = true };
        }

        protected override Tokens Unknown => Tokens.Unknown;

        protected override Dictionary<Tokens, string> TokenDefinitions => new Dictionary<Tokens, string>
        {
            [Tokens.EOL] = "\r?\n",
            [Tokens.Delimiter] = ",|\t",
            [Tokens.EmbeddedQuoted] = "\"(?:[^\n\"]*\"\")+[^\n\"]*\"",
            [Tokens.Quoted] = "\"[^\n\"]*\"",
            [Tokens.Unterminated] = "\"[^\"\n]*(?=\n|$)",
            [Tokens.Value] = "[^,\n\"]+",
        };


        protected override IEnumerable<TokenValue<Tokens>> Rewrite(IEnumerable<TokenValue<Tokens>> tokens)
        {
            TokenValue<Tokens> previous = null;

            foreach (var item in base.Rewrite(tokens))
            {
                switch (item.Id)
                {
                    case Tokens.Quoted:
                        yield return item.Clone(Tokens.Value, item.Value.Trim('"'));
                        break;

                    case Tokens.EmbeddedQuoted:
                        yield return item.Clone(Tokens.Value, item.Value.Trim('"').Replace("\"\"", "\""));
                        break;

                    case Tokens.Delimiter:
                        if (previous?.Id == Tokens.Delimiter && Options.InjectNullValues)
                            yield return new TokenValue<Tokens>(Tokens.Value, string.Empty, string.Empty, item.Start, 0);

                        if (!Options.RemoveDelimeters)
                            yield return item;

                        break;

                    case Tokens.EOL:
                        if (previous?.Id == Tokens.Delimiter && Options.InjectNullValues)
                            yield return new TokenValue<Tokens>(Tokens.Value, string.Empty, string.Empty, item.Start, 0);

                        yield return item;

                        break;

                    // There is no rewriting required for these tokens types so
                    // jsut emit the token as is
                    //case Tokens.Unknown:
                    //case Tokens.EOF:
                    //case Tokens.Value:
                    //case Tokens.NullValue:
                    //case Tokens.Unterminated:

                    default:
                        yield return item;
                        break;
                }
                previous = item;
            }

            if (previous?.Id == Tokens.Delimiter && Options.InjectNullValues)
                yield return new TokenValue<Tokens>(Tokens.Value, string.Empty, string.Empty, previous.Start, 0);
        }
    }
}
