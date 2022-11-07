using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Order : BaseEntity
    {
        public int TableNumber { get; set; }
        public DateTime OrderedTime { get; set; }
    }
}
