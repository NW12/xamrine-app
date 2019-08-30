using Foundation;
using System;
using UIKit;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;

namespace hearingapp_otc.iOS
{
    public partial class UIVCHearinAidSelection : UIViewController
    {
        //public int sessoinId;

        public UIVCHearinAidSelection(IntPtr handle) : base(handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status 

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;
            lblInfo.BackgroundColor = FlatColors.Clouds;
        }

        // Handle to AppDelegate - App
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

    }
}