using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class AdminUserService : IAdminUserService
    {
        public long AddAdminUser(string name, string phoneNum, string password, string email, long? cityId)
        {
            //加盐
            var salt = CommonHelper.CreateVerifyCode(5);

            var user = new AdminUserEntity
            {
                CityId = cityId,
                Email = email,
                Name = name,
                PhoneNum = phoneNum,
                PasswordSalt = salt,
                PasswordHash = CommonHelper.CalcMD5(salt + password),
            };
            using (var ctx = new ZSZDbContext())
            {
                ctx.AdminUsers.Add(user);
                ctx.SaveChanges();
                return user.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            throw new NotImplementedException();
        }

        public AdminUserDTO[] GetAll(long? cityId)
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var all = bs.GetAll().Include(p => p.City)
                    .AsNoTracking().Where(p => p.CityId == cityId);
                return all.Select(p => TODTO(p)).ToArray();
            }
        }

        public AdminUserDTO[] GetAll()
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                return bs.GetAll().Include(p => p.City)
                    .AsNoTracking().Select(p => TODTO(p)).ToArray();
            }
        }

        public AdminUserDTO GetById(long id)
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetAll().Include(p => p.City).Where(p => p.Id == id).SingleOrDefault();
                if (user == null)
                {
                    return null;
                }
                return TODTO(user);
            }
        }

        public AdminUserDTO GetByPhoneNum(string phoneNum)
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var users = bs.GetAll().Include(p => p.City)
                    .AsNoTracking()
                    .Where(p => p.PhoneNum == phoneNum);
                int count = users.Count();
                if (count <= 0)
                {
                    return null;
                }
                else if (count == 1)
                {
                    return TODTO(users.Single());
                }
                else
                {
                    throw new ApplicationException($"找到多个手机号为{phoneNum}的记录");
                }
            }
        }

        public bool HasPermission(long adminUserId, string permissionName)
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                //var user = bs.GetById(adminUserId);
                var user = bs.GetAll().Include(p => p.Roles).AsNoTracking()
                    .SingleOrDefault(p => p.Id == adminUserId);
                if (user == null)
                {
                    throw new ArgumentException($"找不到ID={adminUserId}的用户");
                }
                return user.Roles.SelectMany(p => p.Permissions).Any(p => p.Name == permissionName);
            }
        }

        public void MarkDeleted(long adminUserId)
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                bs.MarkDeleted(adminUserId);
            }
        }

        public void RecordLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void ResetLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdminUser(long id, string name, string phoneNum, string password, string email, long? cityId)
        {
            using (var ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetById(id);
                if (user == null)
                {
                    throw new ArgumentException($"找不到ID={id}的管理员");
                }
                user.Name = name;
                user.PhoneNum = phoneNum;
                user.Email = email;
                user.PasswordHash = CommonHelper.CalcMD5(user.PasswordSalt + password);
                user.CityId = cityId;
                ctx.SaveChanges();
            }
        }


        private AdminUserDTO TODTO(AdminUserEntity user)
        {
            AdminUserDTO dto = new AdminUserDTO
            {
                CityId = user.CityId,
                CityName = user.City == null ? "总部" : user.City.Name,//需要Include提升性能
                CreateDateTime = user.CreateDateTime,
                Email = user.Email,
                Id = user.Id,
                LastLoginErrorDateTime = user.LastLoginErrorDateTime,
                Name = user.Name,
                PhoneNum = user.PhoneNum,
            };
            return dto;
        }
    }
}
