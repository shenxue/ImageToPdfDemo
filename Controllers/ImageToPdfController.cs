using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using PdfSharpCore.Pdf;
using System.Runtime.InteropServices.ComTypes;

namespace ImageToPdfDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageToPdfController : ControllerBase
    {
        [HttpGet("test")]
        public string ImageToPdf()
        {
            string imagePath = @"C:\Users\EDZ\Desktop\2.jpg";
            string pdfPath = @"C:\Users\EDZ\Desktop\2.pdf";
            IImageFormat format = null;
            var imagesTemle = SixLabors.ImageSharp.Image.Load(imagePath, out format);
            using (var stream = new MemoryStream())
            {
                imagesTemle.Save(stream, format);
                stream.Flush();
                BytesToPdf(stream.ToArray(), pdfPath);
            }

            //ImageToPdf(imagePath, pdfPath);
            return "";
        }

        private void ImageToPdf(string imagePath, string pdfPath)
        {
            var doc = new Document();
            var stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            using (stream)
            {
                PdfWriter.GetInstance(doc, stream);
                doc.Open();
                using (var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var image = iTextSharp.text.Image.GetInstance(imageStream);
                    if (image.Height > PageSize.A4.Height)
                    {
                        image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                    }
                    else if (image.Width > PageSize.A4.Width)
                    {
                        image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                    }
                    image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                    doc.Add(image);
                }
                doc.Close();
            }
        }


        private void BytesToPdf(byte[] bytes, string pdfPath)
        {
            var doc = new Document();
            var stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            using (stream)
            {
                PdfWriter.GetInstance(doc, stream);
                doc.Open();

                var image = iTextSharp.text.Image.GetInstance(bytes);
                if (image.Height > PageSize.A4.Height)
                {
                    image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                }
                else if (image.Width > PageSize.A4.Width)
                {
                    image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                }
                image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                doc.Add(image);

                doc.Close();
            }

        }


        private void BytesToPdf2(byte[] bytes, string pdfPath)
        {
            var doc1 = new Document();
            var stream2 = new MemoryStream(bytes, true);
            using (stream2)
            {
                PdfWriter.GetInstance(doc1, stream2);
                doc1.Open();

                var image = iTextSharp.text.Image.GetInstance(bytes);
                if (image.Height > PageSize.A4.Height)
                {
                    image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                }
                else if (image.Width > PageSize.A4.Width)
                {
                    image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                }
                image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                doc1.Add(image);

                doc1.Close();
            }
           
            stream2.ToArray();
            return;
            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                PdfSharpCore.Pdf.PdfPage page = doc.AddPage();//添加1个页面并获得该页面实例
                var gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);

                var stream = new MemoryStream(bytes);
                stream.Seek(0, SeekOrigin.Begin);

                //PdfSharpCore.Drawing.XImage img = PdfSharpCore.Drawing.XImage.FromStream(stream);
                //gfx.Draw
                //page.Width = PdfSharpCore.Pdf. //注意pdf页面的单位是pt点，图片对象一般是px像素，所以要进行转换，大约1px=0.75pt
                //page.Height = 图片对象.Height;
                ////page.Size=PageSize.A4; //也可以直接设置为A4页面规格
                //MemoryStream s = new MemoryStream(); //实例化内存Stream流，用于临时保存图片对象
                //图片对象.Save(s, ImageFormat.Jpeg); //将图片对象保存到内存流中
                //XImage img = XImage.FromStream(s);//从内存流中获取到图片对象，因为Image跟XImage不能直接变换，所以只能通过内存流做中转
                //XGraphics g = XGraphics.FromPdfPage(page);//实例化XGraphics对象，并以刚才的pdf页面作为画布将后续的DrawImage、DrawString作用到画布上（画到画布上）
                //g.DrawImage(img, 0, 0);//将图片画到画布上

                doc.Save(stream, true);

            }
            //    var doc = new Document();
            //var stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            //using (stream)
            //{
            //    PdfWriter.GetInstance(doc, stream);
            //    doc.Open();

            //    var image = iTextSharp.text.Image.GetInstance(bytes);
            //    if (image.Height > PageSize.A4.Height)
            //    {
            //        image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
            //    }
            //    else if (image.Width > PageSize.A4.Width)
            //    {
            //        image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
            //    }
            //    image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            //    doc.Add(image);

            //    doc.Close();
        }
    }
}

