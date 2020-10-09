using System;
using System.IO;
using System.Security.Cryptography;

namespace SendTwilloCall.Crypto
{
    public class CryptoClass
    {
        public bool EncryptMethod(string fileName, string pSid, string pToken, 
            string pNumber1, string pNumber2, string pUri1, string pUri2)
        {
            try
            {
                using (FileStream myStream = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    Aes aes = Aes.Create();

                    byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                    byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

                    //Create a CryptoStream, pass it the FileStream, and encrypt
                    //it with the Aes class.  
                    using (CryptoStream encryptStream = new CryptoStream(
                        myStream,
                        aes.CreateEncryptor(key, iv),
                        CryptoStreamMode.Write))
                    using (StreamWriter sWriter = new StreamWriter(encryptStream))
                    {
                        sWriter.WriteLine(@"{0},{1},{2},{3},{4},{5}",
                            pSid, pToken, pNumber1, pNumber2, pUri1, pUri2);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Inform the user that an exception was raised.  
                Console.WriteLine("The encryption failed: " + ex.Message);
                return false;
            }
        }

        public string DecryptMethod(string fileName)
        {
            //The key and IV must be the same values that were used
            //to encrypt the stream.
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            string retstring;
            try
            {
                //Create a file stream.

                using (FileStream myStream = new FileStream(fileName, FileMode.Open))
                {
                    //Create a new instance of the default Aes implementation class
                    Aes aes = Aes.Create();

                    //Create a CryptoStream, pass it the file stream, and decrypt
                    //it with the Aes class using the key and IV.
                    
                    using (CryptoStream cryptStream = new CryptoStream(
                       myStream,
                       aes.CreateDecryptor(key, iv),
                       CryptoStreamMode.Read))
                    using (StreamReader sReader = new StreamReader(cryptStream))
                    {
                        retstring = sReader.ReadLine();
                    }
                }
                return retstring;
            }
            catch (Exception ex)
            {
                Console.WriteLine("The decryption failed: " + ex.Message);
                return string.Empty;
            }
        }

    }
}
