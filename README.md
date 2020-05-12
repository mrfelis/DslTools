# Dsl.Tools
>Real programmers hand code lexers and parsers in machine code to show other programmers how 
inferior they really are. ;)

Tools for creating domain specific languages using C#/.Net. There are base classes to create lexers 
similar to how you would build a lexer using tools like lex or flex. There's no equivalent base
class to Bison or yacc, but there is a `ParserContext` class to aid in tracking the state of the parser..

## Why another compiler compiler?

* Dsl.Tools integrates well with modern .Net development processes since it is just an .Net assembly
* Dsl.Tools requires no install on the build server, just install the nuget package and let the build 
server restore the package
* Dsl.Tools uses .Net's RegEx engine so there's no need to remember multiple RegEx languages
* Lexers and parsers created with Dsl.Tools can be unit tested just like the rest of the .Net project
* After hand coding a lexer/parser in C#, it was questioned why I didn't use a RegEx to
do the same. Dsl.Tools is the result of answering that challenge


## Getting Started Lexing

"[Lexing] is the process of converting a sequence of characters ... into a sequence of tokens." [^1] A parser will
then process these tokens to produce some result. In a traditional compiler, this result would be object code
that a computer would use to execute a program. With a Domain Specific Language, the lexer will convert the source
into a series of tokens that a parser will translate those tokens into a data structure that your program can use
to perform some useful operation (interpretation). Or the parser might generate source code for some generally programming language 
that could then be compiled to object form (transpile). Of the parser trigger code generation methods to generate a program
directly

1. Select a Lexer base class to derive from:
    1. [LexerBase<T>](#LexerBase): Recognizes a set of tokens in the source. The source 
    may contain additional text that will not be recognized. Characters that are not recognized as 
part of a token will be ignored.
	1. [LexerBaseWithUnknown<T>](#LexerBaseWithUnknown): If you need to ensure that every character in the source be included in
a token use derive from the `LexerBaseWithUnknown` instead.
1. Create an enumerable to identify token types
3. Define .Net Regular Expressions that identify each token to be recognized.
    1. Most regex constructs should work.
    1. DO NOT attempt to capture content with the regex. Override `Rewrite` to provide different content
    2. Consider overriding `Rewrite` and using multiple simple expressions instead of a single complex expression
4. Override the `TokenDefinitions` dictionary.
    1. Initialize the dictionary with a `KeyValuePair` for each token to be recognized
    2. Set the key of each pair to enumeration member with the regular expression identifying
    3. Set the value to the corresponding regular expression
4. Override `Rewrite` as needed to alter the stream of tokens that the Parser will receive
6. Create a method for the parser
7. Pass the source to be lexed to the `Tokenize` method of the derived class.

### LexerBase

Start by defining an enum. Then define a member for each token to be lexed. 
Defining `Unknown` is not required for `LexerBase<T>`. It might be helpful
if you find you need a default value during parsing to define an Invalid member.

```
 public enum Tokens { FirstTwo, LastTwo, EOL}
```

This example lexer will recognize the end of the line (`EOL`), the first 2 digits of a 4 digit number (`FirstTwo`) and
the last 2 digits of a 4 digit number (`LastTwo`) as individual tokens.

Next, derive a class from `LexerBase<T>`. Use the `Tokens` enum for the generic type.

```
    public class SparseLexer : LexerBase<Tokens>
    {

    }
```


Finally, override the `TokenDefinitions` field with a dictionary containing the `Tokens` member
and the regex to recognize that token.

```
        protected override Dictionary<Tokens, string> TokenDefinitions =>
            new Dictionary<Tokens, string>
            {
            };
```

First, add a pair to the dictionary to match 2 digits that are followed by 2 more digits:

```
                [Tokens.FirstTwo] = @"(?<=\s)\d{2}(?=\d{2}[\s:])",

```

Now add a pair that match 2 digits that follow 2 digits:

```
                [Tokens.LastTwo] = @"(?<=\s\d{2})\d{2}(?=[\s:])",

```

Now add a pair to match the new line:
```
                [Tokens.EOL] = "\r?\n"
```

Now the lexer is ready to be used. Use the lexer in a simple unit test that lexes a string resource 
and then prints out the value of each token recognized. (Notice that the test uses `Debug.Write()` 
to only print the value of recognized tokens and the new line characters will only get printed 
because we included EOL as a token.)

```
        [TestMethod]
        public void SparseLexSample()
        {
            var lexer = new SparseLexer();
            var tokens = lexer.Tokenize(Properties.Samples.GettingStarted);

            foreach (var t in tokens)
            {
                Debug.Write(t.Value);
            }
        }
```

Running this unit test using the following [text file](DslTools.Tests/Resources/GettingStarted.txt):

>Source: https://nypost.com/2017/08/12/americas-most-important-historical-dates-will-surprise-you/
>
>1915: NYC’s first female cabbie gets behind the wheel<br />
1903: The teddy bear debuts<br />
1989: ‘Cops’ premieres<br />
1614: Pocahontas marries John Rolfe<br />
1862: Cinco de Mayo becomes a holiday<br />
1752: Yosemite is named as the first US national park<br />
1925: Evolution is debated in the courtroom<br />
1963: MLK delivers “I Have a Dream” speech<br />
1965: Sandy Koufax throws a perfect game<br />
1962: “The Tonight Show” with Johnny Carson premieres<br />
1872: Susan B. Anthony casts a vote<br />
1776: George Washington crosses the Delaware<br />

produces the following output: 

>
>
>1915<br />
1903<br />
1989<br />
1614<br />
1862<br />
1752<br />
1925<br />
1963<br />
1965<br />
1962<br />
1872<br />
1776<br />



### LexerBaseWithUnknown

Using `LexerWithUnknown<T>` is useful when you want to ensure that every character in the source
is recognized.

First, include an `Unknown` member in the `Tokens` enum.

```
 public enum Tokens { Unknown, FirstTwo, LastTwo, EOL}
```

Now derive from LexerBaseWithUnknown:

```
    public class CompleteLexer : LexerBaseWithUnkown<Tokens>
    {

    }
```

Continue overriding `TokenDefinitions` using the definition from `SparseLexer`, and also override the `Unknown` property.

```
        protected override Tokens Unknown => Tokens.Unknown;
```

Write a new unit test that uses the same source but lexes it with 

```
        [TestMethod]
        public void CompleteLexSample()
        {
            var lexer = new CompleteLexer();
            var tokens = lexer.Tokenize(Properties.Samples.GettingStarted);

            foreach (var t in tokens)
            {
                Debug.Write($"({t.Id}){t.Value}");
            }
        }
```

Using the same source file, this produces:

>(Unknown)Source: https://nypost.com/2017/08/12/americas-most-important-historical-dates-will-surprise-you/<br />
(EOL)<br />
(EOL)<br />
(FirstTwo)19(LastTwo)15(Unknown): NYC’s first female cabbie gets behind the wheel<br />
(EOL)<br />
(FirstTwo)19(LastTwo)03(Unknown): The teddy bear debuts<br />
(EOL)<br />
(FirstTwo)19(LastTwo)89(Unknown): ‘Cops’ premieres<br />
(EOL)<br />
(FirstTwo)16(LastTwo)14(Unknown): Pocahontas marries John Rolfe<br />
(EOL)<br />
(FirstTwo)18(LastTwo)62(Unknown): Cinco de Mayo becomes a holiday<br />
(EOL)<br />
(FirstTwo)17(LastTwo)52(Unknown): Yosemite is named as the first US national park<br />
(EOL)<br />
(FirstTwo)19(LastTwo)25(Unknown): Evolution is debated in the courtroom<br />
(EOL)<br />
(FirstTwo)19(LastTwo)63(Unknown): MLK delivers “I Have a Dream” speech<br />
(EOL)<br />
(FirstTwo)19(LastTwo)65(Unknown): Sandy Koufax throws a perfect game<br />
(EOL)<br />
(FirstTwo)19(LastTwo)62(Unknown): “The Tonight Show” with Johnny Carson premieres<br />
(EOL)<br />
(FirstTwo)18(LastTwo)72(Unknown): Susan B. Anthony casts a vote<br />
(EOL)<br />
(FirstTwo)17(LastTwo)76(Unknown): George Washington crosses the Delaware<br />
(EOL)<br />

### Rewriting token streams

Rewriting a stream of tokens form the base class is as simple as overriding `LexerBase.Rewrite.` Iterate over
the `tokens` parameter. Return whichever `TokenValue` element is valid. Ignore whichever `TokenValue`
is not wanted in the stream (whitespace for instance). Return clones of tokens if changes are required (for 
instance to replace escape character sequences in string values). Return new tokens whenever desired. This
rewriting of the token stream to simplify the parser as much as possible. If more properties of `TokenValue` 
need to be changed than just the `Id` and `Value`, a new `TokenValue` should be created. Otherwise the 
`Clone` method can be used to create a new `TokenValue` with all the properties except these two will be copied.

Note: in actuality, the `Rewrite` method iterates over `base.Rewrite(tokens)` and not `tokens`. This is an
important clarification. This allows deriving multiple levels of subclasses without disrupting the process of 
the super class.

The `RewriteLexer` illustrates an override of `Rewrite` on a subclass of the `CompleteLexer` shown above. The
values of `FirstTwo` and `LastTwo` tokens are replaced with the word forms of the number from the token using 
the `NumberAsWords` method (not shown). All other tokens are returned as is. The loop iterates over 
`base.Rewrite(tokens)` to ensure that any superclass in the inheritance chain can also rewrite the token stream.  

```
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
    }
```


The unit test for rewriting tokens is nearly exactly the same as the last unit test. We swap out `CompleteLexer` 
for a `RewriteLexer`. The output is changed to just the `Value` of tokens.


```
        [TestMethod]
        public void RewriteLexSample()
        {
            var lexer = new RewriteLexer();
            var tokens = lexer.Tokenize(Properties.Samples.GettingStarted);

            foreach (var t in tokens)
            {
                Debug.Write(t.Value);
            }
        }
```

A run of the test produces the following output.

>Source: https://nypost.com/2017/08/12/americas-most-important-historical-dates-will-surprise-you/
>
>nineteen fifteen: NYC’s first female cabbie gets behind the wheel<br />
nineteen oh-three: The teddy bear debuts<br />
nineteen eighty-nine: ‘Cops’ premieres<br />
sixteen fourteen: Pocahontas marries John Rolfe<br />
eighteen sixty-two: Cinco de Mayo becomes a holiday<br />
seventeen fifty-two: Yosemite is named as the first US national park<br />
nineteen twenty-five: Evolution is debated in the courtroom<br />
nineteen sixty-three: MLK delivers “I Have a Dream” speech<br />
nineteen sixty-five: Sandy Koufax throws a perfect game<br />
nineteen sixty-two: “The Tonight Show” with Johnny Carson premieres<br />
eighteen seventy-two: Susan B. Anthony casts a vote<br />
seventeen seventy-six: George Washington crosses the Delaware<br />

## Parsing Aids

From a birds eye view, a parser takes a series of tokens, makes sure that those tokens match 
the rules of the language and outputs a structure that passes onto semantic analysis of those
tokens. So, in effect, a parser is just a lexer that operates at a higher level. Theoretically,
a parser could be specified as a regular expression like `LexerBase`. The .Net RegEx engine 
works with characters and not tokens. As a result, Dsl.Tools doesn't (yet) include any
magical base class that allows generating a parser with just regular expressions.

Until that time comes, there's the `ParserContext<T>` class. This class, while currently
very sparse, provides state to make constructing the parser easier. 

### ParserContext

The `ParserContext<T>` is an immutable generic object that keeps track of the parser's
position within the stream of token. Its use is straight forward.

1. The results of the lexer's `Tokenize<T>` are passed into the constructor.
2. The identifier for the end of the file token is also provided to the constructor.
3. The first token will be available in the `Current` property.
4. Call the `Advance` method to pull the next token from the stream into `Current`
5. Once all tokens are exhausted from the source, `Current` will be the End of File token.
6. Once `Current` is at EOF, `Advance` will always return false.

The `Parse` method of [`DslTools.Languages.Csv.Data`](DslTools/Languages/Csv/Data.cs) is a 
fairly simple example. Please note this Parse function is not a production quality parser.

```
        public static Data Parse(string csvContent, ParseOptions options = null)
        {
            var context = new ParseContext<Tokens>(Lexer.Default.Tokenize(csvContent), Tokens.EOF);
            var r = new Data();

            var addHeader = options?.HasHeaders ?? true;

            List<string> headers = null;
            var items = new List<string>();

            while (context.Current.Id != Tokens.EOF)
            {
                switch (context.Current.Id)
                {
                    case Tokens.Value:
                        items.Add(context.Current.Value);
                        break;

                    case Tokens.EOL:
                        if (addHeader)
                            headers = items;
                        else
                            r.AddRow(items);

                        addHeader = false;
                        items = new List<string>();
                        break;
                }

                context.Advance();
            }

            if (items.Any())
                r.AddRow(items);

            r.SetHeaders(headers?.Any() == true ? headers 
				: Enumerable.Range(0, items.Count).Select(i => $"Column {i}").ToList());

            return r;
        }

```

## Changelog

* 0.1 Alpha:
     * Initial prerelease
     * Two base classes for lexing tokens
     * A ParseContext class to help write Parsers. Minimal implementation to date
     * A TokenValue class for reporting the Value, identify, and location of tokens in the source

[^1] https://en.wikipedia.org/wiki/Lexical_analysis