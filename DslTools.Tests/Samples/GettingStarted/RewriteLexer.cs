using System.Collections.Generic;

namespace DslTools.Tests.Samples.GettingStarted
{

    public class RewriteLexer : CompleteLexer
    {
        protected override IEnumerable<TokenValue<Tokens>> Rewrite(IEnumerable<TokenValue<Tokens>> tokens)
        {
            foreach (var t in base.Rewrite(tokens))
            {
                switch (t.Id)
                {
                    case Tokens.FirstTwo:
                        yield return t.Clone(t.Id, NumberAsWords(t.Value));
                        break;
                    case Tokens.LastTwo:
                        yield return t.Clone(t.Id, " " + NumberAsWords(t.Value));
                        break;
                    default:
                        yield return t;
                        break;
                }
            }
        }

        /// <summary>
        /// convert a year into words 2 digits at a time
        /// </summary>
        /// <param name="number">two digits of the number</param>
        /// <returns>the digits written as words</returns>
        /// <remarks>This is a rough conversion to get a reasonable 
        /// result for sample code. Multiple special cases have
        /// been deliberately ignored.</remarks>
        private string NumberAsWords(string number)
        {
            // word form of  numbers 10-19 do not follow 
            // typical rules. Go English!
            if (_oddBalls.TryGetValue(number, out var s))
                return s;

            // split the tens and ones digits
            var one = number.Substring(1, 1);
            var ten = number.Substring(0, 1);

            // there's no '0' entry in either dictionary
            // this would cause a crash so this is another
            // special case to deal with
            if (ten == "0") return "oh-" + _ones[one];
            if (one == "0") return _tens[ten];

            // the values of ten and one are definitely
            // in these dictionaries, so this won't crash
            // but there are plenty of special cases that 
            // have been ignored.
            return _tens[ten] + "-" + _ones[one];
        }

        private Dictionary<string, string> _ones = new Dictionary<string, string>
        {
            ["1"] = "one", ["2"] = "two", ["3"] = "three",
            ["4"] = "four", ["5"] = "five", ["6"] = "six",
            ["7"] = "seven", ["8"] = "eight", ["9"] = "nine",
        };

        private Dictionary<string, string> _tens = new Dictionary<string, string>
        {
            ["2"] = "twenty", ["3"] = "thirty", ["4"] = "forty",
            ["5"] = "fifty", ["6"] = "sixty", ["7"] = "seventy",
            ["8"] = "eighty", ["9"] = "ninety",
        };

        private readonly Dictionary<string, string> _oddBalls = new Dictionary<string, string>
        {
            ["10"] = "ten", ["11"] = "eleven", ["12"] = "twelve", ["13"] = "thirteen",
            ["14"] = "fourteen", ["15"] = "fifteen", ["16"] = "sixteen", ["17"] = "seventeen",
            ["18"] = "eighteen", ["19"] = "nineteen"
        };
    }
}
