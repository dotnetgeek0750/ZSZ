using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.IService
{
    /// <summary>
    /// 一个标识接口，所有服务都实现这个接口
    /// 约定：有实现此接口，AutoFac都认为都是实现类，
    /// 避免了AutoFac加载了许多乱七八糟的类
    /// </summary>
    public interface IServiceSupport
    {
    }
}
