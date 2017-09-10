using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuProjectOnly.SudokuProjectOnly
{
    class RandomNum
    {
        /// <summary>
        /// 随机序列生成
        /// </summary>
        /// <param name="value"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int[] GetRandomNum(int[] value, int num)
        {
            Random randomNum = new Random();
            int[] resultNums = new int[num];
            int temp = 0;
            int nums = value.Length;
            for (int i = 0; i < num; i++)
            {
                temp = randomNum.Next(0, nums - 1);
                resultNums[i] = value[temp];
                value[temp] = value[nums - 1];
                nums--;
            }
            return resultNums;
        }
    }
}
