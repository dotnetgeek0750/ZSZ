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
        public ICityService cityService { get; set; }

        public IRegionService regionService { get; set; }

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


        /// <summary>
        /// 分析200-300、300-* 这样的价格区间
        /// </summary>
        /// <param name="value">200-300</param>
        /// <param name="startMonthRent">解析出来的起始租金</param>
        /// <param name="endMonthRent">解析出来的结束租金</param>
        private void ParseMonthRent(string value, out int? startMonthRent, out int? endMonthRent)
        {
            if (string.IsNullOrEmpty(value))
            {
                startMonthRent = null;
                endMonthRent = null;
                return;
            }
            string[] values = value.Split('-');
            string strStart = values[0];
            string strEnd = values[1];
            if (strStart == "*")
            {
                startMonthRent = null;
            }
            else
            {
                startMonthRent = Convert.ToInt32(strStart);
            }
            if (strEnd == "*")
            {
                endMonthRent = null;
            }
            else
            {
                endMonthRent = Convert.ToInt32(strEnd);
            }
        }


        [HttpGet]
        public ActionResult Search(string keyWord, string monthRent, string orderByType, long? regionId, long typeId)
        {
            //获得当前用户城市ID
            long cityId = FrontUtils.GetCityId(HttpContext);

            //获得城市区域
            var regions = regionService.GetAll(cityId);
            HouseSearchViewModel model = new HouseSearchViewModel
            {
                Regions = regions,
            };


            int? startMonthRent;
            int? endMonthRent;
            ParseMonthRent(monthRent, out startMonthRent, out endMonthRent);

            var orderByTypeEnum = OrderByType.CreateDateDesc;
            switch (orderByType)
            {
                case "MonthRentAsc":
                    orderByTypeEnum = OrderByType.MonthRentAsc;
                    break;
                case "MonthRentDesc":
                    orderByTypeEnum = OrderByType.MonthRentDesc;
                    break;
                case "AreaAsc":
                    orderByTypeEnum = OrderByType.AreaAsc;
                    break;
                case "AreaDesc":
                    orderByTypeEnum = OrderByType.AreaDesc;
                    break;
            }
            HouseSearchOptions searchOption = new HouseSearchOptions
            {
                CityId = cityId,
                CurrentIndex = 1,
                StartMonthRent = startMonthRent,
                EndMonthRent = endMonthRent,
                Keywords = keyWord,
                OrderByType = orderByTypeEnum,
                PageSize = 10,
                RegionId = regionId,
                TypeId = typeId,
            };
            var searchResult = houseService.Search(searchOption);
            model.Houses = searchResult.result;

            return View(model);
        }
    }
}