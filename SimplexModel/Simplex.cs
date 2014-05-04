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
        const long MAX = int.MinValue * 1L;

        Matrix _matrix;
        List<Limit> _limits;
        MathFunction _function;
        List<int> _basis;
        List<Vector> _results;
        List<Vector> _dual_problem;
        bool _isReverse;
        string _solve;
        #endregion

        #region public methods
        public Simplex()
        {
            _limits = new List<Limit>();
            _basis = new List<int>();
            _isReverse = false;
            _solve = "";
            _results = new List<Vector>();
            _dual_problem = new List<Vector>();
        }

        public Simplex(MathFunction math)
            : this()
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
            StringBuilder html = new StringBuilder();
            html.Append("<p>Для нахождения оптимума была заданна следующая функция с данными ограничениям:</p>\n");
            html.Append(_function.toHTMLString());
            foreach (var x in _limits)
                html.Append( x.toHTMLString());
            //Переходим к задаче максимизации
            if (_function.TargetFunction == Target.minimization)
            {
                html.Append("<p>Т.к. функцию нужно минимизировать, перейдем к задаче максимизации и учтем, </br> что значение функции в таблице будет с противоположным знаком");
                _function.ChangeTarget();
                _isReverse = true;
            }
            //Делаем вектор (B) не отрицательным
            foreach (var x in _limits)
            {
                if (x.LeftSide < 0)
                {
                    x.invertSing();
                }
                //Вводим дополнительные переменные
                switch (x.Sing)
                {
                    case Sing.lessEquality:
                        _function.AddNewVariable(0);
                        x.addVar(1, _function.Length - 1);
                        break;
                    case Sing.equality:
                        _function.AddNewVariable(MAX);
                        x.addVar(1, _function.Length - 1);
                        break;
                    case Sing.moreEquality:
                        _function.AddNewVariable(0);
                        x.addVar(-1, _function.Length - 1);
                        _function.AddNewVariable(MAX);
                        x.addVar(1, _function.Length - 1);
                        break;
                }
            }
            foreach (var x in _limits)
            {
                x.addVar(0, _function.Length - 1);
            }
            html.Append("<p>Введем балансовые и искуственые перменные. Получим следующую фукнцию</p>");
            html.Append(_function.toHTMLString());
            foreach (var x in _limits)
                html.Append(x.toHTMLString());
            html.Append("<p>Составим первую симплексную таблицу и перейдем к решению</p>\n");
            //Теперь переходим к шагу 1 и формируем 
            //Первую симплексную таблицу
            _solve += html.ToString();
            Step1();
            try
            {
                
                Fraction answer = step2();
                if (answer > int.MaxValue || answer < int.MinValue)
                    _solve += "<p> Поспольку вывести искустенную переменную из басиза не удалось, решений нет</p>\n";
                return _isReverse ? -answer : answer;
            }
            catch (NoAnswerException e)
            {
                _solve += "<p>" + e.Message + "</p>\n";
            }
            return 0;
        }

        public string SolveAsHTML
        {
            get { return _solve; }
        }

        public List<Vector> Solves
        {
            get { return _results; }
        }

        public List<Vector> DualProblem
        {
            get { return _dual_problem; }
        }

        #endregion

        #region private methods
        private void Step1()
        {
            //определим размер таблицы.
            int n = _limits.Count + 1;
            int m = _function.Length;
            _matrix = new Matrix(n, m);
            //Заполняем первую симплесную таблицу
            //нужно выбрать базис 
            for (int i = 0; i < _matrix.N - 1; i++)
            {
                _basis.Add(_limits[i].LastVar());

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
            StringBuilder html = new StringBuilder();
            html.Append("<p>Сформируем первую симплексную таблицу</p>");
            html.Append(TableAsHTMLString() + "<br/>");
            while (!isFindOptimum())
            {
                int idx_j = 0, idx_i = -1;
                Fraction max_min = int.MaxValue;
                //находим первый из самых больших отрицательных элементов
                for (int j = 1; j < _function.Length; j++)
                {
                    if (_matrix[_matrix.N - 1, j] < 0 && _matrix[_matrix.N - 1, j] < max_min)
                    {
                        idx_j = j;
                        max_min = _matrix[_matrix.N - 1, j];
                    }
                }
                //если нашли отрицательный столбец
                Fraction koef = int.MaxValue;
                for (int i = 0; i < _basis.Count; i++)
                {
                    if (_matrix[i, idx_j] > 0 && _matrix[i, 0] / _matrix[i, idx_j] < koef)
                    {
                        koef = _matrix[i, 0] / _matrix[i, idx_j];
                        idx_i = i;
                    }
                }
                if (idx_i == -1)
                {
                    _solve += html.ToString();
                    throw new NoAnswerException(
                        "В столбце с отрицательной сиплексной разности все значения меньше или равны нулю. <br/>Функция не ограниченно растет");
                }
                html.Append("<p>Найдем направляющие строку и столбец</p>");
                html.Append(TableAsHTMLString(idx_i, idx_j) + "<br/>");
                newSimplexTable(idx_i, idx_j);
                html.Append("<p>Получаем следующую таблицу, после замены базиса</p>");
                html.Append(TableAsHTMLString());
            }
            html.Append("<p>Так как больше нет столбцеов с отрицательно симпексной разностью, решение оптимально</p>\n");
            _results.Add(getCurVector());
            addDualProblem();
            if (isExistAlternative())
            {
                html.Append("<p>Обноруженное решение не единственное. (Список найденых альтернатив смотрите ниже)</p>\n");
                FindAlternative();
            }
            var a = _matrix[_matrix.N - 1, 0];
            html.Append("<p>Оптимум находиться в точке " + (_isReverse ? (-a).toHTMLString() : a.toHTMLString()) + "</p>");
            _solve += html.ToString();
            return _matrix[_matrix.N - 1, 0];
        }

        private Vector getCurVector()
        {
            Vector v = new Vector();
            for (int i = 0; i < _matrix.N - 1; i++)
            {
                v.addVar(_matrix[i, 0], _function.getName(_basis[i]));
            }
            return v;
        }

        private void FindAlternative()
        {
            //Сохранили состояние матрицы и базиса перед началом поиска дальше. 
            Matrix oldMatrix = new Matrix(_matrix.N, _matrix.M);
            for (int i = 0; i < _matrix.N; i++)
                for (int j = 0; j < _matrix.M; j++)
                    oldMatrix[i, j] = _matrix[i, j];
            List<Int32> _oldBasis = new List<Int32>();
            for (int i = 0; i < _basis.Count; i++) _oldBasis.Add(_basis[i]);
            //найти место, где 0 не в базисе
            //ввести это место в базис
            //проверить, если ли такое решение
            //если его нет, занести в лист векторов и продолжить поиск
            //иначе начать "всплывать" 
            for (int j = 1; j < _matrix.M; j++)
            {
                //откатываем матрицу и базисы к предидущему состоянию
                for (int i = 0; i < _matrix.N; i++)
                    for (int j1 = 0; j1 < _matrix.M; j1++)
                        _matrix[i, j1] = oldMatrix[i, j1];
                for (int i = 0; i < _basis.Count; i++) _basis[i] = _oldBasis[i];
                //откатили 
                if (_matrix[_matrix.N-1, j] == 0)
                {
                    //проверим, а не в базисе ли эта переменная
                    bool inBasis = false;
                    for (int i = 0; i < _basis.Count; i++)
                    {
                        if (_basis[i] == j)
                        {
                            inBasis = true;
                            break;
                        }
                    }
                    if (inBasis) continue;
                    //а уж если не в базисе, то давайте найдем еще несколько решений) 
                    int idx_i = -1;
                    Fraction koef = int.MaxValue;
                    for (int i = 0; i < _basis.Count; i++)
                    {
                        if (_matrix[i, j] > 0 && _matrix[i, 0] / _matrix[i, j] < koef)
                        {
                            koef = _matrix[i, 0] / _matrix[i, j];
                            idx_i = i;
                        }
                    }
                    if (idx_i == -1) continue; //на всяйкий случай
                    //пересчитываем таблицу
                    newSimplexTable(idx_i, j);
                    Vector v = getCurVector();
                    bool isFindSolve = false; //полученный вектор уже найдет? 
                    foreach (var x in _results)
                    {
                        if (x == v)
                        {
                            isFindSolve = true;
                            break;
                        }
                    }
                    if (isFindSolve) continue;
                    _results.Add(v);
                    addDualProblem();
                    FindAlternative(); //ищем альтернативы с этой таблицей
                }
            }
            //востанавливаем таблицы 
            _matrix = oldMatrix;
            _basis = _oldBasis;
        }

        private void newSimplexTable(int idx_i, int idx_j)
        {
            Matrix mat = new Matrix(_matrix.N, _matrix.M);
            //заменяем старый базис на новый
            _basis[idx_i] = idx_j;
            //пересчитываем остальные элементы
            for (int i = 0; i < _basis.Count; i++)
            {
                if (i == idx_i) continue;
                for (int j = 0; j < _function.Length; j++)
                {
                    mat[i, j] = _matrix[i, j] - _matrix[idx_i, j] * _matrix[i, idx_j] / _matrix[idx_i, idx_j];
                }
            }
            Fraction f = _matrix[idx_i, idx_j] + 0;
            //пересчитываем коефициенты направляющей строки
            for (int j = 0; j < _function.Length; j++)
                mat[idx_i, j] = _matrix[idx_i, j] / f;
            _matrix = mat;
            CalculateSimplexSub();
        }

        private bool isFindOptimum()
        {
            for (int j = 1; j < _function.Length; j++)
            {
                if (_matrix[_matrix.N - 1, j] < 0)
                    return false;
            }
            return true;
        }

        private bool isExistAlternative()
        {
            for (int j = 1; j < _matrix.M; j++)
            {
                if (_matrix[_matrix.N-1, j] == 0)
                {
                    bool isBasis = false;
                    for (int i=0; i<_basis.Count; i++)
                        if (_basis[i] == j)
                        {
                            isBasis = true;
                            break;
                        }
                    if (!isBasis) return true;
                }
            }
            return false;
        }

        private string TableAsHTMLString()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table border = '1'>\n");
            //создаем шапку
            html.Append("<tr>\n");
            html.Append("<th rowspan = '2'>Базис</th>\n");
            html.Append("<th rowspan = '2'>Cб</th>\n");
            html.Append("<th>C</th>\n");
            for (int i = 1; i < _function.Length; i++)
            {
                html.Append(String.Format("<td>{0}</td>\n", _function[i].ToString()));
            }
            html.Append("</tr>\n");
            html.Append("<tr>\n");
            html.Append("<td>Б</td>\n");
            for (int i = 1; i < _function.Length; i++)
            {
                html.Append(String.Format("<td>{0}</td>\n", _function.getName(i)));
            }
            html.Append("</tr>\n");
            //создали шапку
            //заполняем базисы и их значения. 

            for (int i = 0; i < _basis.Count; i++)
            {
                html.Append("<tr>\n");
                html.Append(String.Format("<td>{0}</td>\n", _function.getName(_basis[i])));
                html.Append(String.Format("<td>{0}</td>\n", _function[_basis[i]].ToString()));
                for (int j = 0; j < _matrix.M; j++)
                {
                    html.Append(String.Format("<td>{0}</td>\n", _matrix[i, j].ToString()));
                }
                html.Append("</tr>\n");
            }
            //заполнили
            //Заполняем строку симплексных разностей
            html.Append("<tr>\n");
            html.Append("<td> </td>\n");
            html.Append("<td>f = </td>\n");
            for (int j = 0; j < _matrix.M; j++)
            {
                html.Append(String.Format("<td>{0}</td>\n", _matrix[_matrix.N - 1, j].ToString()));
            }
            html.Append("</tr>\n");
            //заполнили
            html.Append("</table>\n");
            return html.ToString();
        }

        private string TableAsHTMLString(int ni, int mj)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table border = '1'>\n");
            //создаем шапку
            html.Append("<tr>\n");
            html.Append("<th rowspan = '2'>Базис</th>\n");
            html.Append("<th rowspan = '2'>Cб</th>\n");
            html.Append("<th>C</th>\n");
            for (int i = 1; i < _function.Length; i++)
            {
                html.Append(String.Format("<td>{0}</td>\n", _function[i].ToString()));
            }
            html.Append("</tr>\n");
            html.Append("<tr>\n");
            html.Append("<td>Б</td>\n");
            for (int i = 1; i < _function.Length; i++)
            {
                html.Append(String.Format("<td>{0}</td>\n", _function.getName(i)));
            }
            html.Append("</tr>\n");
            //создали шапку
            //заполняем базисы и их значения. 
            for (int i = 0; i < _basis.Count; i++)
            {
                html.Append("<tr>\n");
                html.Append(String.Format("<td>{0}</td>\n", _function.getName(_basis[i])));
                html.Append(String.Format("<td>{0}</td>\n", _function[_basis[i]].ToString()));
                for (int j = 0; j < _matrix.M; j++)
                {
                    if (j == mj && i == ni)
                        html.Append(String.Format("<td class='Main_Elem'>{0}</td>\n", _matrix[i, j].ToString()));
                    else if (i == ni)
                        html.Append(String.Format("<td class='Main_Row'>{0}</td>\n", _matrix[i, j].ToString()));
                    else if (j == mj)
                        html.Append(String.Format("<td class='Main_Col'>{0}</td>\n", _matrix[i, j].ToString()));
                    else
                        html.Append(String.Format("<td>{0}</td>\n", _matrix[i, j].ToString()));
                }
                html.Append("</tr>\n");
            }
            //заполнили
            //Заполняем строку симплексных разностей
            html.Append("<tr>\n");
            html.Append("<td> </td>\n");
            html.Append("<td>f = </td>\n");
            for (int j = 0; j < _matrix.M; j++)
            {
                html.Append(String.Format("<td>{0}</td>\n", _matrix[_matrix.N - 1, j].ToString()));
            }
            html.Append("</tr>\n");
            //заполнили
            html.Append("</table>\n");
            return html.ToString();
        }

        private void addDualProblem()
        {
            Vector v = new Vector();
            int count = 1;
            //сначала переберем все балансовые переменные
            for (int i=1; i<_function.Length; i++)
            {
                string name = _function.getName(i);
                if (name[0] == '_' && _function[i] == 0) //балансовая 
                {
                    v.addVar(_matrix[_matrix.N - 1, i], "_y" + count.ToString());
                    count++;
                }
            }
            if (count < _limits.Count) //если не хватает переменных, то хапаем с начала
            {
                int i = 1;
                while (count < _limits.Count)
                {
                    v.addVar(_matrix[_matrix.N - 1, i], "_y" + count.ToString());
                    i++;
                    count++;
                }
            }
            _dual_problem.Add(v);
        }
        #endregion
    }
}
