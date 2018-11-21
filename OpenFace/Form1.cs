using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using MetroFramework.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
using Transitions;

namespace OpenFace
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private static int lastTimeSnapshot = DateTime.UtcNow.Millisecond;
        private const String FACE_DETECTOR_PATH = @"C:\Users\User\Documents\Visual Studio 2015\Projects\OpenFace\lbpcascade_frontalface.xml";
        private const String LANDMARK_DETECTOR_PATH = @"C:\Users\User\Documents\Visual Studio 2015\Projects\OpenFace\lbfmodel.yaml";
        private static CascadeClassifier faceDetector;
        private static FacemarkLBF facemark;
        private MKParams mkParams = new MKParams();

        private const int LIP_STICK_COMMAND = 1;
        private const int LIP_LINE_COMMAND = 2;
        private const int EYE_COMMAND = 3;
        private const int EYEBROW_COMMAND = 4;
        private const int EYE_LINE_COMMAND = 5;
        private const int SKIN_COMMAND = 6;
        private const int HAIR_COMMAND = 7;

        private static int currentCommand;


        private Image<Bgr, Byte> image;
        private Image<Gray, byte> grayImage;
        private Image<Bgr, Byte> mainColorImage;
        private Image<Gray, byte> mainGrayImage;
        private FaceModel faceModel;
        public Form1()
        {
            InitializeComponent();
            InitModel();
            image = new Image<Bgr, byte>(@"C:\Users\User\Documents\Visual Studio 2015\Projects\OpenFace\OpenFace\images\2.jpg");
            image = image.Resize(pictureBox1.Width, pictureBox1.Height, Inter.Linear, true);
            mainColorImage = image.Clone();
            grayImage = image.Convert<Gray, byte>();
            mainGrayImage = grayImage.Clone();
            mainPicture.Image = image.ToBitmap();
            faceModel = GetFaceModel(image, grayImage);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetDesktopLocation(600, 600);
        }

        private void InitModel()
        {
            faceDetector = new CascadeClassifier(FACE_DETECTOR_PATH);
            FacemarkLBFParams fParams = new FacemarkLBFParams();
            fParams.ModelFile = LANDMARK_DETECTOR_PATH;
            fParams.NLandmarks = 68; // number of landmark points
            fParams.InitShapeN = 10; // number of multiplier for make data augmentation
            fParams.StagesN = 5; // amount of refinement stages
            fParams.TreeN = 6; // number of tree in the model for each landmark point
            fParams.TreeDepth = 5; //he depth of decision tree
            facemark = new FacemarkLBF(fParams);
            facemark.LoadModel(fParams.ModelFile);
        }

        private FaceModel GetFaceModel(Image<Bgr, Byte> image, Image<Gray, byte> grayImage)
        {
            grayImage._EqualizeHist();
            VectorOfRect faces = new VectorOfRect(faceDetector.DetectMultiScale(grayImage));
            Rectangle[] rects = faces.ToArray();
            VectorOfVectorOfPointF landmarks = new VectorOfVectorOfPointF();
            bool success = facemark.Fit(grayImage, faces, landmarks);
            PointF[] points = landmarks.ToArrayOfArray()[0];
            if (!success)
            {
                return null;
            }
            return new FaceModel(points, rects[0]);
        }

        private void Makeup(Image<Bgr, Byte> image, Image<Gray, byte> grayImage, FaceModel faceModel, MKParams mkParams)
        {
            Graphics g = Graphics.FromImage(image.Bitmap);
            if (mkParams.SkinEnabled || mkParams.HairEnabled)
            {
                ProcessSkin(g, grayImage, image, faceModel, mkParams);
            }
            if (mkParams.LipEnabled)
            {
                ApplyLipStick(g, faceModel, mkParams);
            }
            if (mkParams.EyeLineEnabled)
            {
                ApplyEyeLinear(g, faceModel, mkParams);
            }
            if (mkParams.EyebrowEnabled)
            {
                EyeBrowEffects(g, grayImage, image, faceModel, mkParams);
            }
            if (mkParams.EyeEnabled)
            {
                ColorEyes(g, grayImage, image, faceModel, mkParams);
            }
        }

        private void appendTime(string title)
        {
            int total = DateTime.UtcNow.Millisecond - lastTimeSnapshot;
            this.Text += title + "=" + total + " ";
            lastTimeSnapshot = DateTime.UtcNow.Millisecond;
        }

        private void ProcessSkin(Graphics g, Image<Gray, byte> grayImage, Image<Bgr, Byte> image, FaceModel faceModel, MKParams mkParams)
        {
            double[] minValues;
            double[] maxValues;
            Point[] minLocs;
            Point[] maxLocs;

            int hairY = faceModel.HeadBox.Y - faceModel.HeadBox.Height / 2 + faceModel.HeadBox.Height / 4;
            Rectangle hairBox = new Rectangle(faceModel.HeadBox.X + faceModel.HeadBox.Width / 2, hairY > 0 ? hairY : 0, faceModel.HeadBox.Width / 2, faceModel.HeadBox.Height / 2);

            image.ROI = new Rectangle(faceModel.FaceBox.X + faceModel.FaceBox.Width / 2, faceModel.FaceBox.Y + faceModel.FaceBox.Height / 4, faceModel.FaceBox.Width / 2, faceModel.FaceBox.Height / 2);
            image.MinMax(out minValues, out maxValues, out minLocs, out maxLocs);
            image.ROI = Rectangle.Empty;
            double avg0 = (minValues[0] + maxValues[0]) * mkParams.SkinRatio;
            double avg1 = (minValues[0] + maxValues[0]) * mkParams.SkinRatio;
            double avg2 = (minValues[0] + maxValues[0]) * mkParams.SkinRatio;
            Image<Gray, byte> skinMask = image.InRange(new Bgr(minValues[0], minValues[1], minValues[2]), new Bgr(avg0, avg1, avg2));


            image.ROI = new Rectangle(hairBox.X + hairBox.Width / 2, hairBox.Y + hairBox.Height / 4, hairBox.Width / 2, hairBox.Height / 2);
            image.MinMax(out minValues, out maxValues, out minLocs, out maxLocs);
            image.ROI = Rectangle.Empty;
            avg0 = (minValues[0] + maxValues[0]) * mkParams.HairRatio;
            avg1 = (minValues[0] + maxValues[0]) * mkParams.HairRatio;
            avg2 = (minValues[0] + maxValues[0]) * mkParams.HairRatio;
            Image<Gray, byte> hairMask = image.InRange(new Bgr(minValues[0], minValues[1], minValues[2]), new Bgr(avg0, avg1, avg2));

            //ImageViewer viewer = new ImageViewer();
            //viewer.Image = hairMask;
            //viewer.ShowDialog();

            for (int i = image.Height - 1; i >= 0; i--)
            {
                for (int j = image.Width - 1; j >= 0; j--)
                {
                    Color oldColor = image.Bitmap.GetPixel(j, i);
                    int oldRed = oldColor.R;
                    int oldGreen = oldColor.G;
                    int oldBlue = oldColor.B;
                    bool inFace = isInBox(i, j, faceModel.FaceBox);
                    bool inHair = isInBox(i, j, hairBox);
                    if (skinMask[i, j].MCvScalar.V0 == 0 && inFace && mkParams.SkinEnabled)
                    {
                        Color maskColor = mkParams.SkinMaskColor;
                        int maskRed = maskColor.R;
                        int maskGreen = maskColor.G;
                        int maskBlue = maskColor.B;

                        int newRed = (int)(mkParams.FaceAlpha * maskRed + (1 - mkParams.FaceAlpha) * oldRed);
                        int newGreen = (int)(mkParams.FaceAlpha * maskGreen + (1 - mkParams.FaceAlpha) * oldGreen);
                        int newBlue = (int)(mkParams.FaceAlpha * maskBlue + (1 - mkParams.FaceAlpha) * oldBlue);
                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        image.Bitmap.SetPixel(j, i, newColor);
                    }
                    else if (!isInBoxArray(i, j, faceModel.LandmarkRects) && skinMask[i, j].MCvScalar.V0 != 0 && hairMask[i, j].MCvScalar.V0 == 255 && mkParams.HairEnabled)
                    {
                        Color maskColor = mkParams.HairMaskColor;
                        int maskRed = maskColor.R;
                        int maskGreen = maskColor.G;
                        int maskBlue = maskColor.B;

                        int newRed = (int)(mkParams.HairAlpha * maskRed + (1 - mkParams.HairAlpha) * oldRed);
                        int newGreen = (int)(mkParams.HairAlpha * maskGreen + (1 - mkParams.HairAlpha) * oldGreen);
                        int newBlue = (int)(mkParams.HairAlpha * maskBlue + (1 - mkParams.HairAlpha) * oldBlue);
                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        image.Bitmap.SetPixel(j, i, newColor);
                    }

                }
            }
        }

        private bool isInBoxArray(int i, int j, Rectangle[] rects)
        {
            for (int k = 0; k < rects.Length; k++)
            {
                if (isInBox(i, j, rects[k]))
                {
                    return true;
                }
            }
            return false;
        }
        private bool isInBox(int i, int j, Rectangle rect)
        {
            return (i >= rect.Top && i <= rect.Bottom) && (j >= rect.Left && j <= rect.Right);
        }

        private void ColorEyes(Graphics g, Image<Gray, byte> grayImage, Image<Bgr, Byte> image, FaceModel faceModel, MKParams mkParams)
        {
            grayImage.ROI = faceModel.LeftEyeBox;
            CircleF[] leftEye = CvInvoke.HoughCircles(grayImage, HoughType.Gradient, 1, (double)faceModel.LeftEyeBox.Height / 5.0, 10, (double)faceModel.LeftEyeBox.Height / 5.0, faceModel.LeftEyeBox.Height);
            grayImage.ROI = faceModel.RightEyeBox;
            CircleF[] rightEye = CvInvoke.HoughCircles(grayImage, HoughType.Gradient, 1, (double)faceModel.RightEyeBox.Height / 5.0, 10, (double)faceModel.RightEyeBox.Height / 5.0, faceModel.RightEyeBox.Height);
            grayImage.ROI = Rectangle.Empty;
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(mkParams.EyeAlpha, mkParams.EyeMaskColor));
            if (leftEye.Length > 0)
            {
                g.FillEllipse(solidBrush, faceModel.LeftEyeBox.Left + leftEye[0].Center.X - leftEye[0].Radius / 2, faceModel.LeftEyeBox.Top + leftEye[0].Center.Y - leftEye[0].Radius / 2, leftEye[0].Radius, leftEye[0].Radius);
            }
            if (rightEye.Length > 0)
            {
                g.FillEllipse(solidBrush, faceModel.RightEyeBox.Left + rightEye[0].Center.X - rightEye[0].Radius / 2, faceModel.RightEyeBox.Top + rightEye[0].Center.Y - rightEye[0].Radius / 2, rightEye[0].Radius, rightEye[0].Radius);
            }

        }

        private void EyeBrowEffects(Graphics g, Image<Gray, byte> grayImage, Image<Bgr, Byte> image, FaceModel faceModel, MKParams mkParams)
        {
            ColorEyebrow(faceModel.RightEyebrowBox, image.Bitmap, true, grayImage, mkParams);
            ColorEyebrow(faceModel.LeftEyebrowBox, image.Bitmap, false, grayImage, mkParams);
        }

        private Bitmap ColorEyebrow(Rectangle box, Bitmap image, bool right, Image<Gray, byte> grayImage, MKParams mkParams)
        {
            grayImage.ROI = box;
            Image<Gray, byte> cropped = grayImage.Copy();
            grayImage.ROI = Rectangle.Empty;
            double[] minValues;
            double[] maxValues;
            Point[] minLocs;
            Point[] maxLocs;
            cropped.MinMax(out minValues, out maxValues, out minLocs, out maxLocs);
            CvInvoke.Threshold(cropped, cropped, (minValues[0] + maxValues[0]) / 2, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

            for (int i = box.Bottom + (int)(1 * box.Height); i >= box.Top - (0.1 * box.Height); i--)
            {
                for (int j = box.Right; j >= box.Left; j--)
                {
                    int x = j - box.Left;
                    int y = i - box.Top;
                    bool inBounds = x >= 0 && x < cropped.Bitmap.Width;
                    inBounds = inBounds && (y >= 0 && y < cropped.Bitmap.Height);
                    if (inBounds && cropped.Bitmap.GetPixel(x, y).R == 0)
                    {
                        Color oldColor = image.GetPixel(j, i);
                        int oldRed = oldColor.R;
                        int oldGreen = oldColor.G;
                        int oldBlue = oldColor.B;

                        //Color maskColor = image.GetPixel(j, i - 2 * box.Height / 3);
                        Color maskColor = mkParams.EyebrowMaskColor;
                        int maskRed = maskColor.R;
                        int maskGreen = maskColor.G;
                        int maskBlue = maskColor.B;

                        int newRed = (int)(mkParams.EyebrowAlpha * maskRed + (1 - mkParams.EyebrowAlpha) * oldRed);
                        int newGreen = (int)(mkParams.EyebrowAlpha * maskGreen + (1 - mkParams.EyebrowAlpha) * oldGreen);
                        int newBlue = (int)(mkParams.EyebrowAlpha * maskBlue + (1 - mkParams.EyebrowAlpha) * oldBlue);
                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        image.SetPixel(j, i, newColor);
                    }
                }
            }
            return image;
        }

        private byte avg(byte a1, byte a2, float alpha, float beta)
        {
            return (byte)((int)(alpha * a1 + beta * a2));
        }

        private void ApplyEyeLinear(Graphics g, FaceModel faceModel, MKParams mkParams)
        {
            SolidBrush eyeLineBrush = new SolidBrush(Color.FromArgb(mkParams.EyeLineAlpha, mkParams.EyeLineColor));
            Pen eyeLinePen = new Pen(eyeLineBrush, 4);
            g.DrawCurve(eyeLinePen, faceModel.LeftEyePoints);
            double leftSlope = faceModel.LeftEyeSlope;
            double rightSlope = faceModel.RightEyeSlope;
            leftSlope = leftSlope * mkParams.EyeLineSlope;
            rightSlope = rightSlope * mkParams.EyeLineSlope;
            int deltaX = (int)((faceModel.LeftEyeTopPoint.X - faceModel.LeftEyeAnglePoint.X) / 2.5);
            int deltaY = (int)(leftSlope * deltaX);
            Point leftPoint = new Point((int)faceModel.LeftEyeAnglePoint.X - deltaX, (int)faceModel.LeftEyeAnglePoint.Y + deltaY);
            g.DrawLine(eyeLinePen, faceModel.LeftEyeAnglePoint, leftPoint);

            deltaY = (int)(rightSlope * deltaX);
            Point rightPoint = new Point((int)faceModel.RightEyeAnglePoint.X + deltaX, (int)faceModel.RightEyeAnglePoint.Y + deltaY);
            g.DrawLine(eyeLinePen, faceModel.RightEyeAnglePoint, rightPoint);
            g.DrawCurve(eyeLinePen, faceModel.RightEyePoints);
        }

        private void ApplyLipStick(Graphics g, FaceModel faceModel, MKParams mkParams)
        {
            Point[] bottomLipPoints = faceModel.BottomLipPoints;
            Point[] topLipPoints = faceModel.TopLipPoints;

            Point[] lipLineTop = faceModel.LipLineTop;
            Point[] lipLineBottom = faceModel.LipLineBottom;

            //SolidBrush lipLineBrush = new SolidBrush(Color.FromArgb(mkParams.LipLineAlpha, mkParams.LipLineColor));
            //Pen lipLinePen = new Pen(lipLineBrush, 1);
            //g.DrawClosedCurve(lipLinePen, lipLineTop);
            //g.DrawClosedCurve(lipLinePen, lipLineBottom);
            SolidBrush lipStickBrush = new SolidBrush(Color.FromArgb(mkParams.LipStickAlpha, mkParams.LipStickColor));
            g.FillClosedCurve(lipStickBrush, faceModel.TopLipPoints);
            g.FillClosedCurve(lipStickBrush, faceModel.BottomLipPoints);
        }

        private void loadColors(int code, FlowLayoutPanel panel)
        {
            panel.Controls.Clear();
            Color[] array = null;
            switch (code)
            {
                case LIP_STICK_COMMAND:
                    array = StaticParams.LipColors;
                    break;
                case LIP_LINE_COMMAND:
                    array = StaticParams.LipLineColors;
                    break;
                case EYE_COMMAND:
                    array = StaticParams.EyeColors;
                    break;
                case EYEBROW_COMMAND:
                    array = StaticParams.EyeBrowColors;
                    break;
                case EYE_LINE_COMMAND:
                    array = StaticParams.EyeLineColors;
                    break;
                case SKIN_COMMAND:
                    array = StaticParams.SkinColors;
                    break;
                case HAIR_COMMAND:
                    array = StaticParams.HairColors;
                    break;
            }
            MetroTile disable = new MetroTile();
            disable.Width = 16;
            disable.Height = 16;
            disable.Text = "";
            disable.TileImage = Properties.Resources.disabled;
            disable.UseCustomBackColor = true;
            disable.Tag = code;
            disable.Click += colorItem_Click;
            panel.Controls.Add(disable);
            foreach (Color color in array)
            {
                MetroTile mt = new MetroTile();
                mt.BackColor = color;
                mt.Width = 16;
                mt.Height = 16;
                mt.Text = "";
                mt.UseCustomBackColor = true;
                mt.Tag = code;
                mt.Click += colorItem_Click;
                panel.Controls.Add(mt);
            }
        }
        private void disable_click(object sender, EventArgs e)
        {
            MetroTile mt = (MetroTile)sender;
            int code = (int)mt.Tag;
            switch (code)
            {
                case LIP_STICK_COMMAND:
                    mkParams.LipEnabled = false;
                    break;
                case LIP_LINE_COMMAND:
                    mkParams.LipEnabled = false;
                    break;
                case EYE_COMMAND:
                    mkParams.EyeEnabled = false;
                    break;
                case EYEBROW_COMMAND:
                    mkParams.EyebrowEnabled = false;
                    break;
                case EYE_LINE_COMMAND:
                    mkParams.EyeLineEnabled = false;
                    break;
                case SKIN_COMMAND:
                    mkParams.SkinEnabled = false;
                    break;
                case HAIR_COMMAND:
                    mkParams.HairEnabled = false;
                    break;
            }
            Remakup();
        }

        private void colorItem_Click(object sender, EventArgs e)
        {
            MetroTile mt = (MetroTile)sender;
            Color color = mt.BackColor;
            int code = (int)mt.Tag;
            switch (code)
            {
                case LIP_STICK_COMMAND:
                    mkParams.LipStickColor = color;
                    mkParams.LipEnabled = true;
                    break;
                case LIP_LINE_COMMAND:
                    mkParams.LipLineColor = color;
                    mkParams.LipEnabled = true;
                    break;
                case EYE_COMMAND:
                    mkParams.EyeMaskColor = color;
                    mkParams.EyeEnabled = true;
                    break;
                case EYEBROW_COMMAND:
                    mkParams.EyebrowMaskColor = color;
                    mkParams.EyebrowEnabled = true;
                    break;
                case EYE_LINE_COMMAND:
                    mkParams.EyeLineColor = color;
                    mkParams.EyeLineEnabled = true;
                    break;
                case SKIN_COMMAND:
                    mkParams.SkinMaskColor = color;
                    mkParams.SkinEnabled = true;
                    break;
                case HAIR_COMMAND:
                    mkParams.HairMaskColor = color;
                    mkParams.HairEnabled = true;
                    break;
            }
            Remakup();
        }





        private void slidingTile1_MouseEnter(object sender, EventArgs e)
        {
        }

        private void slidingTile1_MouseHover(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //slidingTile1.transitionPictures();
            //136BAB
        }

        private void metroTile1_MouseHover(object sender, EventArgs e)
        {
            mouseIn(sender);
        }

        private void metroTile1_MouseLeave(object sender, EventArgs e)
        {
            mouseOut(sender);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mouseIn(object sender)
        {
            MetroTile mt = (MetroTile)sender;
            Transition t = new Transition(new TransitionType_Linear(10));
            t.add(mt, "BackColor", Color.LightSlateGray);
            t.run();
        }
        private void mouseOut(object sender)
        {
            MetroTile mt = (MetroTile)sender;
            Transition t = new Transition(new TransitionType_Linear(1000));
            t.add(mt, "BackColor", Color.DeepSkyBlue);
            t.run();
        }

        private void metroTile2_MouseHover(object sender, EventArgs e)
        {
            mouseIn(sender);
        }

        private void metroTile2_MouseLeave(object sender, EventArgs e)
        {
            mouseOut(sender);
        }

        private void metroTile3_MouseHover(object sender, EventArgs e)
        {
            mouseIn(sender);
        }

        private void metroTile3_MouseLeave(object sender, EventArgs e)
        {
            mouseOut(sender);
        }

        private void metroTile4_MouseHover(object sender, EventArgs e)
        {
            mouseIn(sender);
        }

        private void metroTile4_MouseLeave(object sender, EventArgs e)
        {
            mouseOut(sender);
        }

        private void metroTile5_MouseHover(object sender, EventArgs e)
        {
            mouseIn(sender);
        }

        private void metroTile5_MouseLeave(object sender, EventArgs e)
        {
            mouseOut(sender);
        }

        private void metroTile6_MouseHover(object sender, EventArgs e)
        {
            mouseIn(sender);
        }

        private void metroTile6_MouseLeave(object sender, EventArgs e)
        {
            mouseOut(sender);
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            colorBox1.Text = "رژ لب";
            colorBox2.Text = "خط لب";
            colorBox1.Visible = true;
            effectBar.Visible = true;
            colorBox2.Visible = true;
            loadColors(LIP_STICK_COMMAND, colorPanel);
            loadColors(LIP_LINE_COMMAND, colorPanel2);
            currentCommand = LIP_STICK_COMMAND;
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            colorBox1.Text = "لنز چشم";
            colorBox1.Visible = true;
            effectBar.Visible = true;
            colorBox2.Visible = false;
            loadColors(EYE_COMMAND, colorPanel);
            currentCommand = EYE_COMMAND;
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            colorBox1.Text = "مداد ابرو";
            colorBox1.Visible = true;
            effectBar.Visible = true;
            colorBox2.Visible = false;
            loadColors(EYEBROW_COMMAND, colorPanel);
            currentCommand = EYEBROW_COMMAND;
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            colorBox1.Text = "خط چشم";
            colorBox1.Visible = true;
            effectBar.Visible = true;
            colorBox2.Visible = false;
            loadColors(EYE_LINE_COMMAND, colorPanel);
            currentCommand = EYE_LINE_COMMAND;
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            colorBox1.Text = "رنگ پوست";
            colorBox1.Visible = true;
            effectBar.Visible = true;
            colorBox2.Visible = false;
            loadColors(SKIN_COMMAND, colorPanel);
            currentCommand = SKIN_COMMAND;
        }

        private void metroTile6_Click(object sender, EventArgs e)
        {
            colorBox1.Text = "رنگ مو";
            colorBox1.Visible = true;
            effectBar.Visible = true;
            colorBox2.Visible = false;
            loadColors(HAIR_COMMAND, colorPanel);
            currentCommand = HAIR_COMMAND;
        }


        private void Remakup()
        {
            image = mainColorImage.Clone();
            grayImage = mainGrayImage.Clone();
            Makeup(image, grayImage, faceModel, mkParams);
            pictureBox1.Image = image.Bitmap;
        }

        private void effectBar_MouseUp(object sender, MouseEventArgs e)
        {
            switch (currentCommand)
            {
                case LIP_STICK_COMMAND:
                    mkParams.LipStickAlpha = (int)GetValueInRange(StaticParams.MaxLipAlpha, effectBar.Value);
                    mkParams.LipLineAlpha = (int)GetValueInRange(StaticParams.MaxLipAlpha, effectBar.Value);
                    mkParams.LipEnabled = true;
                    break;
                case EYE_COMMAND:
                    mkParams.EyeAlpha = (int)GetValueInRange(StaticParams.MaxEyeAlpha, effectBar.Value);
                    mkParams.EyeEnabled = true;
                    break;
                case EYEBROW_COMMAND:
                    mkParams.EyebrowAlpha = GetValueInRange(StaticParams.MaxEyebrowAlpha, effectBar.Value);
                    mkParams.EyebrowEnabled = true;
                    break;
                case EYE_LINE_COMMAND:
                    mkParams.EyeLineAlpha = (int)GetValueInRange(StaticParams.MaxEyelineAlpha, effectBar.Value);
                    mkParams.EyeLineEnabled = true;
                    break;
                case SKIN_COMMAND:
                    mkParams.FaceAlpha = GetValueInRange(StaticParams.MaxSkinAlpha, effectBar.Value);
                    mkParams.SkinEnabled = true;
                    break;
                case HAIR_COMMAND:
                    mkParams.HairAlpha = GetValueInRange(StaticParams.MaxHairAlpha, effectBar.Value);
                    mkParams.HairEnabled = true;
                    break;
            }
            Remakup();
        }
        private double GetValueInRange(double max, int value)
        {
            return (value / 100.0) * max;
        }
    }
}

