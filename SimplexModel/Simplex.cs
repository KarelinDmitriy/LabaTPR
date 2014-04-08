using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimplexModel
{
    public class Simplex
    {
#region variable
        const long MAX = -int.MaxValue*1L;

        Matrix _matrix;
        List<Limit> _limits;
        MathFunction _function;
        List<int> _basis;
#endregion 

#region public methods
        public Simplex()
        {
            _limits = new List<Limit>();
            _basis = new List<int>();
        }

        public Simplex(MathFunction math)
            :this()
        {
            _function = math;
        }

        public void AddLimit(Limit l)
        {
            _limits.Add(l);
        }
        
        public Fraction Solve()
        {
            //Переходим к задаче максимизации
            if (_function.TargetFunction == Target.minimization)
                _function.ChangeTarget();
            //Делаем вектор (B) не отрицательным
            foreach (var x in _limits)
            {
                if (x.LeftSide<0)
                {
                    x.invertSing();
                }
                //Вводим дополнительные переменные
                switch (x.Sing)
                {
                    case Sing.lessEquality :
                        _function.AddNewVariable(0);
                        x.addVar(1, _function.Length-1);
                        break;
                    case Sing.equality :
                        _function.AddNewVariable(MAX);
                        x.addVar(1, _function.Length-1);
                        break;
                    case Sing.moreEquality:
                        _function.AddNewVariable(0);
                        x.addVar(-1, _function.Length-1);
                        _function.AddNewVariable(MAX);
                        x.addVar(1, _function.Length-1);
                        break;
                }
            }
            foreach (var x in _limits)
            {
                x.addVar(0, _function.Length-1);
            }
            //Теперь переходим к шагу 1 и формируем 
            //Первую симплексную таблицу
            return Step1();
        }

       
#endregion

#region private methods
        private Fraction Step1()
        {
            //определим размер таблицы.
            int n = _limits.Count+1;
            int m =  _function.Length +2;
            _matrix = new Matrix(n, m);
            //Заполняем первую симплесную таблицу
            //нужно выбрать базис 
            for (int i=0; i<_matrix.N-1; i++)
            {
                _basis.Add( _limits[i].LastVar());

                for (int j = 1; j < _function.Length; j++)
                    _matrix[i, j] = _limits[i][j];
                _matrix[i, 0] = _limits[i].LeftSide;
            }
            //Заполнили таблицу, теперь нужно искать симплекс-разности
            for (int j=1; j<_matrix.M-2; j++)
            {
                Fraction sum = 0;
                for (int i = 0; i < _matrix.N - 1; i++)
                    sum += _matrix[i, j] * _function[_basis[i]];
             
                _matrix[_matrix.N - 1, j] = sum - _function[j];
            }
            for (int i=0; i<_matrix.N-1; i++)
            {
                _matrix[_matrix.N - 1, _matrix.M - 1] +=
                    _matrix[i, 0] * _function[_basis[i]];
            }
            //Посчитали вроде
            return step2();
        }

        private Fraction step2()
        {
            throw new NotImplementedException();
        }
#endregion
    }
}
