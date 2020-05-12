using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DslTools.Tests.Samples.GettingStarted
{

    [TestClass]
    public class GettingStartedTests
    {
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

        [TestMethod]
        public void CompleteLexSample()
        {
            var lexer = new CompleteLexer();
            var tokens = lexer.Tokenize(Properties.Samples.GettingStarted);

            var x = Encoding.UTF8.GetBytes(Properties.Samples.GettingStarted).Take(125).ToArray();

            foreach (var t in tokens)
            {
                var b = Encoding.UTF8.GetBytes(t.Value);

                Debug.Write($"({t.Id}){t.Value}");
            }
        }

        [TestMethod]
        public void RewriteLexSample()
        {
            var lexer = new RewriteLexer();
            var tokens = lexer.Tokenize(Properties.Samples.GettingStarted);

            var x = Encoding.UTF8.GetBytes(Properties.Samples.GettingStarted).Take(125).ToArray();

            foreach (var t in tokens)
            {
                var b = Encoding.UTF8.GetBytes(t.Value);

                Debug.Write(t.Value);
            }
        }
    }
}
