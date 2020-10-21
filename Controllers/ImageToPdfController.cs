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

namespace ImageToPdfDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageToPdfController : ControllerBase
    {
        [HttpGet("test")]
        public string ImageToPdf()
        {
            string imagePath = @"D:\pics\1.jpeg";
            string pdfPath = @"D:\pics\1.pdf";
            ImageToPdf(imagePath, pdfPath);
            return "";
        }

        private void ImageToPdf(string imagePath,string pdfPath)
        {
            var doc = new Document();
            var stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            using (stream)
            {
                PdfWriter.GetInstance(doc, stream);
                doc.Open();
                using (var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var image = Image.GetInstance(imageStream);
                    if (image.Height > PageSize.A4.Height)
                    {
                        image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                    }
                    else if (image.Width > PageSize.A4.Width)
                    {
                        image.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                    }
                    image.Alignment = Image.ALIGN_MIDDLE;
                    doc.Add(image);
                }
                doc.Close();
            }
        }
    }
}