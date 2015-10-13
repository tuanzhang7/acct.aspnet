using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acct.common.Helper
{
    public static class NRICValidate
    {
        public static bool validateNIRC(string strNRIC)
        {
            strNRIC=strNRIC.Trim();
            string ic = strNRIC.ToUpper();

            if (ic.Length != 9)
            {
                return false;
            }
            int result = 0;
            if (int.TryParse(ic.Substring(1, 7), out result) == false)
            {
                return false;
            }
            char[] icArray = ic.ToCharArray();
            int[] icArrayInt = new int[8];
            for (int i = 1; i < 8; i++)
            {
                icArrayInt[i] = int.Parse(icArray[i].ToString());
            }
            icArrayInt[1] *= 2;
            icArrayInt[2] *= 7;
            icArrayInt[3] *= 6;
            icArrayInt[4] *= 5;
            icArrayInt[5] *= 4;
            icArrayInt[6] *= 3;
            icArrayInt[7] *= 2;

            int weight = 0;
            for (int i = 1; i < 8; i++)
            {
                weight += icArrayInt[i];
            }

            int offset = (icArray[0] == 'T' || icArray[0] == 'G') ? 4 : 0;
            int temp = (offset + weight) % 11;

            char[] st = { 'J', 'Z', 'I', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A' };
            char[] fg = { 'X', 'W', 'U', 'T', 'R', 'Q', 'P', 'N', 'M', 'L', 'K' };

            char theAlpha = ' ';
            if (icArray[0] == 'S' || icArray[0] == 'T') { theAlpha = st[temp]; }
            else if (icArray[0] == 'F' || icArray[0] == 'G') { theAlpha = fg[temp]; }

            if (icArray[8] != theAlpha)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
