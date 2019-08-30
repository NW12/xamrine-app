// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace hearingapp_otc.iOS
{
    [Register ("UIVCHearinAidSelection")]
    partial class UIVCHearinAidSelection
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnContinueCalibration { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopNav { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView progressCompletion { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnContinueCalibration != null) {
                btnContinueCalibration.Dispose ();
                btnContinueCalibration = null;
            }

            if (lblInfo != null) {
                lblInfo.Dispose ();
                lblInfo = null;
            }

            if (lblTopNav != null) {
                lblTopNav.Dispose ();
                lblTopNav = null;
            }

            if (progressCompletion != null) {
                progressCompletion.Dispose ();
                progressCompletion = null;
            }
        }
    }
}