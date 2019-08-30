using Foundation;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Drawing;
using System.IO;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCSurvey : UIViewController
    {
        public FlatButton btnContinueSurveyFLAT;
        public FlatExitButton btnExitFLAT;

        public UIVCSurvey (IntPtr handle) : base (handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            // Customize the look and feel of the UISegmentControls
            var uisegFont = UIFont.FromName("Helvetica-Bold", 20f);

            // Set DevMode options
            lblTestProgress.Hidden = !App.devMode;
            progressTesting.Hidden = !App.devMode;

            btnContinueSurvey.Hidden = true;
            btnEXITFromSurvey.Hidden = true;

            //uisegQ1.BackgroundColor = FlatColors.Clouds;
            uisegQ1.TintColor = FlatColors.WetAsphalt;
            uisegQ1.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);
            uisegQ2.TintColor = FlatColors.WetAsphalt;
            uisegQ2.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);
            uisegQ3.TintColor = FlatColors.WetAsphalt;
            uisegQ3.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);
            uisegQ4.TintColor = FlatColors.WetAsphalt;
            uisegQ4.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);
            uisegQ5.TintColor = FlatColors.WetAsphalt;
            uisegQ5.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);
            uisegQ6.TintColor = FlatColors.WetAsphalt;
            uisegQ6.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);
            uisegQ7.TintColor = FlatColors.WetAsphalt;
            uisegQ7.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);
        }

        public override void ViewDidLoad()
        {
            // Set background color
            //View.BackgroundColor = FlatColors.Clouds;

            base.ViewDidLoad();

            // Set background color
            View.BackgroundColor = FlatColors.Clouds;

            // Adjust the top title text color to match larger background
            lblWelcome.TextColor = FlatColors.Clouds;

            // Sets the "Welcome {NAME}!" label based on session id
            SetLabelInfo();

            // Setting the progress bar
            progressTesting.Progress = 0.15f;

            //Map a FlatUI button over top of existing btnContinueSurvey
            var newBtnX = btnContinueSurvey.Frame.X;
            var newBtnY = btnContinueSurvey.Frame.Y;
            var newBtnWidth = btnContinueSurvey.Frame.Size.Width;
            var newBtnHeight = btnContinueSurvey.Frame.Size.Height;
            btnContinueSurveyFLAT = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnContinueSurveyFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnContinueSurveyFLAT.SetTitle("Continue →");
            btnContinueSurveyFLAT.TouchUpInside += BtnContinueSurvey_TouchUpInside;
            btnContinueSurveyFLAT.Enabled = false;
            btnContinueSurveyFLAT.UserInteractionEnabled = false;
            View.AddSubview(btnContinueSurveyFLAT);

            // Add Exit button
            btnExitFLAT = new FlatExitButton((int)btnEXITFromSurvey.Frame.X, (int)btnEXITFromSurvey.Frame.Y);
            btnExitFLAT.TouchUpInside += BtnEXIT_TouchUpInside;
            View.AddSubview(btnExitFLAT);

            // When final question is answered
            uisegQ7.ValueChanged += UisegQ7_ValueChanged;
        }


        private void UisegQ7_ValueChanged(object sender, EventArgs e)
        {
            if(!btnContinueSurveyFLAT.Enabled) {
                btnContinueSurveyFLAT.Enabled = true;
                btnContinueSurveyFLAT.UserInteractionEnabled = true;
                btnContinueSurvey.Hidden = true;
            }
        }

        // Sets the "Welcome {NAME}!" label based on session id
        private int SetLabelInfo()
        {
            if (!App.globablSessionId.Equals(null))
            {
                string userName = GetFirstname(App.globablSessionId);
                lblWelcome.Text = string.Format($"Welcome {userName}!");
                Console.WriteLine("DBG UIVCSurvey.SetLabelInfo : SUCCESS!");
                return 0;
            }
            else
            {
                Console.WriteLine("DBG UIVCSurvey.SetLabelInfo : FAIL! NO SESSION INFO SET!");
                return 1;
            }
        }

        // Databse interface method to get first name of user based on App.globalSessionId
        string GetFirstname(int id)
        {
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);

            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);

            return userSession.FirstName;
        }

        private void BtnContinueSurvey_TouchUpInside(object sender, EventArgs e)
        {
            // If validations are disabled AND the questions are not answered (except 7, which enables the Continue button), autofill and move on
            if (App.validationsDisabled && uisegQ1.SelectedSegment == -1 && uisegQ2.SelectedSegment == -1 && 
                uisegQ3.SelectedSegment == -1 && uisegQ4.SelectedSegment == -1 && uisegQ5.SelectedSegment == -1 &&
                uisegQ6.SelectedSegment == -1)
            {
                Console.WriteLine("UIVCSurvey:BtnContinueSurvey_TouchUpInside - validations disabled, continuing");
                uisegQ1.SelectedSegment = 0;
                uisegQ2.SelectedSegment = 0;
                uisegQ3.SelectedSegment = 0;
                uisegQ4.SelectedSegment = 0;
                uisegQ5.SelectedSegment = 0;
                uisegQ6.SelectedSegment = 1;
                uisegQ7.SelectedSegment = 0;

                // Perform segue, which save survey answers to session object
                PerformSegue("SurveyDoneSegue", (Foundation.NSObject)sender);
            }
            else if (btnContinueSurveyFLAT.Enabled)
            {
                // Make sure all the questions have been answered (-1 indicates no segment has been selected)
                if (uisegQ1.SelectedSegment == -1 || uisegQ2.SelectedSegment == -1 || uisegQ3.SelectedSegment == -1 || uisegQ4.SelectedSegment == -1 ||
                    uisegQ5.SelectedSegment == -1 || uisegQ6.SelectedSegment == -1 || uisegQ7.SelectedSegment == -1)
                {
                    // Validations are enabled, alert the user and don't let them continue until they answer the survey questions
                    Console.WriteLine("UIVCSurvey:BtnContinueSurvey_TouchUpInside - user did not complete all survey questions");
                    Console.WriteLine("UIVCSurvey:BtnContinueSurvey_TouchUpInside - " + uisegQ1.Selected);
                    Console.WriteLine("UIVCSurvey:BtnContinueSurvey_TouchUpInside - " + uisegQ1.SelectedSegment);

                    string alert_message = "Please answer either \"No\" or \"Yes\" to all survey questions.";

                    var okAlertController = UIAlertController.Create("Please Complete the Survey", alert_message, UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("Back to Survey", UIAlertActionStyle.Default, null));
                    PresentViewController(okAlertController, true, null);
                }
                // If the user has answered "yes" to any of the survey questions we have a red flag
                else if (uisegQ1.SelectedSegment == 1 || uisegQ2.SelectedSegment == 1 || uisegQ3.SelectedSegment == 1 || uisegQ4.SelectedSegment == 1 ||
                    uisegQ5.SelectedSegment == 1 || uisegQ6.SelectedSegment == 1 || uisegQ7.SelectedSegment == 1)
                {
                    Console.WriteLine("UIVCSurvey:BtnContinueSurvey_TouchUpInside - red flag detected from survey, prompting user");

                    string alert_message = "According to your survey results, you should consider seeing a doctor instead of seeking an over-the-counter solution.";
                    alert_message += System.Environment.NewLine + System.Environment.NewLine;
                    alert_message += "If you would like, you can still complete the hearing test today.";

                    var okAlertController = UIAlertController.Create("Important - Read Carefully", alert_message, UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("Continue", UIAlertActionStyle.Default, alert => ContinueTest(sender)));
                    okAlertController.AddAction(UIAlertAction.Create("Exit Test", UIAlertActionStyle.Default, alert => ExitTest()));
                    PresentViewController(okAlertController, true, null);
                }
                // Ok, good to go. All questions were answered, and were answered as "No"
                else
                {

                    Console.WriteLine("UIVCSurvey:BtnContinueSurvey_TouchUpInside - no red flags, continuing test");

                    // Perform segue, which save survey answers to session object
                    PerformSegue("SurveyDoneSegue", (Foundation.NSObject)sender);
                }
                

                return;
            }

            // If the button is not enabled yet (survey not complete)
            else
                return;
        }

        private void ContinueTest(object sender)
        {
            Console.WriteLine("UIVCSurvey:ContinueTest - continuing test");
            PerformSegue("SurveyDoneSegue", (Foundation.NSObject)sender);
        }

        private void ExitTest()
        {
            Console.WriteLine("UIVCSurvey:ExitTest - exiting test");
            NavigationController.PopToRootViewController(true);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            Console.WriteLine("UIVCSurvey:PrepareForSegue - preparing for segue");

            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session sess = new Session() {
                SurveQ1 = (uisegQ1.SelectedSegment == 0) ? false : true,
                SurveQ2 = (uisegQ2.SelectedSegment == 0) ? false : true,
                SurveQ3 = (uisegQ3.SelectedSegment == 0) ? false : true,
                SurveQ4 = (uisegQ4.SelectedSegment == 0) ? false : true,
                SurveQ5 = (uisegQ5.SelectedSegment == 0) ? false : true,
                SurveQ6 = (uisegQ6.SelectedSegment == 0) ? false : true,
                SurveQ7 = (uisegQ7.SelectedSegment == 0) ? false : true
            };
            DatabaseHelper.SetSurveyAnswers(db_path, App.globablSessionId, sess);

            Console.WriteLine("UIVCSurvey:PrepareForSegue - finished preparing for segue");

        }

        // Handle to AppDelegate - App
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

        //partial void BtnEXITFromSurvey_TouchUpInside(object sender, EventArgs e)
        //{
        //    // Make sure user really wants to exit
        //    Console.WriteLine("UIVCSurvey:BtnEXITFromSurvey_TouchUpInside - user clicked the exit test button");

        //    string alert_message = "Are you sure you want to exit the hearing test? All settings will be reset.";

        //    var okAlertController = UIAlertController.Create("Are you sure?", alert_message, UIAlertControllerStyle.Alert);
        //    okAlertController.AddAction(UIAlertAction.Create("Continue Test", UIAlertActionStyle.Default, null));
        //    okAlertController.AddAction(UIAlertAction.Create("Exit Test", UIAlertActionStyle.Default, alert => ExitTest(sender)));
        //    PresentViewController(okAlertController, true, null);   
        //}

        
        private void BtnEXIT_TouchUpInside(object sender, EventArgs e)
        {
            // Make sure user really wants to exit
            Console.WriteLine("UIVCSurvey:BtnEXIT_TouchUpInside - user clicked the exit test button");

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