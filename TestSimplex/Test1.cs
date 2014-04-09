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

            Assert.AreEqual(res, 12);
        }

        [TestMethod]
        public void SimplexTest2()
        {
            MathFunction fnc = new MathFunction(Target.maximization);
            fnc.AddNewVariable(5, 1);
            fnc.AddNewVariable(6, 2);
            Simplex smp = new Simplex(fnc);
            Limit l1 = new Limit();
            l1.addVar(4, 1);
            l1.addVar(2, 2);
            l1.setSing(Sing.lessEquality);
            l1.setLeftSide(900);
            Limit l2 = new Limit();
            l2.addVar(2, 1);
            l2.addVar(1, 2);
            l2.setSing(Sing.lessEquality);
            l2.setLeftSide(400);
            Limit l3 = new Limit();
            l3.addVar(1, 1);
            l3.addVar(1, 2);
            l3.setSing(Sing.lessEquality);
            l3.setLeftSide(300);
            smp.AddLimit(l1);
            smp.AddLimit(l2);
            smp.AddLimit(l3);

            Fraction res = smp.Solve();

            Assert.AreEqual(res, 1800);
        }

        [TestMethod]
        public void SimplexTest3()
        {
            MathFunction mfc = new MathFunction(Target.minimization);
            mfc.AddNewVariable(15, 1);
            mfc.AddNewVariable(33, 2);
            Simplex smp = new Simplex(mfc);
            Limit l1 = new Limit();
            l1.addVar(3, 1);
            l1.addVar(2, 2);
            l1.setSing(Sing.moreEquality);
            l1.setLeftSide(6);
            Limit l2 = new Limit();
            l2.addVar(6, 1);
            l2.addVar(1, 2);
            l2.setSing(Sing.moreEquality);
            l2.setLeftSide(6);
            Limit l3 = new Limit();
            l3.addVar(0, 1);
            l3.addVar(1, 2);
            l3.setSing(Sing.moreEquality);
            l3.setLeftSide(1);

            smp.AddLimit(l1);
            smp.AddLimit(l2);
            smp.AddLimit(l3);

            Fraction a = smp.Solve();
            Assert.AreEqual(a, 53);
        }
    }
}
