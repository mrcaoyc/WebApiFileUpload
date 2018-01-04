using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApiFileUpload.Tools {
    public class ThumbnailImage {

        /// <summary>
        /// 缩放方式
        /// </summary>
        public ThumbnailImageOption ThumbnailImageOption { get; set; }
        /// <summary>
        /// 缩率图宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 缩率图高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 原图
        /// </summary>
        public Image SourceImage { get; set; }

        /// <summary>
        /// 原图宽度
        /// </summary>
        public int SourceImageWidth => SourceImage?.Width ?? 0;

        /// <summary>
        /// 原图高度
        /// </summary>
        public int SourceImageHeight => SourceImage?.Height ?? 0;

        /// <summary>
        /// 缩小比例
        /// </summary>
        protected decimal ScaleRate => SourceImageHeight * 1.0M / Height > SourceImageWidth * 1.0M / Width
            ? SourceImageHeight * 1.0M / Height
            : SourceImageWidth * 1.0M / Width;

        /// <summary>
        /// 缩率图真实宽度
        /// </summary>
        protected int RealWidth => ThumbnailImageOption == ThumbnailImageOption.Stretch ? Width : Convert.ToInt32(SourceImageWidth / ScaleRate);

        /// <summary>
        /// 缩率图真实高度
        /// </summary>
        protected int RealHeight => ThumbnailImageOption == ThumbnailImageOption.Stretch ? Height : Convert.ToInt32(SourceImageHeight / ScaleRate);

        public ThumbnailImage() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height">缩率图高度</param>
        public ThumbnailImage(int width, int height) {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">缩率图宽度</param>
        /// <param name="height">缩率图高度</param>
        /// <param name="sourceImage">原图</param>
        public ThumbnailImage(int width, int height, Image sourceImage) {
            Width = width;
            Height = height;
            SourceImage = sourceImage;
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <returns></returns>
        public Image GetThumbnailImage() {
            CheckThumbnailParameters();
            if (!IsNeedCompress()) {
                return SourceImage;
            }
            try {
                return CreateThumbnailImage();
            }
            catch (Exception) {
                throw new Exception("转换失败，请重试");
            }
        }

        public void Save(string filename) {
            using (var image = GetThumbnailImage()) {
                image.Save(filename);
            }
        }

        /// <summary>
        /// 创建缩率图过程，父类使用的Image的GetThumbnailImage()方法创建的。
        /// 如有必须要子类需要自行重写该方法。
        /// </summary>
        /// <returns></returns>
        protected virtual Image CreateThumbnailImage() {
            Image.GetThumbnailImageAbort callback = () => false;
            //调用Image对象自带的GetThumbnailImage()方法生成缩略图
            return SourceImage.GetThumbnailImage(RealWidth, RealHeight, callback, IntPtr.Zero);
        }

        /// <summary>
        /// 检查是否需要压缩图片
        /// </summary>
        /// <returns></returns>
        private bool IsNeedCompress() {
            //除非原图片的宽度和高度都不目标图片的宽度和高度都大才不压缩，否则压缩
            if (SourceImageWidth >= Width && SourceImageHeight >= Height) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        private void CheckThumbnailParameters() {
            if (Width <= 0) throw new ArgumentOutOfRangeException(nameof(Width));
            if (Height <= 0) throw new ArgumentOutOfRangeException(nameof(Height));
            if (SourceImage == null) throw new ArgumentNullException(nameof(SourceImage));
        }
    }

    public enum ThumbnailImageOption {
        /// <summary>
        /// 等比例缩放
        /// </summary>
        Scaling,
        /// <summary>
        /// 拉伸
        /// </summary>
        Stretch

    }
}