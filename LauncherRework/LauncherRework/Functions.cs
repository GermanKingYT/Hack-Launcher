using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace LauncherRework
{
    internal class Functions
    {
        // Scramble Function
        public static string Scramble(string Text)
        {
            var chars1 = Text.ToArray();
            var r1 = new Random(259);

            for (var i = 0; i < chars1.Length; i++)
            {
                var randomIndex = r1.Next(0, chars1.Length);
                var temp = chars1[randomIndex];
                chars1[randomIndex] = chars1[i];
                chars1[i] = temp;
            }

            return new string(chars1);
        }

        // Register User Function
        public static bool RegisterUser(string User, string Pass, string Hwid)
        {
            // ResponseStream String
            string ResponseStream = null;

            // Create Response
            HttpWebResponse Response = null;

            try
            {
                // TODO: Put your own Path here
                // Creates a new Web Request
                var Request = WebRequest.Create("LINK TO REGISTER.PHP HERE");

                // WebRequest Method
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";

                // Command that will be send
                var postData = "username=" + User + "&password=" + Pass + "&hwid=" + Hwid;
                var byteArray = Encoding.UTF8.GetBytes(postData);
                Request.ContentLength = byteArray.Length;

                // Send Command
                var dataSend = Request.GetRequestStream();
                dataSend.Write(byteArray, 0, byteArray.Length);
                // Close Connection
                dataSend.Close();

                // Get Response
                Response = (HttpWebResponse) Request.GetResponse();

                // Check Status
                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    // New Stream
                    var responseStream = Response.GetResponseStream();

                    // New StreamReader
                    var Reader = new StreamReader(responseStream);

                    // Send data to Status
                    Globals.RegisterStatus = Reader.ReadToEnd();

                    // Close StreamReader
                    Reader.Close();
                }
            }
            catch (WebException Error)
            {
                // Show Messagebox with ErrorMessage
                MessageBox.Show("Error: " + Error.Message, "Error!");
            }
            finally
            {
                // Check Response Status
                if (Response != null)
                    // Close Response Stream
                    Response.Close();
            }

            // Set FinishedRegister to true
            return Globals.FinishedRegister = true;
        }

        // Check User Function
        public static bool CheckUser(string User, string Pass, string Hwid)
        {
            // ResponseStream String
            string ResponseStream = null;

            // Create Response
            HttpWebResponse Response = null;

            try
            {
                // TODO: Put your own Path here
                // Creates a new Web Request
                var Request = WebRequest.Create("LINK TO LOGIN.PHP HERE");

                // WebRequest Method
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";

                // Command that will be send
                var postData = "username=" + User + "&password=" + Pass + "&hwid=" + Hwid;
                var byteArray = Encoding.UTF8.GetBytes(postData);
                Request.ContentLength = byteArray.Length;

                // Send Command
                var dataSend = Request.GetRequestStream();
                dataSend.Write(byteArray, 0, byteArray.Length);
                // Close Connection
                dataSend.Close();

                // Get Response
                Response = (HttpWebResponse) Request.GetResponse();

                // Check Status
                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    // New Stream
                    var responseStream = Response.GetResponseStream();

                    // New StreamReader
                    var Reader = new StreamReader(responseStream);

                    // Send data to Status
                    Globals.CheckStatus = Reader.ReadToEnd();

                    // Close StreamReader
                    Reader.Close();
                }
            }
            catch (WebException Error)
            {
                // Show Messagebox with ErrorMessage
                MessageBox.Show("Error: " + Error.Message, "Error!");
            }
            finally
            {
                // Check Response Status
                if (Response != null)
                    // Close Response Stream
                    Response.Close();
            }

            // Set Finished Checking to true
            return Globals.FinishedChecking = true;
        }
    }
}