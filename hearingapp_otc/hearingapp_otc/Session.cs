// using Amazon.DynamoDBv2.DataModel; // TODO: AWS
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
//using UIKit; // TODO: FIX UIKit Assembly Ref

namespace hearingapp_otc.Classes
{
    // [DynamoDBTable("UserInfo")]  // TODO: AWS
    public class Session
    {
        public Session()
        {
            this.SessionGUID = Guid.NewGuid().ToString();
            this.DeviceIdForVendor = "DEVICEIDPLACEHOLDER"; // TODO: FIX UIKit Assembly Ref; UIDevice.CurrentDevice.IdentifierForVendor.AsString();

            // TODO: Add timezone
            var creation = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            this.CreationTimestamp = creation.ToString("MM/dd/yyyy HH:mm:ss.fff");

            // Currently only test: 1000, 500, 2000, 4000, 8000
            this.LeftEarThreshold_125Hz = -99f;
            this.RightEarThreshold_125Hz = -99f;
            this.LeftEarThreshold_250Hz = -99f;
            this.RightEarThreshold_250Hz = -99f;
            this.LeftEarThreshold_500Hz = -99f;
            this.RightEarThreshold_500Hz = -99f;
            this.LeftEarThreshold_1000Hz = -99f;
            this.RightEarThreshold_1000Hz = -99f;
            this.LeftEarThreshold_2000Hz = -99f;
            this.RightEarThreshold_2000Hz = -99f;
            this.LeftEarThreshold_4000Hz = -99f;
            this.RightEarThreshold_4000Hz = -99f;
            this.LeftEarThreshold_8000Hz = -99f;
            this.RightEarThreshold_8000Hz = -99f;

            this.SurveQ1 = true;
            this.SurveQ2 = true;
            this.SurveQ3 = true;
            this.SurveQ4 = true;
            this.SurveQ5 = true;
            this.SurveQ6 = true;
            this.SurveQ7 = true;

            this.PhoneNumber = "";
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /*Used to uniquely identify our session across devices We could make it
         more unique by doing something like hashing it along with the user's name
         and the date or something. But this should be good enough for now.*/
        public string SessionGUID { get; set; }
        public string CreationTimestamp { get; set; }
        public bool devModeEnabled { get; set; }

        public string DeviceIdForVendor { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get;  set; }

        public bool SurveQ1 { get; set; }
        public bool SurveQ2 { get; set; }
        public bool SurveQ3 { get; set; }
        public bool SurveQ4 { get; set; }
        public bool SurveQ5 { get; set; }
        public bool SurveQ6 { get; set; }
        public bool SurveQ7 { get; set; }

        public float LeftEarThreshold_125Hz { get; set; }
        public float RightEarThreshold_125Hz { get; set; }

        public float LeftEarThreshold_250Hz { get; set; }
        public float RightEarThreshold_250Hz { get; set; }

        public float LeftEarThreshold_500Hz { get; set; }
        public float RightEarThreshold_500Hz { get; set; }

        public float LeftEarThreshold_1000Hz { get; set; }
        public float RightEarThreshold_1000Hz { get; set; }

        public float LeftEarThreshold_2000Hz { get; set; }
        public float RightEarThreshold_2000Hz { get; set; }

        public float LeftEarThreshold_4000Hz { get; set; }
        public float RightEarThreshold_4000Hz { get; set; }

        public float LeftEarThreshold_8000Hz { get; set; }
        public float RightEarThreshold_8000Hz { get; set; }
        
        public float dBStart { get; set; }

        public bool ExitNoSale { get; set; }

        public string SKUChosen { get; set; }

        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return string.Format($"Session:{Id} ({this.FirstName},{this.LastName})");
        }
    }
}
