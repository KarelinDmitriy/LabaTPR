using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexModel;

namespace TestSimplex
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void SimplexTest1()
        {
            MathFunction mfc = new MathFunction(Target.maximization);
            mfc.AddNewVariable(6, 1);
            mfc.AddNewVariable(2, 2);
            Simplex smp = new Simplex(mfc);
            Limit lim1 = new Limit();
            lim1.addVar(2, 1);
            lim1.addVar(4, 2);
            lim1.setSing(Sing.lessEquality);
            lim1.setLeftSide(9);
            Limit lim2 = new Limit();
            lim2.addVar(3, 1);
            lim2.addVar(1, 2);
            lim2.setSing(Sing.lessEquality);
            lim2.setLeftSide(6);
            smp.AddLimit(lim1);
            smp.AddLimit(lim2);

            Fraction res = smp.Solve();

            Assert.Fail("");
        }
    }
}
