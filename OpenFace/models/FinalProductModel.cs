using System.Collections.Generic;

namespace OpenFace.models
{
    public class FinalProductModel
    {
        string color;
        string title;
        string url;
        string colorCode;
        string type;
        string tool;
        string brandFarsi;
        string brandEnglish;
        string imageId;
        int price;
        int id;
        List<int> features=new List<int>();

        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
            }
        }

        public string ColorCode
        {
            get
            {
                return colorCode;
            }

            set
            {
                colorCode = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Tool
        {
            get
            {
                return tool;
            }

            set
            {
                tool = value;
            }
        }

        public string BrandFarsi
        {
            get
            {
                return brandFarsi;
            }

            set
            {
                brandFarsi = value;
            }
        }

        public string BrandEnglish
        {
            get
            {
                return brandEnglish;
            }

            set
            {
                brandEnglish = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string ImageId
        {
            get
            {
                return imageId;
            }

            set
            {
                imageId = value;
            }
        }

        public int Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public List<int> Features
        {
            get
            {
                return features;
            }

            set
            {
                features = value;
            }
        }
    }
}
