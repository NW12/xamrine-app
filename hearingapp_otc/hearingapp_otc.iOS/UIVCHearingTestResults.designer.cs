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
    [Register ("UIVCHearingTestResults")]
    partial class UIVCHearingTestResults
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCallMe { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnEXITFromResults { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnResultsReviewFinished { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        hearingapp_otc.iOS.HearingResultsGridView hearingTestGrid { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblBottomInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLegend1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLegend2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLegend3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLegend4 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopNav { get; set; }

        [Action ("BtnResultsReviewFinished_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnResultsReviewFinished_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCallMe != null) {
                btnCallMe.Dispose ();
                btnCallMe = null;
            }

            if (btnEXITFromResults != null) {
                btnEXITFromResults.Dispose ();
                btnEXITFromResults = null;
            }

            if (btnResultsReviewFinished != null) {
                btnResultsReviewFinished.Dispose ();
                btnResultsReviewFinished = null;
            }

            if (hearingTestGrid != null) {
                hearingTestGrid.Dispose ();
                hearingTestGrid = null;
            }

            if (lblBottomInfo != null) {
                lblBottomInfo.Dispose ();
                lblBottomInfo = null;
            }

            if (lblInfo != null) {
                lblInfo.Dispose ();
                lblInfo = null;
            }

            if (lblLegend1 != null) {
                lblLegend1.Dispose ();
                lblLegend1 = null;
            }

            if (lblLegend2 != null) {
                lblLegend2.Dispose ();
                lblLegend2 = null;
            }

            if (lblLegend3 != null) {
                lblLegend3.Dispose ();
                lblLegend3 = null;
            }

            if (lblLegend4 != null) {
                lblLegend4.Dispose ();
                lblLegend4 = null;
            }

            if (lblTopInfo != null) {
                lblTopInfo.Dispose ();
                lblTopInfo = null;
            }

            if (lblTopNav != null) {
                lblTopNav.Dispose ();
                lblTopNav = null;
            }
        }
    }
}