using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFace.models
{
    public class BrandModel
    {
        string englishName;
        string farsiName;
        List<ProductModel> products = new List<ProductModel>();

        public string EnglishName
        {
            get
            {
                return englishName;
            }

            set
            {
                englishName = value;
            }
        }

        public string FarsiName
        {
            get
            {
                return farsiName;
            }

            set
            {
                farsiName = value;
            }
        }

        public List<ProductModel> Products
        {
            get
            {
                return products;
            }

            set
            {
                products = value;
            }
        }
    }
}
