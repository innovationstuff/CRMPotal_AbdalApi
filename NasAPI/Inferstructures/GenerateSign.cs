using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Inferstructures
{
    public class GenerateSign
    {

        /*
                string url = Request.Url.AbsoluteUri.Replace("&Sign", "*").Split('*')[0];
                url = Uri.UnescapeDataString(url);
                s = url;
         
         */

        public static string GetSignature(string s)
        {
            long sum = 0;
            sum = s.Length*25;
            sum = sum * sum;
            return sum.ToString();
        }


    }
}