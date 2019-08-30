using Foundation;
using System;
using UIKit;
using MediaPlayer;
using System.Drawing;
using AVKit;
using hearingapp_otc.iOS.UIClasses;
using AVFoundation;
using System.Threading;
using System.IO;
using hearingapp_otc.Classes;

namespace hearingapp_otc.iOS
{
    public partial class UIVCCalibration : UIViewController
    {
        //public int sessionId;

        public UIVCCalibration(IntPtr handle) : base(handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Set view background to app background standard
            View.BackgroundColor = FlatColors.Clouds;

            // Adjust the top title text color to match larger background
            lblQuickCalib.TextColor = FlatColors.Clouds;

            // Set DevMode Options
            lblTestProgress.Hidden = !App.devMode;
            progressTesting.Hidden = !App.devMode;
            txtDevStartDb.Hidden = !App.devMode;
            lblDev1.Hidden = !App.devMode;
            lblDev2.Hidden = !App.devMode;

            // Setting the progress bar
            progressTesting.Progress = 0.25f;

            // Set the developer options text boxes equal to the current application delegate core values
            txtDevStartDb.Text = App.db_start.ToString();

            // Set the color of the calibration slider button (called Thumb)
            slideCalibrate.ThumbTintColor = FlatColors.Sunflower;

            // Set DevMode Options
            
            progressTesting.Hidden = !App.devMode;

            //Map a FlatUI button over top of existing btnCalibrationComplete
            var newBtnX = btnCalibrationComplete.Frame.X;
            var newBtnY = btnCalibrationComplete.Frame.Y;
            var newBtnWidth = btnCalibrationComplete.Frame.Size.Width;
            var newBtnHeight = btnCalibrationComplete.Frame.Size.Height;
            var btnCalibrationCompleteFLAT = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnCalibrationCompleteFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnCalibrationCompleteFLAT.SetTitle("Continue →");
            btnCalibrationCompleteFLAT.TouchUpInside += BtnCalibrationComplete_TouchUpInside;
            View.AddSubview(btnCalibrationCompleteFLAT);
            
            // Only hide slider and add MPVolumeView on real device (won't show up on simulator)
            if (ObjCRuntime.Runtime.Arch.Equals(ObjCRuntime.Arch.DEVICE))
            {
                // Overlay MPVolumeView onto the slider and hide the slider
                slideCalibrate.Hidden = true;
                var newSliderX = slideCalibrate.Frame.X;
                var newSliderY = slideCalibrate.Frame.Y;
                var newSliderWidth = slideCalibrate.Frame.Size.Width;
                var newSliderHeight = slideCalibrate.Frame.Size.Height;
                var mpvolview = new MPVolumeView(new CoreGraphics.CGRect(newSliderX, newSliderY, newSliderWidth, newSliderHeight));
                mpvolview.ShowsRouteButton = false;
                mpvolview.ShowsVolumeSlider = true;
                mpvolview.UserInteractionEnabled = true;
                mpvolview.TintColor = FlatColors.BelizeHole;
                View.AddSubview(mpvolview);
            }

        }

        private void BtnCalibrationComplete_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("CalibrationSegue", (Foundation.NSObject)sender);
        }
        
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            //We only care about the value on an actual device
            if (ObjCRuntime.Runtime.Arch.Equals(ObjCRuntime.Arch.DEVICE))
            {
                if (PerformFormValidation())
                {
                    base.PrepareForSegue(segue, sender);

                    // Get the current session Id
                    string db_name = "sessions_db.sqlite";
                    string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    string db_path = Path.Combine(folderPath, db_name);

                    // Update the session with the values set in the text boxes
                    Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);
                    userSession.dBStart = float.Parse(txtDevStartDb.Text);
                    DatabaseHelper.SetDbInfo(db_path, userSession.Id, userSession.dBStart);

                    // Sync with AWS
                    //AWSHelper.PerformSessionUpdate(userSession);

                    // Set our global
                    App.db_start = float.Parse(txtDevStartDb.Text);

                }
            } 
            // In iPhone Simulator. Let's try to make it work? But not try too hard...
            else
            {
                base.PrepareForSegue(segue, sender);
                // Get the current session Id
                string db_name = "sessions_db.sqlite";
                string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string db_path = Path.Combine(folderPath, db_name);
                // Update the session with the values set in the text boxes
                Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);
                userSession.dBStart = float.Parse(txtDevStartDb.Text);
                DatabaseHelper.SetDbInfo(db_path, userSession.Id, userSession.dBStart);

                //Sync with AWS
                //AWSHelper.PerformSessionUpdate(userSession);
            }


        }

        // Checks to see if the slider is all the way to the right
        private bool PerformFormValidation()
        {
            if (App.AudioManager.GetOutputVolume() != 1.0f)
            {
                var okAlertController = UIAlertController.Create("Please Calibrate", "Please adjust the slider all the way to the right.", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                return false;
            }
            else
            {
                return true;
            }
        }

        // For getting a handle to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }
    }
}
 