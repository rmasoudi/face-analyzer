using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace OpenFace
{
    public class FourPoint
    {
        Point m1;
        Point n1;
        Point m2;
        Point n2;

        public Point[] GetArray()
        {
            Point[] array = new Point[4];
            array[0] = m1;
            array[1] = n1;
            array[2] = n2;
            array[3] = m2;
            return array;
        }

        public Rectangle GetBoundingBox()
        {
            return FaceModel.getBoundingBox(GetArray());
        }

        public void GetColorRange(Image<Bgr, byte> input,out Bgr min,out Bgr max) {
            int x1 = Math.Max(m1.X, m2.X);
            int y1 = Math.Max(m1.Y, n1.Y);
            int x2 = Math.Min(n1.X, n2.X);
            int y2 = Math.Min(m2.Y, n2.Y);
            int width = x2 - x1;
            int height = y2 - y1;

            Rectangle roi = new Rectangle(Math.Min(x1,x2),Math.Min(y1,y2),Math.Abs(width),Math.Abs(height));
            input.ROI = roi;
            double[] minValues;
            double[] maxValues;
            Point[] minLocs;
            Point[] maxLocs;
            input.MinMax(out minValues, out maxValues, out minLocs, out maxLocs);
            input.ROI = Rectangle.Empty;
            min = new Bgr(minValues[0], minValues[1], minValues[2]);
            max= new Bgr(maxValues[0], maxValues[1], maxValues[2]);
        }

        public Point M1
        {
            get
            {
                return m1;
            }

            set
            {
                m1 = value;
            }
        }

        public Point N1
        {
            get
            {
                return n1;
            }

            set
            {
                n1 = value;
            }
        }

        public Point M2
        {
            get
            {
                return m2;
            }

            set
            {
                m2 = value;
            }
        }

        public Point N2
        {
            get
            {
                return n2;
            }

            set
            {
                n2 = value;
            }
        }
    }
}
