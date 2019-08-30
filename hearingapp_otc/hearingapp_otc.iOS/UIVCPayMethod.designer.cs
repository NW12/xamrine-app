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
    [Register ("UIVCPayMethod")]
    partial class UIVCPayMethod
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBackFromPaymentMethod { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPayByCC { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPayByCheck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopNav { get; set; }

        [Action ("BtnBackFromPaymentMethod_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnBackFromPaymentMethod_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnBackFromPaymentMethod != null) {
                btnBackFromPaymentMethod.Dispose ();
                btnBackFromPaymentMethod = null;
            }

            if (btnPayByCC != null) {
                btnPayByCC.Dispose ();
                btnPayByCC = null;
            }

            if (btnPayByCheck != null) {
                btnPayByCheck.Dispose ();
                btnPayByCheck = null;
            }

            if (lblTopNav != null) {
                lblTopNav.Dispose ();
                lblTopNav = null;
            }
        }
    }
}