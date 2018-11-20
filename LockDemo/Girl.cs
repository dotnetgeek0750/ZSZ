using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockDemo
{
     
    public class Girl
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BF { get; set; }
        public byte[] RowVer { get; set; }
    }
}
