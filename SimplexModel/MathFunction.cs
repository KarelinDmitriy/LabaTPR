using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    class MathFunction
    {
#region variable
        List<Fraction> _factors;
        Target _target;
#endregion 

#region public methods
        public MathFunction(Target a)
        {
            _target = a;
            _factors = new List<Fraction>();
        }

        public void  AddNewVariable(Fraction factor, int number)
        {
            if (number >= _factors.Count)
            {
                while (number>=_factors.Count)
                    _factors.Add(new Fraction());
            }
            _factors[number] +=factor;
        }

        public void ChangeTarget()
        {
            if (_target == Target.maximization)
            {
                _target = Target.minimization;
            }
            else
            {
                _target = Target.maximization;
            }
            for (int i = 0; i < _factors.Count; i++)
                _factors[i] = -_factors[i];
        }

        public Target TargetFunction
        {
            get
            {
                return _target;
            }
         }

        public Fraction this[int i]
        {
            get
            {
                return _factors[i];
            }
        }

        public int Length
        {
            get
            {
                return _factors.Count;
            }
        }
    }
#endregion

#region private methods

#endregion
}

