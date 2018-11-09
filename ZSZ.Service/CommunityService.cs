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
    public class CommunityService : ICommunityService
    {

        public CommunityDTO[] GetByRegionId(long regionId)
        {
            using (var ctx = new ZSZDbContext())
            {
                var bs = new BaseService<CommunityEntity>(ctx);
                var cities = bs.GetAll().AsNoTracking().Where(p => p.RegionId == regionId);
                return cities.Select(p => new CommunityDTO
                {
                    BuiltYear = p.BuiltYear,
                    CreateDateTime = p.CreateDateTime,
                    RegionId = p.RegionId,
                    Traffic = p.Traffic,
                }).ToArray();
            }
        }
    }
}
