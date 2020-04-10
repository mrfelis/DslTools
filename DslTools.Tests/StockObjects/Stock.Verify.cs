using DslTools.Tests.StockObjects.Verifiers;

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
