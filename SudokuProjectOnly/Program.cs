using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuProjectOnly.SudokuProjectOnly
{
    class Program
    {
        static void Main(string[] args)
        {
            uniqueness u = new uniqueness();

            //u.creat(100000);

            if (args.Length != 2||args[0]!="-c")
            {
                Console.WriteLine("error:请参考以下格式进行输入;");
                Console.WriteLine("sudoku.exe -c 20");
                return;
            }

            string N = args[1];
            int result;
            if (int.TryParse(N, out result))
            {
                if (result > 0 && result <= 1000000)
                {
                    Console.WriteLine("Begin");
                    u.creat(int.Parse(N));
                    Console.WriteLine("Finish");
                }
                else Console.WriteLine("error:输入范围1<N<=1000000");
            }
            else Console.WriteLine("error:请输入正整数");

        }
    }
}
