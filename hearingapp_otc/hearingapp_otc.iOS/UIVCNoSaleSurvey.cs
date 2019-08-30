using Foundation;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Drawing;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCNoSaleSurvey : UIViewController
    {
        public UIVCNoSaleSurvey(IntPtr handle) : base(handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Correct color background and top nav label colors
            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;

            // Paint flat button - Complete Session
            var newBtnX = btnExitSession.Frame.X;
            var newBtnY = btnExitSession.Frame.Y;
            var newBtnWidth = btnExitSession.Frame.Size.Width;
            var newBtnHeight = btnExitSession.Frame.Size.Height;
            var btnExitSessionFlat = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnExitSessionFlat.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnExitSessionFlat.SetTitle("Complete Survey");
            btnExitSessionFlat.TouchUpInside += BtnExitSessionFlat_TouchUpInside;
            View.AddSubview(btnExitSessionFlat);

            // Pretty up the UISegment views
            var uisegFont = UIFont.FromName("Helvetica-Bold", 20f);

            uisegQFutureContact.TintColor = FlatColors.WetAsphalt;
            uisegQFutureContact.SetTitleTextAttributes(new UITextAttributes() { Font = uisegFont }, UIControlState.Normal);

        }

        private void BtnExitSessionFlat_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("segTYExitFromSurvey", (Foundation.NSObject)sender);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            Console.WriteLine("UIVCNoSaleSurvey:PrepareForSegue - preparing for segue");

            // not really sure what ticket the below todo items are supposed to be linked to...

            //TODO: Send some kind of SessionCompleteTimestamp string - also do after payment

            //TODO: Do a final local/aws update

            //TODO: Unset global session Id

            //TODO: Show a dialog telling them their session has ended, pop to root view
        }
    }
}