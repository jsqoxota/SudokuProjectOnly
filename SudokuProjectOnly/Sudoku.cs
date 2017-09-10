using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuProjectOnly.SudokuProjectOnly
{
    public class Sudoku
    {
        //数据存储
        List<List<int>> X = new List<List<int>>();//垂直
        List<List<int>> Y = new List<List<int>>();//水平
        List<int> Z;//小九宫格
        List<int> exceptYNum;//除去水平方向 剩余的数
        List<int> remainNum;//未使用的数

        //小九宫格 首格坐标 8个
        int[] XLocation = new int[8] { 3, 6, 0, 3, 6, 0, 3, 6 };//水平方向
        int[] YLocation = new int[8] { 0, 0, 3, 3, 3, 6, 6, 6 };//垂直方向 

        //需填充小九宫格数量
        int littleSudokuNum = 8;

        //首位数字
        int firstNum = 5;

        //数独 9*9
        int[,] sudoku = new int[9, 9];

        #region Test
        //{ {5, 6, 7, 0, 0, 0, 0, 0, 0},
        //  {3, 2, 4, 0, 0, 0, 0, 0, 0},
        //  {1, 8, 9, 0, 0, 0, 0, 0, 0},
        //  {0, 0, 0, 4, 2, 5, 0, 0, 0},
        //  {0, 0, 0, 1, 7, 8, 0, 0, 0},
        //  {0, 0, 0, 6, 3, 9, 0, 0, 0},
        //  {0, 0, 0, 0, 0, 0, 4, 7, 8},
        //  {0, 0, 0, 0, 0, 0, 9, 2, 1},
        //  {0, 0, 0, 0, 0, 0, 3, 5, 6} };
        #endregion

        //XLocation和YLocation数组的指针
        int location = 0;
        //小九宫格位置上限
        int maxXL;
        int maxYL;
        //小九宫格完成数量
        int count = 0;
        //目标数量
        int N;
        //完成数量
        int n = 0;

        FileStream f;
        StreamWriter sw;

        /// <summary>
        /// 产生终盘
        /// </summary>
        /// <param name="n"></param>
        public void SudokuCreate(int s)
        {
            N = s;
            for (int i = 0; i < 9; i++)
            {
                X.Add(new List<int>());
                Y.Add(new List<int>());
            }
            AddZ1Num();

            for (int i = 0; i < 81; i++)
            {
                if (sudoku[i % 9, i / 9] != 0)
                {
                    X[i % 9].Add(sudoku[i / 9, i % 9]);
                    Y[i / 9].Add(sudoku[i / 9, i % 9]);
                }
            }

            f = new FileStream("sudoku.txt", FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(f);
            AddNineNum();
            sw.Close();
            f.Close();
        }

        /// <summary>
        /// 随机填充小九宫格数据
        /// </summary>
        /// <param name="sudoku"></param>
        private void AddZNum()
        {
            RandomNum randomNum = new RandomNum();
            int[] value = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] randomNumArr = randomNum.GetRandomNum(value, 9);
            for (int i = 5, z = 8; i >= 3; i--)
                for (int j = 5; j >= 3 && z >= 0; j--, z--)
                    sudoku[j, i] = randomNumArr[z];
        }

        /// <summary>
        /// 添加Z1方块的数据
        /// </summary>
        /// <param name="sudoku"></param>
        public void AddZ1Num()
        {
            sudoku[0, 0] = firstNum;
            RandomNum randomNum = new RandomNum();
            int[] value = new int[8] { 1, 2, 3, 4, 6, 7, 8, 9 };
            int[] randomNumArr = randomNum.GetRandomNum(value, 8);
            for (int i = 2, z = 7; i >= 0; i--)
            {
                for (int j = 2; j >= 0 && z >= 0; j--, z--)
                    sudoku[j, i] = randomNumArr[z];
            }
        }

        /// <summary>
        /// 填满小九宫格&&更换小九宫格
        /// </summary>
        public void AddNineNum()
        {
            if (n >= N) return;
            if (location == littleSudokuNum)
            {
                n++;
                PrintResultFile();
                return;
            }
            List<int> temp;
            int a, b;
            a = maxYL;
            b = maxXL;
            maxYL = YLocation[location] + 3;
            maxXL = XLocation[location] + 3;
            temp = Z;
            Z = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            exceptYNum = Except(Z, Y[YLocation[location]]);//Z.Except(Y[YLocation[location]]).ToList();
            AddOneNum(XLocation[location], YLocation[location], exceptYNum);
            Z = temp;
            maxYL = a;
            maxXL = b;
        }

        /// <summary>
        /// 单格填数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="exceptYNum"></param>
        public void AddOneNum(int x, int y, List<int> exceptYNum)
        {
            if (n >= N) return;
            remainNum = Except(exceptYNum, X[x]);
            foreach (int i in remainNum)
            {
                count++;
                sudoku[y, x] = i;
                exceptYNum.Remove(i);
                X[x].Add(i);
                Y[y].Add(i);
                Z.Remove(i);
                if (x + 1 < maxXL) AddOneNum(x + 1, y, exceptYNum);
                else if (y + 1 < maxYL && Except(Z, Y[y + 1]).Count >= 3)
                    AddOneNum(x - 2, y + 1, Except(Z, Y[y + 1]));
                if (count == 9)
                {
                    count = 0;
                    location++;
                    AddNineNum();
                    location--;
                    count = 9;
                }
                Z.Add(i);
                X[x].RemoveAt(X[x].Count - 1);
                Y[y].RemoveAt(Y[y].Count - 1);
                exceptYNum.Add(i);
                sudoku[y, x] = 0;
                count--;
            }
        }

        /// <summary>
        /// Console输出
        /// </summary>
        public void PrintResultConsole()
        {
            Console.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(sudoku[i, j] + " ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// File输出
        /// </summary>
        public void PrintResultFile()
        {
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                        sw.Write(sudoku[i, j] + " ");
                    sw.WriteLine();
                }
                sw.WriteLine();
                sw.Flush();
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 差集
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public List<int> Except(List<int> A, List<int> B)
        {
            List<int> result = new List<int>(A);
            foreach (int i in B)
                result.Remove(i);
            return result;
        }

        /// <summary>
        /// 返回数独
        /// </summary>
        /// <returns></returns>
        public int[,] getSudoku()
        {
            FileStream fr = new FileStream("sudoku.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fr);

            for (int i = 0; i < 81;)
            {
                char num = (char)sr.Read();
                if (num > 47 && num < 58)
                {
                    sudoku[i / 9, i % 9] = num - 48;
                    i++;
                }
            }

            sr.Close();
            fr.Close();
            return sudoku;
        }
    }
}