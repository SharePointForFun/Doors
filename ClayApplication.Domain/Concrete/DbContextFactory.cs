using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClayApplication.Domain.Abstract;
using ClayApplication.DataAccess;

namespace ClayApplication.Domain.Concrete
{
    public class DbContextFactory: IDbContextFactory
    {
        private readonly ClayDbEntities dbContext;
        public DbContextFactory()
        {
            dbContext = new ClayDbEntities();
        }

        public ClayDbEntities GetContext()
        {
            return dbContext;
        }
    }
}
