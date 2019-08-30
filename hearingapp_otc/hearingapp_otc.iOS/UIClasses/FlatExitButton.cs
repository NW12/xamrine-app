using UIKit;
using System.Drawing;
using System;

namespace hearingapp_otc.iOS.UIClasses
{
    public sealed class FlatExitButton : UIButton
    {
        UIEdgeInsets defaultEdgeInsets;
        UIEdgeInsets normalEdgeInsets;
        UIEdgeInsets highlightedEdgeInsets;

        // Set color and radius
        UIColor color = FlatColors.Pomegranate;
        UIColor shadowColor = FlatColors.Pomegranate;
        float cornerRadius = 6f;
        float shadowHeight = 1f;

        public FlatExitButton(int x, int y)
        {
            //Defaults
            this.defaultEdgeInsets = TitleEdgeInsets;

            // All help buttons are the same size
            this.Frame = new RectangleF(x, y, 80, 47);
            this.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            this.SetTitle("EXIT TEST");
            this.Enabled = true;
            this.UserInteractionEnabled = true;

            TitleLabel.Font = UIFont.BoldSystemFontOfSize(14);
            SetTitleColor(FlatColors.Clouds, UIControlState.Normal);
            SetTitleColor(FlatColors.Clouds, UIControlState.Highlighted);
            CornerRadius = 6f;
            ShadowHeight = 1f;

            //Create button graphics
            ConfigureFlatHelpButton();
        }

        public void SetTitle(string title)
        {
            SetTitle(title, UIControlState.Normal);
            SetTitle(title, UIControlState.Highlighted);
        }

        public override bool Highlighted
        {
            get
            {
                return base.Highlighted;
            }
            set
            {
                TitleEdgeInsets = value ? highlightedEdgeInsets : normalEdgeInsets;
                base.Highlighted = value;
            }
        }

        public UIColor Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                ConfigureFlatHelpButton();
            }
        }

        public UIColor ShadowColor
        {
            get
            {
                return shadowColor;
            }
            set
            {
                shadowColor = value;
                ConfigureFlatHelpButton();
            }
        }

        public float CornerRadius
        {
            get
            {
                return cornerRadius;
            }
            set
            {
                cornerRadius = value;
                ConfigureFlatHelpButton();
            }
        }

        public float ShadowHeight
        {
            get
            {
                return shadowHeight;
            }
            set
            {
                shadowHeight = value;
                var insets = defaultEdgeInsets;
                highlightedEdgeInsets = insets;
                insets.Top -= shadowHeight;
                normalEdgeInsets = insets;
                TitleEdgeInsets = insets;

                ConfigureFlatHelpButton();
            }
        }

        void ConfigureFlatHelpButton()
        {
            UIImage normalBackgroundImage = ImageHelper.ButtonImage(Color, CornerRadius, ShadowColor,
                                                                     new UIEdgeInsets(0, 0, ShadowHeight, 0));
            UIImage highlightedBackgroundImage = ImageHelper.ButtonImage(Color, CornerRadius, UIColor.Clear,
                                                                          new UIEdgeInsets(ShadowHeight, 0, 0, 0));

            SetBackgroundImage(normalBackgroundImage, UIControlState.Normal);
            SetBackgroundImage(highlightedBackgroundImage, UIControlState.Highlighted);
        }

    }
}