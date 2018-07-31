using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Drawing;

namespace ZatsHackBase.UI.GUI
{
    class Colors
    {
        // Active: Enabled = true
        // Inactive: Enabled = false

        // http://hags-club.com/i/7ef560eea5b376c0.png

        public static Color FrameBgActive = new Color();
        public static Color FrameBgInactive = new Color();
        public static Color FrameBorderActive = new Color();
        public static Color FrameBorderInactive = new Color();
        public static Color FrameCaptionActive = new Color();
        public static Color FrameCaptionInactive = new Color();
        public static Color FrameTextInactive = new Color();
        public static Color FrameTextActive = new Color();

        public static Color ButtonBgActive = new Color(); // Enabled = true
        public static Color ButtonBgInactive = new Color(); // Enabled = false
        public static Color ButtonBgHighlight = new Color();
        public static Color ButtonBgPressed = new Color();
        public static Color ButtonBorderActive = ButtonBgActive; // Enabled = true
        public static Color ButtonBorderInactive = ButtonBgInactive; // Enabled = false
        public static Color ButtonBorderHighlight = ButtonBgHighlight;
        public static Color ButtonBorderPressed = ButtonBgPressed;
        public static Color ButtonTextActive = new Color();
        public static Color ButtonTextInactive = new Color();
        public static Color ButtonTextHighlight = new Color();
        public static Color ButtonTextPressed = new Color();

        public static Color ScrollShaftBgActive = new Color();
        public static Color ScrollShaftBgInactive = new Color();
        public static Color ScrollShaftBgHighlight = new Color();
        public static Color ScrollShaftBgPressed = new Color();
        public static Color ScrollThumbBgActive = new Color();
        public static Color ScrollThumbBgInactive = new Color();
        public static Color ScrollThumbBgHighlight = new Color();
        public static Color ScrollThumbBgPressed = new Color();
        public static Color ScrollButtonBgActive = new Color();
        public static Color ScrollButtonBgInactive = new Color();
        public static Color ScrollButtonBgHighlight = new Color();
        public static Color ScrollButtonBgPressed = new Color();
        public static Color ScrollTextActive = new Color();
        public static Color ScrollTextInactive = new Color();
        public static Color ScrollTextHighlight = new Color();
        public static Color ScrollTextPressed = new Color();

        public static Color StaticBorder = ButtonBorderActive;
        
        //public static IndicationDescriptor IdFrameBg = new IndicationDescriptor(FrameBgActive, FrameBgInactive);
        //public static IndicationDescriptor IdFrameBorder = new IndicationDescriptor(FrameBorderActive, FrameBorderInactive);
        //public static IndicationDescriptor IdFrameCaption = new IndicationDescriptor(FrameCaptionActive, FrameCaptionInactive);
        //public static IndicationDescriptor IdFrameText = new IndicationDescriptor(FrameTextActive, FrameTextInactive);

        public static IndicationDescriptor IdScrollShaftBg = new IndicationDescriptor(ScrollShaftBgActive, ScrollShaftBgInactive, ScrollShaftBgHighlight, ScrollShaftBgPressed);
        public static IndicationDescriptor IdScrollThumbBg = new IndicationDescriptor(ScrollThumbBgActive, ScrollThumbBgInactive, ScrollThumbBgHighlight, ScrollThumbBgPressed);
        public static IndicationDescriptor IdScrollButtonBg = new IndicationDescriptor(ScrollButtonBgActive, ScrollButtonBgInactive, ScrollButtonBgHighlight, ScrollButtonBgPressed);
        public static IndicationDescriptor IdScrollText = new IndicationDescriptor(ScrollTextActive, ScrollTextInactive, ScrollTextHighlight, ScrollTextPressed);

        public static IndicationDescriptor IdButtonBg = new IndicationDescriptor(ButtonBgActive, ButtonBgInactive, ButtonBgHighlight, ButtonBgPressed);
        public static IndicationDescriptor IdButtonBorder = new IndicationDescriptor(ButtonBorderActive, ButtonBorderInactive, ButtonBorderHighlight, ButtonBorderPressed);
        public static IndicationDescriptor IdButtonText = new IndicationDescriptor(ButtonTextActive, ButtonTextInactive, ButtonTextHighlight, ButtonTextPressed);

        public static IndicationDescriptor IdStaticBorder = new IndicationDescriptor(StaticBorder, StaticBorder, StaticBorder, StaticBorder);
    }
}
