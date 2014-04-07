using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    class Limit
    {
#region variable
        List<Fraction> _vars;
        Sing _sing;
        Fraction _leftSide;
#endregion 

#region public methods
        public Limit()
        {
            _vars = new List<Fraction>();
            _sing = Sing.unknown;
            _leftSide = new Fraction();
        }

        public void addVar(Fraction a, int number)
        {
            if (number>=_vars.Count)
            {
                while (number >= _vars.Count)
                    _vars.Add(new Fraction());
            }
            _vars[number] += a;
        }

        public void setSing(Sing a)
        {
            _sing = a;
        }

        public void setLeftSide(Fraction a)
        {
            _leftSide = a;
        }

        public void invertSing()
        {
            for (int i=0; i<_vars.Count; i++)
            {
                _vars[i] = -_vars[i];
            }
            if (_sing == Sing.lessEquality)
                _sing = Sing.moreEquality;
            else if (_sing == Sing.moreEquality)
                _sing = Sing.lessEquality;
            _leftSide = -_leftSide;
        }

        public Fraction this[int i]
        {
            get
            {
                return _vars[i];
            }
        }

        public Fraction LeftSide
        {
            get
            {
                return _leftSide;
            }
        }
#endregion

#region private methods

#endregion
    }
}
