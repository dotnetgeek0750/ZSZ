using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public enum OrderByType
    {
        MonthRentDesc = 1,
        MonthRentAsc = 2,
        AreaDesc = 4,
        AreaAsc = 8,
        CreateDateDesc = 16
    }
}
