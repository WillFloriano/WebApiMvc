using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace WEB.Helper
{
    public class Password
    {
        public static string createHash(string password)
        {
            SHA1 hash = SHA1.Create();

            var pwBytes = Encoding.Default.GetBytes(password);

            var pwhashed = hash.ComputeHash(pwBytes);

            return Convert.ToHexString(pwhashed);

            //using(SHA1Managed sha1 = new SHA1Managed())
            //{
            //    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            //    var sb = new StringBuilder(hash.Length * 2);

            //    foreach(byte pw in hash)
            //    {
            //        sb.Append(pw.ToString("X2"));
            //    }

            //    return sb.ToString();
            //}
        }

    }
}
