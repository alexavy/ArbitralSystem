using System;
using System.Security.Cryptography;
using System.Text;

namespace ArbitralSystem.Common.Helpers
{
    public static class HashHelper
    {
        public static long ComputeMD5(string input)
        {
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Math.Abs(BitConverter.ToInt64(hashed, 0));
        }
    }
}