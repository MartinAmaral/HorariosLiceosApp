using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public static class StringExtensions
    {
       /*public static string PadCenter(this string str, int amount)
        {
            amount -= str.Length;
            var left = amount - amount / 2;
            var right = amount / 2;
            return new string(' ', left) + str + new string(' ', right); ;
        }
        */
        public static string PadCenter(this string str, int amount)
        {
            int strLength = str.Length;
            int paddingAmount = amount - strLength;
            int left = paddingAmount / 2;
            int right = paddingAmount - left;

            return new string(' ', left) + str + new string(' ', right);
        }

    }
}
