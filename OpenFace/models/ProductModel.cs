using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFace.models
{
    public class ProductModel
    {
        string imageId;
        string color;
        string title;
        string url;
        string colorCode;
        string type;
        Dictionary<string, string> properties = new Dictionary<string, string>();

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

        public Dictionary<string, string> Properties
        {
            get
            {
                return properties;
            }

            set
            {
                properties = value;
            }
        }
    }
}
