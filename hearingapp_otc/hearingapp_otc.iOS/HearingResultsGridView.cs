using CoreGraphics;
using Foundation;
using System;
using UIKit;
using hearingapp_otc.Classes;
using hearingapp_otc.iOS.UIClasses;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;

namespace hearingapp_otc.iOS
{
    public partial class HearingResultsGridView : UIView
    {
        public static readonly string[] testingHz = new string[] { "500", "1000", "2000", "4000" };
        const int hzLineCount = 5; // c# limitation w cont arrays? to be used in switch must be const. manually assign: testingHz.Length+1 when adjusting frequencies tested
        
        public static readonly string[] testingdB = new string[] { "-10", "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110" };

        public HearingResultsGridView (IntPtr handle) : base (handle)
        {
            BackgroundColor = FlatColors.Clouds;
            Opaque = false;

        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            Console.WriteLine("------------------------------- > Oh neat, viewMyGridTest session ID is - " + App.globablSessionId.ToString());
            
            var gctx = UIGraphics.GetCurrentContext();

            // setting blend mode to clear and filling with
            // a clear color results in a transparent fill
            gctx.SetFillColor(FlatColors.Clouds.CGColor);
            gctx.FillRect(rect);

            gctx.SetBlendMode(CGBlendMode.Clear);

            var startingPoint = new PointF(0, 0);
            gctx.MoveTo(startingPoint.X, startingPoint.Y);  // used to define starting point of line

            // Define our label height, which is also used to determine the top margin
            float labelHeight = 40.0f;
            float labelWidth = 60.0f;

            // Establish some more constants that will help us draw to fit our canvas
            // Vertical bar spacing - First let's calculate how far apart each of the lines needs to be that are parallel to the y-axis
            float hzWidth = ((float)rect.Width - labelWidth) / (float)hzLineCount;
            // Horizontal bar spacing - Calculate how far apart the horizontal dB lines need to be, taking into account margins
            float dBHeight = ((float)rect.Height - labelHeight) / 12.0f;


            //
            // DRAW "NORMAL" HEARING - 30db and up - GREEN (FlatColor: Emerald)
            //
            gctx.SetStrokeColor(FlatColors.Emerland.CGColor);
            gctx.SetFillColor(FlatColors.Emerland.CGColor);
            FlatColors.Emerland.SetColor();
            FlatColors.Emerland.SetFill();
            gctx.SetAlpha(0.3f);

            var badHearingRect = new CGRect(
                labelWidth,                          // X
                labelHeight,                         // Y
                labelWidth + hzWidth * hzLineCount,  // WIDTH
                labelHeight + dBHeight * 3 + 2       // HEIGHT
            );

            CGPath badHearingRectPath = new CGPath();
            badHearingRectPath.AddRect(badHearingRect);
            gctx.AddPath(badHearingRectPath);

            gctx.SetBlendMode(CGBlendMode.Normal);
            gctx.SetStrokeColor(FlatColors.Emerland.CGColor);
            gctx.DrawPath(CGPathDrawingMode.FillStroke);
            gctx.StrokePath();
            
            //
            // DRAW "MILD MODERATE" HEARING LOSS - 55db up to 30db - YELLOW (Sun Flower)
            //
            gctx.SetStrokeColor(FlatColors.Sunflower.CGColor);
            gctx.SetFillColor(FlatColors.Sunflower.CGColor);
            FlatColors.Sunflower.SetColor();
            FlatColors.Sunflower.SetFill();
            gctx.SetAlpha(0.3f);

            badHearingRect = new CGRect(
                labelWidth,                          // X
                labelHeight + dBHeight * 4,          // Y
                labelWidth + hzWidth * hzLineCount,  // WIDTH
                labelHeight + dBHeight * 1.5 + 3     // HEIGHT
            );

            badHearingRectPath = new CGPath();
            badHearingRectPath.AddRect(badHearingRect);
            gctx.AddPath(badHearingRectPath);

            gctx.SetBlendMode(CGBlendMode.Normal);
            gctx.SetStrokeColor(FlatColors.Sunflower.CGColor);
            gctx.DrawPath(CGPathDrawingMode.FillStroke);
            gctx.StrokePath();


            //
            // DRAW "MODERATE SEVERE" HEARING LOSS - 90db up to 55db - BLUE (Peter River)
            //
            gctx.SetStrokeColor(FlatColors.PeterRiver.CGColor);
            gctx.SetFillColor(FlatColors.PeterRiver.CGColor);
            FlatColors.PeterRiver.SetColor();
            FlatColors.PeterRiver.SetFill();
            gctx.SetAlpha(0.3f);

            badHearingRect = new CGRect(
                labelWidth,                          // X
                labelHeight + dBHeight * 6.5,        // Y
                labelWidth + hzWidth * hzLineCount,  // WIDTH
                labelHeight + dBHeight * 2.5 + 2     // HEIGHT
            );

            badHearingRectPath = new CGPath();
            badHearingRectPath.AddRect(badHearingRect);
            gctx.AddPath(badHearingRectPath);

            gctx.SetBlendMode(CGBlendMode.Normal);
            gctx.SetStrokeColor(FlatColors.PeterRiver.CGColor);
            gctx.DrawPath(CGPathDrawingMode.FillStroke);
            gctx.StrokePath();

            //
            // DRAW "SEVERE" HEARING LOSS - 90db and down - RED (Alizarin)
            //
            gctx.SetStrokeColor(FlatColors.Alizarin.CGColor);
            gctx.SetFillColor(FlatColors.Alizarin.CGColor);
            FlatColors.Alizarin.SetColor();
            FlatColors.Alizarin.SetFill();
            gctx.SetAlpha(0.3f);

            badHearingRect = new CGRect(
                labelWidth,                          // X
                labelHeight + dBHeight * 10,         // Y
                labelWidth + hzWidth * hzLineCount,  // WIDTH
                labelHeight + dBHeight * 2           // HEIGHT
            );

            badHearingRectPath = new CGPath();
            badHearingRectPath.AddRect(badHearingRect);
            gctx.AddPath(badHearingRectPath);

            gctx.SetBlendMode(CGBlendMode.Normal);
            gctx.SetStrokeColor(FlatColors.Alizarin.CGColor);
            gctx.DrawPath(CGPathDrawingMode.FillStroke);
            gctx.StrokePath();


            // Restore our graphics context back to normal so we are ready to draw the X/Y grid
            gctx.SetStrokeColor(UIColor.Black.CGColor);
            gctx.SetFillColor(FlatColors.Clouds.CGColor);
            UIColor.Black.SetColor();
            gctx.SetLineWidth(3);
            gctx.SetAlpha(1.0f);

            //
            // DRAW AXIS - HERTZ BARS - PARALLEL TO Y-AXIS
            // CURRENTLY TEST FOR: 500, 1000, 2000, 4000
            // (plus left axis and a unlabeled bar on the right side)
            // |-->500-->1000-->2000-->4000-->8000-->|
            // MADE INTO A PUBLIC CONSTANT -- string[] testingHz = new string[] { "500", "1000", "2000", "4000" };
            //

            // Now let's draw our hzLineCount number of lines. Account for the fact that if we use a perfect 0,0 or
            //  align to the far right of the view, some of the line gets clipped. So we have to
            //  create a 2 pixel margin by shifting in the lines on the far right and left
            for (int i=0; i<= hzLineCount; i++)
            {

                var hzLabel = new UILabel();
                float seg_size;

                switch (i)
                {
                    //labelHeight used here to establish the margin - where the top of the line starts
                    case 0:
                        seg_size = 0;
                        hzLabel = new UILabel(new CGRect(labelWidth - (labelWidth / 2), 0.0f, labelWidth, labelHeight));
                        hzLabel.Text = "";
                        break;
                    case hzLineCount:
                        seg_size = (float)i * hzWidth - 2.0f;
                        hzLabel = new UILabel(new CGRect(seg_size - (labelWidth/2) + labelWidth, 0.0f, labelWidth, labelHeight));
                        hzLabel.Text = "";
                        break;
                    default:
                        seg_size = (float)i * hzWidth;
                        hzLabel = new UILabel(new CGRect(seg_size - (labelWidth/2) + labelWidth, 0.0f, labelWidth + 18.0f, labelHeight));   // 16.0f is to fudge width for adding the "Hz" text
                        hzLabel.Text = testingHz[i - 1] + "Hz";
                        break;
                }

                gctx.AddLines(new CGPoint[] {
                    new PointF ( seg_size + labelWidth, (labelHeight-1.0f) ),
                    new PointF ( seg_size + labelWidth, (float) rect.Height )
                });

                //   Temp note: constructor for CGRect - public CGRect(float x, float y, float width, float height);

                // Let's paint a label 
                hzLabel.BackgroundColor = UIColor.Clear;
                hzLabel.Font = UIFont.SystemFontOfSize(20.0f);
                hzLabel.TextAlignment = UITextAlignment.Center;
                hzLabel.TextColor = UIColor.Black;
                this.Add(hzLabel);
            }
            gctx.StrokePath();

            //
            // DRAW AXIS - dB BARS - PARALLEL TO X-AXIS
            // CURRENTLY TEST FOR: -5 (START MARKINGS AT -10) to 80
            // SO: -10, 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110
            // MADE INTO PUBLIC VAR - string[] testingdB = new string[] { "-10", "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110" };
            // 
            for (int i = 0; i <= (testingdB.Length-1); i++)
            {

                var dbLabel = new UILabel();
                float seg_size;

                switch (i)
                {
                    //labelHeight used here to establish the margin - where the top of hteline starts
                    case 0:
                        seg_size = 0;
                        dbLabel = new UILabel(new CGRect(0.0f - 12.0f, seg_size - (labelHeight / 2) + labelHeight, labelWidth + 18.0f, labelHeight));
                        dbLabel.Text = testingdB[i] + "dB";
                        break;
                    case 12:
                        seg_size = (float)i * dBHeight - 2.0f;
                        dbLabel = new UILabel(new CGRect(0.0f - 12.0f, seg_size - (labelHeight / 2) + labelHeight, labelWidth + 18.0f, labelHeight));
                        dbLabel.Text = testingdB[i] + "dB";
                        break;
                    default:
                        seg_size = (float)i * dBHeight;
                        dbLabel = new UILabel(new CGRect(0.0f - 12.0f, seg_size - (labelHeight / 2) + labelHeight, labelWidth + 18.0f, labelHeight));
                        dbLabel.Text = testingdB[i] + "dB";
                        break;
                }

                gctx.AddLines(new CGPoint[] {
                    new PointF ( (labelWidth-1.0f), seg_size + labelHeight ),
                    new PointF ( (float) rect.Width, seg_size + labelHeight )
                });

                //   Temp note: constructore for CGRect - public CGRect(float x, float y, float width, float height);

                // Let's paint a label 
                dbLabel.BackgroundColor = UIColor.Clear;
                dbLabel.Font = UIFont.SystemFontOfSize(20.0f);
                dbLabel.TextAlignment = UITextAlignment.Center;
                dbLabel.TextColor = UIColor.Black;
                this.Add(dbLabel);
            }
            gctx.StrokePath();

            //
            // GET A HANDLE TO OUR SESSION WE'LL NEED TO DRAW
            //
            string db_name = "sessions_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string db_path = Path.Combine(folderPath, db_name);
            Session sess = DatabaseHelper.GetSesionById(db_path, App.globablSessionId);
            
            //
            // DRAW RESULTS
            //
            float oSizeConstant = 30.0f; // Right ear, red circle constant
            float xSizeConstant = 17.0f; // Left ear, blue X Size

            float xCoord, yCoord, lastXCoord, lastYCoord;
            // Init these to make our foreach legal
            lastXCoord = 0.0f;
            lastYCoord = 0.0f;

            //
            // DRAW RIGHT EAR RESULTS
            //
            // TODO: Move this into the Session update method, the Session class should be maintaining this
            // For testing purposes, set any uninitialized values to -5
            Dictionary<string, float> dictResults = new Dictionary<string, float>()
            {
                { "500",  (sess.RightEarThreshold_500Hz  != -99f) ? sess.RightEarThreshold_500Hz  : -5.0f },
                { "1000", (sess.RightEarThreshold_1000Hz != -99f) ? sess.RightEarThreshold_1000Hz : -5.0f },
                { "2000", (sess.RightEarThreshold_2000Hz != -99f) ? sess.RightEarThreshold_2000Hz : -5.0f },
                { "4000", (sess.RightEarThreshold_4000Hz != -99f) ? sess.RightEarThreshold_4000Hz : -5.0f },
            };
            
            // Iterate over the array and draw our stuff
            foreach (var kvp in dictResults.ToArray())
            {
                //dictResults[kvp.Key] = kvp.Value;
                xCoord = GetXCoord(kvp.Key, hzWidth, labelWidth);
                yCoord = GetYCoord(dictResults[kvp.Key], dBHeight, labelHeight);
                DrawARedCircle(xCoord, yCoord, oSizeConstant, gctx);

                // Skip the line drawing for our first item (it has no previous thing to connect)
                if(kvp.Key != "500")
                {
                    gctx.SetBlendMode(CGBlendMode.Normal);
                    gctx.SetStrokeColor(UIColor.Red.CGColor);
                    gctx.SetLineWidth(2);
                    gctx.AddLines(new CGPoint[] {
                        new PointF ( lastXCoord, lastYCoord ),
                        new PointF ( xCoord, yCoord )
                    });
                    gctx.StrokePath();
                }

                lastXCoord = xCoord;
                lastYCoord = yCoord;
            }

            //
            // DRAW LEFT EAR RESULTS
            //
            dictResults = new Dictionary<string, float>()
            {
                { "500",  (sess.LeftEarThreshold_500Hz  != -99f) ? sess.LeftEarThreshold_500Hz :   -5.0f },
                { "1000", (sess.LeftEarThreshold_1000Hz != -99f) ? sess.LeftEarThreshold_1000Hz : -5.0f },
                { "2000", (sess.LeftEarThreshold_2000Hz != -99f) ? sess.LeftEarThreshold_2000Hz : -5.0f },
                { "4000", (sess.LeftEarThreshold_4000Hz != -99f) ? sess.LeftEarThreshold_4000Hz : -5.0f },
            };

            // Iterate over the array and draw our stuff
            foreach (var kvp in dictResults.ToArray())
            {
                //dictResults[kvp.Key] = kvp.Value;
                xCoord = GetXCoord(kvp.Key, hzWidth, labelWidth);
                yCoord = GetYCoord(kvp.Value, dBHeight, labelHeight);
                DrawABlueX(xCoord, yCoord, xSizeConstant, gctx);

                // Skip the line drawing for our first item (it has no previous thing to connect)
                if (kvp.Key != "500")
                {
                    gctx.SetBlendMode(CGBlendMode.Normal);
                    gctx.SetStrokeColor(UIColor.Blue.CGColor);
                    gctx.SetLineWidth(2);
                    gctx.AddLines(new CGPoint[] {
                        new PointF ( lastXCoord, lastYCoord ),
                        new PointF ( xCoord, yCoord )
                    });
                    gctx.StrokePath();
                }

                lastXCoord = xCoord;
                lastYCoord = yCoord;
            }

            // FIN!

        }

        private float GetYCoord(float dbToFind, float stepSize, float offset)
        {
            //REFERENCE: string[] testingdB = new string[] { "-10", "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110" };
            string dbAsString = dbToFind.ToString();
            int indexLocation = Array.IndexOf(testingdB, dbAsString);
            float yLocation = (float)indexLocation * stepSize;
            yLocation += offset;

            // dB's ending in "0"
            if (indexLocation != -1)
            {
                return yLocation;
            }
            // dB's ending in "5"
            else
            {
                dbToFind -= 5.0f;                           // This should be safe because we only test down to -5
                dbAsString = dbToFind.ToString();
                indexLocation = Array.IndexOf(testingdB, dbAsString);
                yLocation = (float)indexLocation * stepSize;
                yLocation += offset;

                yLocation += stepSize / 2.0f;               // Now move up a half step

                return yLocation;
            }
        }

        private float GetXCoord(string HzToFind, float stepSize, float offset)
        {
            //REFERENCE: private string[] testingHz = new string[] { "500", "1000", "2000", "4000" };
            // If you crash here, you probably haven't registed the new frquency being tested with the array above this comment
            int indexLocation = Array.IndexOf(testingHz, HzToFind) + 1;
            float xLocation = (float)indexLocation * stepSize;
            xLocation += offset;

            return xLocation;
        }

        private void DrawARedCircle(float x, float y, float size, CGContext gctx)
        {
            gctx.SetBlendMode(CGBlendMode.Normal);
            gctx.SetStrokeColor(UIColor.Red.CGColor);
            gctx.SetLineWidth(4);

            float halfOfSize = size / 2;

            var ellipseSize = new SizeF(size, size);
            var ellipsePosition = new PointF(x - halfOfSize, y - halfOfSize);
            gctx.AddEllipseInRect(new RectangleF(ellipsePosition, ellipseSize));

            gctx.StrokePath();
        }

        private void DrawABlueX(float x, float y, float size, CGContext gctx)
        {
            gctx.SetBlendMode(CGBlendMode.Normal);
            gctx.SetStrokeColor(UIColor.Blue.CGColor);
            gctx.SetLineWidth(4);

            float halfOfSize = size / 2.0f;

            // Top left and bottom right points
            gctx.AddLines(new CGPoint[] {
                    new PointF ( x - halfOfSize, y + halfOfSize),
                    new PointF ( x + halfOfSize, y - halfOfSize )
            });

            // Bottom left and top right points
            gctx.AddLines(new CGPoint[] {
                    new PointF ( x - halfOfSize, y - halfOfSize),
                    new PointF ( x + halfOfSize, y + halfOfSize )
            });

            gctx.StrokePath();

        }

        // For getting a reference to our app delegate, in order to get a handle to the AudioManager
        public static AppDelegate App { get { return (AppDelegate)UIApplication.SharedApplication.Delegate; } }

    }
}