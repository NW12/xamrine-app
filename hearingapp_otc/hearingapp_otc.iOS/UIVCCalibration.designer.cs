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
    [Register ("UIVCCalibration")]
    partial class UIVCCalibration
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCalibrationComplete { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblArrowIndicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCalibStart { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDev1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDev2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQuickCalib { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTestProgress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView progressTesting { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider slideCalibrate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtDevStartDb { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCalibrationComplete != null) {
                btnCalibrationComplete.Dispose ();
                btnCalibrationComplete = null;
            }

            if (lblArrowIndicator != null) {
                lblArrowIndicator.Dispose ();
                lblArrowIndicator = null;
            }

            if (lblCalibStart != null) {
                lblCalibStart.Dispose ();
                lblCalibStart = null;
            }

            if (lblDev1 != null) {
                lblDev1.Dispose ();
                lblDev1 = null;
            }

            if (lblDev2 != null) {
                lblDev2.Dispose ();
                lblDev2 = null;
            }

            if (lblQuickCalib != null) {
                lblQuickCalib.Dispose ();
                lblQuickCalib = null;
            }

            if (lblTestProgress != null) {
                lblTestProgress.Dispose ();
                lblTestProgress = null;
            }

            if (progressTesting != null) {
                progressTesting.Dispose ();
                progressTesting = null;
            }

            if (slideCalibrate != null) {
                slideCalibrate.Dispose ();
                slideCalibrate = null;
            }

            if (txtDevStartDb != null) {
                txtDevStartDb.Dispose ();
                txtDevStartDb = null;
            }
        }
    }
}