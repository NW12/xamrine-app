using Foundation;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Drawing;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCPayMethod : UIViewController
    {
        public UIVCPayMethod (IntPtr handle) : base (handle)
        {
        }
        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Correct color background and top nav label colors
            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;

            // Paint Flat Button - Pay by CC
            var newBtnX = btnPayByCC.Frame.X;
            var newBtnY = btnPayByCC.Frame.Y;
            var newBtnWidth = btnPayByCC.Frame.Size.Width;
            var newBtnHeight = btnPayByCC.Frame.Size.Height;
            var btnPayByCCFlat = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnPayByCCFlat.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnPayByCCFlat.SetTitle("Pay by Credit Card");
            btnPayByCCFlat.TouchUpInside += BtnPayByCCFlat_TouchUpInside;
            View.AddSubview(btnPayByCCFlat);

            // Paint Flat Button - Pay by Check
            newBtnX = btnPayByCheck.Frame.X;
            newBtnY = btnPayByCheck.Frame.Y;
            newBtnWidth = btnPayByCheck.Frame.Size.Width;
            newBtnHeight = btnPayByCheck.Frame.Size.Height;
            var btnPayByCheckFlat = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnPayByCheckFlat.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnPayByCheckFlat.SetTitle("Pay by Check");
            btnPayByCheckFlat.TouchUpInside += BtnPayByCheckFlat_TouchUpInside;
            View.AddSubview(btnPayByCheckFlat);

            // Add back button - different from an Exit button because it needs to perform a segue
            btnBackFromPaymentMethod.Hidden = true;
            FlatBackButton btnBackFLAT = new FlatBackButton((int)btnBackFromPaymentMethod.Frame.X, (int)btnBackFromPaymentMethod.Frame.Y);
            btnBackFLAT.TouchUpInside += BtnBack_TouchUpInside;
            View.AddSubview(btnBackFLAT);
        }



        private void BtnPayByCheckFlat_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("segToPayByCheck", (Foundation.NSObject)sender);
        }

        private void BtnPayByCCFlat_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("segToPayByCC", (Foundation.NSObject)sender);
        }

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

        private void BtnBack_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("segBackFromPaymentMethod", (Foundation.NSObject)sender);
        }
    }
}