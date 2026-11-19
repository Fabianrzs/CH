namespace GA2.Application.Main
{
    /// <summary>
    /// Interface Injectable
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>30/04/2021</date>
    public interface ICryptoGA2
    {
        /// <summary>
        /// Decriptador de cadenas
        /// </summary>
        /// <param name="cipherString">Cadena encriptada</param>
        /// <param name="pKey">Palabra secreta de configuraciones</param>
        /// <param name="useHashing">Uso hash por default</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>30/04/2021</date>
        /// <returns>Cadena desencryptada</returns>
        string DecryptMD5(string cipherString, string pKey, bool useHashing = true);

        /// <summary>
        /// Encryptador de cadenas
        /// </summary>
        /// <param name="toEncrypt">Cadena a encryptar</param>
        /// <param name="pKey">Clave secreta</param>
        /// <param name="useHashing">Uso de hash por default</param>
        /// <returns></returns>
        string EncryptMD5(string toEncrypt, string pKey, bool useHashing = true);
        string GenerarHashString(string pCadenaString);
        /// <summary>
        /// Método para cifrar por AES un texto plano
        /// </summary>
        /// <param name="plainText"> Texto a cifrar </param>
        /// <param name="key2"> Valor del key </param>
        /// <param name="IV"> Valor del IV </param>
        /// <returns> Cadena cifrada </returns>
        string EncryptStringAes(string plainText, string key2, byte[] IV);
        /// <summary>
        /// Método para descifrar un texto plano cifrado
        /// </summary>
        /// <param name="cipherText"> Texto cifrado </param>
        /// <param name="key2"> Valor del key </param>
        /// <param name="IV"> Valor del IV </param>
        /// <returns> Cadena de texto legible decifrada </returns>
        string DecryptStringAes(string cipherText, string key2, byte[] IV);
    }
}