using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Empresa.Proyecto.Models
{
    public static class Encryption
    {
        private static string PassEncryt = "53GCptRDqnDb69Wa";

        public static string ToEncrypt(this string value)
        {
            return internalEncrypt(value, PassEncryt);
        }

        public static string ToDecrypt(this string value)
        {
            return internalDecrypt(value, PassEncryt);
        }

        private static string internalEncrypt(this string stringToEncrypt, string sEncryptionKey)
        {
            byte[] key = { };

            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

            byte[] inputByteArray;

            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                return (Convert.ToBase64String(ms.ToArray()));

            }
            catch (System.Exception ex)
            {
                return ex.Message;

            }
        }

        private static string internalDecrypt(this string stringToDecrypt, string sEncryptionKey)
        {
            byte[] key = { };

            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
            byte[] inputByteArray = new byte[stringToDecrypt.Length];

            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());

            }
            catch (System.Exception ex)
            {
                return ex.Message;

            }
        }

    }
}
