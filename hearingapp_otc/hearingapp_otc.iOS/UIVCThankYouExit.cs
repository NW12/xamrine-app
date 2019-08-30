using Foundation;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Drawing;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCThankYouExit : UIViewController
    {
        public UIVCThankYouExit (IntPtr handle) : base (handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Correct color background and top nav label colors
            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;

            // Paint flat button - Exit to root UIVC
            var newBtnX = btnExitOrder.Frame.X;
            var newBtnY = btnExitOrder.Frame.Y;
            var newBtnWidth = btnExitOrder.Frame.Size.Width;
            var newBtnHeight = btnExitOrder.Frame.Size.Height;
            var btnExitOrderFlat = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnExitOrderFlat.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnExitOrderFlat.SetTitle("Exit Session");
            btnExitOrderFlat.TouchUpInside += BtnExitOrder_TouchUpInside;
            View.AddSubview(btnExitOrderFlat);

            // Original button, leaving it wired up for now
            btnExitOrder.TouchUpInside += BtnExitOrder_TouchUpInside;
        }

        private void BtnExitOrder_TouchUpInside(object sender, EventArgs e)
        {
            /*
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var storyboard = UIStoryboard.FromName("Main", null); // passing null means use the main bundle. #hack
                UIViewController rootVC = storyboard.InstantiateViewController("UIVCRegistration") as UIViewController;
                var core = UIApplication.SharedApplication.Windows[0].RootViewController;
                core.ShowViewController(rootVC, (Foundation.NSObject)sender);
            });
            */
            // Transition to new storyboard
            UIStoryboard checkoutProcessBoard = UIStoryboard.FromName("Main", null);
            UIViewController uivcTestingFinished = (UIViewController)checkoutProcessBoard.InstantiateViewController("UIVCRegistration");
            this.PresentViewController(uivcTestingFinished, true, null);
        }

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

    }
}