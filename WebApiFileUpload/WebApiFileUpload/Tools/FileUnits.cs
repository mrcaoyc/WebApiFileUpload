using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiFileUpload.Tools {
    public static class FileUnits {

        /// <summary>
        /// 获取文件扩展名，包含了点
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExtendName(string fileName) {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            return fileName.Substring(fileName.LastIndexOf('.'));
        }
    }
}