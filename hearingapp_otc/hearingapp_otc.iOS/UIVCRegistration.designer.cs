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
    [Register ("UIVCRegistration")]
    partial class UIVCRegistration
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        hearingapp_otc.iOS.awsLoginView awsLoginView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnContinueReg { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDoLogin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTC1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTC2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnViewTC { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInfoEmail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInfoName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLoggingIn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPleaseLogin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPleaseLoginSmall { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTestProgress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblVersionInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView progressCompletion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch switchDevOptions { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtAWSPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtAWSUsername { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtEmailAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtFirstName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtLastName { get; set; }

        [Action ("BtnViewTC_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnViewTC_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (awsLoginView != null) {
                awsLoginView.Dispose ();
                awsLoginView = null;
            }

            if (btnContinueReg != null) {
                btnContinueReg.Dispose ();
                btnContinueReg = null;
            }

            if (btnDoLogin != null) {
                btnDoLogin.Dispose ();
                btnDoLogin = null;
            }

            if (btnTC1 != null) {
                btnTC1.Dispose ();
                btnTC1 = null;
            }

            if (btnTC2 != null) {
                btnTC2.Dispose ();
                btnTC2 = null;
            }

            if (btnViewTC != null) {
                btnViewTC.Dispose ();
                btnViewTC = null;
            }

            if (lblInfoEmail != null) {
                lblInfoEmail.Dispose ();
                lblInfoEmail = null;
            }

            if (lblInfoName != null) {
                lblInfoName.Dispose ();
                lblInfoName = null;
            }

            if (lblLoggingIn != null) {
                lblLoggingIn.Dispose ();
                lblLoggingIn = null;
            }

            if (lblPleaseLogin != null) {
                lblPleaseLogin.Dispose ();
                lblPleaseLogin = null;
            }

            if (lblPleaseLoginSmall != null) {
                lblPleaseLoginSmall.Dispose ();
                lblPleaseLoginSmall = null;
            }

            if (lblTestProgress != null) {
                lblTestProgress.Dispose ();
                lblTestProgress = null;
            }

            if (lblVersionInfo != null) {
                lblVersionInfo.Dispose ();
                lblVersionInfo = null;
            }

            if (progressCompletion != null) {
                progressCompletion.Dispose ();
                progressCompletion = null;
            }

            if (switchDevOptions != null) {
                switchDevOptions.Dispose ();
                switchDevOptions = null;
            }

            if (txtAWSPassword != null) {
                txtAWSPassword.Dispose ();
                txtAWSPassword = null;
            }

            if (txtAWSUsername != null) {
                txtAWSUsername.Dispose ();
                txtAWSUsername = null;
            }

            if (txtEmailAddress != null) {
                txtEmailAddress.Dispose ();
                txtEmailAddress = null;
            }

            if (txtFirstName != null) {
                txtFirstName.Dispose ();
                txtFirstName = null;
            }

            if (txtLastName != null) {
                txtLastName.Dispose ();
                txtLastName = null;
            }
        }
    }
}