using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.service.Helper
{
    public static class StringExt
    {
        public static string TruncateLongString(this string str, int maxLength)
        {
            if (maxLength < 0) { throw new ArgumentNullException("maxLength could not be less than zero"); }
            if (string.IsNullOrEmpty(str)) { 
                return str; 
            }
            else
            {
                return str.Substring(0, Math.Min(str.Length, maxLength));
            }
            
        }
    }
}
