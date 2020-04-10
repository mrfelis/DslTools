using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DslTools.Languages.Csv
{

    /// <summary>
    /// A Wrapper for CSV data.
    /// </summary>
    public class Data
    {
        private List<string> _columnNames;
        private List<List<string>> _data = new List<List<string>>();

        /// <summary>
        /// Returns
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<string> this[int index]
        {
            get
            {
                return _data[index];
            }
        }

        /// <summary>
        /// Returns the column's element in the CSV Data at the row 
        /// </summary>
        /// <param name="row">the row index</param>
        /// <param name="column">the column index</param>
        /// <returns></returns>
        public string this[int row, int column]
        {
            get
            {
                return _data[row][column];
            }
        }

        /// <summary>
        /// Returns the names of columns
        /// </summary>
        public IEnumerable<string> Headers => _columnNames ?? Enumerable.Empty<string>();

        /// <summary>
        /// Returns the number of rows of data
        /// </summary>
        public int Rows => _data.Count;

        /// <summary>
        /// Sets the namdes of the Headers
        /// </summary>
        /// <param name="columnNames"></param>
        public void SetHeaders(List<string> columnNames)
        {
            _columnNames = new List<string>(columnNames);
        }

        /// <summary>
        /// Adds a row of data to the data
        /// </summary>
        /// <param name="data"></param>
        public void AddRow(List<string> data)
        {
            _data.Add(new List<string>(data));
        }


        /// <summary>
        /// Converts a string containing set of values separated by commas into
        /// a <see cref="Data"/> object for interaction with individual values
        /// </summary>
        /// <param name="csvContent"></param>
        /// <param name="options"></param>
        /// <returns></returns>
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

                    // TODO: Add error handling and report these token types as errors
                    //case Tokens.Unknown:
                    //    break;
                     //case Tokens.Unterminated:
                    //    break;

                    // There is n othing to be done with these token types
                   //case Tokens.EOF:
                    //    break;
                    //case Tokens.Delimiter:
                    //    break;
                    //case Tokens.NullValue:
                    //    break;
                    //case Tokens.Quoted:
                    //    break;
                    //case Tokens.EmbeddedQuoted:
                    //    break;
                }

                context.Advance();
            }

            if (items.Any())
                r.AddRow(items);

            r.SetHeaders(headers?.Any() == true ? headers : Enumerable.Range(0, items.Count).Select(i => $"Column {i}").ToList());

            return r;
        }
    }

}
