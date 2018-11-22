using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ZSZ.FrontWeb
{
    //public class MemcacheMgr
    //{
    //    private static MemcachedClientConfiguration Configuration;

    //    private static MemcachedClient Client;

    //    static MemcacheMgr()
    //    {
    //        Configuration = new MemcachedClientConfiguration();
    //        Configuration.Servers.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11211));
    //        Configuration.Protocol = MemcachedProtocol.Binary;
    //        Client = new MemcachedClient(Configuration);
    //    }

    //    public void SetValue(string key, object value)
    //    {
    //        Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, p);//还可以
    //    }

    //    public T GetValue<T>(string key)
    //    {
    //        return Client.Get<T>(key);
    //    }
    //}

    public class MemcacheMgr
    {
        private static MemcachedClient Client;

        public static MemcacheMgr Instance { get; private set; } = new MemcacheMgr();

        private MemcacheMgr()
        {
            var Configuration = new MemcachedClientConfiguration();
            Configuration.Servers.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11211));
            Configuration.Protocol = MemcachedProtocol.Binary;
            Client = new MemcachedClient(Configuration);
        }

        public void SetValue(string key, object value, TimeSpan expired)
        {
            if (!value.GetType().IsSerializable)
            {
                throw new ArgumentException("memcached要求类必须是可序列化的");
            }
            Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value, expired);//还可以
        }

        public T GetValue<T>(string key)
        {
            return Client.Get<T>(key);
        }
    }
}