using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel.Parser
{
    public enum  TokenType
    {
        Sing, 
        Mult, 
        Frac,
        Var, 
        OpBr, 
        ClBr,
        Eq,
        Comma, 
        End, 
        SimCol
    }
}
