﻿using System.Drawing;

namespace OpenFace
{
    class FaceModel
    {
        Point[] mouthPoints;
        Rectangle mouthBox;
        Point[] nosePoints;
        Rectangle noseBox;
        Point[] leftEyebrowPoints;
        Point[] rightEyebrowPoints;
        Rectangle leftEyebrowBox;
        Rectangle rightEyebrowBox;
        Point[] leftEyePoints;
        Rectangle leftEyeBox;
        Point[] rightEyePoints;
        Rectangle rightEyeBox;
        Rectangle headBox;
        Rectangle[] landmarkRects;
        Point[] bottomLipPoints;
        Point[] topLipPoints;
        Point[] lipLineTop;
        Point[] lipLineBottom;
        Rectangle faceBox;
        double leftEyeSlope;
        double rightEyeSlope;
        Point leftEyeAnglePoint;
        Point leftEyeTopPoint;
        Point rightEyeAnglePoint;
        public FaceModel(PointF[] points, Rectangle faceRect)
        {
            MouthPoints = GetMouthPoints(points);
            MouthBox = getBoundingBox(MouthPoints);
            NosePoints = GetNosePoints(points);
            NoseBox = getBoundingBox(NosePoints);
            LeftEyebrowPoints = GetLeftEyebrowPoints(points);
            RightEyebrowPoints = GetRightEyebrowPoints(points);
            LeftEyebrowBox = getBoundingBox(LeftEyebrowPoints);
            RightEyebrowBox = getBoundingBox(RightEyebrowPoints);
            LeftEyePoints = GetLeftEyePoints(points);
            LeftEyeBox = getBoundingBox(LeftEyePoints);
            RightEyePoints = GetRightEyePoints(points);
            RightEyeBox = getBoundingBox(RightEyePoints);
            TopLipPoints = GetTopLipPoints(points);
            BottomLipPoints = GetBottomLipPoints(points);
            HeadBox = new Rectangle(LeftEyebrowBox.Left, faceRect.Y, RightEyebrowBox.Right - LeftEyebrowBox.Left, System.Math.Min(LeftEyebrowBox.Top, RightEyebrowBox.Top) - faceRect.Top);
            LandmarkRects = new Rectangle[6];
            LandmarkRects[0] = LeftEyebrowBox;
            LandmarkRects[1] = RightEyebrowBox;
            LandmarkRects[2] = LeftEyeBox;
            LandmarkRects[3] = RightEyeBox;
            LandmarkRects[4] = NoseBox;
            LandmarkRects[5] = MouthBox;
            LeftEyeSlope = (double)(points[36].Y - points[37].Y) / (double)(points[37].X - points[36].X);
            RightEyeSlope = (double)(points[45].Y - points[44].Y) / (double)(points[45].X - points[44].X);
            LeftEyeAnglePoint = new Point((int)points[36].X, (int)points[36].Y);
            LeftEyeTopPoint = new Point((int)points[37].X, (int)points[37].Y);
            RightEyeAnglePoint = new Point((int)points[45].X, (int)points[45].Y);
        }

        public Point[] MouthPoints
        {
            get
            {
                return mouthPoints;
            }

            set
            {
                mouthPoints = value;
            }
        }

        public Rectangle MouthBox
        {
            get
            {
                return mouthBox;
            }

            set
            {
                mouthBox = value;
            }
        }

        public Point[] NosePoints
        {
            get
            {
                return nosePoints;
            }

            set
            {
                nosePoints = value;
            }
        }

        public Rectangle NoseBox
        {
            get
            {
                return noseBox;
            }

            set
            {
                noseBox = value;
            }
        }

        public Point[] LeftEyebrowPoints
        {
            get
            {
                return leftEyebrowPoints;
            }

            set
            {
                leftEyebrowPoints = value;
            }
        }

        public Point[] RightEyebrowPoints
        {
            get
            {
                return rightEyebrowPoints;
            }

            set
            {
                rightEyebrowPoints = value;
            }
        }

        public Rectangle LeftEyebrowBox
        {
            get
            {
                return leftEyebrowBox;
            }

            set
            {
                leftEyebrowBox = value;
            }
        }

        public Rectangle RightEyebrowBox
        {
            get
            {
                return rightEyebrowBox;
            }

            set
            {
                rightEyebrowBox = value;
            }
        }

        public Point[] LeftEyePoints
        {
            get
            {
                return leftEyePoints;
            }

            set
            {
                leftEyePoints = value;
            }
        }

        public Rectangle LeftEyeBox
        {
            get
            {
                return leftEyeBox;
            }

            set
            {
                leftEyeBox = value;
            }
        }

        public Point[] RightEyePoints
        {
            get
            {
                return rightEyePoints;
            }

            set
            {
                rightEyePoints = value;
            }
        }

        public Rectangle RightEyeBox
        {
            get
            {
                return rightEyeBox;
            }

            set
            {
                rightEyeBox = value;
            }
        }

        public Rectangle HeadBox
        {
            get
            {
                return headBox;
            }

            set
            {
                headBox = value;
            }
        }

        public Rectangle[] LandmarkRects
        {
            get
            {
                return landmarkRects;
            }

            set
            {
                landmarkRects = value;
            }
        }

        public Point[] BottomLipPoints
        {
            get
            {
                return bottomLipPoints;
            }

            set
            {
                bottomLipPoints = value;
            }
        }

        public Point[] TopLipPoints
        {
            get
            {
                return topLipPoints;
            }

            set
            {
                topLipPoints = value;
            }
        }

        public Point[] LipLineTop
        {
            get
            {
                return lipLineTop;
            }

            set
            {
                lipLineTop = value;
            }
        }

        public Point[] LipLineBottom
        {
            get
            {
                return lipLineBottom;
            }

            set
            {
                lipLineBottom = value;
            }
        }

        public Rectangle FaceBox
        {
            get
            {
                return faceBox;
            }

            set
            {
                faceBox = value;
            }
        }

        public double LeftEyeSlope
        {
            get
            {
                return leftEyeSlope;
            }

            set
            {
                leftEyeSlope = value;
            }
        }

        public double RightEyeSlope
        {
            get
            {
                return rightEyeSlope;
            }

            set
            {
                rightEyeSlope = value;
            }
        }

        public Point LeftEyeAnglePoint
        {
            get
            {
                return leftEyeAnglePoint;
            }

            set
            {
                leftEyeAnglePoint = value;
            }
        }

        public Point RightEyeAnglePoint
        {
            get
            {
                return rightEyeAnglePoint;
            }

            set
            {
                rightEyeAnglePoint = value;
            }
        }

        public Point LeftEyeTopPoint
        {
            get
            {
                return leftEyeTopPoint;
            }

            set
            {
                leftEyeTopPoint = value;
            }
        }

        private Point[] GetLeftEyebrowPoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[5];
            topLipPoints[0] = new Point((int)points[17].X, (int)points[17].Y);
            topLipPoints[1] = new Point((int)points[18].X, (int)points[18].Y);
            topLipPoints[2] = new Point((int)points[19].X, (int)points[19].Y);
            topLipPoints[3] = new Point((int)points[20].X, (int)points[20].Y);
            topLipPoints[4] = new Point((int)points[21].X, (int)points[21].Y);
            return topLipPoints;
        }

        private Point[] GetRightEyebrowPoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[5];
            topLipPoints[0] = new Point((int)points[22].X, (int)points[22].Y);
            topLipPoints[1] = new Point((int)points[23].X, (int)points[23].Y);
            topLipPoints[2] = new Point((int)points[24].X, (int)points[24].Y);
            topLipPoints[3] = new Point((int)points[25].X, (int)points[25].Y);
            topLipPoints[4] = new Point((int)points[26].X, (int)points[26].Y);
            return topLipPoints;
        }

        private Point[] GetLeftEyePoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[4];
            topLipPoints[0] = new Point((int)points[36].X, (int)points[36].Y);
            topLipPoints[1] = new Point((int)points[37].X, (int)points[37].Y);
            topLipPoints[2] = new Point((int)points[38].X, (int)points[38].Y);
            topLipPoints[3] = new Point((int)points[39].X, (int)points[39].Y);
            return topLipPoints;
        }
        private Point[] GetRightEyePoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[4];
            topLipPoints[0] = new Point((int)points[42].X, (int)points[42].Y);
            topLipPoints[1] = new Point((int)points[43].X, (int)points[43].Y);
            topLipPoints[2] = new Point((int)points[44].X, (int)points[44].Y);
            topLipPoints[3] = new Point((int)points[45].X, (int)points[45].Y);
            return topLipPoints;
        }
        private Point[] GetLipLinearTop(PointF[] points)
        {

            Point[] topLipPoints = new Point[7];
            topLipPoints[0] = new Point((int)points[48].X, (int)points[48].Y);
            topLipPoints[1] = new Point((int)points[49].X, (int)points[49].Y);
            topLipPoints[2] = new Point((int)points[50].X, (int)points[50].Y);
            topLipPoints[3] = new Point((int)points[51].X, (int)points[51].Y);
            topLipPoints[4] = new Point((int)points[52].X, (int)points[52].Y);
            topLipPoints[5] = new Point((int)points[53].X, (int)points[53].Y);
            topLipPoints[6] = new Point((int)points[54].X, (int)points[54].Y);
            return topLipPoints;
        }


        private Point[] GetLipLinearBottom(PointF[] points)
        {
            Point[] topLipPoints = new Point[7];
            topLipPoints[0] = new Point((int)points[54].X, (int)points[54].Y);
            topLipPoints[1] = new Point((int)points[55].X, (int)points[55].Y);
            topLipPoints[2] = new Point((int)points[56].X, (int)points[56].Y);
            topLipPoints[3] = new Point((int)points[57].X, (int)points[57].Y);
            topLipPoints[4] = new Point((int)points[58].X, (int)points[58].Y);
            topLipPoints[5] = new Point((int)points[59].X, (int)points[59].Y);
            topLipPoints[6] = new Point((int)points[60].X, (int)points[60].Y);
            return topLipPoints;
        }

        private Point[] GetNosePoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[9];
            for (int i = 0; i < 9; i++)
            {
                topLipPoints[i] = new Point((int)points[i + 27].X, (int)points[i + 27].Y);
            }
            return topLipPoints;
        }
        private Point[] GetMouthPoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[20];
            for (int i = 0; i < 20; i++)
            {
                topLipPoints[i] = new Point((int)points[i + 48].X, (int)points[i + 48].Y);
            }
            return topLipPoints;
        }

        private Point[] GetBottomLipPoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[10];
            topLipPoints[0] = new Point((int)points[65].X, (int)points[65].Y);
            topLipPoints[1] = new Point((int)points[66].X, (int)points[66].Y);
            topLipPoints[2] = new Point((int)points[67].X, (int)points[67].Y);
            topLipPoints[3] = new Point((int)points[54].X, (int)points[54].Y);
            topLipPoints[4] = new Point((int)points[55].X, (int)points[55].Y);
            topLipPoints[5] = new Point((int)points[56].X, (int)points[56].Y);
            topLipPoints[6] = new Point((int)points[57].X, (int)points[57].Y);
            topLipPoints[7] = new Point((int)points[58].X, (int)points[58].Y);
            topLipPoints[8] = new Point((int)points[59].X, (int)points[59].Y);
            topLipPoints[9] = new Point((int)points[60].X, (int)points[60].Y);
            return topLipPoints;
        }

        private Point[] GetTopLipPoints(PointF[] points)
        {
            Point[] topLipPoints = new Point[12];
            topLipPoints[0] = new Point((int)points[60].X, (int)points[60].Y);
            topLipPoints[1] = new Point((int)points[61].X, (int)points[61].Y);
            topLipPoints[2] = new Point((int)points[62].X, (int)points[62].Y);
            topLipPoints[3] = new Point((int)points[63].X, (int)points[63].Y);
            topLipPoints[4] = new Point((int)points[64].X, (int)points[64].Y);
            topLipPoints[5] = new Point((int)points[48].X, (int)points[48].Y);
            topLipPoints[6] = new Point((int)points[49].X, (int)points[49].Y);
            topLipPoints[7] = new Point((int)points[50].X, (int)points[50].Y);
            topLipPoints[8] = new Point((int)points[51].X, (int)points[51].Y);
            topLipPoints[9] = new Point((int)points[52].X, (int)points[52].Y);
            topLipPoints[10] = new Point((int)points[53].X, (int)points[53].Y);
            topLipPoints[11] = new Point((int)points[54].X, (int)points[54].Y);
            return topLipPoints;
        }
        public Rectangle getBoundingBox(Point[] arr)
        {
            int minX = 100000;
            int minY = 100000;
            int maxX = 0;
            int maxY = 0;
            foreach (Point point in arr)
            {
                if (point.X < minX)
                {
                    minX = point.X;
                }
                if (point.Y < minY)
                {
                    minY = point.Y;
                }

                if (point.X > maxX)
                {
                    maxX = point.X;
                }
                if (point.Y > maxY)
                {
                    maxY = point.Y;
                }
            }

            return new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
        }
    }
}
