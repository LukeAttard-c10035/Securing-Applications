using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Presentation.Utilities
{
    public class Cryptographic
    {
        public Stream HybridEncryption(Stream file, string publicKey)
        {
            file.Position = 0;

            #region Symmetric part
            Rijndael myAlg = Rijndael.Create();

            myAlg.GenerateIV(); //16 bytes
            myAlg.GenerateKey(); //32 bytes


            CryptoStream cs = new CryptoStream(file, myAlg.CreateEncryptor(), CryptoStreamMode.Read);
            MemoryStream cipher = new MemoryStream();
            cs.CopyTo(cipher);
            cs.Close();
            #endregion

            #region Asymmetric part
            RSA myAsymAlg = RSACryptoServiceProvider.Create();
            myAsymAlg.FromXmlString(publicKey);

            byte[] encryptedSecretKey = myAsymAlg.Encrypt(myAlg.Key, RSAEncryptionPadding.Pkcs1);
            byte[] encryptedIv = myAsymAlg.Encrypt(myAlg.IV, RSAEncryptionPadding.Pkcs1);

            #endregion

            #region Saving everything into a single file
            MemoryStream outputFile = new MemoryStream();
            outputFile.Write(encryptedSecretKey, 0, encryptedSecretKey.Length);
            outputFile.Write(encryptedIv, 0, encryptedIv.Length);
            cipher.Position = 0;
            cipher.CopyTo(outputFile);
            #endregion
            return outputFile;
        }

        public Stream HybridDecryption(Stream cipher, string privateKey)
        {
            cipher.Position = 0;
            //reading the encrypted key and iv
            byte[] encryptedKey = new byte[64];
            cipher.Read(encryptedKey, 0, 64); //file pointer will move 128 positions


            byte[] encryptedIv = new byte[64];
            cipher.Read(encryptedIv, 0, 64); //file pointer will move ANOTHER 128 positions i.e. 255

            MemoryStream encryptedFileContent = new MemoryStream();
            cipher.CopyTo(encryptedFileContent); //file pointer will move to the eof reading what's left
            encryptedFileContent.Position = 0;

            RSA myAlg_Asym  = RSACryptoServiceProvider.Create();
            myAlg_Asym.FromXmlString(privateKey);
            byte[] originalKey = myAlg_Asym.Decrypt(encryptedKey, RSAEncryptionPadding.Pkcs1);
            byte[] originalIv = myAlg_Asym.Decrypt(encryptedIv, RSAEncryptionPadding.Pkcs1);

            Rijndael myAlg_Sym = Rijndael.Create();
            myAlg_Sym.Key = originalKey;
            myAlg_Sym.IV = originalIv;
            CryptoStream cs = new CryptoStream(encryptedFileContent, myAlg_Sym.CreateDecryptor(), CryptoStreamMode.Read);
            MemoryStream originalFileData = new MemoryStream();
            originalFileData.Position = 0;
            cs.CopyTo(originalFileData);
            cs.Close();

            //...missing code which you need to implement
            //1. asymmetric decrypt the encrypted key and iv
            //2. symmetrically decrypt the cipher
            return originalFileData;
        }


        //1. Digital Signing is a mitigation against repudiation (when an attacker denies a malicious activity)
        //2. You sign using the private key

        public string DigitalSigning(string data, string privateKey)
        {
            RSA myAlg = RSACryptoServiceProvider.Create();
            myAlg.FromXmlString(privateKey);

            string hashedString = Hash(data);
            byte[] digest = Convert.FromBase64String(hashedString);

            byte[] signature =
                myAlg.SignHash(digest, new HashAlgorithmName("SHA512"), RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signature);

        }

        //finish this
        public bool DigitalVerification(string data, string signature, string publicKey)
        {
            RSA myAlg = RSACryptoServiceProvider.Create();
            myAlg.FromXmlString(publicKey);

            string hashedString = Hash(data);
            byte[] digest = Convert.FromBase64String(hashedString);

            //myAlg.VerifyHash(digest, new HashAlgorithmName("SHA512"), RSASignaturePadding.Pkcs1);
            myAlg.SignHash(digest, new HashAlgorithmName("SHA512"), RSASignaturePadding.Pkcs1);
            //you have to use VerifyHash instead of SignHash
            return true;
        }

        public string Hash(string originalText)
        {
            //1. when input is human readable you have to use Encoding to convert to bytes[]
            //2. when you have 100% certainty that data being handled is already base64 format you have to
            //   use Convert.ToBase64String / Convert.FromBase64String
            //note: every cryptographic algorithm outputs base64 format
            //e.g. Md5 (weak & broken), Sha1 (weak & broken), Sha256, Sha512

            SHA512 myAlg = SHA512.Create();
            byte[] myData = Convert.FromBase64String(originalText); //Encoding.UTF32.GetBytes(originalText);
            byte[] digest = myAlg.ComputeHash(myData);

            return Convert.ToBase64String(digest);
        }
    }
}
