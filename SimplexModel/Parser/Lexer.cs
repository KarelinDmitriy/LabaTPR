using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel.Parser
{
    public class Lexer
    {
#region variable
        StringReader _stream;
        int _line;
        int _position; 
#endregion 

#region public methods
        public Lexer(string text)
        {
            _stream = new StringReader(text);
            _line = 1;
            _position = 1;
        }

        public Token  getNextToken()
        {
            while (_stream.Peek() != -1)
            {
                char symbol = (char)_stream.Read();
                if (symbol == '\n')
                {
                    _line++;
                    _position = 1;
                    continue;
                }
                if (symbol == '\r') continue;
                if (symbol == ' ')
                {
                    _position++;
                    continue;
                }
                _position++;
                if (char.IsLetter(symbol))
                    return getVar(symbol);
                if (char.IsDigit(symbol))
                    return getDigits(symbol);
                if (symbol == '+') return new Token(TokenType.Sing, "+");
                if (symbol == '-') return new Token(TokenType.Sing, "-");
                if (symbol == '*') return new Token(TokenType.Mult, "*");
                if (symbol == '/') return new Token(TokenType.Frac, "/");
                if (symbol == '(') return new Token(TokenType.OpBr, "(");
                if (symbol == ')') return new Token(TokenType.ClBr, ")");
                if (symbol == ';') return new Token(TokenType.SimCol, ";");
                if (symbol == '>' || symbol == '<') return getEq(symbol);
                if (symbol == '=') return new Token(TokenType.Eq, "=");
                if (symbol == ',') return new Token(TokenType.Comma, ",");
                throw new ParseErrorException("Обнаружен не изветсный символ: " + symbol);
            }
            return new Token(TokenType.End, "");
        }

#endregion

#region private methods
        private Token getVar(char symbol)
        {
            string res = symbol+"";
            while (char.IsLetterOrDigit((char)_stream.Peek()))
            {
                res += (char)_stream.Read()+"";
                _position++;
            }
            return new Token(TokenType.Var, res);
        }

        private Token getDigits(char symbol)
        {
            string res = symbol + "";
            while (char.IsDigit((char)_stream.Peek()))
            {
                res += (char)_stream.Read() + "";
                _position++;
            }
            return new Token(TokenType.Number, res);
        }

        private Token getEq(char symbol)
        {
            string res = symbol + "";
            symbol = (char)_stream.Peek();
            if (symbol != '=')
                throw new ParseErrorException("Неопознананя последовательность символов: " + res + symbol);
            _position++;
            _stream.Read();
            return new Token(TokenType.Eq, res + symbol);
        }

#endregion
    }
}
