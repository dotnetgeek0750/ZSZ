using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.IService
{
    public interface ICityService : IServiceSupport
    {
        /// <summary>
        /// 新增城市
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <returns>新增城市ID</returns>
        long AddNew(string cityName);
    }
}
