using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Store.Models
{
    public class UtilitiesModel
    {
        public static void AddCookie(string Name,string[] Keys, string[] Values, bool Expire,HttpResponseBase response)
        {
            HttpCookie _cookie = new HttpCookie(Name);
            for (int i = 0; i < Keys.Length; i++)
                _cookie.Values.Add(Encrypt(Keys[i]).Replace("==", ""), Encrypt(Values[i]).Replace("==", ""));
            _cookie.Expires = Expire ? DateTime.Now.AddYears(1) : DateTime.Now.AddYears(-1);
            response.Cookies.Add(_cookie);
        }

        public static string[] ReadCookie(string Name, string[] Keys, HttpRequestBase request)
        {
            string[] Values = new string[Keys.Length];
            for (int i = 0; i < Keys.Length; i++)
                Values[i] = Decrypt(request.Cookies[Name][Encrypt(Keys[i]).Replace("==", "")]);
            return Values;

        }

        public static string ReadCookie(string Name, string Key, HttpRequestBase request)
        {
            string Value;
                Value = Decrypt(request.Cookies[Name][Encrypt(Key).Replace("==", "")]);
            return Value;
        }

        public static void DeleteCookie(string CookieName, HttpResponseBase response)
        {
            response.Cookies.Remove(CookieName);
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "$N@P4$0FTDEV7ISTL00P!EN";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x50, 0x88, 0x65, 0x2c, 0x2e, 0x4f, 0x6d, 0x0a, 0x66, 0x56, 0x65, 0x77, 0xee });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "$N@P4$0FTDEV7ISTL00P!EN";
                byte[] cipherBytes;

                try { cipherBytes = Convert.FromBase64String(cipherText); }
                catch { cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+")); }
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x50, 0x88, 0x65, 0x2c, 0x2e, 0x4f, 0x6d, 0x0a, 0x66, 0x56, 0x65, 0x77, 0xee });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch { cipherText = "Invalid"; }
            return cipherText;

        }

        public static string GetGenratedID(string ID)
        {
            string result = "";
            byte[] originalBytes, encryptedBytes;
            string NewID = ID.Replace("_", "");
            MD5 md5 = new MD5CryptoServiceProvider();
            int IDD = Convert.ToInt16(NewID);
            int Result = 0;
            Result = (IDD * 3) + (IDD * 2);
            originalBytes = Encoding.Default.GetBytes(Result.ToString());
            encryptedBytes = md5.ComputeHash(originalBytes);
            result = BitConverter.ToString(encryptedBytes);
            return result.Replace("-", "");
        }
    }
}