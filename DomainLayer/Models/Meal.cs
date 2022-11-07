using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Meal : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
    }
}
