using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;
using System.IO;

namespace OpenFace
{
    public class Labrator
    {
        private static Image<Bgr, Byte> image;
        private static Image<Gray, byte> grayImage;
        private static Image<Bgr, Byte> mainColorImage;
        private static Image<Gray, byte> mainGrayImage;
        private static FaceModel faceModel;
        private static CascadeClassifier faceDetector;
        private static FacemarkLBF facemark;
        private static MKParams mkParams = new MKParams();
        public static void test()
        {
            InitModel();
            DirectoryInfo d = new DirectoryInfo(@"D:\Face\Data_Collection\Data_Collection");
            FileInfo[] Files = d.GetFiles("*.jpg");
            foreach (FileInfo file in Files)
            {
                image = new Image<Bgr, byte>(file.FullName);
                mainColorImage = image.Clone();
                grayImage = image.Convert<Gray, byte>();
                mainGrayImage = grayImage.Clone();
                faceModel = GetFaceModel(image, grayImage);

                #region Face Skin
                Bgr from = new Bgr(), to = new Bgr();
                GetColorRange(new FourPoint[] { faceModel.RightCheek, faceModel.LeftCheek, faceModel.ChainArea }, out from, out to);
                Image<Bgr, Double> con = image.Convert<Bgr, Double>();

                image.ROI = faceModel.HeadArea;
                Image<Gray, byte> headColorMask = image.InRange(from, to);
                image.ROI = Rectangle.Empty;


                Image<Gray, byte> skinMask = new Image<Gray, byte>(image.Width, image.Height, new Gray(0));
                skinMask.ROI = faceModel.HeadArea;
                headColorMask.CopyTo(skinMask);
                skinMask.ROI = Rectangle.Empty;

                CvInvoke.DrawContours(skinMask, GetVVP(faceModel.FaceBoundry), -1, new Bgr(Color.White).MCvScalar, -1, LineType.EightConnected);
                CvInvoke.DrawContours(skinMask, GetVVP(faceModel.LeftEyePoints), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
                CvInvoke.DrawContours(skinMask, GetVVP(faceModel.RightEyePoints), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
                CvInvoke.DrawContours(skinMask, GetVVP(faceModel.LipBoundry), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
                CvInvoke.DrawContours(skinMask, GetVVP(faceModel.NoseBottom), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
                Image<Bgr, Double> temp = new Image<Bgr, Double>(con.Width, con.Height, new Bgr(mkParams.SkinMaskColor));
                con.AccumulateWeighted(temp, mkParams.FaceAlpha, skinMask);
                con.Draw(faceModel.HeadArea, new Bgr(Color.Blue), 2);
                con.Draw(faceModel.RightCheek.GetBoundingBox(), new Bgr(Color.Red), 2);
                con.Draw(faceModel.LeftCheek.GetBoundingBox(), new Bgr(Color.Red), 2);
                #endregion

                con.Save("d:\\skin.jpg");
            }
        }

        private static void GetColorRange(FourPoint[] areas, out Bgr from, out Bgr to)
        {
            from = new Bgr(255, 255, 255);
            to = new Bgr(0, 0, 0);
            foreach (FourPoint area in areas)
            {
                Bgr tempFrom, tempTo;
                area.GetColorRange(image, out tempFrom, out tempTo);
                from = new Bgr(Math.Min(from.Blue, tempFrom.Blue), Math.Min(from.Green, tempFrom.Green), Math.Min(from.Red, tempFrom.Red));
                to = new Bgr(Math.Max(to.Blue, tempTo.Blue), Math.Max(to.Green, tempTo.Green), Math.Max(to.Red, tempTo.Red));
            }
        }
        private static void InitModel()
        {
            faceDetector = new CascadeClassifier(Constants.FACE_DETECTOR_PATH);
            FacemarkLBFParams fParams = new FacemarkLBFParams();
            fParams.ModelFile = Constants.LANDMARK_DETECTOR_PATH;
            fParams.NLandmarks = 68; // number of landmark points
            fParams.InitShapeN = 10; // number of multiplier for make data augmentation
            fParams.StagesN = 5; // amount of refinement stages
            fParams.TreeN = 6; // number of tree in the model for each landmark point
            fParams.TreeDepth = 5; //he depth of decision tree
            facemark = new FacemarkLBF(fParams);
            facemark.LoadModel(fParams.ModelFile);
        }

        private static FaceModel GetFaceModel(Image<Bgr, Byte> image, Image<Gray, byte> grayImage)
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

        private static VectorOfVectorOfPoint GetVVP(Point[] points)
        {
            VectorOfVectorOfPoint vvp = new VectorOfVectorOfPoint();
            VectorOfPoint vp = new VectorOfPoint();
            vp.Push(points);
            vvp.Push(vp);
            return vvp;
        }
    }
}
