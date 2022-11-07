using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class PricelistRepository : GenericRepository<Pricelist>, IPricelistRepository
    {
        public PricelistRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
