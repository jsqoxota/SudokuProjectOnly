using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuProjectOnly.SudokuProjectOnly
{
    class uniqueness
    {
        FileStream fr;
        StreamReader sr;
        FileStream fw;
        StreamWriter sw;
        Sudoku S = new Sudoku();

        //int num = 60; //空的数量
        int minL = 30; //空的最小数量
        int n = 0;

        int[,] sudokuA = new int[9, 9];
        int[,] sudokuB = new int[9, 9];

        List<List<int>> X = new List<List<int>>();//水平
        List<List<int>> Y = new List<List<int>>();//垂直
        List<List<int>> Z = new List<List<int>>();
        List<List<int>> list = new List<List<int>>();

        public void creat(int N)
        {
            S.SudokuCreate(N);
            fr = new FileStream("sudoku.txt", FileMode.Open, FileAccess.Read);
            fw = new FileStream("sudokuOnly.txt", FileMode.Create, FileAccess.Write);
            sr = new StreamReader(fr);
            sw = new StreamWriter(fw);

            int max;

            int count;
            int[] blank = new int[81];

            int total = 0;
            int[] final = new int[81];

            for (int i = 0; i < 9; i++)
            {
                X.Add(new List<int>());
                Y.Add(new List<int>());
                Z.Add(new List<int>());
            }

            for (int i = 0; i < 81; i++)
                list.Add(new List<int>());

            while (n < N)
            {
                total = 0;
                input();
                while (total < minL)
                {
                    #region 初始化
                    max = 0;
                    count = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        X[i].Clear();
                        Y[i].Clear();
                        Z[i].Clear();
                    }

                    for (int i = 0; i < 81; i++)
                        list[i].Clear();
                    #endregion

                    scoop();
                   
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (sudokuB[i, j] == 0)
                            {
                                list[i * 9 + j] = intersection(X[j], Y[i], Z[i / 3 * 3 + j / 3]);
                                blank[count] = i * 9 + j;
                                if (max < list[i * 9 + j].Count) max = list[i * 9 + j].Count;
                                count++;
                            }
                        }
                    }

                    while (count != 0)
                    {
                        int temp;
                        int min = 9;
                        for (int i = 0; i < count; i++)
                        {
                            if (min > list[blank[i]].Count) min = list[blank[i]].Count;
                            if (min == 1) break;
                        }
                        for (int j = 0; j < count; j++)
                        {
                            if (min == list[blank[j]].Count)
                            {
                                if (min == 1)
                                {
                                    final[total] = blank[j];
                                    total++;
                                }

                                temp = sudokuA[blank[j] / 9, blank[j] % 9];
                                for (int z = 0; z < count - 1; z++)
                                {
                                    if (z != j)
                                    {
                                        if (blank[z] / 9 == blank[j] / 9)
                                        {
                                            list[blank[z]].Remove(temp);
                                        }
                                        else if (blank[z] % 9 == blank[j] % 9)
                                        {
                                            list[blank[z]].Remove(temp);
                                        }
                                        else if (blank[z] / 9 / 3 * 3 + blank[z] % 9 / 3 == blank[j] / 9 / 3 * 3 + blank[j] % 9 / 3)
                                        {
                                            list[blank[z]].Remove(temp);
                                        }
                                    }
                                }

                                temp = blank[j];
                                blank[j] = blank[count - 1];
                                blank[count - 1] = temp;

                                count--;

                                if (min > 1) break;
                            }
                        }
                    }

                    for (int i = 0; i < total; i++)
                        sudokuA[final[i] / 9, final[i] % 9] = 0;

                    PrintResultFile();
                }
                n++;
            }

            sr.Close();
            sw.Close();
            fr.Close();
            fw.Close();
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
                        sw.Write(sudokuA[i, j] + " ");
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
        /// File输出
        /// </summary>
        public void PrintResultFileB()
        {
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                        sw.Write(sudokuB[i, j] + " ");
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
        /// 输入终盘
        /// </summary>
        public void input()
        {
            for (int i = 0; i < 81;)
            {
                char num = (char)sr.Read();
                if (num > 47 && num < 58)
                {
                    sudokuA[i / 9, i % 9] = num - 48;
                    sudokuB[i / 9, i % 9] = num - 48;
                    i++;
                }
            }
        }

        /// <summary>
        /// 挖空
        /// </summary>
        public void scoop()
        {
            int[] temps = new int[81];
            for(int i = 0; i < 81; i++)
                temps[i] = i;
            RandomNum rn = new RandomNum();
            Random r = new Random();
            int[] result = rn.GetRandomNum(temps, r.Next(35,60));
            for (int i = 0; i < result.Length; i++)
            {
                Y[result[i] / 9].Add(sudokuB[result[i] / 9, result[i] % 9]);
                X[result[i] % 9].Add(sudokuB[result[i] / 9, result[i] % 9]);
                Z[result[i] / 9 / 3 * 3 + result[i] % 9 / 3].Add(sudokuB[result[i] / 9, result[i] % 9]);
                sudokuB[result[i] / 9, result[i] % 9] = 0;
            }
        }

        /// <summary>
        /// 交集
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public List<int> intersection(List<int> A, List<int> B,List<int> C)
        {
            List<int> result = new List<int>();
            foreach (int i in A)
            {
                foreach(int j in B)
                {
                    if(i == j)
                    {
                        foreach (int k in C)
                            if (i == k)
                            {
                                result.Add(i);
                                break;
                            }
                        break;
                    }
                }
            }
            return result;
        }
    }
}
