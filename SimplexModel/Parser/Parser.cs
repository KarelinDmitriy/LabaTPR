using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel.Parser
{
    public class Parser
    {
        #region variable
        Lexer _lexer;
        Token _curToken;
        Dictionary<string, int> _table; //переменые и их номер для системы, если 0, то зарезервированное слово
        int _varCount;//кол-во введенных переменных
        #endregion

        #region public methods
        public Parser(string text)
        {
            _lexer = new Lexer(text);
            _curToken = null;
            _table = new Dictionary<string, int>();
            _table.Add("f", 0);
            _table.Add("Max", 0);
            _table.Add("Min", 0);
            _varCount = 0;
        }

        public Simplex Parse()
        {
            _curToken = _lexer.getNextToken();
            Simplex ret = MainStep();
            if (_curToken.Type != TokenType.End)
                throw new ParseErrorException("Не предвиденное окончание окончание потока. Проверте правильность описания");
            return ret;
        }

        #endregion

        #region private methods
        private Simplex MainStep()
        {
            Simplex smp = new Simplex();
            MathFunction fnc = FuncStep();
            var limits = LimitsStep();
            smp.SetFunction(fnc);
            foreach (var x in limits)
                smp.AddLimit(x);
            return smp;
        }

        private MathFunction FuncStep()
        {
            MathFunction fnc;
            if (_curToken.Value != "Max" && _curToken.Value != "Min")
                throw new ParseErrorException("Не верное объявление функции, ожидалось Max или Min");
            if (_curToken.Value == "Max") fnc = new MathFunction(Target.maximization);
            else fnc = new MathFunction(Target.minimization);
            Match(TokenType.Var);
            if (_curToken.Value != "f")
                throw new ParseErrorException("Не верное объявление функции, ожидалось f");
            Match(TokenType.Var);
            if (_curToken.Type != TokenType.OpBr)
                throw new ParseErrorException("Не верное объявление функции, ожидалась открывающая скобка");
            Match(TokenType.OpBr);
            if (_curToken.Type != TokenType.Var)
                throw new ParseErrorException("Ожидалось объявление переменной");
            while (_curToken.Type == TokenType.Var) //читаем все переменные
            {
                if (_table.ContainsKey(_curToken.Value))
                    throw new ParseErrorException("Переменная " + _curToken.Value + " уже объявленна");
                _varCount++;
                _table.Add(_curToken.Value, _varCount);
                fnc.AddNewVariable(0, _varCount, _curToken.Value);
                Match(TokenType.Var);
                if (_curToken.Type == TokenType.Comma) Match(TokenType.Comma);
            }
            if (_curToken.Type != TokenType.ClBr)
                throw new ParseErrorException("Ожидалась закрывающая скобка");
            Match(TokenType.ClBr);
            if (_curToken.Value != "=")
                throw new ParseErrorException("Ожидалось начало описания функии. Не обнаружен знак '='");
            Match(_curToken.Type);
            while (_curToken.Type != TokenType.SimCol)
            {
                if (!(_curToken.Type == TokenType.Sing ||
                    _curToken.Type == TokenType.Var ||
                    _curToken.Type == TokenType.Number ||
                    _curToken.Type == TokenType.OpBr))
                    throw new ParseErrorException("Не верное описание слагаемого");
                int koef = 1;
                Fraction koefVar = 1;
                if (_curToken.Type == TokenType.Sing)
                {
                    if (_curToken.Value == "-") koef = -1;
                    Match(_curToken.Type);
                }
                if (_curToken.Type == TokenType.Number || _curToken.Type == TokenType.OpBr)
                {
                    koefVar = Number();
                }
                if (_curToken.Type != TokenType.Var)
                    throw new ParseErrorException("Ожидалась переменная");
                if (!_table.ContainsKey(_curToken.Value))
                    throw new ParseErrorException("Переменная " + _curToken.Value + " не объявленная");
                if (_table[_curToken.Value] == 0)
                    throw new ParseErrorException("Ключевое слово " + _curToken.Value + " не может использоваться, как переменная");
                fnc.AddNewVariable(koef * koefVar, _table[_curToken.Value]);
                Match(_curToken.Type);
            }
            Match(_curToken.Type);
            return fnc;
        }

        private List<Limit> LimitsStep()
        {
            List<Limit> ret = new List<Limit>();
            while (_curToken.Type != TokenType.End)
                ret.Add(LimitStep());
            return ret;
        }

        private Limit LimitStep()
        {
            Limit limit = new Limit();
            do
            {
                if (!(_curToken.Type == TokenType.Sing ||
                        _curToken.Type == TokenType.Var ||
                        _curToken.Type == TokenType.Number ||
                        _curToken.Type == TokenType.OpBr))
                    throw new ParseErrorException("Не верное описание слагаемого");
                int koef = 1;
                Fraction koefVar = 1;
                if (_curToken.Type == TokenType.Sing)
                {
                    if (_curToken.Value == "-") koef = -1;
                    Match(_curToken.Type);
                }
                if (_curToken.Type == TokenType.Number || _curToken.Type == TokenType.OpBr)
                {
                    koefVar = Number();
                }
                if (_curToken.Type != TokenType.Var)
                    throw new ParseErrorException("Ожидалась переменная");
                if (!_table.ContainsKey(_curToken.Value))
                    throw new ParseErrorException("Переменная " + _curToken.Value + " не объявленная");
                if (_table[_curToken.Value] == 0)
                    throw new ParseErrorException("Ключевое слово " + _curToken.Value + " не может использоваться, как переменная");
                limit.addVar(koef * koefVar, _table[_curToken.Value], _curToken.Value);
                Match(_curToken.Type);
            } while (_curToken.Type != TokenType.Eq);
            if (_curToken.Value == "=")
                limit.setSing(Sing.equality);
            else if (_curToken.Value == ">=")
                limit.setSing(Sing.moreEquality);
            else limit.setSing(Sing.lessEquality);
            Match(_curToken.Type);
            Fraction left = Number();
            limit.setLeftSide(left);
            if (_curToken.Type != TokenType.SimCol)
                throw new ParseErrorException("Ожидалась точка с зяпятой");
            Match(_curToken.Type);
            return limit;
        }

        private Fraction Number()
        {
            bool needClBr = false;
            if (_curToken.Type==TokenType.OpBr)
            {
                Match(_curToken.Type);
                needClBr = true;
            }
            int koef = 1;
            int numerator = 0, denominator=1;
            if (_curToken.Type == TokenType.Sing)
            {
                if (_curToken.Value == "-")
                    koef = -1;
                Match(_curToken.Type);
            }
            if (_curToken.Type != TokenType.Number)
                throw new ParseErrorException("Не верное определение числа");
            numerator = Convert.ToInt32(_curToken.Value);
            Match(_curToken.Type);
            if (_curToken.Type== TokenType.Frac)
            {
                Match(TokenType.Frac);
                if (_curToken.Type != TokenType.Number)
                    throw new ParseErrorException("Знаменатеель дроби должен быть определен");
                denominator = Convert.ToInt32(_curToken.Value);
                if (denominator == 0)
                    throw new ParseErrorException("Знаменатель дроби не может быть 0");
                Match(_curToken.Type);
            }
            if (needClBr && _curToken.Type == TokenType.ClBr)
                Match(_curToken.Type);
            else if (needClBr) 
                throw new ParseErrorException("Ожидалась закрывающая скобка");
            return new Fraction(koef * numerator, denominator);
        }

        void Match(TokenType t)
        {
            if (_curToken.Type == t)
                _curToken = _lexer.getNextToken();
            else throw new ParseErrorException("Не верная последоватьность");
        }
        #endregion
    }
}
