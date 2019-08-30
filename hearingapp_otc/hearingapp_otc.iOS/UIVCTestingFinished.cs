using Foundation;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCTestingFinished : UIViewController
    {
        public FlatButton btnContinueyFLAT;
        private int lblTopNavTapCount;
        private bool secretSKUChosen = false;

        public UIVCTestingFinished (IntPtr handle) : base (handle)
        {
        }

        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Correct color background and top nav label colors
            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;

            // Set TouchUpInside delegates for buttons
            btnContinue.Hidden = true;
            btnContinue.TouchUpInside += BtnContinue_TouchUpInside;
            btnNoSale.TouchUpInside += BtnNoSale_TouchUpInside;

            btnMoxiFitS2AmberSuede.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitS7TealBlast.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitS6Sandstorm.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitP6Platinum.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitS5Pewtershine.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitP7Pewter.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitS3EspressoBoost.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitP4Espresso.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitQ9Cinnamon.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitP8Charcoal.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFit01Beige.TouchUpInside += BtnMoxiFit_TouchUpInside;
            btnMoxiFitP2Amber.TouchUpInside += BtnMoxiFit_TouchUpInside;

            // Map flat Continue button over existing button
            var newBtnX = btnContinue.Frame.X;
            var newBtnY = btnContinue.Frame.Y;
            var newBtnWidth = btnContinue.Frame.Size.Width;
            var newBtnHeight = btnContinue.Frame.Size.Height;
            btnContinueyFLAT = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnContinueyFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnContinueyFLAT.SetTitle("Continue →");
            btnContinueyFLAT.TouchUpInside += BtnContinue_TouchUpInside;
            btnContinueyFLAT.Enabled = false;
            btnContinueyFLAT.UserInteractionEnabled = false;
            View.AddSubview(btnContinueyFLAT);

            // Map flat no purchase button over existing button
            newBtnX = btnNoSale.Frame.X;
            newBtnY = btnNoSale.Frame.Y;
            newBtnWidth = btnNoSale.Frame.Size.Width;
            newBtnHeight = btnNoSale.Frame.Size.Height;
            FlatButton btnNoSaleFLAT = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnNoSaleFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnNoSaleFLAT.SetTitle("I Do Not Wish to Purchase Today");
            btnNoSaleFLAT.Color = FlatColors.Alizarin;
            btnNoSaleFLAT.TouchUpInside += BtnNoSale_TouchUpInside;
            View.AddSubview(btnNoSaleFLAT);

            // Apply the button styling
            UpdateButtonBorderColors(UIColor.Black);
            // Fake select one of the SKUs so a picture is displayed
            UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-S2-AmberSuede.jpg");

            // This is for the hidden SKU for production testing of the checkout process
            secretSKUChosen = false;
            App.pricePointCents = AppDelegate.truePricePointCents;  // When creating a price point override, you'll need to remove all the paranoid calls to truePricePointCents
            lblRetailPrice.Text = string.Format("All for ${0}!", (float)App.pricePointCents / 100); // grab hardcoded value from UIVCPayCC

            lblTopNavTapCount = 0;
            UITapGestureRecognizer labelTap = new UITapGestureRecognizer(() => {
                // Add to our count whenever the top nav is tapped
                lblTopNavIncreaseTapCount();
            });
            lblTopNav.UserInteractionEnabled = true;
            lblTopNav.AddGestureRecognizer(labelTap);

        }

        private void lblTopNavIncreaseTapCount()
        {
            // Require a dozen taps in order to display the UIAlert - after the UIAlert reset the tap count
            lblTopNavTapCount += 1;
            Console.WriteLine("UIVCTestingFinished:lblTopNavIncreaseTapCount - increased tap count to {0}", lblTopNavTapCount);

            if (lblTopNavTapCount >= 12)
            {
                // Set up UIAlert and get secret SKU password from user
                Console.WriteLine("UIVCTestingFinished:lblTopNavIncreaseTapCount - user attempting to access secret SKU mode");
                UIAlertController alert = UIAlertController.Create("Developer Login", "Enter your passcode", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Login", UIAlertActionStyle.Default, action => {
                    // This code is invoked when the user taps on login, and this shows how to access the field values
                    Console.WriteLine("UIVCTestingFinished:lblTopNavIncreaseTapCount - Password attempted: {0}", alert.TextFields[0].Text);

                    if (alert.TextFields[0].Text == "showmethemoney")
                    {
                        App.pricePointCents = 198;
                        lblRetailPrice.Text = string.Format("All for ${0}!", (float)App.pricePointCents / 100);
                        secretSKUChosen = true;
                        lblTopNavTapCount = 0;

                        UpdateButtonBorderColors(FlatColors.Emerland);

                        // Make sure the Continue button is enabled
                        btnContinueyFLAT.Enabled = true;
                        btnContinueyFLAT.UserInteractionEnabled = true;

                        Console.WriteLine("UIVCTestingFinished:lblTopNavIncreaseTapCount - price point updated to 198 cents");
                    }
                    else
                    {
                        secretSKUChosen = false;
                        lblTopNavTapCount = 0;
                        UpdateButtonBorderColors(UIColor.Black);
                    }

                }));
                alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, action =>
                {
                    secretSKUChosen = false;
                    lblTopNavTapCount = 0;
                    UpdateButtonBorderColors(UIColor.Black);
                }));
                alert.AddTextField((field) => {
                    field.SecureTextEntry = true;
                });
                PresentViewController(alert, animated: true, completionHandler: null);
            }
        }

        private void BtnMoxiFit_TouchUpInside(object sender, EventArgs e)
        {
            // Clear out any colors
            UpdateButtonBorderColors(UIColor.Black);

            // If any other item is chosen after the secret SKU, make sure to clear out the secret SKU and reset the price point
            if(secretSKUChosen)
            {
                secretSKUChosen = false;
                App.pricePointCents = AppDelegate.truePricePointCents;
                lblRetailPrice.Text = string.Format("All for ${0}!", (float)App.pricePointCents / 100);
            }

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                // Set sender button's border to "Emerald" (brighter soft green)
                (sender as UIButton).Layer.BorderColor = FlatColors.Emerland.CGColor;

                if ((sender as UIButton) == btnMoxiFitS2AmberSuede)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-S2-AmberSuede.jpg");
                    lblColorChoice.Text = "Color: Amber Suede";
                }
                else if ((sender as UIButton) == btnMoxiFitS7TealBlast)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-S7-TealBlast.jpg");
                    lblColorChoice.Text = "Color: Teal Blast";
                }
                else if ((sender as UIButton) == btnMoxiFitS6Sandstorm)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-S6-Sandstorm.jpg");
                    lblColorChoice.Text = "Color: Sand Storm";
                }
                else if ((sender as UIButton) == btnMoxiFitP6Platinum)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-P6-Platinum.jpg");
                    lblColorChoice.Text = "Color: Platinum";
                }
                else if ((sender as UIButton) == btnMoxiFitS5Pewtershine)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-S5-Pewtershine.jpg");
                    lblColorChoice.Text = "Color: Pewter Shine";
                }
                else if ((sender as UIButton) == btnMoxiFitP7Pewter)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-P7-Pewter.jpg");
                    lblColorChoice.Text = "Color: Pewter";
                }
                else if ((sender as UIButton) == btnMoxiFitS3EspressoBoost)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-S3-Espressoboost.jpg");
                    lblColorChoice.Text = "Color: Espresso Boost";
                }
                else if ((sender as UIButton) == btnMoxiFitP4Espresso)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-P4-Espresso.jpg");
                    lblColorChoice.Text = "Color: Espresso";
                }
                else if ((sender as UIButton) == btnMoxiFitQ9Cinnamon)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-Q9-Cinnamon.jpg");
                    lblColorChoice.Text = "Color: Cinnamon";
                }
                else if ((sender as UIButton) == btnMoxiFitP8Charcoal)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-P8-Charcoal.jpg");
                    lblColorChoice.Text = "Color: Charcoal";
                }
                else if ((sender as UIButton) == btnMoxiFit01Beige)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-01-Beige.jpg");
                    lblColorChoice.Text = "Color: Beige";
                }
                else if ((sender as UIButton) == btnMoxiFitP2Amber)
                {
                    UIIProduct.Image = UIImage.FromBundle("Images/MoxiFit-P2-Amber.jpg");
                    lblColorChoice.Text = "Color: Amber";
                }

                // Make sure the Continue button is enabled
                btnContinueyFLAT.Enabled = true;
                btnContinueyFLAT.UserInteractionEnabled = true;
            });
        }

        private void UpdateButtonBorderColors(UIColor borderColor)
        {
            int borderWidth = 2;
            int cornerRadius = 4;
            //var borderColor = UIColor.Black.CGColor;

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                // Make the buttons look unofirm
                btnMoxiFitS2AmberSuede.Layer.BorderWidth = borderWidth;
                btnMoxiFitS7TealBlast.Layer.BorderWidth = borderWidth;
                btnMoxiFitS6Sandstorm.Layer.BorderWidth = borderWidth;
                btnMoxiFitP6Platinum.Layer.BorderWidth = borderWidth;
                btnMoxiFitS5Pewtershine.Layer.BorderWidth = borderWidth;
                btnMoxiFitP7Pewter.Layer.BorderWidth = borderWidth;
                btnMoxiFitS3EspressoBoost.Layer.BorderWidth = borderWidth;
                btnMoxiFitP4Espresso.Layer.BorderWidth = borderWidth;
                btnMoxiFitQ9Cinnamon.Layer.BorderWidth = borderWidth;
                btnMoxiFitP8Charcoal.Layer.BorderWidth = borderWidth;
                btnMoxiFit01Beige.Layer.BorderWidth = borderWidth;
                btnMoxiFitP2Amber.Layer.BorderWidth = borderWidth;

                btnMoxiFitS2AmberSuede.Layer.CornerRadius = cornerRadius;
                btnMoxiFitS7TealBlast.Layer.CornerRadius = cornerRadius;
                btnMoxiFitS6Sandstorm.Layer.CornerRadius = cornerRadius;
                btnMoxiFitP6Platinum.Layer.CornerRadius = cornerRadius;
                btnMoxiFitS5Pewtershine.Layer.CornerRadius = cornerRadius;
                btnMoxiFitP7Pewter.Layer.CornerRadius = cornerRadius;
                btnMoxiFitS3EspressoBoost.Layer.CornerRadius = cornerRadius;
                btnMoxiFitP4Espresso.Layer.CornerRadius = cornerRadius;
                btnMoxiFitQ9Cinnamon.Layer.CornerRadius = cornerRadius;
                btnMoxiFitP8Charcoal.Layer.CornerRadius = cornerRadius;
                btnMoxiFit01Beige.Layer.CornerRadius = cornerRadius;
                btnMoxiFitP2Amber.Layer.CornerRadius = cornerRadius;

                // Set the border colors (w proper cast)
                btnMoxiFitS2AmberSuede.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitS7TealBlast.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitS6Sandstorm.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitP6Platinum.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitS5Pewtershine.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitP7Pewter.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitS3EspressoBoost.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitP4Espresso.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitQ9Cinnamon.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitP8Charcoal.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFit01Beige.Layer.BorderColor = borderColor.CGColor;
                btnMoxiFitP2Amber.Layer.BorderColor = borderColor.CGColor;
            });
        }

        private void BtnNoSale_TouchUpInside(object sender, EventArgs e)
        {
            // User pressed the "i do not wish to purchase today" button
            Console.WriteLine("UIVCTestingFinished:BtnNoSale_TouchUpInside - user clicked the no sale button");

            string alert_message = "Are you sure you want to exit the checkout process?";

            var okAlertController = UIAlertController.Create("Are you sure?", alert_message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Exit", UIAlertActionStyle.Default, alert => ExitSale(sender)));
            okAlertController.AddAction(UIAlertAction.Create("Continue Checkout", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);
        }

        private void ExitSale(object sender)
        {
            Console.WriteLine("UIVCTestingFinished:ExitSale - exiting sale to questionairre");

            SetNoSaleInfo(true);

            PerformSegue("segToExitSurvey", (Foundation.NSObject)sender);
        }

        private void BtnContinue_TouchUpInside(object sender, EventArgs e)
        {
            Console.WriteLine("UIVCTestingFinished:BtnContinue_TouchUpInside - exiting sale to questionairre");

            SetNoSaleInfo(false);
            SetSaleChoice();

            PerformSegue("segToPaymentMethod", (Foundation.NSObject)sender);
        }

        private void SetSaleChoice()
        {
            Console.WriteLine("UIVCTestingFinished:SetSaleChoice - called");

            // Button SKU mapping
            Dictionary<string, string> buttonSKUMap = new Dictionary<string, string>();
            buttonSKUMap.Add("btnMoxiFitS2AmberSuede", "001_MoxiFitS2AmberSuede");
            buttonSKUMap.Add("btnMoxiFitS7TealBlast", "001_MoxiFitS7TealBlast");
            buttonSKUMap.Add("btnMoxiFitS6Sandstorm", "001_MoxiFitS6Sandstorm");
            buttonSKUMap.Add("btnMoxiFitP6Platinum", "001_MoxiFitP6Platinum");
            buttonSKUMap.Add("btnMoxiFitS5Pewtershine", "001_MoxiFitS5Pewtershine");
            buttonSKUMap.Add("btnMoxiFitP7Pewter", "001_MoxiFitP7Pewter");
            buttonSKUMap.Add("btnMoxiFitS3EspressoBoost", "001_MoxiFitS3EspressoBoost");
            buttonSKUMap.Add("btnMoxiFitP4Espresso", "001_MoxiFitP4Espresso");
            buttonSKUMap.Add("btnMoxiFitQ9Cinnamon", "001_MoxiFitQ9Cinnamon");
            buttonSKUMap.Add("btnMoxiFitP8Charcoal", "001_MoxiFitP8Charcoal");
            buttonSKUMap.Add("btnMoxiFit01Beige", "001_MoxiFit01Beige");
            buttonSKUMap.Add("btnMoxiFitP2Amber", "001_MoxiFitP2Amber");

            string skuChosen = "UNASSIGNED";

            // Based on the choice made, write to db - THIS CAN BE DONE MUCH BETTER!
            if (secretSKUChosen)
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - secret SKU chosen");
                skuChosen =  "999_prod_test_sku_01";
            }
            else if (btnMoxiFitS2AmberSuede.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitS2AmberSuede chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitS2AmberSuede", out skuChosen);
            }
            else if (btnMoxiFitS7TealBlast.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitS7TealBlast chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitS7TealBlast", out skuChosen);
            }
            else if (btnMoxiFitS6Sandstorm.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitS6Sandstorm chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitS6Sandstorm", out skuChosen);
            }
            else if (btnMoxiFitP6Platinum.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitP6Platinum chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitP6Platinum", out skuChosen);
            }
            else if (btnMoxiFitS5Pewtershine.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitS5Pewtershine chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitS5Pewtershine", out skuChosen);
            }
            else if (btnMoxiFitP7Pewter.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitP7Pewter chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitP7Pewter", out skuChosen);
            }
            else if (btnMoxiFitS3EspressoBoost.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitS3EspressoBoost chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitS3EspressoBoost", out skuChosen);
            }
            else if (btnMoxiFitP4Espresso.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitP4Espresso chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitP4Espresso", out skuChosen);
            }
            else if (btnMoxiFitQ9Cinnamon.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitQ9Cinnamon chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitQ9Cinnamon", out skuChosen);
            }
            else if (btnMoxiFitP8Charcoal.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitP8Charcoal chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitP8Charcoal", out skuChosen);
            }
            else if (btnMoxiFit01Beige.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFit01Beige chosen");
                buttonSKUMap.TryGetValue("btnMoxiFit01Beige", out skuChosen);
            }
            else if (btnMoxiFitP2Amber.Layer.BorderColor.Equals(FlatColors.Emerland.CGColor))
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - btnMoxiFitP2Amber chosen");
                buttonSKUMap.TryGetValue("btnMoxiFitP2Amber", out skuChosen);
            }
            else
            {
                Console.WriteLine("UIVCTestingFinished:SetSaleChoice - unable to determine which SKU was chosen!");
                throw new InvalidOperationException("COULD NOT DETERMINE CORRECT SKU");
            }

            Console.WriteLine("UIVCTestingFinished:SetSaleChoice - will be using the price point of: {0} cents", App.pricePointCents);

            // Prep db stuff
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);

            userSession.SKUChosen = skuChosen;

            // Local SQLite Update
            DatabaseHelper.SetSKUInfo(db_path, App.globablSessionId, userSession.SKUChosen);
            // AWS Update
            // AWSHelper.PerformSessionUpdate(userSession); // TODO: AWS
        }

        private void SetNoSaleInfo(bool noSaleFlag)
        {
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);

            userSession.ExitNoSale = noSaleFlag;

            // Local SQLite Update
            DatabaseHelper.SetNoSaleInfo(db_path, App.globablSessionId, noSaleFlag);
            // AWS Update
            // AWSHelper.PerformSessionUpdate(userSession); // TODO: AWS
        }

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

    }
}