using System.Drawing;

namespace OpenFace
{
    public class MKParams
    {
        private double skinRatio = .95;
        private double hairRatio = .9;
        private double faceAlpha = .1;
        private double hairAlpha = .9;
        private double eyebrowAlpha = .18;
        private double eyeLineSlope = .5;
        private int lipStickAlpha = 100;
        private int lipLineAlpha = 100;
        private int eyeAlpha = 100;
        private int eyeLineAlpha = 100;
        private Color skinMaskColor = Color.FromArgb(255, 255, 255);
        private Color hairMaskColor = Color.FromArgb(6, 89, 35);
        private Color eyebrowMaskColor = Color.FromArgb(61, 12, 12);
        private Color eyeMaskColor = Color.FromArgb(0, 100, 100);
        private Color eyeLineColor = Color.FromArgb(10, 0, 0);
        private Color lipLineColor = Color.FromArgb(100, 0, 0);
        private Color lipStickColor = Color.FromArgb(220, 13, 75);
        private bool hairEnabled = false;
        private bool skinEnabled = false;
        private bool eyeEnabled = false;
        private bool eyeLineEnabled = false;
        private bool eyebrowEnabled = false;
        private bool lipEnabled = false;

        public double SkinRatio
        {
            get
            {
                return skinRatio;
            }

            set
            {
                skinRatio = value;
            }
        }

        public double HairRatio
        {
            get
            {
                return hairRatio;
            }

            set
            {
                hairRatio = value;
            }
        }

        public double FaceAlpha
        {
            get
            {
                return faceAlpha;
            }

            set
            {
                faceAlpha = value;
            }
        }

        public double HairAlpha
        {
            get
            {
                return hairAlpha;
            }

            set
            {
                hairAlpha = value;
            }
        }

        public double EyebrowAlpha
        {
            get
            {
                return eyebrowAlpha;
            }

            set
            {
                eyebrowAlpha = value;
            }
        }

        public double EyeLineSlope
        {
            get
            {
                return eyeLineSlope;
            }

            set
            {
                eyeLineSlope = value;
            }
        }

        public Color SkinMaskColor
        {
            get
            {
                return skinMaskColor;
            }

            set
            {
                skinMaskColor = value;
            }
        }

        public Color HairMaskColor
        {
            get
            {
                return hairMaskColor;
            }

            set
            {
                hairMaskColor = value;
            }
        }

        public Color EyebrowMaskColor
        {
            get
            {
                return eyebrowMaskColor;
            }

            set
            {
                eyebrowMaskColor = value;
            }
        }

        public Color EyeMaskColor
        {
            get
            {
                return eyeMaskColor;
            }

            set
            {
                eyeMaskColor = value;
            }
        }

        public Color LipLineColor
        {
            get
            {
                return lipLineColor;
            }

            set
            {
                lipLineColor = value;
            }
        }

        public Color LipStickColor
        {
            get
            {
                return lipStickColor;
            }

            set
            {
                lipStickColor = value;
            }
        }

        public Color EyeLineColor
        {
            get
            {
                return eyeLineColor;
            }

            set
            {
                eyeLineColor = value;
            }
        }

        public bool HairEnabled
        {
            get
            {
                return hairEnabled;
            }

            set
            {
                hairEnabled = value;
            }
        }

        public bool SkinEnabled
        {
            get
            {
                return skinEnabled;
            }

            set
            {
                skinEnabled = value;
            }
        }

        public bool EyeEnabled
        {
            get
            {
                return eyeEnabled;
            }

            set
            {
                eyeEnabled = value;
            }
        }

        public bool EyebrowEnabled
        {
            get
            {
                return eyebrowEnabled;
            }

            set
            {
                eyebrowEnabled = value;
            }
        }

        public bool LipEnabled
        {
            get
            {
                return lipEnabled;
            }

            set
            {
                lipEnabled = value;
            }
        }

        public bool EyeLineEnabled
        {
            get
            {
                return eyeLineEnabled;
            }

            set
            {
                eyeLineEnabled = value;
            }
        }

        public int LipStickAlpha
        {
            get
            {
                return lipStickAlpha;
            }

            set
            {
                lipStickAlpha = value;
            }
        }

        public int LipLineAlpha
        {
            get
            {
                return lipLineAlpha;
            }

            set
            {
                lipLineAlpha = value;
            }
        }

        public int EyeAlpha
        {
            get
            {
                return eyeAlpha;
            }

            set
            {
                eyeAlpha = value;
            }
        }

        public int EyeLineAlpha
        {
            get
            {
                return eyeLineAlpha;
            }

            set
            {
                eyeLineAlpha = value;
            }
        }
    }
}
