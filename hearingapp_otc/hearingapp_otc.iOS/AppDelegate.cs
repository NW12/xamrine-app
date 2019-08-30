using Amazon;
using Foundation;
using UIKit;

namespace hearingapp_otc.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {

        public float db_start = 0.05f;
        public int globablSessionId;
        public bool userIsTesting;
        public bool validationsDisabled = true;
        public bool isLoggedIn = false;
        public bool devMode = false;

        public int pricePointCents = 69999;
        public const int truePricePointCents = 69999; // used as a "safe" restore when pricePointCents gets updated

        #region Computed Properties
        public HearingTestAudioManager AudioManager { get; set; } = new HearingTestAudioManager();
        #endregion

        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            // AWS Config stuff
            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;
            //loggingConfig.LogResponses = "Always" / "Never" / "OnError"; // Can configure this if you want stuff sent to cloud instead of console
            //AWSConfigs.CorrectForClockSkew = true; // Can configure this if you want the client to try and figure out the time (ntp?) and correct

            AWSConfigs.AWSRegion = "us-east-1";

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
            AudioManager.SuspendBackgroundMusic();
            AudioManager.DeactivateAudioSession();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
            AudioManager.ReactivateAudioSession();
            AudioManager.RestartBackgroundMusic();
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
            AudioManager.StopBackgroundMusic();
            AudioManager.DeactivateAudioSession();
        }

        // Handling orientation lock in AppDelegate because the stupid ass Info.plist isn't limiting it correctl (or.. it's maybe because
        // of something else I've overriend in here somewhere else where I need to be calling the superclass?)
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Portrait | UIInterfaceOrientationMask.PortraitUpsideDown;
        }


    }
}

