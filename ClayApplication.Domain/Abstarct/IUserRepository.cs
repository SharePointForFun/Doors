using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClayApplication.DataAccess;
using System.Linq.Expressions;

namespace ClayApplication.Domain.Abstract
{
    public interface IUserRepository
    {
        User Get(int id);
        User Get(string login, string pwd);
        ICollection<Door> GetAllDoorsByUserId(int userId);
        ICollection<User> GetAll();
        bool DoesUserExist(string login, string pwd);
        IQueryable<User> Where(Expression<Func<User, bool>> predicate);
    }
}
