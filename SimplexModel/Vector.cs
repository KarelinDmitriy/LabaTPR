using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    public class Vector
    {
#region variable
        Dictionary<String, Fraction> _vector;
#endregion 

#region public methods
        public Vector()
        {
            _vector = new Dictionary<string, Fraction>();
        }

        public void addVar(Fraction a, string name)
        {
            if (_vector.ContainsKey(name))
                _vector[name] += a;
            else _vector.Add(name, a);
        }

        public static bool operator ==(Vector a, Vector b)
        {
            if (a._vector.Count != b._vector.Count) return false;
            foreach (var x in a._vector)
            {
                if (!b._vector.ContainsKey(x.Key)) return false;
                if (x.Value != b._vector[x.Key])
                    return false;
            }
            return true;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public string ToHTMLString()
        {
            StringBuilder html1 = new StringBuilder();
            StringBuilder html2 = new StringBuilder();
            html1.Append("<p>(");
            html2.Append("(");
            bool first = true;
            foreach (var x in _vector)
            {
                if (first)
                {
                    html1.Append(x.Key);
                    html2.Append(x.Value);
                    first = false;
                }
                else
                {
                    html1.Append("," + x.Key);
                    html2.Append("," + x.Value);
                }
            }
            html1.Append(")");
            html2.Append(")</p>\n");
            return html1.ToString() + " = " + html2.ToString();
        }
#endregion

#region private methods

#endregion
    }
}
