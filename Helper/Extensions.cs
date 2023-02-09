using System.Security.Cryptography;
using System.Text;

namespace EmsiSoft.Extension
{
    public static class Extensions
    {
        public static string Hash(this string input)
        {
            using var sha1 = SHA1.Create();
            return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        public static string Hash(this Guid input)
        {
            return Hash(input.ToString());
        }
    }
}