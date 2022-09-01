#region References
using System.Security.Cryptography;
using System.Text;
using Validator.Infracturure.Interfaces;
#endregion

namespace Validator.Infracturure.Tool.SHAs
{
    public class SHA256 : ISHA256
    {
        public string GetSHA256(string text)
        {
            if (text.Equals(null))
                throw new ArgumentNullException(null);

            SHA256CryptoServiceProvider provider = new();

            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }
    }
}
