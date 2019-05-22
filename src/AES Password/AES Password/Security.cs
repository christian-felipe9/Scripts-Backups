using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AES_Password
{
    /// <summary>     
    /// Classe para implenteção de segurança do porjeto 
    /// </summary> 
    public static class Security
    {
        /// <summary>     
        /// Vetor de bytes utilizados para a criptografia (Chave Externa) 
        /// NOTA: Apenas leitura.    
        /// </summary>  
        private static readonly byte[] bIV = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };
        /// <summary>     
        /// Representação de valor em base 64 da Chave Interna.
        /// O Valor representa a transformação para base64 de um conjunto de 32 caracteres (8 * 32 = 256bits)
        /// </summary>  
        //private const string Key = "WmlOZ2wzYXBpTkI0azBOMTI4ZzlGNjdIMGpydDIlJGQ="; //Base64 Resposta de 32bits

        /// <summary>     
        /// Criptografa o valor informado utilizando Rijndael/AES   
        /// </summary>     
        /// <param name="Value">Valor de Entrada</param>
        /// <param name="key">Representação de valor em base 64 da Chave Interna.</param>
        /// <returns>Saída criptografada</returns>
        public static string Encrypt(string Value, string Key)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia
                if (!string.IsNullOrEmpty(Value))
                {
                    byte[] bKey = Convert.FromBase64String(Key);
                    byte[] bText = new UTF8Encoding().GetBytes(Value);

                    Rijndael aes = new RijndaelManaged();
                    aes.KeySize = 256;

                    MemoryStream mStream = new MemoryStream();
                    CryptoStream encryptor = new CryptoStream(
                        mStream,
                        aes.CreateEncryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    encryptor.Write(bText, 0, bText.Length);
                    encryptor.FlushFinalBlock();
                    return Convert.ToBase64String(mStream.ToArray());
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
                throw new ApplicationException("ERROR: ", ex);
#else
                return null;
#endif
            }
        }

        /// <summary>     
        /// Descriptografa o valor informado utilizando Rijndael/AES com padrão de base 
        /// </summary>     
        /// <param name="Value">Entrada criptografada</param>   
        /// <param name="key">Representação de valor em base 64 da Chave Interna.</param>
        /// <returns>Saída descriptografada</returns>     
        public static string Decrypt(string Value, string Key)
        {
            try
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                MemoryStream mStream = new MemoryStream();

                // Se a string não está vazia, executa a descriptografia           
                if (!string.IsNullOrEmpty(Value))
                {
                    byte[] bKey = Convert.FromBase64String(Key);
                    byte[] bText = Convert.FromBase64String(Value);

                    Rijndael aes = new RijndaelManaged();
                    aes.KeySize = 256;

                    CryptoStream decryptor = new CryptoStream(
                        mStream,
                        aes.CreateDecryptor(bKey, bIV),
                        CryptoStreamMode.Write);

                    decryptor.Write(bText, 0, bText.Length);
                    decryptor.FlushFinalBlock();

                    return utf8.GetString(mStream.ToArray());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
                throw new ApplicationException("ERROR: ", ex);
#else
                return null;
#endif
            }
        }

    }
}