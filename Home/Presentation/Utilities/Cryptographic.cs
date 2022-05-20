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
            cipher.Position =0;
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
            byte[] encryptedKey = new byte[128];
            cipher.Read(encryptedKey, 0, 128); //file pointer will move 128 positions


            byte[] encryptedIv = new byte[128];
            cipher.Read(encryptedIv, 0, 128); //file pointer will move ANOTHER 128 positions i.e. 255

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

            return originalFileData;
        }


        public string DigitalSigning(byte[] data, string privateKey)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(privateKey);

           byte[] signature = myAlg.SignData(data, SHA512.Create());

            return Convert.ToBase64String(signature);

        }

        public bool DigitalVerification(string data, string signature, string publicKey)
        {
            RSA myAlg = RSACryptoServiceProvider.Create();
            myAlg.FromXmlString(publicKey);

            byte[] dataByte = Convert.FromBase64String(data);
            byte[] signatureByte = Convert.FromBase64String(signature);
            return myAlg.VerifyData(dataByte, signatureByte, new HashAlgorithmName("SHA512"), RSASignaturePadding.Pkcs1);
        }
    }
}
