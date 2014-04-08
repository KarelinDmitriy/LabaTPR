using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    class Simplex
    {
#region variable
        Matrix _matrix;
        List<Limit> _limits;
        MathFunction _function;
        List<Fraction> _basis;
#endregion 

#region public methods
        public Simplex()
        {
            _limits = new List<Limit>();
            _basis = new List<Fraction>();
        }

        public Simplex(MathFunction math)
            :this()
        {
            _function = math;
        }

        public void AddLimit(Limit l)
        {
            _limits.Add(l);
        }
        
        public void Solve()
        {
            //Переходим к задаче максимизации
            if (_function.TargetFunction == Target.minimization)
                _function.ChangeTarget();
            //Делаем вектор (B) не отрицательным
            foreach (var x in _limits)
            {
                if (x.LeftSide<0)
                {
                    x.invertSing();
                }
            }
            
            
        }
#endregion

#region private methods

#endregion
    }
}
