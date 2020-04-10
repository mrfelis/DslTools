using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DslTools.Languages.Csv
{
    public class ParseOptions
    {
        /// <summary>
        /// If true (default) the first line in the CSV stream data are the headers 
        /// for the columns. If false, the headers are not included and 
        /// <see cref="Data.SetHeaders(List{string})"/>> must be called to provide custom
        /// headers. Default coilumn header will be provided.
        /// </summary>
        public bool HasHeaders { get; set; }

        /// <summary>
        /// If true (default) the <see cref="Data.Parse(string, ParseOptions)"/> method will fill in
        /// missing values with nulls. Missing columns will be filled null. Missing headers will be
        /// named "Column x" where x is the o based index of the column. If false, all
        /// </summary>
        public bool AllowMissingColumns { get; set; }
    }
}
