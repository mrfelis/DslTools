using DslTools.Tests.StockObjects.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DslTools.Tests.StockObjects
{
    public static partial class Stock
    {
        public static class Verify
        {
            public static TokenExpectation<T> CreateTokenStreamExpectation<T>(Options options = Options.None, string prefix = null)
            {
                return new TokenExpectation<T>(options, prefix);
            }
        }
    }

}
