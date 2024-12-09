using System.Security.Cryptography;
using System.Text;
using Validator.Infracturure.Interfaces;

namespace Validator.Infracturure.Tool.SHAs
{
    public class SHA1 : ISHA1
    {
        public string GetSHA1(string text)
        {
            if (text.Equals(null))
                throw new ArgumentNullException(null);

            SHA1CryptoServiceProvider SHA1 = new();
            byte[] vectoBytes = Encoding.UTF8.GetBytes(text);
            byte[] inArray = SHA1.ComputeHash(vectoBytes);
            SHA1.Clear();
            return Convert.ToBase64String(inArray);
        }
    }
}
