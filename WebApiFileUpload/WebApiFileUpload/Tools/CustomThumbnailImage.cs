using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace WebApiFileUpload.Tools {
    public class CustomThumbnailImage:ThumbnailImage {
        public CustomThumbnailImage() {}
        public CustomThumbnailImage(int width, int height) : base(width, height) {}
        public CustomThumbnailImage(int width, int height, Image sourceImage) : base(width, height, sourceImage) {}

        protected override Image CreateThumbnailImage() {
            int x = 0;
            int y = 0;

            //创建画布
            Bitmap bmp = new Bitmap(RealWidth, RealHeight, PixelFormat.Format24bppRgb);
            //设置分辨率
            bmp.SetResolution(SourceImage.HorizontalResolution, SourceImage.VerticalResolution);
            using (Graphics g = Graphics.FromImage(bmp)) {

                //用白色清空
                g.Clear(Color.White);

                //指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //高质量，抗锯齿呈现
                g.SmoothingMode = SmoothingMode.HighQuality;

                //在指定的位置并按指定的大小绘制指定的Image的指定部分
                g.DrawImage(SourceImage,
                    new Rectangle(x, y, RealWidth, RealHeight),
                    new Rectangle(0, 0, SourceImageWidth, SourceImageHeight),
                    GraphicsUnit.Pixel
                    );

                return bmp;
            }

        }
    }
}