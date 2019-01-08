using OpenFace.persistence;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace OpenFace.controller
{
    public class FaceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetFace(string clientIp)
        {
            if (String.IsNullOrEmpty(clientIp))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream memoryStream = new MemoryStream();
            FaceMap.GetResult(clientIp).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            response.Content = new StreamContent(memoryStream);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "face.jpg";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return response;
        }
    }
}
