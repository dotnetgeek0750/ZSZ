using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseController : Controller
    {
        // GET: House
        public ActionResult PicUpload()
        {
            return View();
        }


        public ActionResult UploadPic(HttpPostedFileBase file)
        {
            //用文件的流来计算文件的MD5值
            var fileMD5 = Common.CommonHelper.CalcMD5(file.InputStream);
            var ext = Path.GetExtension(file.FileName);
            var path = $"/upload/{DateTime.Now.ToString("yyyy/MM/dd")}/{fileMD5}{ext}";
            var thumbPath = $"/upload/{DateTime.Now.ToString("yyyy/MM/dd")}/{fileMD5}_thumb{ext}";
            var fullPath = HttpContext.Server.MapPath("~" + path);
            var thumbFullPath = HttpContext.Server.MapPath("~" + thumbPath);

            //如果文件夹不存在，则创建
            new FileInfo(fullPath).Directory.Create();


            //缩略图
            ImageProcessingJob jobThumb = new ImageProcessingJob();
            //缩略图尺寸200*200
            jobThumb.Filters.Add(new FixedResizeConstraint(200, 200));
            //保存缩略图到磁盘
            jobThumb.SaveProcessedImageToFileSystem(file.InputStream, thumbFullPath);


            //水印
            ImageWatermark imgWatermark = new ImageWatermark(HttpContext.Server.MapPath("~/images/watermark/water.jpg"));
            imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;
            imgWatermark.Alpha = 50;
            ImageProcessingJob jobNormal = new ImageProcessingJob();
            jobNormal.Filters.Add(imgWatermark);
            jobNormal.Filters.Add(new FixedResizeConstraint(600, 600));
            //保存大图（含水印）到磁盘
            jobNormal.SaveProcessedImageToFileSystem(file.InputStream, fullPath);

            //file.SaveAs(HttpContext.Server.MapPath("~/" + file.FileName));
            //file.SaveAs(fullPath);
            return Content("ok");
        }

    }
}