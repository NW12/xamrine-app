using AVFoundation;
using CoreFoundation;
using Foundation;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace hearingapp_otc.iOS
{
    public partial class UIVCHearingTest : UIViewController
    {
        public float dbStart;
        public float dbStep;

        public static bool didPressButton = false;
        public static bool soundIsPlaying;

        public float userThreshold = -99f;
        private float threshold1 = -99f;
        private float threshold2 = -99f;

        public string wav_filename = "";

        private int prevFreqDbLevel = 30; //init hearing test at 30dB on first frequency

        public UIActivityIndicatorView actInd;
        private FlatBigGreenButton btnToneHeardFLAT;
        private AVAudioRecorder recorder;
        public UIVCHearingTest(IntPtr handle) : base(handle)
        {
        }
        public override bool PrefersStatusBarHidden() { return true; } // Cover Status Bar

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = FlatColors.Clouds;

            // Set DevMode Options
            lblTestProgress.Hidden = !App.devMode;
            progressCompletion.Hidden = !App.devMode;
            lblTemp1.Hidden = !App.devMode;
            lblTemp2.Hidden = !App.devMode;
            lblStartDbLevel.Hidden = !App.devMode;
            btnStopTest.Hidden = !App.devMode;


            //btn1000hz_30db.TouchUpInside += Btn1000hz_30db_TouchUpInside;
            btn1000hz_30db.Hidden = true;
            //btn1000hz_40db.TouchUpInside += Btn1000hz_40db_TouchUpInside;
            btn1000hz_40db.Hidden = true;
            //btn1000hz_80db.TouchUpInside += Btn1000hz_80db_TouchUpInside;
            btn1000hz_80db.Hidden = true;

            lblTopNav.TextColor = FlatColors.Clouds;

            btnStopTest.TouchUpInside += BtnStopTest_TouchUpInside;

            // Get current session info, get starting db level and step size, set labels
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);
            lblStartDbLevel.Text = userSession.dBStart.ToString();
            this.dbStart = userSession.dBStart;

            // Create a new label and activity indicator to let the user know the test is happening, but hide both at first
            var actIndFrame = new CoreGraphics.CGRect(371 - 80, 245 - 80, 200, 200);  // x, y, w, h
            actInd = new UIActivityIndicatorView(actIndFrame);
            actInd.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
            actInd.Color = FlatColors.Emerland;
            actInd.HidesWhenStopped = true;
            actInd.StopAnimating();
            View.AddSubview(actInd);

            // Map a FlatBigGreenButton over existing btnToneHeard
            //btnToneHeard.TouchUpInside += BtnToneHeard_TouchUpInside;
            //Map a FlatUI button over top of existing btnCalibrationComplete
            var newBtnX = btnToneHeard.Frame.X;
            var newBtnY = btnToneHeard.Frame.Y;
            var newBtnWidth = btnToneHeard.Frame.Size.Width;
            var newBtnHeight = btnToneHeard.Frame.Size.Height;
            btnToneHeardFLAT = new FlatBigGreenButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnToneHeardFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnToneHeardFLAT.SetTitle("Heard Tone");
            // TESTING: Trying to make the button touch on its own thread... though this probably
            //      doesn't remove the actual ui stuff from the ui thread... so this might be pointless
            //btnToneHeardFLAT.TouchUpInside += BtnToneHeard_TouchUpInside;
            btnToneHeardFLAT.TouchUpInside += async (sender, e) =>
            {
                BtnToneHeard_TouchUpInside(sender, e);
            };
            btnToneHeardFLAT.Enabled = btnToneHeard.Enabled;
            View.AddSubview(btnToneHeardFLAT);

            // Add Exit Button
            btnEXITFromTest.Hidden = true;
            FlatExitButton btnExitFLAT = new FlatExitButton((int)btnEXITFromTest.Frame.X, (int)btnEXITFromTest.Frame.Y);
            btnExitFLAT.TouchUpInside += BtnEXIT_TouchUpInside;
            View.AddSubview(btnExitFLAT);

            // Hide the hearing test stuff until the test is started
            lblInstructions.Hidden = true;
            btnToneHeardFLAT.Hidden = true;

            // Map a FlatUI button over top of existing btnBeginHearingTest
            newBtnX = btnBeginHearingTest.Frame.X;
            newBtnY = btnBeginHearingTest.Frame.Y;
            newBtnWidth = btnBeginHearingTest.Frame.Size.Width;
            newBtnHeight = btnBeginHearingTest.Frame.Size.Height;
            var btnBeginHearingTestFLAT = new FlatButton(new RectangleF((int)newBtnX, (int)newBtnY, (int)newBtnWidth, (int)newBtnHeight));
            btnBeginHearingTestFLAT.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            btnBeginHearingTestFLAT.SetTitle("Begin Hearing Test");
            // Set up the button to start the hearing test
            //await Task.Delay(5);
            //btnBeginHearingTest.TouchUpInside += async (sender, e) =>
            btnBeginHearingTestFLAT.TouchUpInside += async (sender, e) =>
            {
                try
                {

                    var session = AVAudioSession.SharedInstance();
                    session.RequestRecordPermission((granted) =>
                    {
                        Console.WriteLine($"Audio Permission: {granted}");

                        if (granted)
                        {
                          
                            //.AVAudioSessionCategoryPlayAndRecord
                            session.SetCategory(AVAudioSession.CategoryPlayAndRecord, out NSError error);


                            if (error == null)
                            {
                                session.SetActive(true, out error);
                                if (error == null)
                                {
                                    InvokeOnMainThread(() =>
                                    {

                                        Record();
                                        btnBeginHearingTestFLAT.Hidden = true;
                                        lblInstructions.Enabled = true;
                                        lblInstructions.Hidden = false;
                                        btnToneHeardFLAT.Enabled = true;
                                        btnToneHeardFLAT.Hidden = false;

                                        lblActivityIndicator.Hidden = false;
                                        actInd.StartAnimating();

                                        //  await Task.Delay(1500); // Insert artificial wait before test becomes active
                                        BtnBeginHearingTest_TouchUpInside(sender, e);
                                    });

                                }
                            }
                            else
                            {
                                Console.WriteLine(error.LocalizedDescription);
                            }
                        }
                        else
                        {
                            Console.WriteLine("YOU MUST ENABLE MICROPHONE PERMISSION");
                        }
                    });

                }
                catch (Exception ex)
                {

                }




                //btnBeginHearingTestFLAT.Hidden = true;
                //lblInstructions.Enabled = true;
                //lblInstructions.Hidden = false;
                //btnToneHeardFLAT.Enabled = true;
                //btnToneHeardFLAT.Hidden = false;

                //lblActivityIndicator.Hidden = false;
                //actInd.StartAnimating();

                //await Task.Delay(1500); // Insert artificial wait before test becomes active
                //BtnBeginHearingTest_TouchUpInside(sender, e);
            };
            View.AddSubview(btnBeginHearingTestFLAT);


        }

        private void BtnEXIT_TouchUpInside(object sender, EventArgs e)
        {
            // Make sure user really wants to exit
            Console.WriteLine("UIVCHearingTest:BtnEXIT_TouchUpInside - user clicked the exit test button");

            string alert_message = "Are you sure you want to exit the hearing test? All settings will be reset.";

            var okAlertController = UIAlertController.Create("Are you sure?", alert_message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Continue Test", UIAlertActionStyle.Default, null));
            okAlertController.AddAction(UIAlertAction.Create("Exit Test", UIAlertActionStyle.Default, alert => ExitTestToRegistration(sender)));
            PresentViewController(okAlertController, true, null);
        }

        private void BtnStopTest_TouchUpInside(object sender, EventArgs e)
        {
            // User pressed the "Stop Hearing Test" button, let's make sure that is what they want to do
            Console.WriteLine("UIVCHearingTest:BtnStopTest_TouchUpInside - user clicked the stop test button");

            string alert_message = "Are you sure you want to exit the hearing test? All results will be lost.";

            var okAlertController = UIAlertController.Create("Are you sure?", alert_message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Continue Test", UIAlertActionStyle.Default, null));
            okAlertController.AddAction(UIAlertAction.Create("Exit Test", UIAlertActionStyle.Default, alert => ExitTest(sender)));
            PresentViewController(okAlertController, true, null);
        }

        private void ExitTestToRegistration(object sender)
        {
            // Stop the audio tasks from continuing
            App.userIsTesting = false;

            // Transition back to registration screen
            UIStoryboard checkoutProcessBoard = UIStoryboard.FromName("Main", null);
            UIViewController uivcTestingFinished = checkoutProcessBoard.InstantiateViewController("UIVCRegistration");
            this.PresentViewController(uivcTestingFinished, true, null);
        }

        private void ExitTest(object sender)
        {
            Console.WriteLine("UIVCHearingTest:ExitTest - exiting test - popping current UIVC");
            /* BELOW ARE WAYS OF GETTING BACK TO THE PREVIOUS VIEW CONTROLLER THAT DIDNT WORK
            //NavigationController.PopViewController(true);
            //PerformSegue("SurveyDoneSegue", (Foundation.NSObject)sender);

            // None of the above work, so just crash us for now
            //this.NavigationController.PopViewController(true);

            //var sb = UIStoryboard.FromName("Main", null);
            //var vc = sb.InstantiateViewController("UIVCCalibration");
            //this.NavigationController.PushViewController(vc, true);
            */

            // Break out of the test loop (otherwise sounds play forever in the background)
            App.userIsTesting = false;

            // Artificially fill test results before hoppping over to test results
            Session sess = new Session()
            {
                /*
                LeftEarThreshold_500Hz = 15.0f,
                RightEarThreshold_500Hz = 20.0f,

                LeftEarThreshold_1000Hz = 20.0f,
                RightEarThreshold_1000Hz = 25.0f,

                LeftEarThreshold_2000Hz = 25.0f,
                //RightEarThreshold_2000Hz = 30.0f,
                RightEarThreshold_2000Hz = 40.0f,

                LeftEarThreshold_4000Hz = 30.0f,
                //RightEarThreshold_4000Hz = 25.0f,
                RightEarThreshold_4000Hz = 50.0f,
                */
                LeftEarThreshold_500Hz = 30.0f,
                RightEarThreshold_500Hz = 30.0f,

                LeftEarThreshold_1000Hz = 50.0f,
                RightEarThreshold_1000Hz = 45.0f,

                LeftEarThreshold_2000Hz = 70.0f,
                RightEarThreshold_2000Hz = 80.0f,

                LeftEarThreshold_4000Hz = 30.0f,
                //RightEarThreshold_4000Hz = 25.0f,
                RightEarThreshold_4000Hz = 30.0f,
            };

            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);

            DatabaseHelper.SetThresholdInfo(db_path, App.globablSessionId, sess);

            // AWS Update
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);
            //AWSHelper.PerformSessionUpdate(userSession);

            PerformSegue("HearingTestSuccessSegue", (Foundation.NSObject)sender);
        }

        // Runs the hearing test after the user clicks "Begin Hearing Test" button
        async private void BtnBeginHearingTest_TouchUpInside(object sender, EventArgs e)
        { 

            DispatchQueue.GetGlobalQueue(DispatchQueuePriority.Background).DispatchAsync(() =>
            {
                RunTest((Foundation.NSObject)sender);
            });

        }
        private NSUrl CreateOutputUrl()
        {
            var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.aac";
            var tempRecording = Path.Combine(Path.GetTempPath(), fileName);

            return NSUrl.FromFilename(tempRecording);
        }
        private void Record()
        {
            var audioSettings = new AudioSettings
            {
                SampleRate = 44100,
                NumberChannels = 1,
                AudioQuality = AVAudioQuality.High,
                Format = AudioToolbox.AudioFormatType.MPEG4AAC,

            };

            // Set recorder parameters
            recorder = AVAudioRecorder.Create(CreateOutputUrl(), audioSettings, out NSError error);
            if (error == null)
            {
                // Set Recorder to Prepare To Record
                if (!recorder.PrepareToRecord())
                {
                    recorder.Dispose();
                    recorder = null;
                }
                else
                {
                    recorder.MeteringEnabled = true;
                    recorder.Record();
                    recorder.UpdateMeters();
                }
            }
            else
            {
                Console.WriteLine(error.LocalizedDescription);
            }

        }

        private void RunTest(Foundation.NSObject nSObject)
        {
            /*
           // Start the test
           Console.WriteLine("UIVCHearingTest:BtnBeginHearingTest_TouchUpInside - delaying 5 seconds");
           await Task.Delay(5000);
           Console.WriteLine("UIVCHearingTest:BtnBeginHearingTest_TouchUpInside - done delaying 5 seconds");
           await TestFrequency(1000);
           */

            App.userIsTesting = true;

            // Using mom's info, just going to test these frequencies and in this order: 1000, 500, 2000, 4000
            int[] frequenciesToTest = { 1000, 500, 2000, 4000 };
            //int[] frequenciesToTest = { 1000 };
            string[] earsToTest = { "Right", "Left" };

            // TMP ASYNC REF - https://devblogs.microsoft.com/xamarin/getting-started-with-async-await/
            // TMP ASYNC REF - https://github.com/jgold6/AsyncAllTheWaySamples/blob/master/AsyncAllTheWayiOS/AsyncAllTheWayiOS/ViewController.cs

            // Test one ear at a time
            foreach (string ear in earsToTest)
            {
                // Loop over each frequency, then move to the next ear
                foreach (int testFrequency in frequenciesToTest)
                {
                    Console.WriteLine("UIVCHearingTest:BtnBeginHearingTest_TouchUpInside - Testing: " + testFrequency + "Hz on " + ear);
                    TestFrequency(testFrequency, ear);
                    InvokeOnMainThread(() =>
                    {
                        progressCompletion.Progress += 0.1f;
                    });
                }
            }
            InvokeOnMainThread(() =>
            {
                recorder.Stop();
                recorder.Dispose();
                
                // Since we're done, stop the activity indicator before the segue
                lblActivityIndicator.Hidden = true;
                actInd.StopAnimating();

                App.userIsTesting = false;

                // Once we're done, segue to the results view after setting its sessionId
                PerformSegue("HearingTestSuccessSegue", nSObject);
            });
        }
        private void TestFrequency(int freqToTest, string LeftRight)
        {
            //await SetThreshold(freqToTest, LeftRight);    //OLD
            threshold1 = -99f;
            threshold2 = -98f;
            while (threshold1 != threshold2)
            {

                threshold1 = SetThreshold(1, freqToTest, LeftRight);

                if (threshold1 == threshold2)
                {
                    break;
                }

                threshold2 = SetThreshold(2, freqToTest, LeftRight);

            }

            // CURRENT BUG: Does not currently check if the audio testing is complete. Thresholds get set for
            //  remaining values based on defaults defined at the beggining of the method, regardless
            //  of if the test finished or not after above while loops is aborted on the exit.


            //
            // Testing of current frequency complete, do all the bookkeeping stuff
            //

            // Record results to proper session object and update the database
            Console.WriteLine("UIVCHearingTest:TestFrequency - HURRAY! SAVING RESULTS!");

            // Get the current session Id
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);

            // Update the session with the values set in the text boxes
            Session userSession = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);

            // Currently test: 1000, 500, 2000, 4000
            if (freqToTest == 1000)
            {
                if (LeftRight == "Left")
                    userSession.LeftEarThreshold_1000Hz = threshold1;// userThreshold;
                else if (LeftRight == "Right")
                    userSession.RightEarThreshold_1000Hz = threshold1;// userThreshold;
            }
            else if (freqToTest == 500)
            {
                if (LeftRight == "Left")
                    userSession.LeftEarThreshold_500Hz = threshold1;// userThreshold;
                else if (LeftRight == "Right")
                    userSession.RightEarThreshold_500Hz = threshold1;// userThreshold;
            }
            else if (freqToTest == 2000)
            {
                if (LeftRight == "Left")
                    userSession.LeftEarThreshold_2000Hz = threshold1;// userThreshold;
                else if (LeftRight == "Right")
                    userSession.RightEarThreshold_2000Hz = threshold1;// userThreshold;
            }
            else if (freqToTest == 4000)
            {
                if (LeftRight == "Left")
                    userSession.LeftEarThreshold_4000Hz = threshold1;// userThreshold;
                else if (LeftRight == "Right")
                    userSession.RightEarThreshold_4000Hz = threshold1;// userThreshold;
            }

            // Sync to local database
            DatabaseHelper.SetThresholdInfo(db_path, userSession.Id, userSession);

            // Sync to AWS
            //AWSHelper.PerformSessionUpdate(userSession);

        }


        private async void BtnToneHeard_TouchUpInside(object sender, EventArgs e)
        {
            //new System.Threading.Thread(new System.Threading.ThreadStart(() => {
            InvokeOnMainThread(() =>
            {
                didPressButton = true;
            });
            // })).Start();
        }

        private float SetThreshold(int thresholdNumber, int freqToTest, string LeftRight)
        {
            float setThreshold = -96.0f;
            int lastHitdBLevel = -100;

            // Set the dbLevel to 30 to start, then carry it forward
            int dbLevel = prevFreqDbLevel;

            while (App.userIsTesting)
            {
                recorder.UpdateMeters();
                float decibelLevel = recorder.AveragePower(0);

                if (decibelLevel > -30)
                {
                    didPressButton = false;
                    InvokeOnMainThread(() =>
                    {
                        lblInstructions.Text = "Test paused, too much noise in background";
                        lblInstructions.TextColor = FlatColors.Carrot;
                        lblInstructions.TextAlignment = UITextAlignment.Center;

                        btnToneHeardFLAT.SetTitle("▶ Continue");
                        btnToneHeardFLAT.Color = FlatColors.Carrot;// UIColor.Red;
                        btnToneHeardFLAT.ShadowColor = FlatColors.Pumpkin;// Alizarin;

                    });
                    while (!didPressButton)
                    {
                    }
                    InvokeOnMainThread(() =>
                    {
                        lblInstructions.Text = "When you hear a tone, press the big green button below";
                        lblInstructions.TextColor = UIColor.Black;

                        btnToneHeardFLAT.SetTitle("Heard Tone");
                        btnToneHeardFLAT.Color = FlatColors.Turquoise;
                        btnToneHeardFLAT.ShadowColor = FlatColors.GreenSea;
                    });
                }
                else
                {

                    // Reset our variables
                    didPressButton = false;
                    soundIsPlaying = true;

                    DoSoundButtonCycle(freqToTest, dbLevel, LeftRight);
                    System.Threading.Thread.Sleep(GetDelayVariation());
                    soundIsPlaying = false;

                    ////////////////////////
                    ///
                    // Hit
                    if (didPressButton)
                    {
                        Console.WriteLine("UIVCHearingTest:SetThreshold - HIT on " + freqToTest + "Hz " + dbLevel + "dB");
                        InvokeOnMainThread(() =>
                        {
                            lblTemp1.Text = "Tone identified: " + freqToTest + "Hz " + dbLevel + "dB";
                        });
                        didPressButton = false;

                        // This is what we want to hit - it means they hit the same level twice in a row
                        if (dbLevel == lastHitdBLevel)
                        {
                            setThreshold = lastHitdBLevel;
                            prevFreqDbLevel = lastHitdBLevel;
                            return setThreshold;
                        }

                        // Drop down ("up" on the actual graph/audiogram) 10dB, unless we are at the bottom of our range of dB
                        if (dbLevel != -5)
                        {
                            lastHitdBLevel = dbLevel;
                            dbLevel = dbLevel - 10;
                            setThreshold = lastHitdBLevel;
                            // Make sure our dbLevel is still valid, otherwise set our edge levels and exit loop
                            if (dbLevel < -5)
                            {
                                // We must have been at 0, set our threshold and exit loop
                                setThreshold = lastHitdBLevel;
                                prevFreqDbLevel = lastHitdBLevel;
                                return setThreshold;
                            }
                            else if (dbLevel > 80)
                            {
                                setThreshold = 80;
                                prevFreqDbLevel = 30;
                                return setThreshold;
                            }

                        }
                        else
                        {
                            // If we are at the bottom of our range (-5db) then set the threshold and exit loop
                            setThreshold = dbLevel;
                            prevFreqDbLevel = dbLevel;
                        }
                        return setThreshold;
                    }
                    // Miss
                    else
                    {
                        Console.WriteLine("UIVCHearingTest:SetThreshold - MISS on " + freqToTest + "Hz " + dbLevel + "dB");
                        InvokeOnMainThread(() =>
                        {
                            lblTemp1.Text = "Missed identifying tone: " + freqToTest + "Hz " + dbLevel + "dB";
                        });

                        // Go up ("down" in technical speak) 5dB
                        dbLevel = dbLevel + 5;

                        if (dbLevel > 80)
                        {
                            setThreshold = 80;
                            prevFreqDbLevel = 30;
                            return setThreshold;
                        }

                    }
                    ////////////////////////
                    ///
                }
            }

            // Should never get here...
            return setThreshold;

        }

        private void DoSoundButtonCycle(int freqToTest, int dbLevel, string LeftRight)
        {

            InvokeOnMainThread(() =>
            {
                PlayRequestedSound(freqToTest, dbLevel, LeftRight);
                //Task.Delay(GetDelayVariation());
            });
            /*
            CancellationTokenSource cts = new CancellationTokenSource();
            var ct = cts.Token;

            Task.Run(async () =>
            {
                try
                {
                    InvokeOnMainThread(() =>
                    {
                        PlayRequestedSound(freqToTest, dbLevel, LeftRight);
                        Task.Delay(GetDelayVariation());
                    });
                }
                catch (System.OperationCanceledException ex)
                {
                    Console.WriteLine($"UIVCHearingTest::TestFrequency - 1::Operation cancelled: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }, ct);
            */

            return;
        }


        // Gets a millisecond time delay between 3s and 5s
        private int GetDelayVariation()
        {
            Random timeDelayGenerator = new Random();
            int timeDelay = timeDelayGenerator.Next(3000, 4500);
            Console.WriteLine("UIVCHearingTesting:GetDelayVariation - Returning time delay of " + timeDelay);
            return timeDelay;
        }

        #region Debug_Buttons
        async private void Btn1000hz_30db_TouchUpInside(object sender, EventArgs e)
        {
            await PlayRequestedSound(1000, 30, "Left");
        }
        async private void Btn1000hz_40db_TouchUpInside(object sender, EventArgs e)
        {
            await PlayRequestedSound(1000, 40, "Left");
        }
        async private void Btn1000hz_80db_TouchUpInside(object sender, EventArgs e)
        {
            await PlayRequestedSound(1000, 80, "Left");
        }
        #endregion

        async private Task PlayRequestedSound(int hz, int db, string LeftRight)
        {
            string wav_filename = GetWaveFilenameAndSetVolume(hz, db);
            //await GetWaveFilenameAndSetVolume(hz, db);

            if (wav_filename != "INVALID")
            {
                if (App.devMode)
                    lblSoundPlaying.Text = "🔊 " + hz + "Hz " + db + "dB " + LeftRight + " ear";

                await App.AudioManager.PlayAudioTask(wav_filename, LeftRight);

                if (App.devMode)
                    lblSoundPlaying.Text = "";
            }
        }

        private string GetWaveFilenameAndSetVolume(int hz, int db)
        //async private Task GetWaveFilenameAndSetVolume(int hz, int db)
        {
            // Assumes the caller has passed a valid Hz value: 125, 250, 500, 1000, 2000, 4000, 8000
            string wav_filename = "INVALID";
            //wav_filename = "INVALID";
            // TODO: Move these out to some kind of public constant so we aren't recreating them every time this is called
            List<int> valid_hz = new List<int>
            {
                125,
                250,
                500,
                1000,
                2000,
                4000,
                8000
            };
            bool isHzValid = valid_hz.IndexOf(hz) != -1;

            List<int> valid_db = new List<int>
            {
                -5,
                0,
                5,
                10,
                15,
                20,
                25,
                30,
                35,
                40,
                45,
                50,
                55,
                60,
                65,
                70,
                75,
                80
            };
            bool isDbValid = valid_db.IndexOf(db) != -1;

            if (isHzValid == true && isDbValid == true)
            {
                // TODO: Make a lookup table for this nonsense?

                if (db == -5 || db == 0 || db == 5 || db == 10 || db == 15 || db == 20 || db == 30 || db == 40 || db == 50 || db == 60 || db == 70 || db == 80)
                {
                    // Happy path - we have a wav file for this exact frequency and db level
                    //App.AudioManager.EffectsVolume = 1.0f;
                    App.AudioManager.EffectsVolume = this.dbStart;
                    wav_filename = hz + "Hz_" + db + "db.wav";
                }
                else if (db == 25 || db == 35 || db == 45 || db == 55 || db == 65 || db == 75)
                {
                    // For these we don't actually have a wave file. So we just use the next one up
                    //  then reduce the system volume an appropriate percentage
                    int db_wav_to_use = db + 5;
                    float percent_vol = db / (float)db_wav_to_use;
                    float new_vol = percent_vol * this.dbStart;
                    App.AudioManager.EffectsVolume = new_vol;
                    wav_filename = hz + "Hz_" + db_wav_to_use + "db.wav";
                    Console.WriteLine("DBG UIVCHearingTest.GetWaveFilenameAndSetVolume : [Adjusted Volume: " + percent_vol + "]");
                }

                Console.WriteLine("DBG UIVCHearingTest.GetWaveFilenameAndSetVolume : Successful Lookup. Returning: " + wav_filename);
                return wav_filename;

            }
            else
            {
                Console.WriteLine("DBG UIVCHearingTest.GetWaveFilenameAndSetVolume : Bad Lookup. Returning: " + wav_filename);
                return wav_filename; // "INVALID"
            }

        }


        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            //We only care about the value on an actual device
            //if (ObjCRuntime.Runtime.Arch.Equals(ObjCRuntime.Arch.DEVICE))
            //{
            base.PrepareForSegue(segue, sender);
        }

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

    }
}