using System.ComponentModel;

namespace DslTools.Languages.Csv
{
    public enum Tokens
    {
        [Description("The Lexer was not able to identify what this string represents")]
        Unknown,

        [Description("This is a delimiter (comma or tab character) between two values. It will only be returned of RemoveDelimeters options is set to False")]
        Delimiter,

        [Description("There are no more tokens available. Technically, this is also a delimiter")]
        EOF,

        [Description("The CSV stream data is starting another record. Technically, this is also a delimiter")]
        EOL,

        [Description("This is a value within the CSV")]
        Value,

        #region Error Tokens

        [Description("The Lexer has encountered an end of file or an end of line when it expected to find a quote")]
        Unterminated,

        #endregion

        #region Internal user only. These tokens should not be returned by the Lexer

        //[Description("A null value created by two delimiters or a delimiter and either an end of line of end of file with no other characters in between")]
        //NullValue,

        [Description("A string that starts and ends with double quotes")]
        Quoted,

        [Description("A quoted string that contains an escaped quote (two double quotes together)")]
        EmbeddedQuoted,

        #endregion
    }
}
