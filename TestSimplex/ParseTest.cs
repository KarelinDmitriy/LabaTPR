using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexModel.Parser;
using SimplexModel;

namespace TestSimplex
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void Test1()
        {
            string text =
            @" Max f(x,y) = 6x+2y;
   
               2x + 4y <= 9;
               3x + y <=6;
            ";
            Parser p = new Parser(text);
            var smp = p.Parse();

            Fraction res = smp.Solve();

            Assert.AreEqual(res, 12);
        }

        [TestMethod]
        public void Test2()
        {
            string text =
            @"Max f(x1, x2) = 5x1 + 6x2;

              4x1 + 2x2 <= 900;
              2x1 + x2 <= 400;
              x1  + x2 <=300;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, 1800);
        }

        [TestMethod]
        public void Test3()
        {
            string text =
            @"Min f(x1, x5) = 15x1 + 33x5;
              3x1+2x5>=6;
              x5 + 6x1 >=6;
              x5 >=1;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, 53);
        }

        [TestMethod]
        public void Test4()
        {
            string text =
            @"Min f(x1, x2, x3, x4) = x1 + 3x2 - x3 + 2x4;
              x1 + 2x2 - 2x3 + x4 = 4;
              3x1 - x2 + x3 - x4 >= 3;
              x1 - x3 + 2x4 <= 4;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, 4);
        }

        [TestMethod]
        public void Test5()
        {
            string text =
            @"Max f(x, y, z) = -x + z -y -y;
              3y + x + z >=4;
              x + 2y - z >=6;
              x + z <=12;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, -6);
        }

        [TestMethod]
        [ExpectedException(typeof(NoAnswerException))]
        public void Test6()
        {
            string text =
            @"Min f(x, y, z) = (8/1)x + (-10/1)y - (12/1)z;
              x -2y - 3y <=4;
              x + y -2z <=3;
              +(+1/1)x - 3y -1z <=5;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, -30);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseErrorException))]
        public void Test7()
        {
            string text =
            @"Max f x, y, z) = -x + z -2y;
              3y + x + z >=4;
              x + 2y - z >=6;
              x + z <=12;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, -6);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseErrorException))]
        public void Test8()
        {
            string text =
            @"Max  (x, y, z) = -x + z -2y;
              3y + x + z >=4;
              x + 2y - z >=6;
              x + z <=12;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, -6);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseErrorException))]
        public void Test9()
        {
            string text =
            @"Max f (x, y, z) = -x + z -2y
              3y + x + z >=4;
              x + 2y - z >=6;
              x + z <=12;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, -6);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseErrorException))]
        public void Test10()
        {
            string text =
            @"Max  f (x, y, z) = -x + z -2y;
              3y + x + z >=4;
              x + 2y - z >=6;
              x + a <=12;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, -6);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseErrorException))]
        public void Test11()
        {
            string text =
            @"Max f (x, y, z) = -a + z -2y;
              3y + x + z >=4;
              x + 2y - z >=6;
              x + z <=12;";
            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, -6);
        }

        [TestMethod]
        [ExpectedException(typeof(NoAnswerException))]
        public void Test12()
        {
            string text =
            @"Min f (y1, y2, y3) = y1 - 2y2+3y3;
              3y1 - 5y2 - 7y3 = 1;
              -4y1 - 6y2 + 8y3 >=2;";

            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, 0);
        }

        [TestMethod]
      //  [ExpectedException(typeof(NoAnswerException))]
        public void Test13()
        {
            string text =
            @"Min f(x1, x2, x3) = 5x1 + 7x2 +13x3;
                2x1+x2+4x3>=22;
                x1 + x2 + x3 = 9;
                x1 + 2x2+2x3 <=18;";

            var res = new Parser(text).Parse().Solve();

            Assert.AreEqual(res, 61);
        }
    }
}
