using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SANYUKT.Commonlib.Utility
{
    public class AesEncDsc512
    {
        private static int pswdIterations = 65536;
        private static int keySize = 512;
        private static readonly byte[] ivBytes = {
    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
};


        //public static string Decrypt(string encryptedText, string key)
        //{
        //    try
        //    {
        //        byte[] saltBytes = Encoding.UTF8.GetBytes(key);
        //        byte[] encryptedTextBytes = HexToByteArray(encryptedText);

        //        var factory = new Rfc2898DeriveBytes(key, saltBytes, pswdIterations);
        //        byte[] keyBytes = factory.GetBytes(keySize / 8);
        //        var secretKey = new SymmetricSecurityKey(keyBytes);
        //        var iv = new byte[16]; // Assuming the IV is 16 bytes long
        //        var localIvParameterSpec = new IvParameterSpec(iv);

        //        var cipher = Aes.Create();
        //        cipher.Mode = CipherMode.CBC;
        //        cipher.Padding = PaddingMode.PKCS7;
        //        cipher.Key = secretKey.Key;
        //        cipher.IV = localIvParameterSpec.IV;

        //        byte[] decryptedTextBytes = cipher.DecryptFinal(encryptedTextBytes);
        //        return Encoding.UTF8.GetString(decryptedTextBytes);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Exception while decrypting data: {e}");
        //    }

        //    return null;
        //}

        private static byte[] HexToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
        private static string ByteToHex(byte[] byData)
        {
            StringBuilder sb = new StringBuilder(byData.Length * 2);
            foreach (byte b in byData)
            {
                int v = b & 0xFF;
                if (v < 16)
                    sb.Append('0');
                sb.Append(v.ToString("X"));
            }
            return sb.ToString();
        }

        private static byte[] Hex2ByteArray(string sHexData)
        {
            byte[] rawData = new byte[sHexData.Length / 2];
            for (int i = 0; i < rawData.Length; ++i)
            {
                int index = i * 2;
                int v = int.Parse(sHexData.Substring(index, 2), System.Globalization.NumberStyles.HexNumber);
                rawData[i] = (byte)v;
            }
            return rawData;
        }



    }
}

