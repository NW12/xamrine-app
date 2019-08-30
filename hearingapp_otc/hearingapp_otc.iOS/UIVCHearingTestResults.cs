using CoreGraphics;
using Foundation;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCHearingTestResults : UIViewController
    {
        public bool hasBadHearing;

        private CustomPopUpView cpuvCallMe;
        private UITextField txtPhoneNum;
        private FlatButton btnSubmitNumber;

        public UIVCHearingTestResults(IntPtr handle) : base(handle)
        {
        }
        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;
            lblTopInfo.BackgroundColor = FlatColors.Clouds;
            lblBottomInfo.BackgroundColor = FlatColors.Clouds;

            // Get userSession reference
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);

            // Checks if any of the hearing loss is below 30db
            lblBottomInfo.Lines = 4;
            lblBottomInfo.Text = PerformLossCheck(userSession);

            btnResultsReviewFinished.TouchUpInside += BtnResultsReviewFinished_TouchUpInside;

            //Map a FlatUI button over top of existing btnContinueReg
            var newX = btnResultsReviewFinished.Frame.X;
            var newY = btnResultsReviewFinished.Frame.Y;
            var newWidth = btnResultsReviewFinished.Frame.Size.Width;
            var newHeight = btnResultsReviewFinished.Frame.Size.Height;
            var btnContinueRegFLAT = new FlatButton(new RectangleF((int)newX, (int)newY, (int)newWidth, (int)newHeight)); //e.g. new RectangleF(20, 20, 140, 43)
            btnContinueRegFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            btnCallMe.Hidden = true;
            btnContinueRegFLAT.SetTitle("Exit Test");

            /* TODO: RESTORE W AWS
            if (hasBadHearing)
            {
                btnContinueRegFLAT.SetTitle("Continue →"); // Unicode arrows: https://unicode-table.com/en/sets/arrows-symbols/

                // Add "Please Call Me About My Results" button over btnCallM
                FlatButton btnCallMeFLAT = new FlatButton(new RectangleF(
                    (int)btnCallMe.Frame.X, (int)btnCallMe.Frame.Y, (int)btnCallMe.Frame.Size.Width, (int)btnCallMe.Frame.Size.Height)
                    );
                btnCallMeFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
                btnCallMeFLAT.SetTitle("Please Call Me About My Results");
                btnCallMeFLAT.Color = FlatColors.Sunflower;
                btnCallMeFLAT.ShadowColor = FlatColors.Tangerine;
                btnCallMeFLAT.TouchUpInside += BtnCallMe_TouchUpInside;
                View.AddSubview(btnCallMeFLAT);
            }
            else
                btnContinueRegFLAT.SetTitle("Exit Test");
            */
            btnContinueRegFLAT.TouchUpInside += BtnResultsReviewFinished_TouchUpInside;
            View.AddSubview(btnContinueRegFLAT);

            // Add Exit Test button
            btnEXITFromResults.Hidden = true;
            FlatExitButton btnExitFLAT = new FlatExitButton((int)btnEXITFromResults.Frame.X, (int)btnEXITFromResults.Frame.Y);
            btnExitFLAT.TouchUpInside += BtnEXIT_TouchUpInside;
            View.AddSubview(btnExitFLAT);

        }

        private void BtnCallMe_TouchUpInside(object sender, EventArgs e)
        {
            // Let's just manually paint this bitch

            // Create a new popover to hold the T&C UITextView
            int vwWidth = 400;
            int vwHeight = 390;
            cpuvCallMe = new CustomPopUpView(new CoreGraphics.CGSize(vwWidth, vwHeight));

            int vwCloseButtonHeight = 40;
            
            UIView vwCallMe = new UIView(new RectangleF(0, 0, vwWidth, vwHeight));

            // Add label explaining wtf
            int lblCallMeInfoWidth = vwWidth - 20;
            int lblCallMeInfoHeight = 200;
            int lblCallMeInfoX = 10;
            int lblCallMeInfoY = 5;
            UILabel lblCallMeInfo = new UILabel(new RectangleF(lblCallMeInfoX, lblCallMeInfoY, lblCallMeInfoWidth, lblCallMeInfoHeight));
            lblCallMeInfo.Text = "Please submit your phone number and an audiologist will call you within 2 business days. The phone call will come from 1-715-381-3111.";
            //lblCallMeInfo.LineBreakMode = UILineBreakMode.WordWrap;
            lblCallMeInfo.Lines = 5;
            lblCallMeInfo.Font = lblCallMeInfo.Font.WithSize(24f);
            lblCallMeInfo.TextAlignment = UITextAlignment.Center;

            // Add textbox for the user to enter their phone number
            int txtPhoneNumWidth = vwWidth - 80;
            int txtPhoneNumHeight = 40;
            int txtPhoneNumX = 40;
            int txtPhoneNumY = vwHeight / 2;
            txtPhoneNum = new UITextField(new RectangleF(txtPhoneNumX, txtPhoneNumY, txtPhoneNumWidth, txtPhoneNumHeight));
            txtPhoneNum.Placeholder = "6515551234";
            txtPhoneNum.BorderStyle = UITextBorderStyle.Line;
            txtPhoneNum.Font = txtPhoneNum.Font.WithSize(32f);
            txtPhoneNum.TextAlignment = UITextAlignment.Center;
            txtPhoneNum.KeyboardType = UIKeyboardType.NumberPad;

            Console.WriteLine("UIVCHearingTestResultes:BtnCallMe_TouchUpInside - adding editingchanged to txtPhoneNum!");
            txtPhoneNum.EditingChanged += TxtPhoneNum_ValueChanged;

            // Add button that submits the number to AWS
            int btnSubmitHeight = 67;
            int btnSubmitWidth = vwWidth - 20;
            int btnSubmitFromButtonX = 10; //10=half the margin from btnSubmitWidth
            int btnSubmitFromButtonY = vwHeight - vwCloseButtonHeight - btnSubmitHeight - 20; //0,0 starts from top left && 20=margin from close button
            btnSubmitNumber = new FlatButton(new RectangleF(btnSubmitFromButtonX, btnSubmitFromButtonY, btnSubmitWidth, btnSubmitHeight));
            btnSubmitNumber.SetTitle("Submit Phone Number");
            btnSubmitNumber.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnSubmitNumber.TouchUpInside += BtnSubmitNumber_TouchUpInside;
            btnSubmitNumber.Enabled = false;
            btnSubmitNumber.UserInteractionEnabled = false;

            // Add the controls to the view
            vwCallMe.AddSubview(lblCallMeInfo);
            vwCallMe.AddSubview(txtPhoneNum);
            vwCallMe.AddSubview(btnSubmitNumber);

            // Add the textview as a subview to the popver and pop the dang thing
            cpuvCallMe.AddSubview(vwCallMe);
            cpuvCallMe.PopUp(true, delegate {
                Console.WriteLine("UIVCHearingTestResults:BtnCallMe_TouchUpInside - Custom popover will close");
            });
        }

        private void BtnSubmitNumber_TouchUpInside(object sender, EventArgs e)
        {
            // Assume that if we got here we passed validation
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);

            // Set up session info
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);
            userSession.PhoneNumber = txtPhoneNum.Text;

            // Local SQLite Update
            DatabaseHelper.SetPhoneNumber(db_path, App.globablSessionId, userSession.PhoneNumber);
            // AWS Update
            //AWSHelper.PerformSessionUpdate(userSession);

            // Close the popup
            cpuvCallMe.Close();
        }

        private void TxtPhoneNum_ValueChanged(object sender, EventArgs e)
        {
            // Trim it down if the user is trying somethin crazeballs
            if (txtPhoneNum.Text.Length >= 11)
            {
                txtPhoneNum.Text = txtPhoneNum.Text.Substring(0, 11);
            }

            // Basically we just check and make sure it is an integer that is 10 or 11 characters long
            // then enable the submit button
            string userEnteredPhoneNum = txtPhoneNum.Text;

            Console.WriteLine("UIVCHearingTestResult:BtnSubmitNumber_TouchUpInside - value being evaluated: {0}", userEnteredPhoneNum);

            bool isValid = ValidatePhoneNumber(userEnteredPhoneNum, true);

            if(isValid)
            {
                btnSubmitNumber.Enabled = true;
                btnSubmitNumber.UserInteractionEnabled = true;
            }
            else
            {
                btnSubmitNumber.Enabled = false;
                btnSubmitNumber.UserInteractionEnabled = false;
            }

        }

        public static bool ValidatePhoneNumber(string phone, bool IsRequired)
        {
            //blamo: https://stackoverflow.com/questions/29970244/how-to-validate-a-phone-number

            if (string.IsNullOrEmpty(phone) & !IsRequired)
                return true;

            if (string.IsNullOrEmpty(phone) & IsRequired)
                return false;

            var cleaned = RemoveNonNumeric(phone);
            if (IsRequired)
            {
                if (cleaned.Length == 10)
                    return true;
                else if (cleaned.Length == 11)
                    return true;
                else
                    return false;
            }
            else
            {
                if (cleaned.Length == 0)
                    return true;
                else if (cleaned.Length > 0 & cleaned.Length < 10)
                    return false;
                else if (cleaned.Length == 10)
                    return true;
                else if (cleaned.Length == 11)
                    return true;
                else
                    return false;
            }
        }

        public static string RemoveNonNumeric(string phone)
        {
            return Regex.Replace(phone, @"[^0-9]+", "");
        }

        /*
        private bool ValidatePhoneNumber(string phoneNum)
        {
            // Clean the string up of common phone number stuff
            phoneNum = phoneNum.Replace("-", "");
            phoneNum = phoneNum.Replace("+", "");
            phoneNum = phoneNum.Replace(" ", "");
            
            // Basically we just check and make sure it is an integer that is 10 or 11 characters long
            var isNumeric = int.TryParse(phoneNum, out int n);

            if(!isNumeric)
            {
                Console.WriteLine("UIVCHearingTestResult:ValidatePhoneNumber - isNumeric failed, returning false");
                return false;
            }

            if(phoneNum.Length == 10 || phoneNum.Length == 11)
            {
                Console.WriteLine("UIVCHearingTestResult:ValidatePhoneNumber - phone number passed validation!");
                return true;
            }

            Console.WriteLine("UIVCHearingTestResult:ValidatePhoneNumber - phone number failed validation, returnign false");
            return false;
        }
        */

        private string PerformLossCheck(Session currentSession)
        {
            // Initialize our results
            hasBadHearing = false;

            // Check right ear results
            Dictionary<string, float> dictRightEarResults = new Dictionary<string, float>()
            {
                { "500",  (currentSession.RightEarThreshold_500Hz  != -99f) ? currentSession.RightEarThreshold_500Hz  : -5.0f },
                { "1000", (currentSession.RightEarThreshold_1000Hz != -99f) ? currentSession.RightEarThreshold_1000Hz : -5.0f },
                { "2000", (currentSession.RightEarThreshold_2000Hz != -99f) ? currentSession.RightEarThreshold_2000Hz : -5.0f },
                { "4000", (currentSession.RightEarThreshold_4000Hz != -99f) ? currentSession.RightEarThreshold_4000Hz : -5.0f },
            };
            foreach (var kvp in dictRightEarResults.ToArray())
            {
                // Access using: kvp.Key and kvp.Value
                // If any kvp.Value is >= 30.0f then set the hasBadHearing flag
                if (kvp.Value > 30.0f)
                {
                    System.Console.WriteLine("UIVCHearingTestResults : PerformDiagnosis - Found bad hearing at " + kvp.Key);
                    hasBadHearing = true;
                    break;
                }
            }

            // Check left ear results
            Dictionary<string, float> dictLeftEarResults = new Dictionary<string, float>()
            {
                { "500",  (currentSession.LeftEarThreshold_500Hz  != -99f) ? currentSession.LeftEarThreshold_500Hz  : -5.0f },
                { "1000", (currentSession.LeftEarThreshold_1000Hz != -99f) ? currentSession.LeftEarThreshold_1000Hz : -5.0f },
                { "2000", (currentSession.LeftEarThreshold_2000Hz != -99f) ? currentSession.LeftEarThreshold_2000Hz : -5.0f },
                { "4000", (currentSession.LeftEarThreshold_4000Hz != -99f) ? currentSession.LeftEarThreshold_4000Hz : -5.0f },
            };
            foreach (var kvp in dictLeftEarResults.ToArray())
            {
                // Access using: kvp.Key and kvp.Value
                // If any kvp.Value is >= 30.0f then set the hasBadHearing flag
                if (kvp.Value > 30.0f)
                {
                    System.Console.WriteLine("UIVCHearingTestResults : PerformDiagnosis - Found bad hearing at " + kvp.Key);
                    hasBadHearing = true;
                    break;
                }
            }

            //TODO: If you want to format some of the string, like make bold text, you'll need to create a FormattedString
            //      like this: https://developer.xamarin.com/guides/xamarin-forms/user-interface/text/label/#Formatted_Text

            if (hasBadHearing)
            {
                string returnString = "Our testing indicates that you may suffer from a hearing loss.\n";
                returnString += "Please continue on to see a list of our recommended products.";
                return returnString;

            }
            else
            {
                string returnString = "Congratulations! It looks like your hearing is ok! We do not have\n";
                returnString += "any additional products to recommend today.";
                return returnString;

            }

        }

        private void BtnResultsReviewFinished_TouchUpInside(object sender, EventArgs e)
        {
            /* TODO: RESTORE W AWS
            if (hasBadHearing)
            {
                // Transition to new storyboard
                UIStoryboard checkoutProcessBoard = UIStoryboard.FromName("CheckoutProcess", null);
                UIViewController uivcTestingFinished = (UIViewController)checkoutProcessBoard.InstantiateViewController("UIVCTestingFinished");
                //uivcTestingFinished.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
                this.PresentViewController(uivcTestingFinished, true, null);
            }
            else
            {
                PerformSegue("segTestResultsToReg", (Foundation.NSObject)sender);
            }
            */
            PerformSegue("segTestResultsToReg", (Foundation.NSObject)sender);
        }

        // Handle to AppDelegate - App
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

        private void BtnEXIT_TouchUpInside(object sender, EventArgs e)
        {
            // Make sure user really wants to exit
            Console.WriteLine("UIVCHearingTest:BtnEXIT_TouchUpInside - user clicked the exit test button");

            string alert_message = "Are you sure you want to exit the hearing test? All settings will be reset.";

            var okAlertController = UIAlertController.Create("Are you sure?", alert_message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Continue Test", UIAlertActionStyle.Default, null));
            okAlertController.AddAction(UIAlertAction.Create("Exit Test", UIAlertActionStyle.Default, alert => ExitTest(sender)));
            PresentViewController(okAlertController, true, null);
        }

        private void ExitTest(object sender)
        {
            // Transition back to registration screen
            UIStoryboard checkoutProcessBoard = UIStoryboard.FromName("Main", null);
            UIViewController uivcTestingFinished = (UIViewController)checkoutProcessBoard.InstantiateViewController("UIVCRegistration");
            this.PresentViewController(uivcTestingFinished, true, null);
        }
    }
}