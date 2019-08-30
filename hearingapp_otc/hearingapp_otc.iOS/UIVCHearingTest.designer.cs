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
    [Register ("UIVCHearingTest")]
    partial class UIVCHearingTest
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn1000hz_30db { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn1000hz_40db { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btn1000hz_80db { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBeginHearingTest { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnEXITFromTest { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnStopTest { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnToneHeard { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblActivityIndicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInstructions { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSoundPlaying { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblStartDbLevel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTemp1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTemp2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTestProgress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopNav { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView progressCompletion { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btn1000hz_30db != null) {
                btn1000hz_30db.Dispose ();
                btn1000hz_30db = null;
            }

            if (btn1000hz_40db != null) {
                btn1000hz_40db.Dispose ();
                btn1000hz_40db = null;
            }

            if (btn1000hz_80db != null) {
                btn1000hz_80db.Dispose ();
                btn1000hz_80db = null;
            }

            if (btnBeginHearingTest != null) {
                btnBeginHearingTest.Dispose ();
                btnBeginHearingTest = null;
            }

            if (btnEXITFromTest != null) {
                btnEXITFromTest.Dispose ();
                btnEXITFromTest = null;
            }

            if (btnStopTest != null) {
                btnStopTest.Dispose ();
                btnStopTest = null;
            }

            if (btnToneHeard != null) {
                btnToneHeard.Dispose ();
                btnToneHeard = null;
            }

            if (lblActivityIndicator != null) {
                lblActivityIndicator.Dispose ();
                lblActivityIndicator = null;
            }

            if (lblInstructions != null) {
                lblInstructions.Dispose ();
                lblInstructions = null;
            }

            if (lblSoundPlaying != null) {
                lblSoundPlaying.Dispose ();
                lblSoundPlaying = null;
            }

            if (lblStartDbLevel != null) {
                lblStartDbLevel.Dispose ();
                lblStartDbLevel = null;
            }

            if (lblTemp1 != null) {
                lblTemp1.Dispose ();
                lblTemp1 = null;
            }

            if (lblTemp2 != null) {
                lblTemp2.Dispose ();
                lblTemp2 = null;
            }

            if (lblTestProgress != null) {
                lblTestProgress.Dispose ();
                lblTestProgress = null;
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