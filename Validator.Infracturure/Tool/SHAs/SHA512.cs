#region References
using System.Security.Cryptography;
using System.Text;
using Validator.Infracturure.Interfaces;
#endregion

namespace Validator.Infracturure.Tool.SHAs
{
    public class SHA512 : ISHA512
    {
        public string GetSHA512(string text)
        {
            if (text.Equals(null))
                throw new ArgumentNullException(null);

            byte[] message = Encoding.UTF8.GetBytes(text);
            byte[] hashValue;
            SHA512Managed hashString = new();
            string hex = String.Empty;
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
                hex += String.Format("{0:x2}", x);

            return hex;
        }
    }
}
