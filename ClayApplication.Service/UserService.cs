using ClayApplication.DataAccess;
using ClayApplication.Domain.Abstract;
using ClayApplication.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClayApplication.Service
{
    public class UserService: IUserService
    {
        private readonly IRepo<User> repo;
        public UserService(IRepo<User> repo)
        {
            this.repo = repo;  
        }

        private UserViewModel ConvertToViewModel(User user)
        {
            var userViewModel = new UserViewModel()
            {
                Id = user.id,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Login = user.login
            };

            return userViewModel;
        }

        public UserViewModel GetUser(string login, string pwd)
        {
            var user = repo.Where(x => x.login.Equals(login) && x.pwd.Equals(pwd));
            return ConvertToViewModel(user as User);
        }

        public UserViewModel GetUser(int id)
        {
            var user = repo.Get(id);
            return ConvertToViewModel(user);
        }
    }
}
