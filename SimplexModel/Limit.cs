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
        List<Factor> _vars;
        Sing _sing;
        Factor _leftSide;
#endregion 

#region public methods
        public Limit()
        {
            _vars = new List<Factor>();
            _sing = Sing.unknown;
            _leftSide = new Factor();
        }

        public void addVar(Factor a)
        {
            _vars.Add(a);
        }

        public void setSing(Sing a)
        {
            _sing = a;
        }

        public void setLeftSide(Factor a)
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
#endregion

#region private methods

#endregion
    }
}
