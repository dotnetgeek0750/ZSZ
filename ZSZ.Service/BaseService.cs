using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    class BaseService<T> where T : BaseEntity
    {
        public ZSZDbContext ctx;

        public BaseService(ZSZDbContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// 获取所有没有软删除的数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return ctx.Set<T>().Where(p => p.IsDeleted == false);
        }

        /// <summary>
        /// 获取总数据条数
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount()
        {
            //最终是一个select count(1) 的操作
            return GetAll().LongCount();
        }


        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IQueryable<T> GetPagedData(int startIndex, int count)
        {
            return GetAll().OrderBy(p => p.CreateDateTime)
                .Skip(startIndex).Take(count);
        }

        /// <summary>
        /// 查找ID=id的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            return GetAll().Where(p => p.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// 软删除ID的记录
        /// </summary>
        /// <param name="id"></param>
        public void MarkDeleted(long id)
        {
            var data = GetById(id);
            if (data != null)
            {
                data.IsDeleted = true;
                ctx.SaveChanges();
            }


        }

    }
}
