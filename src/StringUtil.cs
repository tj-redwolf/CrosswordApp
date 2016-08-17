using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordGenerator
{
    class StringUtil
    {
        public static string ReplaceAt(string s, int index, char c)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Input String");
            }
            StringBuilder builder = new StringBuilder(s);
            builder[index] = c;
            return builder.ToString();
        }

    }
}
