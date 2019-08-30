using Amazon;
using Amazon.CognitoIdentity;
using CoreGraphics;
using Foundation;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Drawing;
using System.IO;
using UIKit;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;
// using static hearingapp_otc.AWSHelper; // TODO: AWS
using Security;
using System.Reflection;

namespace hearingapp_otc.iOS
{
    public partial class UIVCRegistration : UIViewController
    {
        public static CognitoUser regUser;
        public static AuthFlowResponse AWSAuthFlowResponseContext;

        private FlatButton btnContinueRegFLAT;
        private FlatButton btnAgreement1FLAT;
        private FlatButton btnAgreement2FLAT;

        private bool boolAgreement1 = false;
        private bool boolAgreement2 = false;

        private int lblVersionInfoTapCount;

        public UIVCRegistration(IntPtr handle) : base(handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Reset version info label stuff
            lblVersionInfoTapCount = 0;
            lblVersionInfo.Text = "";
            lblVersionInfo.Hidden = false; // apparently you can't hide stuff AND have user interaction enabled
            UITapGestureRecognizer labelTap = new UITapGestureRecognizer(() => {
                // Add to our count whenever the top nav is tapped
                lblVersionInfoIncreaseTapCount();
            });
            lblVersionInfo.UserInteractionEnabled = true;
            lblVersionInfo.AddGestureRecognizer(labelTap);

            View.BackgroundColor = FlatColors.Clouds;
            txtFirstName.BackgroundColor = FlatColors.Clouds;
            txtLastName.BackgroundColor = FlatColors.Clouds;
            txtEmailAddress.BackgroundColor = FlatColors.Clouds;
            
            // Capitalize the first letters of people's names and disable autocorrect stuff
            txtFirstName.AutocapitalizationType = UITextAutocapitalizationType.Words;
            txtFirstName.AutocorrectionType = UITextAutocorrectionType.No;
            txtFirstName.SpellCheckingType = UITextSpellCheckingType.No;
            txtFirstName.Layer.BorderColor = UIColor.Gray.CGColor;

            txtLastName.AutocapitalizationType = UITextAutocapitalizationType.Words;
            txtLastName.AutocorrectionType = UITextAutocorrectionType.No;
            txtLastName.SpellCheckingType = UITextSpellCheckingType.No;
            txtLastName.Layer.BorderColor = UIColor.White.CGColor;

            txtEmailAddress.AutocorrectionType = UITextAutocorrectionType.No;
            txtEmailAddress.SpellCheckingType = UITextSpellCheckingType.No;
            txtEmailAddress.AutocapitalizationType = UITextAutocapitalizationType.None;
            txtEmailAddress.Layer.BorderColor = FlatColors.Clouds.CGColor;

            // Disable the developer options by default
            switchDevOptions.OnTintColor = FlatColors.BelizeHole;
            App.devMode = false;
            switchDevOptions.On = App.devMode;
            switchDevOptions.TouchUpInside += SwitchDevOptions_TouchUpInside;
            progressCompletion.Hidden = !App.devMode;
            lblTestProgress.Hidden = !App.devMode;

            //Map a FlatUI button over top of existing btnContinueReg
            btnContinueReg.Hidden = true;
            var newX = btnContinueReg.Frame.X;
            var newY = btnContinueReg.Frame.Y;
            var newWidth = btnContinueReg.Frame.Size.Width;
            var newHeight = btnContinueReg.Frame.Size.Height;
            btnContinueRegFLAT = new FlatButton(new RectangleF((int)newX, (int)newY, (int)newWidth, (int)newHeight)); //e.g. new RectangleF(20, 20, 140, 43)
            btnContinueRegFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnContinueRegFLAT.SetTitle("Continue →"); // Unicode arrows: https://unicode-table.com/en/sets/arrows-symbols/
            btnContinueRegFLAT.TouchUpInside += BtnContinueReg_TouchUpInside;

            // Disable Continue button until we pass form validation AND the user agrees to the T&Cs
            btnContinueRegFLAT.Enabled = false;
            btnContinueRegFLAT.UserInteractionEnabled = false;

            View.AddSubview(btnContinueRegFLAT);

            // Add T&C flat ui buttons - btnAgreement1/2FLAT
            btnTC1.Hidden = true;
            btnAgreement1FLAT = new FlatButton(new RectangleF((int)btnTC1.Frame.X, (int)btnTC1.Frame.Y, (int)btnTC1.Frame.Size.Width, (int)btnTC1.Frame.Size.Height));
            btnAgreement1FLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnAgreement1FLAT.SetTitle("Tap to Agree");
            btnAgreement1FLAT.TitleLabel.Font = UIFont.BoldSystemFontOfSize(14);
            btnAgreement1FLAT.TouchUpInside += BtnAgreement1Toggle_TouchUpInside;
            View.AddSubview(btnAgreement1FLAT);
            btnTC2.Hidden = true;
            btnAgreement2FLAT = new FlatButton(new RectangleF((int)btnTC2.Frame.X, (int)btnTC2.Frame.Y, (int)btnTC2.Frame.Size.Width, (int)btnTC2.Frame.Size.Height));
            btnAgreement2FLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnAgreement2FLAT.TitleLabel.Font = UIFont.BoldSystemFontOfSize(14);
            btnAgreement2FLAT.SetTitle("Tap to Agree");
            btnAgreement2FLAT.TouchUpInside += BtnAgreement2Toggle_TouchUpInside;
            View.AddSubview(btnAgreement2FLAT);

            // Handle AWS Login crap
            awsLoginView.Hidden = true; // REMOVE WHEN AWS IS ADDED BACK IN
            /*
            if (App.isLoggedIn==false)
            {
                View.AddSubview(awsLoginView);
                // Attempt to reposition AWS login form near the top
                awsLoginView.Frame = new CoreGraphics.CGRect(53, 42, 668, 311);

                btnDoLogin.TouchUpInside += BtnDoLogin_TouchUpInside;

                newX = btnDoLogin.Frame.X;
                newY = btnDoLogin.Frame.Y;
                newWidth = btnDoLogin.Frame.Size.Width;
                newHeight = btnDoLogin.Frame.Size.Height;
                var btnDoLoginFLAT = new FlatButton(new RectangleF((int)newX, (int)newY, (int)newWidth, (int)newHeight)); //e.g. new RectangleF(20, 20, 140, 43)
                btnDoLoginFLAT.Color = FlatColors.Concrete;
                btnDoLoginFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
                btnDoLoginFLAT.SetTitle("Login");
                btnDoLoginFLAT.TouchUpInside += BtnDoLogin_TouchUpInside;
                awsLoginView.AddSubview(btnDoLoginFLAT);

                lblPleaseLogin.TextColor = UIColor.White;

                var canAutoLogin = AttemptAutoLogin(awsLoginView, lblLoggingIn);
                if (canAutoLogin)
                    awsLoginView.Hidden = true;
                
            }
            else
            {
                awsLoginView.Hidden = true;
            }
            */
        }

        private void lblVersionInfoIncreaseTapCount()
        {
            // Require a dozen taps in order to display the UIAlert - after the UIAlert reset the tap count
            lblVersionInfoTapCount += 1;
            Console.WriteLine("UIVCRegistration:lblVersionInfoIncreaseTapCount - increased tap count to {0}", lblVersionInfoTapCount);

            // Give a couple taps fudge factor, then reset after a few more
            if (lblVersionInfoTapCount >= 12 && lblVersionInfoTapCount <= 14)
            {
                lblVersionInfo.Text = "v" + NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
            }
            else if (lblVersionInfoTapCount > 14)
            {
                lblVersionInfoTapCount = 0;
                lblVersionInfo.Text = "";
            }

        }

        private void BtnAgreement1Toggle_TouchUpInside(object sender, EventArgs e)
        {
            // Toggle between styles and text (checkbox for when agreed)
            if(!boolAgreement1)
            {
                // Toggle to agreed
                btnAgreement1FLAT.Color = FlatColors.Emerland;
                btnAgreement1FLAT.ShadowColor = FlatColors.Nephritis;
                btnAgreement1FLAT.SetTitle("✔");

                boolAgreement1 = true;

                ToggleContinueButton();
            }
            else
            {
                // We were already agreed, toggle back to not agreed
                btnAgreement1FLAT.Color = FlatColors.WetAsphalt;
                btnAgreement1FLAT.ShadowColor = FlatColors.MidnightBlue;
                btnAgreement1FLAT.SetTitle("Tap to Agree");

                boolAgreement1 = false;

                ToggleContinueButton();
            }
        }

        private void BtnAgreement2Toggle_TouchUpInside(object sender, EventArgs e)
        {
            // Toggle between styles and text (checkbox for when agreed)
            if (!boolAgreement2)
            {
                // Toggle to agreed
                btnAgreement2FLAT.Color = FlatColors.Emerland;
                btnAgreement2FLAT.ShadowColor = FlatColors.Nephritis;
                btnAgreement2FLAT.SetTitle("✔");

                boolAgreement2 = true;

                ToggleContinueButton();
            }
            else
            {
                // We were already agreed, toggle back to not agreed
                btnAgreement2FLAT.Color = FlatColors.WetAsphalt;
                btnAgreement2FLAT.ShadowColor = FlatColors.MidnightBlue;
                btnAgreement2FLAT.SetTitle("Tap to Agree");

                boolAgreement2 = false;

                ToggleContinueButton();
            }
        }

        private void ToggleContinueButton()
        {
            // Check that they've filled in the forms and clicked the agreement buttons
            if (PerformFormValidation() && boolAgreement1 && boolAgreement2) {
                btnContinueRegFLAT.Enabled = true;
                btnContinueRegFLAT.UserInteractionEnabled = true;
            }
            // Nope? too bad, leave Continue disabled
            else
            {
                btnContinueRegFLAT.Enabled = false;
                btnContinueRegFLAT.UserInteractionEnabled = false;
            }
        }

        private void SwitchDevOptions_TouchUpInside(object sender, EventArgs e)
        {
            // As soon as the user touches the switch, it gets turned On. So our job is to
            // validate and if they fail to log in, turn it back off

            // If developer mode is currently off, and the user is trying to enable it
            if (switchDevOptions.On)
            {
                // Get developer mode password from user
                Console.WriteLine("UIVCRegistration:SwitchDevOptions_TouchUpInside - user attempting to access developer mode");
                UIAlertController alert = UIAlertController.Create("Developer Login", "Enter your passcode", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Login", UIAlertActionStyle.Default, action => {
                    // This code is invoked when the user taps on login, and this shows how to access the field values
                    Console.WriteLine("UIVCRegistration:SwitchDevOptions_TouchUpInside - Password attempted: {0}", alert.TextFields[0].Text);

                    if (alert.TextFields[0].Text == "hammertime")
                    {
                        switchDevOptions.On = true;

                        App.devMode = switchDevOptions.On;
                        progressCompletion.Hidden = !switchDevOptions.On;
                        lblTestProgress.Hidden = !switchDevOptions.On;
                        btnContinueRegFLAT.Enabled = switchDevOptions.On;
                        btnContinueRegFLAT.UserInteractionEnabled = switchDevOptions.On;
                    }
                    else
                    {
                        switchDevOptions.On = false;
                    }

                }));
                alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, action =>
                {
                    switchDevOptions.On = false;
                }));
                //alert.AddTextField((field) => {
                //    field.Placeholder = "email address";
                //});
                alert.AddTextField((field) => {
                    field.SecureTextEntry = true;
                });
                PresentViewController(alert, animated: true, completionHandler: null);
            }

            // Disable developer mode without prompting
            else
            {
                Console.WriteLine("UIVCRegistration:SwitchDevOptions_TouchUpInside - disabling developer mode");

                switchDevOptions.On = false;

                App.devMode = switchDevOptions.On;
                progressCompletion.Hidden = !switchDevOptions.On;
                lblTestProgress.Hidden = !switchDevOptions.On;
                btnContinueRegFLAT.Enabled = switchDevOptions.On;
                btnContinueRegFLAT.UserInteractionEnabled = switchDevOptions.On;
            }
            
        }

        public static bool AttemptAutoLogin(UIView viewToHide, UILabel statusLabel)
        {
            // Checks keychain for saved username and password and attempts to auth with them
            var queryRec = new SecRecord(SecKind.GenericPassword) { Service = "HIUAWS" };

            SecStatusCode retCode; // https://developer.xamarin.com/api/type/MonoTouch.Security.SecStatusCode/

            int maxMatches = 100;
            SecRecord[] matches = SecKeyChain.QueryAsRecord(queryRec, maxMatches, out retCode);

            if (matches != null)
            {
                foreach (SecRecord secRec in matches)
                {
                    string username = secRec.Account.ToString();
                    string password = secRec.ValueData.ToString();
                    Console.WriteLine("UIVCRegistration::AttemptAutoLogin - attempting to authenticate as: " + username);
                    //AttemptLogin(username, password, viewToHide, statusLabel); // TODO: AWS
                }
            }

            // Nothing found? Return false because we can't authenticate
            return false;
        }

        private void BtnDoLogin_TouchUpInside(object sender, EventArgs e)
        {
            // Performs AWS Login
            string username = txtAWSUsername.Text;
            string password = txtAWSPassword.Text;

            lblLoggingIn.TextColor = FlatColors.Clouds;
            lblLoggingIn.Hidden = false;
            // AttemptLogin(username, password, awsLoginView, lblLoggingIn); // TODO: AWS
        }

        // TODO: AWS
        //public static void AttemptLogin(string username, string password, UIView viewToHide, UILabel statusLabel)
        //{
        //    //ClearCognitoCreds();

        //    var task = AWSHelper.AttemptLogin(username, password);
        //    task.Wait();
        //    var result = task.Result;

        //    UIApplication.SharedApplication.InvokeOnMainThread(() =>
        //    {
        //        // check if the account is in the keychain already
        //        var queryRec = new SecRecord(SecKind.GenericPassword)
        //        {
        //            //Account = Convert.ToString(NSData.FromString(username))
        //            Service = "HIUAWS"
        //        };

        //        SecStatusCode retCode; // https://developer.xamarin.com/api/type/MonoTouch.Security.SecStatusCode/

        //        // Clean out any old records if it already exists in the keychain
        //        int maxMatches = 100;
        //        SecRecord[] matches = SecKeyChain.QueryAsRecord(queryRec, maxMatches, out retCode);

        //        if (matches != null)
        //        {
        //            /*
        //            foreach (SecRecord secRec in matches)
        //            {
        //                var removeresult = SecKeyChain.Remove(secRec);
        //                Console.WriteLine("UIVCRegistration::AttemptLogin - old keychain entry found, deleting...");
        //            }
        //            */
        //            var removeresult = SecKeyChain.Remove(queryRec);
        //            Console.WriteLine("UIVCRegistration::AttemptLogin - old keychain entry found, deleting...");
        //        }

        //        if (result == true)
        //        {
        //            // If the query result was successful, hide the login panel
        //            viewToHide.Hidden = true;

        //            // save the new credentials to the keychain
        //            var awsKeychainRecord = new SecRecord(SecKind.GenericPassword)
        //            {
        //                Label = "HIUApp",
        //                Description = "HIU AWS Credentials for device",
        //                Account = username,
        //                Service = "HIUAWS",
        //                ValueData = NSData.FromString(password),
        //                //Comment = "Save AWS Creds for app",
        //                //Generic = NSData.FromString("foo")
        //            };

        //            retCode = SecKeyChain.Add(awsKeychainRecord);

        //            // Now make sure that we've actually successfully aded to the keychain
        //            if (retCode != SecStatusCode.Success && retCode != SecStatusCode.DuplicateItem)
        //            {
        //                Console.WriteLine("UIVCRegistration::AttemptLogin - Error adding/updating to keychain: {0}", retCode);
        //            }
        //            else
        //            {
        //                Console.WriteLine("UIVCRegistration::AttemptLogin - Credentials successfully added to keychain!");
        //            }
        //        }
        //        else
        //        {
        //            viewToHide.Hidden = false;
        //            UIApplication.SharedApplication.InvokeOnMainThread(() =>
        //            {
        //                statusLabel.Text = "Login failed. Please check your credentials.";
        //            });
        //        }
        //    });
            
        //    // For setting debug breakpoints ;)
        //    return;
        //}

        private void BtnContinueReg_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("RegistrationSegue", (Foundation.NSObject)sender);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);

            // Validations are disabled, and the developer didn't enter information - autofill and create a new session
            // The dev *must* leave the fields blank, otherwise they will go through validation anyway
            if (App.validationsDisabled && txtFirstName.Text == "" && txtLastName.Text == "")
            {
                Session newUserSession = new Session()
                {
                    FirstName = "Abe",
                    LastName = "Worrell",
                    EmailAddress = "autotesting@fyrecode.com",
                    devModeEnabled = App.devMode
                };

                if (DatabaseHelper.Insert(ref newUserSession, db_path))
                    Console.WriteLine("DBG UIVCRegistration.BtnContinue_TouchUPInside : noValidation : SUCCESS!");
                else
                    Console.WriteLine("DBG UIVCRegistration.BtnContinue_TouchUPInside : noValidation : FAIL!");

                // AWSHelper.RegisterNewSession(newUserSession); // TODO: AWS

                // Set the GlobalSessionId for this hearing test
                App.globablSessionId = newUserSession.Id;
            }
            // Normal happy path - User has entered data, we perform validation
            else if (PerformFormValidation())
            {
                Session newUserSession = new Session() {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    EmailAddress = txtEmailAddress.Text,
                    devModeEnabled = App.devMode
                };

                if (DatabaseHelper.Insert(ref newUserSession, db_path))
                    Console.WriteLine("DBG UIVCRegistration.BtnContinue_TouchUPInside : SUCCESS!");
                else
                    Console.WriteLine("DBG UIVCRegistration.BtnContinue_TouchUPInside : FAIL!");

                // AWSHelper.RegisterNewSession(newUserSession); // TODO: AWS

                // Set the GlobalSessionId for this hearing test
                App.globablSessionId = newUserSession.Id;
            }

        }

        private bool PerformFormValidation()
        {
            if (txtFirstName.Text == "" && txtLastName.Text == "" && txtEmailAddress.Text == "")
            {
                var okAlertController = UIAlertController.Create("Please Finish Registration", "Please enter your information.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return false;
            }
            else if (txtFirstName.Text == "")
            {
                var okAlertController = UIAlertController.Create("Please Finish Registration", "Please enter your first name.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return false;
            }
            else if (txtLastName.Text == "")
            {
                var okAlertController = UIAlertController.Create("Please Finish Registration", "Please enter your last name.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return false;
            }
            else if (txtEmailAddress.Text == "")
            {
                var okAlertController = UIAlertController.Create("Please Finish Registration", "Please enter your email address.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return false;
            }
            else
            {
                return true;
            }
        }

        // Handle to AppDelegate - App
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

        partial void BtnViewTC_TouchUpInside(UIButton sender)
        {
            // Read in T&Cs from File
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(UIVCRegistration)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("hearingapp_otc.iOS.Resources.hippa_tc.txt");

            string fileText = "";
            using (var reader = new StreamReader(stream))
            {
                fileText = reader.ReadToEnd();
            }

            // Create the text that is going to fill the thing
            var atts = new UIStringAttributes();
            atts.ForegroundColor = UIColor.Black;
            var attributedString = new NSMutableAttributedString(fileText, atts);
            attributedString.BeginEditing();
            attributedString.AddAttribute(UIStringAttributeKey.ForegroundColor, UIColor.Black, new NSRange(0, 10));
            attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.GetPreferredFontForTextStyle(UIFontTextStyle.Headline), new NSRange(0, 13));
            attributedString.EndEditing();

            // Create a new popover to hold the T&C UITextView
            CustomPopUpView cpuv = new CustomPopUpView(new CoreGraphics.CGSize(400, 500));

            // Create a UITextView to hold the T&C text
            UITextView tctextView = new UITextView(new RectangleF(0, 0, 400, 460))
            {
                Editable = false,
                AttributedText = attributedString,
                ScrollEnabled = true,
                UserInteractionEnabled = true
            };

            // Add the textview as a subview to the popver
            cpuv.AddSubview(tctextView);
            cpuv.PopUp(true, delegate {
                Console.WriteLine("UIVCRegistration:BtnViewTC_TouchUpInside - Custom popover will close");
            });

        }
    }

}