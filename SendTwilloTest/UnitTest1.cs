using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SendTwilloCall;
using SendTwilloCall.Crypto;
using System.Windows.Forms;

namespace SendTwilloTest
{
    [TestClass]
    public class UnitTest1
    {
        CryptoClass testCrypto;
        Form1 form;

        [TestInitialize]
        public void TestInitialize()
        {
            form = new Form1();
            testCrypto = new CryptoClass();
            Directory.SetCurrentDirectory(@"C:\Users\alexh\documents");
        }

        [TestMethod]
        public void TestEncryption()
        {
            string fileName = "testfile.key";

            var testSid = "testsid";
            var testToken = "testtoken";
            var testNum1 = "9876543210";
            var testNum2 = "1234567890";
            var uri1 = @"https://google.com";
            var uri2 = @"https://google.com";

            var testEncrpt = testCrypto.EncryptMethod(fileName, testSid, testToken, testNum1, testNum2, uri1, uri2);

            Assert.IsTrue(testEncrpt);
        }

        [TestMethod]
        public void TestGetKeys()
        {
            Assert.IsTrue(form.GetKeys());
        }

        [TestMethod]
        public void TestSendTwillo()
        {
            var testSid = "testsid";
            var testToken = "testtoken";
            var testNum1 = "9876543210";
            var testNum2 = "1234567890";
            var uri1 = @"https://google.com";
            var uri2 = @"https://google.com";

            form.accountSID = testSid;
            form.authorizationToken = testToken;
            form.CallerIDNumber1 = testNum1;
            form.CallerIDNumber2 = testNum2;
            form.uri1 = uri1;
            form.uri2 = uri2;
                        
            var twilloBool = form.DoSendTwillo(@"4025551212");

            Assert.IsTrue(twilloBool);

        }

        [TestMethod]
        public void TestDecryption()
        {
            string fileName = "testfile.key";

            var testSid = "testsid";
            var testToken = "testtoken";
            var testNum1 = "9876543210";
            var testNum2 = "1234567890";
            var uri1 = @"https://google.com";
            var uri2 = @"https://google.com";

            testCrypto.EncryptMethod(fileName, testSid, testToken, testNum1, testNum2, uri1, uri2);
            var testKey = testCrypto.DecryptMethod(fileName);

            Assert.IsTrue(!string.IsNullOrEmpty(testKey));

            var testStrArr = testKey.Split(',');

            Assert.AreEqual(testSid, testStrArr[0].Trim());
            Assert.AreEqual(testToken, testStrArr[1].Trim());
            Assert.AreEqual(testNum1, testStrArr[2].Trim());
            Assert.AreEqual(testNum2, testStrArr[3].Trim());
            Assert.AreEqual(uri1, testStrArr[4].Trim());
            Assert.AreEqual(uri2, testStrArr[5].Trim());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists("testfile.key"))
            {
                File.Delete("testfile.key");
            }
        }

    }
}
