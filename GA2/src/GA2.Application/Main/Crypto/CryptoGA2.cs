using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GA2.Application.Main
{
    /// <summary>
    /// Cryptografia GA2
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>30/02/2021</date>
    public class CryptoGA2 : ICryptoGA2
    {
        /// <summary>
        /// Decryptar cadena
        /// </summary>
        /// <param name="cipherString">Cipher de configuracion</param>
        /// <param name="pKey">Clave secreta</param>
        /// <param name="useHashing">Hash de configuracion aplicacion</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>30/04/2021</date>
        /// <returns>Cadena decryptada</returns>
        public string DecryptMD5(string cipherString, string pKey, bool useHashing = true)
        {
            byte[] array = Convert.FromBase64String(cipherString);

            byte[] key;

            if (useHashing)
            {
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                key = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(pKey));
                mD5CryptoServiceProvider.Clear();
            }
            else
            {
                key = Encoding.UTF8.GetBytes(pKey);
            }

            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
            byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            tripleDESCryptoServiceProvider.Clear();

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Encryptar cadena clave
        /// </summary>
        /// <param name="toEncrypt">Cadena a encryptar</param>
        /// <param name="pKey">Clave clave secreta</param>
        /// <param name="useHashing">Hash de configuracion</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>30/04/2021</date>
        /// <returns>Cadena encryptada</returns>
        public string EncryptMD5(string toEncrypt, string pKey, bool useHashing = true)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] key;

            if (useHashing)
            {
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                key = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(pKey));
                mD5CryptoServiceProvider.Clear();
            }
            else
            {
                key = Encoding.UTF8.GetBytes(pKey);
            }

            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
            byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            tripleDESCryptoServiceProvider.Clear();

            return Convert.ToBase64String(array, 0, array.Length);
        }

        /// <summary>
        /// Generador cadena hash 
        /// </summary>
        /// <param name="pCadenaString">Cadena que se convertirar en hash</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>30/04/2021</date>
        /// <returns>Cadena en formato hash</returns>
        public string GenerarHashString(string pCadenaString)
        {
            using MD5 mD = MD5.Create();
            return BitConverter.ToString(mD.ComputeHash(Encoding.UTF8.GetBytes(pCadenaString))).Replace("-", string.Empty);
        }

        /// <summary>
        /// Método para cifrar por AES un texto plano
        /// </summary>
        /// <param name="plainText"> Texto a cifrar </param>
        /// <param name="key2"> Valor del key </param>
        /// <param name="IV"> Valor del IV </param>
        /// <returns> Cadena cifrada </returns>
        /// <exception cref="ArgumentNullException"> Generación de excepción lanzada </exception>
        public string EncryptStringAes(string plainText, string key2, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("No fue definido un texto valido para el cifrado.");
            if (key2 == null || key2.Length <= 0)
                throw new ArgumentNullException("No fue definido un key valido para el cifrado.");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("No fue definido un IV valido para el cifrado.");
            byte[] encrypted;
            byte[] key;

            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            key = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key2));
            mD5CryptoServiceProvider.Clear();

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted, 0, encrypted.Length);
        }

        /// <summary>
        /// Método para descifrar un texto plano cifrado
        /// </summary>
        /// <param name="cipherText"> Texto cifrado </param>
        /// <param name="key2"> Valor del key </param>
        /// <param name="IV"> Valor del IV </param>
        /// <returns> Cadena de texto legible decifrada </returns>
        /// <exception cref="ArgumentNullException"> Generación de excepción lanzada </exception>
        public string DecryptStringAes(string cipherText, string key2, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("No fue definido un texto valido para el descifrado.");
            if (key2 == null || key2.Length <= 0)
                throw new ArgumentNullException("No fue definido un key valido para el cifrado.");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("No fue definido un IV valido para el cifrado.");

            string plaintext = null;
            byte[] key;
            byte[] cipherByte = Convert.FromBase64String(cipherText);

            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            key = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key2));
            mD5CryptoServiceProvider.Clear();
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
               
                using (MemoryStream msDecrypt = new MemoryStream(cipherByte))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }


    }
}
