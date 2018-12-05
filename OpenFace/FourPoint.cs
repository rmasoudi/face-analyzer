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
