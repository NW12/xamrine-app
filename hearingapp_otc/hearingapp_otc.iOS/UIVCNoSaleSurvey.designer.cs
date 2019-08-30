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
    [Register ("UIVCNoSaleSurvey")]
    partial class UIVCNoSaleSurvey
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnExitSession { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQFutureContact { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopNav { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl uisegQFutureContact { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnExitSession != null) {
                btnExitSession.Dispose ();
                btnExitSession = null;
            }

            if (lblQFutureContact != null) {
                lblQFutureContact.Dispose ();
                lblQFutureContact = null;
            }

            if (lblTopNav != null) {
                lblTopNav.Dispose ();
                lblTopNav = null;
            }

            if (uisegQFutureContact != null) {
                uisegQFutureContact.Dispose ();
                uisegQFutureContact = null;
            }
        }
    }
}