using Emgu.CV;
using Emgu.CV.Util;
using Emgu.Util;
using System;
using System.Drawing;

namespace OpenFace
{
    public class HistUtil
    {
        public static VectorOfMat[] GenerateHistograms(IImage image, int numberOfBins)
        {
            Mat[] channels = new Mat[image.NumberOfChannels];
            Type imageType;
            if ((imageType = Toolbox.GetBaseType(image.GetType(), "Image`2")) != null
               || (imageType = Toolbox.GetBaseType(image.GetType(), "Mat")) != null
               || (imageType = Toolbox.GetBaseType(image.GetType(), "UMat")) != null)
            {
                for (int i = 0; i < image.NumberOfChannels; i++)
                {
                    Mat channel = new Mat();
                    CvInvoke.ExtractChannel(image, channel, i);
                    channels[i] = channel;
                }

            }
            else if ((imageType = Toolbox.GetBaseType(image.GetType(), "CudaImage`2")) != null)
            {
                IImage img = imageType.GetMethod("ToImage").Invoke(image, null) as IImage;
                for (int i = 0; i < img.NumberOfChannels; i++)
                {
                    Mat channel = new Mat();
                    CvInvoke.ExtractChannel(img, channel, i);
                    channels[i] = channel;
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The input image type of {0} is not supported", image.GetType().ToString()));
            }

            Type[] genericArguments = imageType.GetGenericArguments();
            String[] channelNames;
            Color[] colors;
            Type typeOfDepth;
            if (genericArguments.Length > 0)
            {
                IColor typeOfColor = Activator.CreateInstance(genericArguments[0]) as IColor;
                channelNames = Emgu.CV.Reflection.ReflectColorType.GetNamesOfChannels(typeOfColor);
                colors = Emgu.CV.Reflection.ReflectColorType.GetDisplayColorOfChannels(typeOfColor);
                typeOfDepth = imageType.GetGenericArguments()[1];
            }
            else
            {
                channelNames = new String[image.NumberOfChannels];
                colors = new Color[image.NumberOfChannels];
                for (int i = 0; i < image.NumberOfChannels; i++)
                {
                    channelNames[i] = String.Format("Channel {0}", i);
                    colors[i] = Color.Red;
                }

                if (image is Mat)
                {
                    typeOfDepth = CvInvoke.GetDepthType(((Mat)image).Depth);
                }
                else if (image is UMat)
                {
                    typeOfDepth = CvInvoke.GetDepthType(((UMat)image).Depth);
                }
                else
                {
                    throw new ArgumentException(String.Format("Unable to get the type of depth from image of type {0}", image.GetType().ToString()));
                }

            }

            float minVal, maxVal;
            #region Get the maximum and minimum color intensity values

            if (typeOfDepth == typeof(Byte))
            {
                minVal = 0.0f;
                maxVal = 256.0f;
            }
            else
            {
                #region obtain the maximum and minimum color value
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                double min = minValues[0], max = maxValues[0];
                for (int i = 1; i < minValues.Length; i++)
                {
                    if (minValues[i] < min) min = minValues[i];
                    if (maxValues[i] > max) max = maxValues[i];
                }
                #endregion

                minVal = (float)min;
                maxVal = (float)max;
            }
            #endregion
            VectorOfMat[] result = new VectorOfMat[channels.Length];
            for (int i = 0; i < channels.Length; i++)
            {

                //using (DenseHistogram hist = new DenseHistogram(numberOfBins, new RangeF(minVal, maxVal)))
                using (Mat hist = new Mat())
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(channels[i]);

                    float[] ranges = new float[] { minVal, maxVal };
                    CvInvoke.CalcHist(vm, new int[] { 0 }, null, hist, new int[] { numberOfBins }, ranges, false);
                    result[i] = vm;
                    //hist.Calculate(new IImage[1] { channels[i] }, true, null);
                }
            }
            return result;
        }
    }
}
