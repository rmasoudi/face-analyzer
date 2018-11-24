using System.Drawing;

namespace OpenFace
{
    public class StaticParams
    {
        public static Color[] LipColors = new[] { Color.FromArgb( 201, 32, 201), Color.FromArgb( 201, 32, 147), Color.FromArgb(163, 14, 69), Color.FromArgb(163, 14, 59) };
        public static Color[] LipLineColors = new[] { Color.FromArgb(201, 32, 201), Color.FromArgb(201, 32, 147), Color.FromArgb(163, 14, 59) };
        public static Color[] EyeColors = new[] { Color.FromArgb(39, 111, 191), Color.FromArgb(119, 29, 9), Color.FromArgb(56, 12, 2), Color.FromArgb(20, 3, 0), Color.FromArgb(99, 48, 3) };
        public static Color[] EyeBrowColors = new[] { Color.FromArgb(130, 43, 14), Color.FromArgb(94, 26, 3), Color.FromArgb(94, 3, 3), Color.FromArgb(191, 34, 34), Color.FromArgb(196, 118, 1), Color.FromArgb(3, 68, 38) };
        public static Color[] EyeLineColors = new[] { Color.FromArgb(33, 16, 2), Color.FromArgb(0, 0, 0), Color.FromArgb(2, 25, 9) };
        public static Color[] SkinColors = new[] { Color.FromArgb(255, 255, 255), Color.FromArgb(252, 247, 239), Color.FromArgb(237, 226, 208), Color.FromArgb(237, 234, 208) };
        public static Color[] HairColors = new[] { Color.FromArgb(73, 42, 9), Color.FromArgb(67, 73, 9), Color.FromArgb(9, 73, 50), Color.FromArgb(14, 0, 20), Color.FromArgb(20, 0, 7), Color.FromArgb(96, 8, 38) };

        public static double MaxLipAlpha = 100;
        public static double MaxEyeAlpha = 90;
        public static double MaxEyebrowAlpha = .5;
        public static double MaxEyelineAlpha = 100;
        public static double MaxSkinAlpha = .5;
        public static double MaxHairAlpha = .5;
    }
}
