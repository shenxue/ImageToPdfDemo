using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using Spire.Pdf;
using Spire.License;

namespace ImageToPdfDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfToImageController : ControllerBase
    {
        [HttpGet("test")]
        public void test()
        {
            string pdfPath = @"C:\Users\EDZ\Desktop\1601455012549.pdf";
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
            doc.LoadFromFile(pdfPath);//pdf物理路径
                                      //doc.SaveToFile(@"C:\Users\Administrator\Desktop\高效Web前端框架Layui教程.doc", FileFormat.DOC);//生成word的物理路径

            doc.SaveToFile(@"C:\Users\EDZ\Desktop\1.docx", FileFormat.DOCX);
            Console.WriteLine("转换成功");

            return;

            ExtractImage(pdfPath);
            //PDFTranImgHelp.ConvertPDF2Image(pdfPath, "F:\\", "NImage", 1, 1, ImageFormat.Png, Definition.Five);
            //IImageFormat format = null;
            //var imagesTemle = SixLabors.ImageSharp.Image.Load(imagePath, out format);
            //using (var stream = new MemoryStream())
            //{
            //    imagesTemle.Save(stream, format);
            //    stream.Flush();
            //    BytesToPdf2(stream.ToArray(), pdfPath);
            //}
        }
        /// <summary>
        /// 编写提取图片的方法，代码如下：
        /// </summary>
        /// <param name="pdfFile"></param>
        public void ExtractImage(string pdfFile)
        {
            PdfReader pdfReader = new PdfReader(pdfFile);
            int count = 1;
            for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
            {
                PdfReader pdf = new PdfReader(pdfFile);
                PdfDictionary pg = pdf.GetPageN(pageNumber);
                PdfObject obj1 = pdf.GetPdfObject(pageNumber);
                //if (obj1.IsIndirect())
                //{
                //    PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj1);
                //    string width = pg.Get(PdfName.WIDTH).ToString();
                //    string height = pg.Get(PdfName.HEIGHT).ToString();
                //    //bug 出现位置：未将对象引用....
                //    ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new GraphicsState(), (PRIndirectReference)obj1, tg);
                //    //RenderImage(imgRI);
                //    imgRI.GetImage().GetDrawingImage().Save(@"C:\Users\EDZ\Desktop\1\" + (count++) + ".jpg");
                //}
                //continue;
                var test = PdfReader.GetPdfObject(pg.Get(PdfName.MEDIABOX));
                PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
                PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
                try
                {
                    foreach (PdfName name in xobj.Keys)
                    {
                        PdfObject obj = xobj.Get(name);
                        if (obj.IsIndirect())
                        {
                            PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                            string width = tg.Get(PdfName.WIDTH).ToString();
                            string height = tg.Get(PdfName.HEIGHT).ToString();
                            //bug 出现位置：未将对象引用....
                            ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new GraphicsState(), (PRIndirectReference)obj, tg);
                            //RenderImage(imgRI);
                            imgRI.GetImage().GetDrawingImage().Save(@"C:\Users\EDZ\Desktop\1\" + (count++) + ".jpg");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }
            }
        }

        public void aaaa(string path, string name)
        {
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new Density(600, 600); //设置质量
            using (MagickImageCollection images = new MagickImageCollection())
            {
                try
                {
                    images.Read(path, settings);

                    for (int i = 0; i < images.Count; i++)
                    {
                        MagickImage image = (MagickImage)images[i];
                        image.Format = MagickFormat.Png;
                        image.MatteColor = new MagickColor("#ffffff");
                        image.Scale(730, 1030);
                        image.Write(@"C:\Users\EDZ\Desktop\1\" + name + ".png");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }


    }
}