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
    [Register ("UIVCPayCheck")]
    partial class UIVCPayCheck
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBackFromPayByCheck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFinishedPayment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopNav { get; set; }

        [Action ("BtnFinishedPayment_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnFinishedPayment_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnBackFromPayByCheck != null) {
                btnBackFromPayByCheck.Dispose ();
                btnBackFromPayByCheck = null;
            }

            if (btnFinishedPayment != null) {
                btnFinishedPayment.Dispose ();
                btnFinishedPayment = null;
            }

            if (lblTopNav != null) {
                lblTopNav.Dispose ();
                lblTopNav = null;
            }
        }
    }
}