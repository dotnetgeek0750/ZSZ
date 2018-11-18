using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.DTO;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class HouseController : Controller
    {
        public IHouseService houseService { get; set; }

        public IAttachmentService attachmentService { get; set; }

        // GET: House
        public ActionResult Index(long id)
        {
            var house = houseService.GetById(id);
            if (house == null)
            {
                return View("Error", (object)"不存在的房源ID");
            }
            var pics = houseService.GetPics(id);
            var attachments = attachmentService.GetAttachments(id);
            var model = new HouseIndexViewModel
            {
                House = house,
                Pics = pics,
                Attachments = attachments,
            };
            return View();
        }
    }
}