using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for EncryptionEngine
/// </summary>
namespace Engine
{
    public class EncryptionEngine
    {
        public EncryptionEngine()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = GetKey(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string input, string key)
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = GetKey(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }

        public static byte[] GetKey(string Key)
        {
            using (var sha256 = SHA256.Create())
            {
                // Compute hash from Key
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(Key));
                // Take first 24 bytes of hash as key
                byte[] key = new byte[24];
                Array.Copy(hash, key, 24);
                return key;
            }
        }

    }
}