using Foundation;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Drawing;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCPayCheck : UIViewController
    {
        public UIVCPayCheck (IntPtr handle) : base (handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status 

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Correct color background and top nav label colors
            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;

            // Paint flat button - Exit - Finished Payment
            var newBtnX = btnFinishedPayment.Frame.X;
            var newBtnY = btnFinishedPayment.Frame.Y;
            var newBtnWidth = btnFinishedPayment.Frame.Size.Width;
            var newBtnHeight = btnFinishedPayment.Frame.Size.Height;
            var btnFinishedPaymentFlat = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnFinishedPaymentFlat.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnFinishedPaymentFlat.SetTitle("Exit - Payment Complete");
            btnFinishedPaymentFlat.TouchUpInside += BtnFinishedPaymentFlat_TouchUpInside;
            View.AddSubview(btnFinishedPaymentFlat);

            // Add back button
            btnBackFromPayByCheck.Hidden = true;
            FlatBackButton btnBackFLAT = new FlatBackButton((int)btnBackFromPayByCheck.Frame.X, (int)btnBackFromPayByCheck.Frame.Y);
            btnBackFLAT.TouchUpInside += BtnBack_TouchUpInside;
            View.AddSubview(btnBackFLAT);

        }


        private void BtnFinishedPaymentFlat_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("segTYExitFromCheck", (Foundation.NSObject)sender);
        }

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

        private void BtnBack_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("segBackFromPayByCheck", (Foundation.NSObject)sender);
        }


    }
}