using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace LauncherRework
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Login Button
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Check if UsernameTextbox contains something
            if (UsernameTextbox.Text.Length == 0)
            {
                MessageBox.Show("Username Textbox is empty.");
                return;
            }

            // Check if PasswordTextbox contains something
            if (PasswordTextbox.Text.Length == 0)
            {
                MessageBox.Show("Password Textbox is empty.");
                return;
            }

            // Set Username & Password in Globals
            Globals.Username = UsernameTextbox.Text;
            Globals.Password = PasswordTextbox.Text;

            // Start Check Function
            CheckUser();
        }

        // Register Button
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // Check if UsernameTextbox contains something
            if (UsernameTextbox.Text.Length == 0)
            {
                MessageBox.Show("Username Textbox is empty.");
                return;
            }

            // Check if PasswordTextbox contains something
            if (PasswordTextbox.Text.Length == 0)
            {
                MessageBox.Show("Password Textbox is empty.");
                return;
            }

            // Set Username & Password in Globals
            Globals.Username = UsernameTextbox.Text;
            Globals.Password = PasswordTextbox.Text;

            // Start Register Function
            RegisterUser();
        }

        // Start Register User Functions
        public void RegisterUser()
        {
            // Set Finished Checking back to Default
            Globals.FinishedRegister = false;

            // Create new Background Worker
            var BW = new BackgroundWorker();
            BW.DoWork += delegate
            {
                // We have to invoke this cause we want to change it from another Thread
                Invoke((MethodInvoker) delegate
                {
                    // Update Status Text
                    StatusLabel.Text = "Securing Data...";
                });

                // Scramble Username, Password & HWID
                var ScrambledUsername = Functions.Scramble(Globals.Username);
                var ScrambledPassword = Functions.Scramble(Globals.Password);
                var ScrambledHWID = Functions.Scramble(HardwareID.UniqueHWID("C"));

                // Encrypt Scrambled Username, Password & HWID
                var SecureUsername = Encryption.Encrypt(ScrambledUsername);
                var SecurePassword = Encryption.Encrypt(ScrambledPassword);
                var SecureHWID = Encryption.Encrypt(ScrambledHWID);

                // We have to invoke this cause we want to change it from another Thread
                Invoke((MethodInvoker) delegate
                {
                    // Update Status Text
                    StatusLabel.Text = "Register User...";
                });

                // Start Check Function
                Functions.RegisterUser(SecureUsername, SecurePassword, SecureHWID);

                // Wait for Check Function to Finish
                while (!Globals.FinishedRegister)
                    Thread.Sleep(500);
            };

            // Update Status Text after it is Complete
            BW.RunWorkerCompleted += delegate
            {
                // Return if Status is null
                if (Globals.RegisterStatus == null)
                {
                    StatusLabel.Text = "Failed";
                    return;
                }

                // If login.php output equals
                if (Globals.RegisterStatus.Equals("User already exists") ||
                    Globals.RegisterStatus.Equals("You already have an Account"))
                    StatusLabel.Text = Globals.RegisterStatus;
                else if (Globals.RegisterStatus.Equals("Registered"))
                    StatusLabel.Text = Globals.RegisterStatus;
                else
                    MessageBox.Show(Globals.RegisterStatus, "Status");
            };

            // Run Background Worker
            BW.RunWorkerAsync();
        }

        // Start Check User Functions
        public void CheckUser()
        {
            // Set Finished Checking back to Default
            Globals.FinishedChecking = false;

            // Create new Background Worker
            var BW = new BackgroundWorker();
            BW.DoWork += delegate
            {
                // We have to invoke this cause we want to change it from another Thread
                Invoke((MethodInvoker) delegate
                {
                    // Update Status Text
                    StatusLabel.Text = "Securing Data...";
                });

                // Scramble Username, Password & HWID
                var ScrambledUsername = Functions.Scramble(Globals.Username);
                var ScrambledPassword = Functions.Scramble(Globals.Password);
                var ScrambledHWID = Functions.Scramble(HardwareID.UniqueHWID("C"));

                // Encrypt Scrambled Username, Password & HWID
                var SecureUsername = Encryption.Encrypt(ScrambledUsername);
                var SecurePassword = Encryption.Encrypt(ScrambledPassword);
                var SecureHWID = Encryption.Encrypt(ScrambledHWID);

                // We have to invoke this cause we want to change it from another Thread
                Invoke((MethodInvoker) delegate
                {
                    // Update Status Text
                    StatusLabel.Text = "Checking User...";
                });

                // Start Check Function
                Functions.CheckUser(SecureUsername, SecurePassword, SecureHWID);

                // Wait for Check Function to Finish
                while (!Globals.FinishedChecking)
                    Thread.Sleep(500);
            };

            // Update Status Text after it is Complete
            BW.RunWorkerCompleted += delegate
            {
                // Return if Status is null
                if (Globals.CheckStatus == null)
                {
                    StatusLabel.Text = "Failed";
                    return;
                }

                // If login.php output equals
                if (Globals.CheckStatus.Equals("User does not exist")
                    || Globals.CheckStatus.Equals("Wrong Password")
                    || Globals.CheckStatus.Equals("HWID Changed")
                    || Globals.CheckStatus.Equals("No Subscription")
                    || Globals.CheckStatus.Equals("Error: failed executing command")
                    || Globals.CheckStatus.Equals("Wrong Password, 3 Try's Left")
                    || Globals.CheckStatus.Equals("Wrong Password, 2 Try's Left")
                    || Globals.CheckStatus.Equals("Wrong Password, 1 Try Left")
                    || Globals.CheckStatus.Equals("Account is now Locked please Message an Admin")
                    || Globals.CheckStatus.Equals("Account is Locked please message an Admin")
                    || Globals.CheckStatus.Equals("Connection Failed: Could not Connect to Server"))
                    StatusLabel.Text = Globals.CheckStatus;
                else if (Globals.CheckStatus.Equals("Successfully Logged in."))
                    StatusLabel.Text = Globals.CheckStatus;
                else
                    MessageBox.Show("Error: " + Globals.CheckStatus, "Status");
            };

            // Run Background Worker
            BW.RunWorkerAsync();
        }
    }
}