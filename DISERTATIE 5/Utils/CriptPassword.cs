using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DISERTATIE_5.Utils
{
    public static class CriptPassword
    {
        public static string ComputeHash(string input)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}