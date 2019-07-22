using System;
using LDN.Expression.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDN.Expression
{
    [TestClass]
    public class ExpressionLearn
    {
        [TestMethod]
        public void ExpressionTestMethod1()
        {
            Func<Bird, bool> temp = x => x.Id > 0;
        }
    }
}
