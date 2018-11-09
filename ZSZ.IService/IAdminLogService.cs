using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.IService
{
    public interface IAdminLogService : IServiceSupport
    {
        //插入一条日志：adminUserId为操作用户id，message为消息
        void AddNew(long adminUserId, String message);
        //以后做：如果做日志搜索等的话就要增加新的方法
    }
}
