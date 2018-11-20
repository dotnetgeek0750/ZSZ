using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockDemo
{
    public class GirlConfig : EntityTypeConfiguration<Girl>
    {
        public GirlConfig()
        {
            ToTable("T_Girls");
            Property(p => p.RowVer).IsRowVersion();
        }
    }
}
