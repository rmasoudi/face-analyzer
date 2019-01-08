using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenFace.models;
using System;
using System.Drawing;
using System.IO;

namespace OpenFace
{
    public class Labrator
    {
        private static CascadeClassifier faceDetector;
        private static FacemarkLBF facemark;
        public static void test()
        {
            Image<Bgr, Byte> image;
            Image<Gray, byte> grayImage;
            Image<Bgr, Byte> mainColorImage;
            Image<Gray, byte> mainGrayImage;
            FaceModel faceModel;
            MKParams mkParams = new MKParams();
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
                GetColorRange(new FourPoint[] { faceModel.RightCheek, faceModel.LeftCheek, faceModel.ChainArea
    }, image, out from, out to);
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

                con.FillConvexPoly(faceModel.TopLipPoints, new Bgr(mkParams.LipStickColor));
                con.FillConvexPoly(faceModel.BottomLipPoints, new Bgr(mkParams.LipStickColor));
                DrawPoints(faceModel.TopLipPoints, con);
                DrawPoints(faceModel.BottomLipPoints, con);

                con.Save("d:\\skin.jpg");
            }
        }
        public static Bitmap Process(FaceCacheModel cacheItem)
        {
            MKParams mkParams = new MKParams();

            Bgr from = new Bgr(), to = new Bgr();
            GetColorRange(new FourPoint[] { cacheItem.Model.RightCheek, cacheItem.Model.LeftCheek, cacheItem.Model.ChainArea }, cacheItem.Image, out from, out to);
            Image<Bgr, Double> con = cacheItem.Image.Convert<Bgr, Double>();

            cacheItem.Image.ROI = cacheItem.Model.HeadArea;
            Image<Gray, byte> headColorMask = cacheItem.Image.InRange(from, to);
            cacheItem.Image.ROI = Rectangle.Empty;


            Image<Gray, byte> skinMask = new Image<Gray, byte>(cacheItem.Image.Width, cacheItem.Image.Height, new Gray(0));
            skinMask.ROI = cacheItem.Model.HeadArea;
            headColorMask.CopyTo(skinMask);
            skinMask.ROI = Rectangle.Empty;

            CvInvoke.DrawContours(skinMask, GetVVP(cacheItem.Model.FaceBoundry), -1, new Bgr(Color.White).MCvScalar, -1, LineType.EightConnected);
            CvInvoke.DrawContours(skinMask, GetVVP(cacheItem.Model.LeftEyePoints), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
            CvInvoke.DrawContours(skinMask, GetVVP(cacheItem.Model.RightEyePoints), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
            CvInvoke.DrawContours(skinMask, GetVVP(cacheItem.Model.LipBoundry), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
            CvInvoke.DrawContours(skinMask, GetVVP(cacheItem.Model.NoseBottom), -1, new Bgr(Color.Black).MCvScalar, -1, LineType.EightConnected);
            Image<Bgr, Double> temp = new Image<Bgr, Double>(con.Width, con.Height, new Bgr(mkParams.SkinMaskColor));
            con.AccumulateWeighted(temp, mkParams.FaceAlpha, skinMask);
            con.Draw(cacheItem.Model.HeadArea, new Bgr(Color.Blue), 2);
            con.Draw(cacheItem.Model.RightCheek.GetBoundingBox(), new Bgr(Color.Red), 2);
            con.Draw(cacheItem.Model.LeftCheek.GetBoundingBox(), new Bgr(Color.Red), 2);
            return con.Bitmap;
        }
        private static void GetColorRange(FourPoint[] areas, Image<Bgr, byte> image, out Bgr from, out Bgr to)
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
        public static void InitModel()
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

        public static FaceModel GetFaceModel(Image<Bgr, Byte> image, Image<Gray, byte> grayImage)
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

        private static void DrawPoints(Point[] points, Image<Bgr, Double> con)
        {
            foreach (Point point in points)
            {
                con.Draw(new CircleF(new PointF(point.X - 2, point.Y - 2), 4), new Bgr(Color.Black), 1);
            }
        }
    }
}
