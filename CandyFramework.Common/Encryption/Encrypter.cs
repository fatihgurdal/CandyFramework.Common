using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.Common.Encryption
{
    public class Encrypter
    {
        private const int Keysize = 256;

        private const int DerivationIterations = 1000;

        #region - Encrypt -
        /// <summary>
        /// Candy Frameworj Metin Şifreleme
        /// </summary>
        /// <param name="plainText">Şifrelenecek metin</param>
        /// <param name="passPhrase">Şifrelenecek metinde kullanılaca private key</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string passPhrase)
        {
            return Encrypt(plainText, passPhrase, CipherMode.CBC);
        }
        /// <summary>
        /// Candy Frameworj Metin Şifreleme
        /// </summary>
        /// <param name="plainText">Şifrelenecek metin</param>
        /// <param name="passPhrase">Şifrelenecek metinde kullanılaca private key</param>
        /// <param name="cipherMode">Şifreleme için kullanılacak blok şifreleme modunu belirtir.</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string passPhrase, CipherMode cipherMode)
        {
            return Encrypt(plainText, passPhrase, cipherMode, PaddingMode.PKCS7);
        }
        /// <summary>
        /// Candy Frameworj Metin Şifreleme
        /// </summary>
        /// <param name="plainText">Şifrelenecek Metin</param>
        /// <param name="passPhrase">Şifrelenen metnin şifresi nedir?</param>
        /// <param name="cipherMode">Şifreleme için kullanılacak blok şifreleme modunu belirtir.</param>
        /// <param name="paddingMode">Mesaj veri bloğu daha kısa olduğunda uygulanacak dolgu türünü belirtir</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string passPhrase, CipherMode cipherMode, PaddingMode paddingMode)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = cipherMode;
                    symmetricKey.Padding = paddingMode;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region - Decrypt -
        /// <summary>
        /// Candy Frameworj Metin Şifre Çözücü
        /// </summary>
        /// <param name="plainText">Şifrelenmiş Metin</param>
        /// <param name="passPhrase">Şifrelenmiş metnin şifresi nedir?</param>
        /// <returns></returns>
        public static string Decrypt(string plainText, string passPhrase)
        {
            return Decrypt(plainText, passPhrase, CipherMode.CBC);
        }
        /// <summary>
        /// Candy Frameworj Metin Şifre Çözücü
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase">Şifrelenmiş metnin şifresi nedir?</param>
        /// <param name="cipherMode">Şifreleme için kullanılacak blok şifreleme modunu belirtir.</param>
        /// <returns></returns>
        public static string Decrypt(string plainText, string passPhrase, CipherMode cipherMode)
        {
            return Decrypt(plainText, passPhrase, cipherMode, PaddingMode.PKCS7);
        }
        /// <summary>
        /// Candy Frameworj Metin Şifre Çözücü
        /// </summary>
        /// <param name="cipherText">Şifreli Metin</param>
        /// <param name="passPhrase">Şifrelenmiş metnin şifresi nedir?</param>
        /// <param name="cipherMode">Şifreleme için kullanılacak blok şifreleme modunu belirtir.</param>
        /// <param name="paddingMode">Mesaj veri bloğu daha kısa olduğunda uygulanacak dolgu türünü belirtir</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string passPhrase, CipherMode cipherMode, PaddingMode paddingMode)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = cipherMode;
                    symmetricKey.Padding = paddingMode;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
