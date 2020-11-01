using System;
using System.Security.Cryptography;
using System.Text;

namespace SecuringWebApiUsingJwtAuthentication.Helpers
{
    public class HashingHelper
    {
        public static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
    }
}
