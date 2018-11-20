using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LockDemo
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("name=connstr")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //加载配置类
            Assembly asm = Assembly.GetExecutingAssembly();
            modelBuilder.Configurations.AddFromAssembly(asm);
        }

        public DbSet<Girl> Girls { get; set; }
    }
}
