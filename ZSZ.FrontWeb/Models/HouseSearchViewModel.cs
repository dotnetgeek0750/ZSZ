using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.FrontWeb.Models
{
    public class HouseSearchViewModel
    {
        public RegionDTO[] Regions { get; set; }

        public HouseDTO[] Houses { get; set; }
    }
}