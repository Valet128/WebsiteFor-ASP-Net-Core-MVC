using System.Security.Cryptography;
using System.Text;

namespace ShvedovaAV.Services
{
    public class HashService
    {
        public static string GetHash(string str)
        {
            string salt = "salt12094";
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str + salt));
            return  Convert.ToBase64String(hash);
        }
    }
}
