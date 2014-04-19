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
        const long MAX = -int.MaxValue*2L;

        Matrix _matrix;
        List<Limit> _limits;
        MathFunction _function;
        List<int> _basis;
        bool _isReverse;
#endregion 

#region public methods
        public Simplex()
        {
            _limits = new List<Limit>();
            _basis = new List<int>();
            _isReverse = false;
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

        public void SetFunction(MathFunction func)
        {
            _function = func;
        }
        
        public Fraction Solve()
        {
            //Переходим к задаче максимизации
            if (_function.TargetFunction == Target.minimization)
            {
                _function.ChangeTarget();
                _isReverse = true;
            }
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
            Step1();
            Fraction answer =  step2();
            return _isReverse ? -answer : answer;
        }

       
#endregion

#region private methods
        private void Step1()
        {
            //определим размер таблицы.
            int n = _limits.Count+1;
            int m =  _function.Length;
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
            CalculateSimplexSub();
            //Посчитали вроде
        }

        private void CalculateSimplexSub()
        {
            for (int j = 1; j < _matrix.M; j++)
            {
                Fraction sum = 0;
                for (int i = 0; i < _limits.Count; i++)
                    sum += _matrix[i, j] * _function[_basis[i]];

                _matrix[_matrix.N - 1, j] = sum - _function[j];
            }
            _matrix[_matrix.N - 1, 0] = 0;
            for (int i = 0; i < _limits.Count; i++)
            {
                _matrix[_matrix.N - 1, 0] +=
                    _matrix[i, 0] * _function[_basis[i]];
            }
        }

        private Fraction step2()
        {
            while (!isFindOptimum())
            {
                int idx_j=0, idx_i=-1;
                Fraction max_min = int.MaxValue;
                //находим первый из самых больших отрицательных элементов
                for (int j=1; j<_function.Length; j++)
                {
                    if (_matrix[_matrix.N-1, j]<0 && _matrix[_matrix.N-1, j] < max_min)
                    {
                        idx_j = j;
                        max_min = _matrix[_matrix.N - 1, j];
                    }
                }
                //если нашли отрицательный столбец
                Fraction koef = int.MaxValue ;
                for (int i=0; i<_basis.Count; i++)
                {
                    if (_matrix[i,idx_j] > 0 && _matrix[i,0]/_matrix[i, idx_j]<koef)
                    {
                        koef = _matrix[i, 0] / _matrix[i, idx_j];
                        idx_i = i;
                    }
                }
                if (idx_i == -1)
                    throw new NoAnswerException("Функция не ограниченно растет");
                newSimplexTable(idx_i, idx_j);
            }
            while (isExistAlternative())
            {

            }
            return _matrix[_matrix.N - 1, 0];
        }

        private void newSimplexTable(int idx_i, int idx_j)
        {
            Matrix mat = new Matrix(_matrix.N, _matrix.M);
            //заменяем старый базис на новый
            _basis[idx_i] = idx_j;
            //пересчитываем остальные элементы
            for (int i=0; i<_basis.Count; i++)
            {
                if (i == idx_i) continue;
                for (int j=0; j<_function.Length; j++)
                {
                    mat[i,j] = _matrix[i, j] - _matrix[idx_i, j] * _matrix[i, idx_j] / _matrix[idx_i, idx_j];
                }
            }
            Fraction f = _matrix[idx_i, idx_j] + 0;
            //пересчитываем коефициенты направляющей строки
            for (int j = 0; j < _function.Length; j++)
                mat[idx_i,j] =  _matrix[idx_i, j] / f;
            _matrix = mat;
            CalculateSimplexSub();
        }

        private bool isFindOptimum()
        {
            for (int j=1; j<_function.Length; j++)
            {
                if (_matrix[_matrix.N - 1, j] < 0)
                    return false;
            }
            return true;
        }

        private bool isExistAlternative()
        {
            //TODO: Implemet0))))0
            return false;
        }
#endregion
    }
}
