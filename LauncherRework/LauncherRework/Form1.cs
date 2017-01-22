using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace LauncherRework
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
            int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        // Login Button
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Give Focus to Username Textbox to prevent the weird fous border on buttons
            UsernameTextbox.Focus();

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

            // Set Username, Password & Token in Globals
            Globals.Username = UsernameTextbox.Text;
            Globals.Password = PasswordTextbox.Text;
            Globals.Token = Functions.CreateString(65);

            // Start Check Function
            CheckUser();
        }

        // Register Button
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // Give Focus to Username Textbox to prevent the weird fous border on buttons
            UsernameTextbox.Focus();

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
            // Set RegisterStatus back to Default
            Globals.RegisterStatus = null;

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
                    StatusLabel.Text = "Failed getting Status";
                    return;
                }

                // Read/Translate Website Output
                ReadRegisterCode();
            };

            // Run Background Worker
            BW.RunWorkerAsync();
        }

        // Start Check User Functions
        public void CheckUser()
        {
            // Set CheckStatus back to Default
            Globals.CheckStatus = null;

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

                // Scramble & Encrypt our Random String
                var ScrambleToken = Functions.Scramble(Globals.Token);
                // Set Encrypted Token in Globals
                Globals.Token = ScrambleToken;

                // We have to invoke this cause we want to change it from another Thread
                Invoke((MethodInvoker) delegate
                {
                    // Update Status Text
                    StatusLabel.Text = "Checking User...";
                });

                // Start Check Function
                Functions.CheckUser(SecureUsername, SecurePassword, SecureHWID, Globals.Token);

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
                    StatusLabel.Text = "Failed getting Status";
                    return;
                }

                // Read/Translate Website Output
                ReadCheckUserCode();
            };

            // Run Background Worker
            BW.RunWorkerAsync();
        }

        // TODO: You may want to change these so you have unique ones
        // Read the Register Output
        private void ReadRegisterCode()
        {
            // Check the Code from the PHP Script
            if (Globals.RegisterStatus.Equals("Code: 731446"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Could not Connect to Server";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.RegisterStatus.Equals("Code: 140483"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "´Settings File not set up!";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.RegisterStatus.Equals("Code: 269598"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "You already have an Account";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.RegisterStatus.Equals("Code: 708385"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "User already exists";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.RegisterStatus.Equals("Code: 160674"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Could not Register Account";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.RegisterStatus.Equals("Code: 934045"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Successfully Registered";
                return;
            }

            // Set StatusLabel Text
            StatusLabel.Text = "Could not Translate Code";
        }

        // TODO: You may want to change these so you have unique ones
        // Read the Check User Output
        private void ReadCheckUserCode()
        {
            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 731446"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Could not Connect to Server";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 140483"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "´Settings File not set up!";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 694143"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Error checking Data";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 107177"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "User does not exist";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 933545"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Account is currently locked";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 974498"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Wrong Password, 3 Try's left";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 375292"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Wrong Password, 2 Try's left";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 865696"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Wrong Password, 1 Try left";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 548234"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "Wrong Password, Account is now locked";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 526798"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "HWID Changed";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals("Code: 913280"))
            {
                // Translate the Code, Update the StatusLabel & exit the Function
                StatusLabel.Text = "No Subscription";
                return;
            }

            // Check the Code from the PHP Script
            if (Globals.CheckStatus.Equals(Globals.Token))
            {
                // Translate the Code, Update the StatusLabel & exit the Functionv
                StatusLabel.Text = "Successfully logged in";

                //TODO: What to do after Successfully logged in?

                return;
            }

            // Set StatusLabel Text
            StatusLabel.Text = "Could not Translate Code";
        }

        // Enable Dragging for our "Custom Design"
        private void EnableWindowDragging(object sender, MouseEventArgs e)
        {
            // Check if Left Mouse Button is pressed
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // Close Form
        private void FormCloseButton_Click(object sender, EventArgs e)
        {
            // Exit Programm
            Application.Exit();
        }

        // On MouseOver
        private void FormCloseButton_MouseHover(object sender, EventArgs e)
        {
            // Change "Text" Color to White
            FormCloseButton.ForeColor = Color.White;
        }

        // On MouseLeave
        private void FormCloseButton_MouseLeave(object sender, EventArgs e)
        {
            // Change "Text" Color to Black
            FormCloseButton.ForeColor = Color.Black;
        }
    }
}
