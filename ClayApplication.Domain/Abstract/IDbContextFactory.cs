using ClayApplication.DataAccess;

namespace ClayApplication.Domain.Abstract
{
    public interface IDbContextFactory
    {
        ClayDbEntities GetContext();
    }
}
