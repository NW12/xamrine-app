using System;
using AVFoundation;
using AudioToolbox;
using Foundation;
using UIKit;
using System.Threading.Tasks;

namespace hearingapp_otc.iOS
{
    public class HearingTestAudioManager
    {
        #region Private Variables
        private AVAudioPlayer backgroundMusic;
        private AVAudioPlayer soundEffect;
        private string backgroundSong = "";
        #endregion

        #region Computed Properties
        public float BackgroundMusicVolume
        {
            get { return backgroundMusic.Volume; }
            set { backgroundMusic.Volume = value; }
        }

        public bool MusicOn { get; set; } = true;
        public float MusicVolume { get; set; } = 0.5f;

        public bool EffectsOn { get; set; } = true;
        public float EffectsVolume { get; set; } = 1.0f;
        #endregion

        #region Constructors
        public HearingTestAudioManager()
        {
            // Initialize
            ActivateAudioSession();
        }
        #endregion

        #region Public Methods
        public void ActivateAudioSession()
        {
            // Initialize Audio
            var session = AVAudioSession.SharedInstance();
            session.SetCategory(AVAudioSessionCategory.Ambient);
            session.SetActive(true);
        }

        public void DeactivateAudioSession()
        {
            var session = AVAudioSession.SharedInstance();
            session.SetActive(false);
        }

        public void ReactivateAudioSession()
        {
            var session = AVAudioSession.SharedInstance();
            session.SetActive(true);
        }

        public void PlayBackgroundMusic(string filename)
        {
            NSUrl songURL;

            // Music enabled?
            if (!MusicOn) return;

            // Any existing background music?
            if (backgroundMusic != null)
            {
                //Stop and dispose of any background music
                backgroundMusic.Stop();
                backgroundMusic.Dispose();
            }

            // Initialize background music
            songURL = new NSUrl("AudioAssets/" + filename);
            NSError err;
            backgroundMusic = new AVAudioPlayer(songURL, "wav", out err);
            backgroundMusic.Volume = MusicVolume;
            backgroundMusic.FinishedPlaying += delegate {
                // backgroundMusic.Dispose(); 
                backgroundMusic = null;
            };
            backgroundMusic.NumberOfLoops = -1;
            backgroundMusic.Play();
            backgroundSong = filename;

        }

        public void StopBackgroundMusic()
        {

            // If any background music is playing, stop it
            backgroundSong = "";
            if (backgroundMusic != null)
            {
                backgroundMusic.Stop();
                backgroundMusic.Dispose();
                backgroundMusic = null;
                GC.Collect();
            }
        }

        public void SuspendBackgroundMusic()
        {

            // If any background music is playing, stop it
            if (backgroundMusic != null)
            {
                backgroundMusic.Stop();
                backgroundMusic.Dispose();
            }
        }

        public void RestartBackgroundMusic()
        {

            // Music enabled?
            if (!MusicOn) return;

            // Was a song previously playing?
            if (backgroundSong == "") return;

            // Restart song to fix issue with wonky music after sleep
            PlayBackgroundMusic(backgroundSong);
        }

        //public void PlaySound(string filename)
        async public void PlaySound(string filename)
        {
            NSUrl songURL;

            // Music enabled?
            if (!EffectsOn) return;

            // Any existing sound effect?
            if (soundEffect != null)
            {
                //Stop and dispose of any sound effect
                soundEffect.Stop();
                soundEffect.Dispose();
            }

            // Initialize background music
            songURL = new NSUrl("AudioAssets/" + filename);
            NSError err;
            soundEffect = new AVAudioPlayer(songURL, "wav", out err);
            soundEffect.Volume = EffectsVolume;
            soundEffect.FinishedPlaying += delegate {
                soundEffect = null;
            };
            soundEffect.NumberOfLoops = 0;
            soundEffect.Play();

        }

        public Task<bool> PlayAudioTask(string fileName, string LeftRight)
        {
            var tcs = new TaskCompletionSource<bool>();

            // Any existing sound playing?
            if (soundEffect != null)
            {
                //Stop and dispose of any sound
                soundEffect.Stop();
                soundEffect.Dispose();
            }

            NSUrl url = new NSUrl("AudioAssets/" + fileName);

            soundEffect = AVAudioPlayer.FromUrl(url);

            soundEffect.Volume = App.db_start;

            // Treat 4000Hz as special case - play 15% louder
            if (fileName.Substring(0,6) == "4000Hz")
                soundEffect.Volume = (App.db_start * 1.3f);

            if (LeftRight == "Left")
                soundEffect.Pan = -1.0f;
            else if (LeftRight == "Right")
                soundEffect.Pan = 1.0f;
            else
            {
                System.Diagnostics.Debug.WriteLine("HearingTestAudioManager:PlayAudioTask - WARNING! LEFTRIGHT WAS " + LeftRight + ". Playing center pan");
                soundEffect.Pan = 0.0f;
            }

            soundEffect.FinishedPlaying += (object sender, AVStatusEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("DONE PLAYING");
                soundEffect = null;
                tcs.SetResult(true);
            };

            soundEffect.NumberOfLoops = 0;
            System.Diagnostics.Debug.WriteLine("STARTED PLAYING");
            soundEffect.Play();

            /*
            System.Console.WriteLine("HearingTestAudioManager:PlayAudioTask - soundEffect.NumberOfChannels.ToString() - " + soundEffect.NumberOfChannels.ToString()); //1
            System.Console.WriteLine("HearingTestAudioManager:PlayAudioTask - soundEffect.Volume.ToString() - " + soundEffect.Volume.ToString()); //1
            System.Console.WriteLine("HearingTestAudioManager:PlayAudioTask - soundEffect.AveragePower(0).ToString() - " + soundEffect.AveragePower(0).ToString()); //-160
            System.Console.WriteLine("HearingTestAudioManager:PlayAudioTask - soundEffect.PeakPower(0).ToString() - " + soundEffect.PeakPower(0).ToString()); //-160
            */

            return tcs.Task;
        }

        public float GetOutputVolume()
        {
            AVAudioSession.SharedInstance().SetActive(true);
            return AVAudioSession.SharedInstance().OutputVolume;
        }
        #endregion

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }
    }

  
}