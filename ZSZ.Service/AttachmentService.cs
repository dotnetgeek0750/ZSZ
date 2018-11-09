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
    public class AttachmentService : IAttachmentService
    {
        private AttachmentDTO ToDTO(AttachmentEntity att)
        {
            var dto = new AttachmentDTO
            {
                CreateDateTime = att.CreateDateTime,
                IconName = att.IconName,
                Id = att.Id,
                Name = att.Name,
            };
            return dto;
        }


        public AttachmentDTO[] GetAll()
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AttachmentEntity> bs = new BaseService<AttachmentEntity>(ctx);
                var items = bs.GetAll().AsNoTracking();
                return items.Select(p => ToDTO(p)).ToArray();
            }
        }

        public AttachmentDTO[] GetAttachments(long houseId)
        {
            using (var ctx = new ZSZDbContext())
            {
                var bs = new BaseService<HouseEntity>(ctx);
                var house = bs.GetAll().Include(p => p.Attachments)
                    .AsNoTracking().SingleOrDefault();
                if (house == null)
                {
                    throw new ArgumentException($"House：{houseId}不存在");
                }
                return house.Attachments.Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
