using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderedTime { get; set; }
    }
}
