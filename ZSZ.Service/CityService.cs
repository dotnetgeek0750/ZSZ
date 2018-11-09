using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    class CityService : ICityService
    {
        public long AddNew(string cityName)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                var exist = bs.GetAll().Any(p => p.Name == cityName);
                if (exist)
                {
                    throw new ArgumentException("城市已存在");
                }
                CityEntity c = new CityEntity();
                c.Name = cityName;
                ctx.Cities.Add(c);
                ctx.SaveChanges();
                return c.Id;
            }
        }
    }
}
