using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ZSZ.Service
{
    public class HouseService : IHouseService
    {
        /*
        public long AddNew(HouseDTO house)
        {
            HouseEntity houseEntity = new HouseEntity();
            houseEntity.Address = house.Address;
            houseEntity.Area = house.Area;

            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AttachmentEntity> attBS
                    = new BaseService<AttachmentEntity>(ctx);
                var atts = attBS.GetAll().Where(a => house.AttachmentIds.Contains(a.Id));
                foreach (var att in atts)
                {
                    houseEntity.Attachments.Add(att);
                }
                houseEntity.CheckInDateTime = house.CheckInDateTime;
                houseEntity.CommunityId = house.CommunityId;
                houseEntity.CreateDateTime = house.CreateDateTime;
                houseEntity.DecorateStatusId = house.DecorateStatusId;
                houseEntity.Description = house.Description;
                houseEntity.Direction = house.Direction;
                houseEntity.FloorIndex = house.FloorIndex;
                //houseEntity.HousePics 新增后再单独添加
                houseEntity.LookableDateTime = house.LookableDateTime;
                houseEntity.MonthRent = house.MonthRent;
                houseEntity.OwnerName = house.OwnerName;
                houseEntity.OwnerPhoneNum = house.OwnerPhoneNum;
                houseEntity.RoomTypeId = house.RoomTypeId;
                houseEntity.StatusId = house.StatusId;
                houseEntity.TotalFloorCount = house.TotalFloorCount;
                houseEntity.TypeId = house.TypeId;
                ctx.Houses.Add(houseEntity);
                ctx.SaveChanges();
                return houseEntity.Id;
            }
            

        }*/

        public long AddNew(HouseAddNewDTO house)
        {
            HouseEntity houseEntity = new HouseEntity();
            houseEntity.Address = house.Address;
            houseEntity.Area = house.Area;

            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AttachmentEntity> attBS
                    = new BaseService<AttachmentEntity>(ctx);
                //拿到house.AttachmentIds为主键的房屋配套设施
                var atts = attBS.GetAll().Where(a => house.AttachmentIds.Contains(a.Id));
                //houseEntity.Attachments = new List<AttachmentEntity>();
                foreach (var att in atts)
                {
                    houseEntity.Attachments.Add(att);
                }
                houseEntity.CheckInDateTime = house.CheckInDateTime;
                houseEntity.CommunityId = house.CommunityId;
                houseEntity.DecorateStatusId = house.DecorateStatusId;
                houseEntity.Description = house.Description;
                houseEntity.Direction = house.Direction;
                houseEntity.FloorIndex = house.FloorIndex;
                //houseEntity.HousePics 新增后再单独添加
                houseEntity.LookableDateTime = house.LookableDateTime;
                houseEntity.MonthRent = house.MonthRent;
                houseEntity.OwnerName = house.OwnerName;
                houseEntity.OwnerPhoneNum = house.OwnerPhoneNum;
                houseEntity.RoomTypeId = house.RoomTypeId;
                houseEntity.StatusId = house.StatusId;
                houseEntity.TotalFloorCount = house.TotalFloorCount;
                houseEntity.TypeId = house.TypeId;
                ctx.Houses.Add(houseEntity);
                ctx.SaveChanges();
                return houseEntity.Id;
            }
        }

        public long AddNewHousePic(HousePicDTO housePic)
        {
            HousePicEntity entity = new HousePicEntity();
            entity.HouseId = housePic.HouseId;
            entity.ThumbUrl = housePic.ThumbUrl;
            entity.Url = housePic.Url;
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                ctx.HousePics.Add(entity);
                ctx.SaveChanges();
                return entity.Id;
            }
        }

        public void DeleteHousePic(long housePicId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                //复习EF状态转换
                /*
                HousePicEntity entity = new HousePicEntity();
                entity.Id = housePicId;
                ctx.Entry(entity).State = EntityState.Deleted;
                ctx.SaveChanges();*/
                var entity = ctx.HousePics
                    .SingleOrDefault(p => p.IsDeleted == false && p.Id == housePicId);
                if (entity != null)
                {
                    ctx.HousePics.Remove(entity);
                    ctx.SaveChanges();
                }
            }
        }

        private HouseDTO ToDTO(HouseEntity entity)
        {
            HouseDTO dto = new HouseDTO();
            dto.Address = entity.Address;
            dto.Area = entity.Area;
            dto.AttachmentIds = entity.Attachments.Select(a => a.Id).ToArray();
            dto.CheckInDateTime = entity.CheckInDateTime;
            dto.CityId = entity.Community.Region.CityId;
            dto.CityName = entity.Community.Region.City.Name;
            dto.CommunityBuiltYear = entity.Community.BuiltYear;
            dto.CommunityId = entity.CommunityId;
            dto.CommunityLocation = entity.Community.Location;
            dto.CommunityName = entity.Community.Name;
            dto.CommunityTraffic = entity.Community.Traffic;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.DecorateStatusId = entity.DecorateStatusId;
            dto.DecorateStatusName = entity.DecorateStatus.Name;
            dto.Description = entity.Description;
            dto.Direction = entity.Direction;
            var firstPic = entity.HousePics.FirstOrDefault();
            if (firstPic != null)
            {
                dto.FirstThumbUrl = firstPic.ThumbUrl;
            }
            dto.FloorIndex = entity.FloorIndex;
            dto.Id = entity.Id;
            dto.LookableDateTime = entity.LookableDateTime;
            dto.MonthRent = entity.MonthRent;
            dto.OwnerName = entity.OwnerName;
            dto.OwnerPhoneNum = entity.OwnerPhoneNum;
            dto.RegionId = entity.Community.RegionId;
            dto.RegionName = entity.Community.Region.Name;
            dto.RoomTypeId = entity.RoomTypeId;
            dto.RoomTypeName = entity.RoomType.Name;
            dto.StatusId = entity.StatusId;
            dto.StatusName = entity.Status.Name;
            dto.TotalFloorCount = entity.TotalFloorCount;
            dto.TypeId = entity.TypeId;
            dto.TypeName = entity.Type.Name;
            return dto;
        }

        public HouseDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> houseBS = new BaseService<HouseEntity>(ctx);
                var house = houseBS.GetAll()
                    .Include(h => h.Attachments).Include(h => h.Community)
                    //Include("Community.Region.City");
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                        + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type)
                    .SingleOrDefault(h => h.Id == id);
                //.Where(h => h.Id == id).SingleOrDefault();
                if (house == null)
                {
                    return null;
                }
                return ToDTO(house);
            }
        }

        public long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> houseBS = new BaseService<HouseEntity>(ctx);
                return houseBS.GetAll()
                    .LongCount(h => h.Community.Region.CityId == cityId
                    && h.CreateDateTime >= startDateTime && h.CreateDateTime <= endDateTime);
            }
        }

        public HouseDTO[] GetPagedData(long cityId, long typeId, int pageSize, int currentIndex)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> houseBS = new BaseService<HouseEntity>(ctx);
                var houses = houseBS.GetAll()
                    .Include(h => h.Attachments).Include(h => h.Community)
                    .Include(h => nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                        + "." + nameof(RegionEntity.City))
                    .Include(h => nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type)
                    .OrderByDescending(h => h.CreateDateTime)
                    .Skip(currentIndex).Take(pageSize)
                    .Where(h => h.Community.Region.CityId == cityId && h.TypeId == typeId);
                return houses.ToList().Select(h => ToDTO(h)).ToArray();
            }
        }

        public HousePicDTO[] GetPics(long houseId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                /*
                BaseService<HousePicEntity> bs = new BaseService<HousePicEntity>(ctx);
                return bs.GetAll().AsNoTracking().Where(p => p.HouseId == houseId)
                    .Select(p => new HousePicDTO
                    {
                        CreateDateTime = p.CreateDateTime,
                        HouseId = p.HouseId,
                        Id = p.Id,
                        ThumbUrl = p.ThumbUrl,
                        Url = p.Url
                    })
                    .ToArray();*/
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                return bs.GetById(houseId).HousePics.Select(p => new HousePicDTO
                {
                    CreateDateTime = p.CreateDateTime,
                    HouseId = p.HouseId,
                    Id = p.Id,
                    ThumbUrl = p.ThumbUrl,
                    Url = p.Url
                }).ToArray();
            }
        }

        public long GetTotalCount(long cityId, long typeId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                return bs.GetAll()
                    .LongCount(h => h.Community.Region.CityId == cityId && h.TypeId == typeId);
            }
        }

        public void MarkDeleted(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                bs.MarkDeleted(id);
            }
        }

        public HouseSearchResult Search(HouseSearchOptions options)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                var items = bs.GetAll().Where(p => p.Community.Region.CityId == options.CityId && p.TypeId == options.TypeId);
                if (options.RegionId != null)
                {
                    items = items.Where(p => p.Community.RegionId == options.RegionId);
                }
                if (options.StartMonthRent != null)
                {
                    items = items.Where(p => p.MonthRent >= options.StartMonthRent);
                }
                if (options.EndMonthRent != null)
                {
                    items = items.Where(p => p.MonthRent <= options.EndMonthRent);
                }
                if (string.IsNullOrWhiteSpace(options.Keywords))
                {
                    items = items.Where(p => p.Address.Contains(options.Keywords)
                       || p.Description.Contains(options.Keywords)
                       || p.Community.Name.Contains(options.Keywords)
                       || p.Community.Location.Contains(options.Keywords)
                       || p.Community.Traffic.Contains(options.Keywords)
                    );
                }

                var totalCount = items.LongCount();

                items = items.Include(h => h.Attachments).Include(h => h.Community)
                     .Include(h => nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                         + "." + nameof(RegionEntity.City))
                     .Include(h => nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                     .Include(h => h.DecorateStatus)
                     .Include(h => h.HousePics)
                     .Include(h => h.RoomType)
                     .Include(h => h.Status)
                     .Include(h => h.Type);


                switch (options.OrderByType)
                {
                    case OrderByType.MonthRentDesc:
                        items = items.OrderByDescending(p => p.MonthRent);
                        break;
                    case OrderByType.MonthRentAsc:
                        items = items.OrderBy(p => p.MonthRent);
                        break;
                    case OrderByType.AreaDesc:
                        items = items.OrderByDescending(p => p.Area);
                        break;
                    case OrderByType.AreaAsc:
                        items = items.OrderBy(p => p.Area);
                        break;
                    case OrderByType.CreateDateDesc:
                        items = items.OrderByDescending(p => p.CreateDateTime);
                        break;
                    default:
                        break;
                }
                items = items.Skip((options.CurrentIndex - 1) * options.PageSize).Take(options.PageSize);

                HouseSearchResult searchResult = new HouseSearchResult();
                searchResult.totalCount = totalCount;
                List<HouseDTO> houses = new List<HouseDTO>();
                foreach (var item in items)
                {
                    houses.Add(ToDTO(item));
                }
                searchResult.result = houses.ToArray();
                return searchResult;
            }
        }

        public void Update(HouseDTO house)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                HouseEntity entity = bs.GetById(house.Id);
                entity.Address = house.Address;
                entity.Area = house.Area;
                //2,3,4
                entity.Attachments.Clear();//先删再加
                var atts = ctx.Attachments.Where(a => a.IsDeleted == false &&
                    house.AttachmentIds.Contains(a.Id));
                foreach (AttachmentEntity att in atts)
                {
                    entity.Attachments.Add(att);
                }
                //3,4,5
                entity.CheckInDateTime = house.CheckInDateTime;
                entity.CommunityId = house.CommunityId;
                entity.DecorateStatusId = house.DecorateStatusId;
                entity.Description = house.Description;
                entity.Direction = house.Direction;
                entity.FloorIndex = house.FloorIndex;
                entity.LookableDateTime = house.LookableDateTime;
                entity.MonthRent = house.MonthRent;
                entity.OwnerName = house.OwnerName;
                entity.OwnerPhoneNum = house.OwnerPhoneNum;
                entity.RoomTypeId = house.RoomTypeId;
                entity.StatusId = house.StatusId;
                entity.TotalFloorCount = house.TotalFloorCount;
                entity.TypeId = house.TypeId;
                ctx.SaveChanges();
            }
        }

        public int GetTodayNewHouseCount(long cityId)
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                return bs.GetAll().Where(p => p.Community.Region.CityId == cityId
                && SqlFunctions.DateDiff("hh", p.CreateDateTime, DateTime.Now) <= 24)
                .Count();
            }
        }
    }
}
