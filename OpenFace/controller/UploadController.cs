using Emgu.CV;
using Emgu.CV.Structure;
using OpenFace.models;
using OpenFace.persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Forms;

namespace OpenFace.controller
{
    public class UploadController : ApiController
    {
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

        [HttpPost]
        public Task<HttpResponseMessage> Post([FromUri]string filename)
        {
            Guid uploadedFile = Guid.NewGuid();
            Task<HttpResponseMessage> task = Request.Content.ReadAsStreamAsync().ContinueWith<HttpResponseMessage>(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);

                try
                {
                    using (Stream stream = t.Result)
                    {
                        Image bitmap = Bitmap.FromStream(stream);
                    }
                }
                catch (Exception e)
                {
                    Object o = e;
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.GetBaseException().Message);
                }

                return Request.CreateResponse(HttpStatusCode.Created, uploadedFile.ToString());
            });
            return task;
        }

        [HttpPost, Route("api/upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                Bitmap inputBitmap;

                using (var ms = new MemoryStream(buffer))
                {
                    inputBitmap = new Bitmap(ms);
                    IEnumerable<string> headerValues = Request.Headers.GetValues("MyCustomID");
                    var id = "aa";
                    string clientIp = Request.Headers.Referrer.Authority;
                    FaceCacheModel cacheItem = new FaceCacheModel();
                    cacheItem.Image = new Image<Bgr, byte>(inputBitmap);
                    cacheItem.GrayImage = cacheItem.Image.Convert<Gray, byte>();
                    try
                    {
                        cacheItem.Model = Labrator.GetFaceModel(cacheItem.Image, cacheItem.GrayImage);
                        FaceMap.Add(clientIp, cacheItem);
                        Bitmap result = Labrator.Process(cacheItem);
                        FaceMap.SetResult(clientIp, result);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("");
                    }

                }

            }
            return Ok();
        }
        public static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
