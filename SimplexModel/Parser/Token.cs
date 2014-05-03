using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel.Parser
{
    public class Token
    {
#region variable
        TokenType _type;
        string _value;
#endregion 

#region public methods
        public Token(TokenType t, string v)
        {
            _type = t;
            _value = v;
        }

        public TokenType Type
        {
            get { return _type; }
        }

        public string Value
        {
            get { return _value; }
        }
#endregion

#region private methods

#endregion
    }
}
