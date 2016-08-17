using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Test.Util.Common
{
    public partial class EncryptHelper
    {
        private static readonly string encryptKey = "jyzx_z(j%j+y%z@&gt;:x./2$g3";
        private static readonly string decryptKey = "jyzx_z(j%j+y%z@&gt;:x./2$g3";

        #region "3des加密字符串"

        /// <summary>
        ///     DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptString(string encryptString)
        {
            try
            {
                //return encryptString;
                var rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
                var inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dCSP = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray()).Replace("+", "@Jia@");
            }
            catch
            {
                return encryptString;
            }
        }

        #endregion

        #region "3des解密字符串"

        /// <summary>
        ///     DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptString(string decryptString)
        {
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
                var inputByteArray = Convert.FromBase64String(decryptString);
                var DCSP = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        #endregion
    }
}