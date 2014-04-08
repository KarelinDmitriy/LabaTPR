using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    class Matrix
    {
#region variable
        int _n, _m;
        Fraction[,] _matrix;
#endregion 

#region public methods
        public Matrix(int n, int m)
        {
            _matrix = new Fraction[n, m];
            _n = n;
            _m = m;
        }

        public Fraction this[int i, int j]
        {
            get
            {
                return _matrix[i, j];
            }
            set
            {
                _matrix[i, j] = value;
            }
        }

        public int N
        {
            get { return _n; }
        }
        public int M
        {
            return {return _m;}
        }
#endregion

#region private methods

#endregion
    }
}
