using System.Security.Cryptography;
using System.Text;

namespace JuliusCaesarCryptographyChallengeCodenation.Tools
{
    public class Sha1Generator
    {
        public string Hash(string text)
        {
            byte[] hash;

            using (var sha1 = SHA1.Create())
                hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(text));

            var stringBuilder = new StringBuilder();

            foreach (var b in hash)
                stringBuilder.AppendFormat(b.ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
