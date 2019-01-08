using OpenFace.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenFace.controller
{
    public class ProductsController : ApiController
    {
        [HttpPost]
        public IEnumerable<FinalProductModel> GetAllProducts()
        {
            return DBUtil.GetProducts();
        }
    }
}
