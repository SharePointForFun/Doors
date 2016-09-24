using ClayApplication.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClayApplication.Domain.Abstract;
using System.Linq.Expressions;

namespace ClayApplication.Domain.Concrete
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private ClayDbEntities clayEntities;
        public UserRepository()
        {
            clayEntities = new ClayDbEntities();
        }

        public User Get(int id)
        {
            return clayEntities.User.Find(id);
        }

        public virtual IQueryable<User> Where(Expression<Func<User, bool>> predicate)
        {
            //if (typeof(IDel).IsAssignableFrom(typeof(T)))
            //    return IoC.Resolve<IDelRepo<T>>().Where(predicate, showDeleted);
            return clayEntities.Set<User>().Where(predicate);
        }


        public User Get(string login, string pwd)
        {
            User user = null;
            try
            {
                user = clayEntities.User.FirstOrDefault(x => x.pwd == pwd && x.login == login);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return user;
        }

        public bool DoesUserExist(string login, string pwd)
        {
            bool found = false;
            try
            {
                var user = clayEntities.User.FirstOrDefault(x => x.pwd == pwd && x.login == login);
                found = user != null;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return found;
        }

        public ICollection<Door> GetAllDoorsByUserId(int userId)
        {
            var user = clayEntities.User.Find(userId);
            return user.Door;
        }

        public ICollection<User> GetAll()
        {
            return clayEntities.User.ToList();
        }

        public void Dispose()
        {
            if (clayEntities == null)
            {
                return;
            }
            else
            {
                clayEntities.Dispose();
                clayEntities = null;
            }
        }
    }
}