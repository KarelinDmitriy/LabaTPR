using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    class Factor
    {
        #region variable
        int _n;
        int _d;
        #endregion

        #region public methods
        public Factor(int n, int d)
        {
            _n = n;
            if (d == 0)
                throw new InvalidOperationException("Denominator = 0");
            _d = d;
            cut();
        }

        public static Factor operator +(Factor a, Factor b)
        {
            return new Factor(a._d * b._n + b._d * a._n, a._n * b._n);
        }

        public static Factor operator -(Factor a, Factor b)
        {
            return new Factor(a._d * b._n - b._d * a._n, a._n * b._n);
        }

        public static Factor operator *(Factor a, Factor b)
        {
            return new Factor(a._d * b._d, a._n * b._n);
        }

        public static Factor operator /(Factor a, Factor b)
        {
            return new Factor(a._d * b._n, a._n * b._d);
        }

        public static Factor operator -(Factor a)
        {
            return new Factor(-a._d, a._n);
        }

        public static bool operator <(Factor a, Factor b)
        {
            a._d *= b._n;
            b._d *= a._n;
            a._n = b._n *= a._n;
            bool ret;
            if (a._d < b._d) ret = true;
            else ret = false;
            a.cut();
            b.cut();
            return ret;
        }
        #endregion

        #region private methods
        void cut()
        {
            int a = gcd(Math.Abs(_n), Math.Abs(_d));
            _n /= a;
            _d /= a;
            if (_d < 0)
            {
                _n = -_n;
                _d = -_d;
            }
        }

        int gcd(int a, int b)
        {
            return (b != 0) ? gcd(b, a % b) : a;
        }
        #endregion
    }
}
