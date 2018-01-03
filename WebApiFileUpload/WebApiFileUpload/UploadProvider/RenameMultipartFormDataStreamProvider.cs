using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace WebApiFileUpload.UploadProvider {
    public class RenameMultipartFormDataStreamProvider: MultipartFormDataStreamProvider {
        public RenameMultipartFormDataStreamProvider(string rootPath) : base(rootPath) {}
        public RenameMultipartFormDataStreamProvider(string rootPath, int bufferSize) : base(rootPath, bufferSize) {}

        public override string GetLocalFileName(HttpContentHeaders headers) {
            string fileName = headers.ContentDisposition.FileName.Trim('"');
            string fileExt = fileName.Substring(fileName.LastIndexOf('.'));
            return Guid.NewGuid() + fileExt;
        }
    }
}