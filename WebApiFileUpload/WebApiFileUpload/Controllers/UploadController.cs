using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiFileUpload.Tools;
using WebApiFileUpload.UploadProvider;

namespace WebApiFileUpload.Controllers
{
    public class UploadController : ApiController {
        private IList<string> _imageTypes;

        public UploadController() {
            _imageTypes = new List<string> {
                "image/jpeg",
                "image/png",
                "image/bmp",
                "image/gif"
            };
        }

        [HttpPost]
        public async Task<Dictionary<string,string>> Files() {
            if (!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var dic = new Dictionary<string, string>();
            var root = System.Web.Hosting.HostingEnvironment.MapPath("/Resource");
            // ReSharper disable once AssignNullToNotNullAttribute
            Directory.CreateDirectory(root);
            var provider = new RenameMultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.FileData) {
                dic.Add(file.Headers.ContentDisposition.Name, file.LocalFileName);
                Debug.WriteLine(file.Headers.ContentDisposition.Name + "|" + file.LocalFileName);
            }
            return dic;
        }

        public async Task<Dictionary<string, string>> Images() {
            if (!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var dic = new Dictionary<string, string>();
            var root = System.Web.Hosting.HostingEnvironment.MapPath("/Resource");
            // ReSharper disable once AssignNullToNotNullAttribute
            Directory.CreateDirectory(root);
            var provider = new RenameMultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);
            int widht = 300;
            int height = 200;
            foreach (var file in provider.FileData) {
                var isImage =_imageTypes.Any(imageType =>string.Equals(imageType, file.Headers.ContentType.MediaType,StringComparison.OrdinalIgnoreCase));
                if (isImage) {
                    var filename = file.LocalFileName.Substring(file.LocalFileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                    using (var image = Image.FromFile(file.LocalFileName)) {
                        ThumbnailImage thumbnailImage = new CustomThumbnailImage(widht, height, image);
                        thumbnailImage.Save(root + "/thumb_" + filename);
                    }
                }

                dic.Add(file.Headers.ContentDisposition.Name, file.LocalFileName);
                Debug.WriteLine(file.Headers.ContentDisposition.Name + "|" + file.LocalFileName);
            }
            return dic;
        }
    }
}
