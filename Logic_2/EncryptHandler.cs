using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Logic
{
    public class EncryptHandler
    {
        private const int Keysize = 256;

        private const int DerivationIterations = 1000;

        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        public static string Encrypt(string Text, string SaltKey)
        {
            if (Text == "")
            {
                throw new Exception("There was no text given");
            }
            else if (SaltKey == "")
            {
                throw new Exception("There was no salt key given");
            }

            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(Text);

            byte[] KeyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var SymmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] CipherTextBytes;

            using (var MemoryStream = new MemoryStream())
            {
                using (var CryptoStream = new CryptoStream(MemoryStream, Encryptor, CryptoStreamMode.Write))
                {
                    CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                    CryptoStream.FlushFinalBlock();
                    CipherTextBytes = MemoryStream.ToArray();
                    CryptoStream.Close();
                }
                MemoryStream.Close();
            }
            return Convert.ToBase64String(CipherTextBytes);
        }

        private static Random Random = new Random();
        public static string RandomString(int Length)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            return new string(Enumerable.Repeat(Chars, Length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}