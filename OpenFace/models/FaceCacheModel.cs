using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFace.models
{
    public class FaceCacheModel
    {
        FaceModel model;
        Image<Bgr, byte> image;
        Image<Gray, byte> grayImage;
        Bitmap result;

        public Image<Bgr, byte> Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        public Image<Gray, byte> GrayImage
        {
            get
            {
                return grayImage;
            }

            set
            {
                grayImage = value;
            }
        }

        internal FaceModel Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        public Bitmap Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }
    }
}
