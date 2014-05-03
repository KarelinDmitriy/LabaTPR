using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    public class MathFunction
    {
#region variable
        List<Fraction> _factors;
        List<String> _names;
        Target _target;
#endregion 

#region public methods
        public MathFunction(Target a)
        {
            _target = a;
            _factors = new List<Fraction>();
            _names = new List<string>();
        }

        public void AddNewVariable(Fraction factor)
        {
            _factors.Add(factor);
            _names.Add("_temp" + (_factors.Count - 1).ToString());
        }
        public void AddNewVariable(Fraction factor, int number)
        {
            if (number >= _factors.Count)
            {
                while (number >= _factors.Count)
                {
                    _factors.Add(new Fraction());
                    _names.Add("_temp" + (_factors.Count - 1).ToString());
                }
            }
            _factors[number] +=factor;
        }

        public void AddNewVariable(Fraction factor, int number, string name)
        {
            if (number >= _factors.Count)
            {
                while (number >= _factors.Count)
                {
                    _factors.Add(new Fraction());
                    _names.Add("_temp" + (_factors.Count - 1).ToString());
                }
            }
            _names[number] = name;
            _factors[number] += factor;
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

        public string getName(int idx)
        {
            return _names[idx];
        }
    }
#endregion

#region private methods

#endregion
}

