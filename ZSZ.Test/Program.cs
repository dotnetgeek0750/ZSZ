using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service;

namespace ZSZ.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                ctx.AdminUserLogs.ToList();
                int i = 1;
            }

        }
    }
}
