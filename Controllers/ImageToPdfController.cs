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
            //string imagePath = @"D:\pics\1.jpeg";
            //string pdfPath = @"D:\pics\11.pdf";‘
            string imagePath = @"C:\Users\EDZ\Desktop\2.jpeg";
            string pdfPath = @"C:\Users\EDZ\Desktop\2.pdf";
            IImageFormat format = null;
            var imagesTemle = SixLabors.ImageSharp.Image.Load(imagePath, out format);
            using (var stream = new MemoryStream())
            {
                imagesTemle.Save(stream, format);
                stream.Flush();
                BytesToPdf2(stream.ToArray(), pdfPath);
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
            MemoryStream pdfStream = new MemoryStream();
            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                PdfSharpCore.Pdf.PdfPage page = doc.AddPage();//添加1个页面并获得该页面实例

                page.Size = PdfSharpCore.PageSize.A4;
                PdfSharpCore.Drawing.XImage img = PdfSharpCore.Drawing.XImage.FromStream(() => new MemoryStream(bytes));

                PdfSharpCore.Drawing.XGraphics g = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);//实例化XGraphics对象，并以刚才的pdf页面作为画布将后续的DrawImage、DrawString作用到画布上（画到画布上）
                g.DrawImage(img, 10, 10);//将图片画到画布上
                doc.Save(pdfPath);//将pdf文件保存到保存对话框获取到的路径上
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


        private void BytesToPdf3(byte[] bytes, string pdfPath)
        {
            var doc = new Document();
            var stream = new MemoryStream();
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
            var result = stream.ToArray();
        }
    }
}

