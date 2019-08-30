using System;
using UIKit;

namespace hearingapp_otc.iOS.UIClasses
{
    public class FlatFonts
    {
        public FlatFonts()
        {
        }

        public static UIFont BoldFontsWithSize(int size)
        {
            var font = UIFont.FromName("Lato-Bold", size);
            return font;
        }
    }
}
