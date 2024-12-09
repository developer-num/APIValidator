#region References
using System.Security.Cryptography;
using System.Text;
using Validator.Infracturure.Interfaces;
#endregion

namespace Validator.Infracturure.Tool.SHAs
{
    public class SHA384 : ISHA384
    {
        public string GetSHA384(string text)
        {
            if (text.Equals(null))
                throw new ArgumentNullException(null);

            UnicodeEncoding UE = new();
            Byte[] HashValue, MessageBytes = UE.GetBytes(text);
            SHA384Managed SHhash = new();
            string strHex = string.Empty;

            HashValue = SHhash.ComputeHash(MessageBytes);
            foreach (Byte b in HashValue)
            {
                strHex += string.Format("{0:x2}", b);
            }
            return strHex;
        }
    }
}
