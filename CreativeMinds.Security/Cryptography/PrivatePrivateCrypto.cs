// CreativeMinds.Security.Cryptography.PrivatePrivateCrypto
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CreativeMinds.Security.Cryptography
{

    public class PrivatePrivateCrypto
    {
        private string phrase = "";

        private byte[] iv;

        private byte[] key;

        public string Phrase
        {
            set
            {
                phrase = value;
                CalculateKey();
            }
        }

        private void CalculateKey()
        {
            key = new byte[24];
            iv = new byte[16];
            byte[] bytes = Encoding.Unicode.GetBytes(phrase);
            SHA384Managed sHA384Managed = new SHA384Managed();
            sHA384Managed.ComputeHash(bytes);
            byte[] hash = sHA384Managed.Hash;
            for (int i = 0; i < 24; i++)
            {
                key[i] = hash[i];
            }
            for (int i = 24; i < 40; i++)
            {
                iv[i - 24] = hash[i];
            }
        }

        public string Encrypt(string data)
        {
            CryptoStream cryptoStream = null;
            RijndaelManaged rijndaelManaged = null;
            ICryptoTransform cryptoTransform = null;
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                if (data.Length > 0)
                {
                    rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.Key = key;
                    rijndaelManaged.IV = iv;
                    cryptoTransform = rijndaelManaged.CreateEncryptor();
                    cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                    byte[] bytes = Encoding.Unicode.GetBytes(data);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
                return "";
            }
            finally
            {
                if (rijndaelManaged != null)
                {
                    rijndaelManaged.Clear();
                    rijndaelManaged = null;
                }
                cryptoTransform?.Dispose();
                memoryStream?.Close();
            }
        }

        public string Decrypt(string data)
        {
            CryptoStream cryptoStream = null;
            RijndaelManaged rijndaelManaged = null;
            ICryptoTransform cryptoTransform = null;
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                if (data.Length > 0)
                {
                    rijndaelManaged = new RijndaelManaged();
                    rijndaelManaged.Key = key;
                    rijndaelManaged.IV = iv;
                    cryptoTransform = rijndaelManaged.CreateDecryptor();
                    cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                    byte[] array = Convert.FromBase64String(data);
                    cryptoStream.Write(array, 0, array.Length);
                    cryptoStream.FlushFinalBlock();
                    return Encoding.Unicode.GetString(memoryStream.ToArray());
                }
                return "";
            }
            finally
            {
                if (rijndaelManaged != null)
                {
                    rijndaelManaged.Clear();
                    rijndaelManaged = null;
                }
                cryptoTransform?.Dispose();
                memoryStream?.Close();
            }
        }
    }

}
