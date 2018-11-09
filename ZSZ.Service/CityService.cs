using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    class CityService : ICityService
    {
        private CityDTO ToDTO(CityEntity city)
        {
            var dto = new CityDTO
            {
                CreateDateTime = city.CreateDateTime,
                Id = city.Id,
                Name = city.Name,
            };
            return dto;
        }

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

        public CityDTO[] GetAll()
        {
            using (var ctx = new ZSZDbContext())
            {
                var bs = new BaseService<CityEntity>(ctx);
                return bs.GetAll().AsNoTracking().Select(p => ToDTO(p)).ToArray();
            }
        }

        public CityDTO GetById(long id)
        {
            using (var ctx = new ZSZDbContext())
            {
                var bs = new BaseService<CityEntity>(ctx);
                var city = bs.GetById(id);
                if (city == null)
                {
                    return null;
                }
                return ToDTO(city);
            }
        }
    }
}
