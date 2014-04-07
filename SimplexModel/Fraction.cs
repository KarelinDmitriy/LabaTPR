using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    class Fraction
    {
        #region variable
        int _n;
        int _d;
        #endregion

        #region public methods
        public Fraction()
        {
            _n = 0;
            _d= 1;
        }

        public Fraction(int n, int d)
        {
            _n = n;
            if (d == 0)
                throw new InvalidOperationException("Denominator = 0");
            _d = d;
            cut();
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a._d * b._n + b._d * a._n, a._n * b._n);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a._d * b._n - b._d * a._n, a._n * b._n);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a._d * b._d, a._n * b._n);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a._d * b._n, a._n * b._d);
        }

        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a._d, a._n);
        }

        public static bool operator <(Fraction a, Fraction b)
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

        public static bool operator >(Fraction a, Fraction b)
        {
            a._d *= b._n;
            b._d *= a._n;
            a._n = b._n *= a._n;
            bool ret;
            if (a._d > b._d) ret = true;
            else ret = false;
            a.cut();
            b.cut();
            return ret;
        }

        public static bool operator ==(Fraction a, Fraction b)
        {
            a._d *= b._n;
            b._d *= a._n;
            a._n = b._n *= a._n;
            bool ret;
            if (a._d == b._d) ret = true;
            else ret = false;
            a.cut();
            b.cut();
            return ret;
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }

        public static bool operator >=(Fraction a, Fraction b)
        {
            return !(a < b);
        }

        public static bool operator <=(Fraction  a, Fraction b)
        {
            return !(a > b);
        }

        public override string ToString()
        {
            if (_d == 1) return String.Format(_n.ToString());
            return String.Format("{0}/{1}", _n, _d);
        }

        public override bool Equals(object obj)
        {
            if (obj is Fraction)
            {
                Fraction a = obj as Fraction;
                if (a._d == _d && a._n == _n)
                    return true;
                else return false;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            //Warning, Danger!!!!!!!!!!!!!!!!!!!!!!!!!
            return _n.GetHashCode() + _d.GetHashCode();
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
