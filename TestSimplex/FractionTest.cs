using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexModel;

namespace TestSimplex
{
    [TestClass]
    public class FractionTest
    {
        [TestMethod]
        public void SumTest()
        {
            Fraction a = 6;
            Fraction b = 0;
            Fraction c = a + b;
            Assert.AreEqual(c, 6);
        }
        [TestMethod]
        public void SubMethod()
        {
            Fraction a = 6;
            Fraction b = 0;
            Fraction c = a - b;
            Assert.AreEqual(c, 6);
        }
        [TestMethod]
        public void MultTest()
        {
            Fraction a = 6;
            Fraction b = 1;
            Fraction c = a * b;
            Assert.AreEqual(c, 6);
        }
        [TestMethod]
        public void DivTest()
        {
            Fraction a = 6;
            Fraction b = 1;
            Fraction c = a / b;
            Assert.AreEqual(c, 6);
        }
        [TestMethod]
        public void NotEqul()
        {
            Fraction a = 1;
            Assert.AreEqual(a != 0, true);
            Assert.AreEqual(a, 1);
        }
    }
}
