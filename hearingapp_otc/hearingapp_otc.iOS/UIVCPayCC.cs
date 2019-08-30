using CoreGraphics;
using Foundation;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCPayCC : UIViewController
    {
        /**** REMOVED PICKER CRAP
        private GenericUIPickerModel shipStatePickerModel;
        private GenericUIPickerModel billStatePickerModel;
        private GenericUIPickerModel monthPickerModel;
        private GenericUIPickerModel yearPickerModel;
        ****/

        /********************************************************/
        //KEYBOARD TESTING STUFF
        private UIView activeview; // stores active view information
        private float scroll_amount = 0.0f;    // amount to scroll 
        private float bottom = 0.0f;           // bottom point
        private float offset = 100.0f;          // extra offset
        private bool moveViewUp = false;           // which direction are we moving
        /********************************************************/

        private bool useShipAsBilling;
        private FlatBigGreenButton btnProcessPaymentFlat;

        public UIVCPayCC (IntPtr handle) : base (handle)
        {
        }
        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Correct color background and top nav label colors
            View.BackgroundColor = FlatColors.Clouds;
            lblTopNav.TextColor = FlatColors.Clouds;

            // Paint flat button - Process Payment
            btnProcessPayment.Hidden = true;
            var newBtnX = btnProcessPayment.Frame.X;
            var newBtnY = btnProcessPayment.Frame.Y;
            var newBtnWidth = btnProcessPayment.Frame.Size.Width;
            var newBtnHeight = btnProcessPayment.Frame.Size.Height;
            btnProcessPaymentFlat = new FlatBigGreenButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnProcessPaymentFlat.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnProcessPaymentFlat.SetTitle("Process Payment");
            btnProcessPaymentFlat.TouchUpInside += BtnProcessPayment_TouchUpInside;
            View.AddSubview(btnProcessPaymentFlat);

            // Add back button
            btnBackFromPayByCC.Hidden = true;
            FlatBackButton btnBackFLAT = new FlatBackButton((int)btnBackFromPayByCC.Frame.X, (int)btnBackFromPayByCC.Frame.Y);
            btnBackFLAT.TouchUpInside += BtnBack_TouchUpInside;
            View.AddSubview(btnBackFLAT);

            /**** REMOVED PICKER CRAP
            // Populate pickerShipState and pickerBillState
            List<string> states = new List<string>(new string[] { "AL","AK","AS","AZ","AR","CA","CO","CT","DE","DC","FM","FL","GA"
                "GU","HI","ID","IL","IN","IA","KS","KY","LA","ME","MH","MD","MA","MI","MN","MS","MO","MT","NE","NV","NH","NJ","NM",
                "NY","NC","ND","MP","OH","OK","OR","PW","PA","PR","RI","SC","SD","TN","TX","UT","VT","VI","VA","WA","WV","WI","WY" });
            shipStatePickerModel = new GenericUIPickerModel(states);
            pickerShipState.Model = shipStatePickerModel;

            billStatePickerModel = new GenericUIPickerModel(states);
            pickerBillState.Model = billStatePickerModel;

            // Populate pickerCCExpMonth
            List<string> months = new List<string>();
            months.Add("January (01)");
            months.Add("February (02)");
            months.Add("March (03)");
            months.Add("April (04)");
            months.Add("May (05)");
            months.Add("June (06)");
            months.Add("July (07)");
            months.Add("August (08)");
            months.Add("September (09)");
            months.Add("October (10)");
            months.Add("November (11)");
            months.Add("December (12)");
            monthPickerModel = new GenericUIPickerModel(months);
            pickerCCExpMonth.Model = monthPickerModel;

            // Populate pickerCCExpYear
            List<string> years = new List<string>(new string[] { "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032" });
            yearPickerModel = new GenericUIPickerModel(years);
            pickerCCExpYear.Model = yearPickerModel;
            ****/

            // Get an instance of the user session
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);
            // Fill in user's name
            txtFLName.Text = userSession.FirstName + " " + userSession.LastName;
            txtEmailAddress.Text = userSession.EmailAddress;

            // If developer options are enabled, adjust our price point
            if (App.devMode)
                App.pricePointCents = 199;

            // Set bottom label correctly to reflect how much the user will be charged
            float pricePointDollars = ((float)App.pricePointCents) / 100;
            string confirmTotal = String.Format("After processing, your card will be charged {0:C} by Hear It Up", pricePointDollars);
            lblConfirmTotal.Text = confirmTotal;

            // Setup UISwitch that controls if shipping and billing addresses are the same, and disable access to billing until swtich is flipped
            switchBillIsShipAddr.TouchUpInside += SwitchBillIsShipAddr_TouchUpInside;
            txtBillAddressLine1.UserInteractionEnabled = false;
            txtBillAddressLine2.UserInteractionEnabled = false;
            txtBillCity.UserInteractionEnabled = false;
            txtBillState.UserInteractionEnabled = false;
            txtBillZip.UserInteractionEnabled = false;
            useShipAsBilling = true;

            // Set up Process Payment button
            btnProcessPayment.TouchUpInside += BtnProcessPayment_TouchUpInside;

            // Enforce proper capitalization, make it easier for the user
            txtFLName.AutocapitalizationType = UITextAutocapitalizationType.Words;

            txtShipAddressLine1.AutocapitalizationType = UIKit.UITextAutocapitalizationType.Words;
            txtShipAddressLine2.AutocapitalizationType = UIKit.UITextAutocapitalizationType.Words;
            txtShipCity.AutocapitalizationType = UIKit.UITextAutocapitalizationType.Words;
            txtShipState.AutocapitalizationType = UIKit.UITextAutocapitalizationType.AllCharacters;

            txtBillAddressLine1.AutocapitalizationType = UIKit.UITextAutocapitalizationType.Words;
            txtBillAddressLine2.AutocapitalizationType = UIKit.UITextAutocapitalizationType.Words;
            txtBillCity.AutocapitalizationType = UIKit.UITextAutocapitalizationType.Words;
            txtBillState.AutocapitalizationType = UIKit.UITextAutocapitalizationType.AllCharacters;

            /************************************************************************************************/
            // START OF KEYBOARD TESTING STUFF
            // Show Keyboard
            UIToolbar kbToolbar = new UIToolbar(RectangleF.Empty);
            kbToolbar.BarStyle = UIBarStyle.Default;
            kbToolbar.Translucent = true;
            kbToolbar.UserInteractionEnabled = true;
            kbToolbar.SizeToFit();
            UIBarButtonItem btnKBFlexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null);
            UIBarButtonItem btnKBDone = new UIBarButtonItem(UIBarButtonSystemItem.Done, KBToolbarButtonDoneHandler);
            UIBarButtonItem[] btnKBItems = new UIBarButtonItem[] { btnKBFlexibleSpace, btnKBDone };
            kbToolbar.SetItems(btnKBItems, true);

            // Link keyboard to the Text Control
            txtShipAddressLine1.InputAccessoryView = kbToolbar;
            txtShipAddressLine1.ClipsToBounds = true;
            txtShipAddressLine1.LayoutIfNeeded();
            txtShipAddressLine2.InputAccessoryView = kbToolbar;
            txtShipAddressLine2.ClipsToBounds = true;
            txtShipAddressLine2.LayoutIfNeeded();
            txtShipCity.InputAccessoryView = kbToolbar;
            txtShipCity.ClipsToBounds = true;
            txtShipCity.LayoutIfNeeded();
            txtShipState.InputAccessoryView = kbToolbar;
            txtShipState.ClipsToBounds = true;
            txtShipState.LayoutIfNeeded();
            txtShipZip.InputAccessoryView = kbToolbar;
            txtShipZip.ClipsToBounds = true;
            txtShipZip.LayoutIfNeeded();

            txtBillAddressLine1.InputAccessoryView = kbToolbar;
            txtBillAddressLine1.ClipsToBounds = true;
            txtBillAddressLine1.LayoutIfNeeded();
            txtBillAddressLine2.InputAccessoryView = kbToolbar;
            txtBillAddressLine2.ClipsToBounds = true;
            txtBillAddressLine2.LayoutIfNeeded();
            txtBillCity.InputAccessoryView = kbToolbar;
            txtBillCity.ClipsToBounds = true;
            txtBillCity.LayoutIfNeeded();
            txtBillState.InputAccessoryView = kbToolbar;
            txtBillState.ClipsToBounds = true;
            txtBillState.LayoutIfNeeded();
            txtBillZip.InputAccessoryView = kbToolbar;
            txtBillZip.ClipsToBounds = true;
            txtBillZip.LayoutIfNeeded();

            txtCCNum.InputAccessoryView = kbToolbar;
            txtCCNum.ClipsToBounds = true;
            txtCCNum.LayoutIfNeeded();
            txtCCCVV.InputAccessoryView = kbToolbar;
            txtCCCVV.ClipsToBounds = true;
            txtCCCVV.LayoutIfNeeded();
            txtCCMonth.InputAccessoryView = kbToolbar;
            txtCCMonth.ClipsToBounds = true;
            txtCCMonth.LayoutIfNeeded();
            txtCCYear.InputAccessoryView = kbToolbar;
            txtCCYear.ClipsToBounds = true;
            txtCCYear.LayoutIfNeeded();

            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification, KeyBoardDownNotification);

        }

        private void KeyBoardUpNotification(NSNotification notification)
        {
            // get the keyboard size
            CoreGraphics.CGRect r = UIKeyboard.BoundsFromNotification(notification);

            // Find what opened the keyboard
            foreach (UIView view in this.View.Subviews)
            {
                if (view.IsFirstResponder)
                    activeview = view;
            }

            if (activeview != null)
            {
                // Bottom of the controller = initial position + height + offset      
                bottom = ((float)(activeview.Frame.Y + activeview.Frame.Height + offset));

                // Calculate how far we need to scroll
                scroll_amount = ((float)(r.Height - (View.Frame.Size.Height - bottom)));

                // Perform the scrolling
                if (scroll_amount > 0)
                {
                    moveViewUp = true;
                    ScrollTheView(moveViewUp);
                }
                else
                {
                    moveViewUp = false;
                }
            }

        }
        private void KeyBoardDownNotification(NSNotification notification)
        {
            if (moveViewUp) { ScrollTheView(false); }
        }

        private void ScrollTheView(bool move)
        {
            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.1);

            CoreGraphics.CGRect frame = View.Frame;

            if (move)
            {
                frame.Y -= scroll_amount;
            }
            else
            {
                frame.Y += scroll_amount;
                scroll_amount = 0;
            }

            View.Frame = frame;

            UIView.CommitAnimations();
        }

        public void KBToolbarButtonDoneHandler(object sender, EventArgs e)
        {
            txtShipAddressLine1.ResignFirstResponder();
            txtShipAddressLine2.ResignFirstResponder();
            txtShipCity.ResignFirstResponder();
            txtShipState.ResignFirstResponder();
            txtShipZip.ResignFirstResponder();

            txtBillAddressLine1.ResignFirstResponder();
            txtBillAddressLine2.ResignFirstResponder();
            txtBillCity.ResignFirstResponder();
            txtBillState.ResignFirstResponder();
            txtBillZip.ResignFirstResponder();

            txtCCNum.ResignFirstResponder();
            txtCCCVV.ResignFirstResponder();
            txtCCMonth.ResignFirstResponder();
            txtCCYear.ResignFirstResponder();
        }

        private void TextField_TouchDown(object sender, EventArgs e)
        {
            if (moveViewUp) { ScrollTheView(false); }
        }

        // END OF KEYBOARD TESTING STUFF
        /************************************************************/


        private async void BtnProcessPayment_TouchUpInside(object sender, EventArgs e)
        {
            togglePaymentButton(); // Disable it so user can't do double payments

            try
            {
                // Very simple validations for now only
                if (txtShipState.Text.Length > 2 || txtBillState.Text.Length > 2)
                {
                    ShowSimpleAlert("Data validation issue", "State codes can only be 2 letters long (e.g. MN, WI). Please correct the information and try again.");
                    togglePaymentButton();
                    return;
                }
                else if (txtCCMonth.Text.Length != 2)
                {
                    ShowSimpleAlert("Data validation issue", "Please enter a 2 digit month code (e.g. 01, 02, 03, ... 12). Please correct the information and try again.");
                    togglePaymentButton();
                    return;
                }
                else if (txtCCYear.Text.Length != 4)
                {
                    ShowSimpleAlert("Data validation issue", "Please enter a 4 digit year (e.g. 2018, 2019, etc.). Please correct the information and try again.");
                    togglePaymentButton();
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught during validation", ex);
                togglePaymentButton();
                return;
            }

            
            // Get an instance of the user session
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);

            // Assemble attributes for the Payment Model
            var newPayment = new PaymentModel();
            try
            {
                newPayment.FirstName = userSession.FirstName;
                newPayment.LastName = userSession.LastName;
                newPayment.EmailAddress = userSession.EmailAddress;
                newPayment.SessionGuid = userSession.SessionGUID;

                newPayment.ShipLine1 = txtShipAddressLine1.Text;
                newPayment.ShipLine2 = txtShipAddressLine2.Text;
                newPayment.ShipCity = txtShipCity.Text;
                //newPayment.ShipState = shipStatePickerModel.selectedValue;
                newPayment.ShipState = txtShipState.Text;
                newPayment.ShipZip = txtShipZip.Text;

                // Use the shipping address as the billing address
                if (useShipAsBilling)
                {
                    newPayment.BillLine1 = txtShipAddressLine1.Text;
                    newPayment.BillLine2 = txtShipAddressLine2.Text;
                    newPayment.BillCity = txtShipCity.Text;
                    //newPayment.BillState = shipStatePickerModel.selectedValue;
                    newPayment.BillState = txtShipState.Text;
                    newPayment.BillZip = txtShipZip.Text;
                }
                // Billing address is different than shipping address, save it
                else
                {
                    newPayment.BillLine1 = txtBillAddressLine1.Text;
                    newPayment.BillLine2 = txtBillAddressLine2.Text;
                    newPayment.BillCity = txtBillCity.Text;
                    //newPayment.BillState = billStatePickerModel.selectedValue;
                    newPayment.BillState = txtBillState.Text;
                    newPayment.BillZip = txtBillZip.Text;
                }

                newPayment.SKU = userSession.SKUChosen;

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught during packaging up of data", ex);
                togglePaymentButton();
                return;
            }

            try
            {
                // Create customer token
                string ccNum = txtCCNum.Text;
                ccNum.Replace(" ", String.Empty);
                ccNum.Replace("-", String.Empty);
                //string ccMonth = monthPickerModel.selectedValue;
                //ccMonth = ccMonth.Substring(ccMonth.Length - 3).Replace(")", String.Empty); // "January (01)"->"01"
                                                                                            //string ccYear = yearPickerModel.selectedValue;
                string ccMonth = txtCCMonth.Text;
                string ccYear = txtCCYear.Text;
                ccYear = ccYear.Substring(2, 2); // "2018"->"18"
                string ccCVV = txtCCCVV.Text;

                string newPaymentToken = CreateStripeToken(ccNum, ccMonth, ccYear, ccCVV);
                newPayment.sourceStripeToken = newPaymentToken;

                // Grab the current pricepoint from app delegate
                newPayment.chargeAmount = App.pricePointCents;
                Console.WriteLine("!!! UIVCPayCC:BtnProcessPayment_TouchUpInside - charging {0} to card given", newPayment.chargeAmount);

                // Serialize our payment model
                HttpClient client = new HttpClient();
                var jsonData = JsonConvert.SerializeObject(newPayment);

                // Encode it
                var toBeSent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send request - charge the card
                HttpResponseMessage ret;
                if (App.devMode)
                    ret = await client.PostAsync("https://REMOVED.hearitup.net:35001/charge", toBeSent);
                else
                    ret = await client.PostAsync("https://REMOVED.hearitup.net:35000/charge", toBeSent);

                // Check the response - if it worked tell the user then kick off segue, if not pop an alert
                if (ret.IsSuccessStatusCode == true)
                {
                    // IT WORKED!
                    string alert_message = "Payment processed successfully!";
                    var okAlertController = UIAlertController.Create("Success!", alert_message, UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, alert => CompleteToThankyou(sender)));
                    PresentViewController(okAlertController, true, null);
                }
                else
                {
                    // ORDER CREATION FAILED - Something went terribly wrong, probably with server side code
                    ShowSimpleAlert("Payment processing failed!", "Payment processing failed. Please contact support.");
                    togglePaymentButton();
                    return;
                }
            }
            catch (Stripe.StripeException ex)
            {
                ShowSimpleAlert("Payment processing failed!", "Stripe - " + ex.Message);
                togglePaymentButton();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Generic Exception caught during data processing", ex);
                togglePaymentButton();
                return;
            }

            Console.WriteLine("UIVPayCC::BtnProcessPayment_TouchUpInside - WE MADE IT!!!");
            togglePaymentButton();
            return;
        }

        private void ShowSimpleAlert(string title, string message)
        {
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);
            return;
        }

        private void CompleteToThankyou(object sender)
        {
            Console.WriteLine("UIVCPayCC::CompleteToThankyou - we made it!");
            PerformSegue("segTYExitFromCC", (Foundation.NSObject)sender);
        }

        public string CreateStripeToken(string cardNumber, string cardExpMonth, string cardExpYear, string cardCVC)
        {
            // If the application has developer options enabled, adjust price point and use Stripe sandbox
            if (App.devMode)
                StripeConfiguration.SetApiKey("pk_test_REMOVED");
            // Otherwise leave the pricepoint as-is, and use the live Production key
            else
                StripeConfiguration.SetApiKey("pk_live_REMOVED");

            //e.g. https://www.c-sharpcorner.com/blogs/implement-stripe-payment-gateway-in-asp-net-mvc

            var tokenOptions = new TokenCreateOptions()
            {
                Card = new CreditCardOptions()
                {
                    Number = cardNumber,
                    ExpYear = Int32.Parse(cardExpYear),
                    ExpMonth = Int32.Parse(cardExpMonth),
                    Cvc = cardCVC
                }
            };

            var tokenService = new TokenService();
            Token stripeToken = tokenService.Create(tokenOptions);

            return stripeToken.Id; // This is the token

        }

        private void SwitchBillIsShipAddr_TouchUpInside(object sender, EventArgs e)
        {
            useShipAsBilling = switchBillIsShipAddr.On;

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                // If the shipping is same as billing, clear billing fields and disable text boxes
                if (switchBillIsShipAddr.On)
                {
                    txtBillAddressLine1.Text = "";
                    txtBillAddressLine2.Text = "";
                    txtBillCity.Text = "";
                    txtBillState.Text = "";
                    txtBillZip.Text = "";

                    txtBillAddressLine1.UserInteractionEnabled = false;
                    txtBillAddressLine2.UserInteractionEnabled = false;
                    txtBillCity.UserInteractionEnabled = false;
                    //pickerBillState.UserInteractionEnabled = false;
                    txtBillState.UserInteractionEnabled = false;
                    txtBillZip.UserInteractionEnabled = false;
                }

                else
                {
                    txtBillAddressLine1.UserInteractionEnabled = true;
                    txtBillAddressLine2.UserInteractionEnabled = true;
                    txtBillCity.UserInteractionEnabled = true;
                    //pickerBillState.UserInteractionEnabled = true;
                    txtBillState.UserInteractionEnabled = true;
                    txtBillZip.UserInteractionEnabled = true;
                }
            });
        }

        private void togglePaymentButton()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                if(btnProcessPayment.Enabled)
                {
                    btnProcessPayment.Enabled = false;
                    btnProcessPayment.UserInteractionEnabled = false;
                    btnProcessPaymentFlat.Enabled = false;
                    btnProcessPaymentFlat.UserInteractionEnabled = false;
                }
                else
                {
                    btnProcessPayment.Enabled = true;
                    btnProcessPayment.UserInteractionEnabled = true;
                    btnProcessPaymentFlat.Enabled = true;
                    btnProcessPaymentFlat.UserInteractionEnabled = true;
                }
            });
        }

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

        private void BtnBack_TouchUpInside(object sender, EventArgs e)
        {
            PerformSegue("segBackFromPayByCC", (Foundation.NSObject)sender);
        }
    }
}
 