using SendTwilloCall.Crypto;
using System;
using System.IO;
using System.Windows.Forms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace SendTwilloCall
{
    public partial class Form1 : Form
    {
        public string accountSID;
        public string authorizationToken;
        public string CallerIDNumber1; //Phone Number 1
        public string CallerIDNumber2; //Phone Number 2
        public string uri1;
        public string uri2;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Visible = false;
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("^([0-9]( |-)?)?(\\(?[0-9]{3}\\)?|[0-9]{3})( |-)?([0-9]{3}( |-)?[0-9]{4}|[a-zA-Z0-9]{7})$");

            if (rx.IsMatch(phoneNumber.Text) && GetKeys())
            {
                DoSendTwillo(phoneNumber.Text);
            }
            else
            {
                MessageBox.Show("Enter a valid phone number.");
            }
        }

        public bool GetKeys()
        {
            try
            {
                CryptoClass cc = new CryptoClass();
                var fileName = "keyfile.key";
                var GetKeys = cc.DecryptMethod(fileName).Split(',');

                this.accountSID = GetKeys[0].Trim();
                this.authorizationToken = GetKeys[1].Trim();
                this.CallerIDNumber1 = GetKeys[2].Trim();
                this.CallerIDNumber2 = GetKeys[3].Trim();
                this.uri1 = GetKeys[4].Trim();
                this.uri2 = GetKeys[5].Trim();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DoSendTwillo(string telephone)
        {
            try
            {
                // Find your Account Sid and Auth Token at twilio.com/console
                TwilioClient.Init(accountSID, authorizationToken);
                PhoneNumber toNum = new PhoneNumber("+1" + telephone);  //Telephone Number entered by user
                PhoneNumber fromNum;
                string uriValue;

                //using milliseconds as a random dial controller
                int randomSeconds = System.DateTime.Now.Millisecond;
                if (randomSeconds <= 500)
                {
                    fromNum = new PhoneNumber(CallerIDNumber1);
                    uriValue = uri1;
                }
                else
                {
                    fromNum = new PhoneNumber(CallerIDNumber2);
                    uriValue = uri2;
                }

                if (accountSID != "testsid")
                {
                    var call = CallResource.Create(toNum, fromNum, url: new Uri(uriValue));
                    textBox2.Visible = true; //unhide the textbox and show the results.
                    textBox2.Text = "Call SID: " + call.Sid;  //display the results of the phone call.
                }

                phoneNumber.Text = string.Empty;  //clear out on success
                return true;

            }
            catch (Exception ex)
            {
                phoneNumber.Text = string.Empty;  //clear out on failure
                MessageBox.Show("There was an exception in the code: " + ex.Message);
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.SetCurrentDirectory(@"C:\Users\alexh\documents");
        }
    }
}