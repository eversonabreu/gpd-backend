using System;
using System.Security.Cryptography;
using System.Text;

namespace EAN.GPD.Infrastructure.Utils
{
    public static class Criptografia
    {
        private const string chaveCodificacao = "gPd@a2#E[(2018)]*n";

        public static string Codificar(string conteudo)
        {
            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                var utf8 = new UTF8Encoding();
                using (var serviceProvider = new TripleDESCryptoServiceProvider
                {
                    Key = hashProvider.ComputeHash(utf8.GetBytes(chaveCodificacao)),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    var data = utf8.GetBytes(conteudo);
                    ICryptoTransform Encryptor = serviceProvider.CreateEncryptor();
                    var result = Encryptor.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result);
                }
            }
        }

        public static string Descodificar(string conteudo)
        {
            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                var utf8 = new UTF8Encoding();
                using (var serviceProvider = new TripleDESCryptoServiceProvider
                {
                    Key = hashProvider.ComputeHash(utf8.GetBytes(chaveCodificacao)),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    var data = Convert.FromBase64String(conteudo);
                    ICryptoTransform Decryptor = serviceProvider.CreateDecryptor();
                    var result = Decryptor.TransformFinalBlock(data, 0, data.Length);
                    return utf8.GetString(result);
                }
            }
        }
    }
}
