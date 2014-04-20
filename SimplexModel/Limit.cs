using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    public class Limit
    {
#region variable
        List<Fraction> _vars;
        List<String> _names;
        Sing _sing;
        Fraction _leftSide;
#endregion 

#region public methods
        public Limit()
        {
            _vars = new List<Fraction>();
            _sing = Sing.unknown;
            _leftSide = new Fraction();
            _names = new List<string>();
        }

        public void addVar(Fraction a, int number)
        {
            if (number>=_vars.Count)
            {
                while (number >= _vars.Count)
                {
                    _vars.Add(new Fraction());
                    _names.Add("_temp" + (_vars.Count - 1).ToString());
                }
            }
            _vars[number] += a;
        }

        //Переиминовать переменную нелься, поэтмоу если она уже введена
        //то _names[number] не измениться
        public void addVar(Fraction a, int number, string name)
        {
            if (number >= _vars.Count)
            {
                while (number >= _vars.Count)
                {
                    _vars.Add(new Fraction());
                    _names.Add("_temp" + (_vars.Count - 1).ToString());
                }
            }
            _names[number] = name;
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

        public Sing Sing
        {
            get
            {
                return _sing;
            }
        }

        public int LastVar()
        {
            for (int i = _vars.Count - 1; i >= 0; i--)
                if (_vars[i] != 0) return i;
            return -1;
        }

        public string getName(int idx)
        {
            return _names[idx];
        }
#endregion

#region private methods

#endregion
    }
}
