using ClayApplication.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClayApplication.Service
{
    public interface IUserService
    {
        UserViewModel GetUser(int id);
        UserViewModel GetUser(string login, string pwd);
        IEnumerable<DoorViewModel> GetAllDoorsByUserId(int userId);
        bool DoesUserExist(string login, string pwd);
        ICollection<UserViewModel> GetAll();
        IEnumerable<DoorViewModel> DoorsUserHasAccess(int id);
    }
}
