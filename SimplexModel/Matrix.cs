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
        double[,] _matrix;
#endregion 

#region public methods
        public Matrix(int n, int m)
        {
            _matrix = new double[n, m];
            _n = n;
            _m = m;
        }

        public double this[int i, int j]
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
#endregion

#region private methods

#endregion
    }
}
