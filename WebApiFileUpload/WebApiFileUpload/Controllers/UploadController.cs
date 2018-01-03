using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiFileUpload.UploadProvider;

namespace WebApiFileUpload.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        public async Task<Dictionary<string,string>> File() {
            if (!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var dic = new Dictionary<string, string>();
            var root = System.Web.Hosting.HostingEnvironment.MapPath("/Resource");
            Directory.CreateDirectory(root);
            var provider = new RenameMultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.FileData) {
                dic.Add(file.Headers.ContentDisposition.Name, file.LocalFileName);
                Debug.WriteLine(file.Headers.ContentDisposition.Name + "|" + file.LocalFileName);
            }
            return dic;
        }
    }
}
