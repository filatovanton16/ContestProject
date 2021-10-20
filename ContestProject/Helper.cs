using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestProject
{
    public static class Helper
    {
        public static string ReduceSpaces(string initStr)
        {
            string reducedStr = initStr.Replace("  ", " ");
            
            while(reducedStr != initStr)
            {
                initStr = reducedStr;
                reducedStr = initStr.Replace("  ", " ");
            }

            return reducedStr;
        }
    }
}
