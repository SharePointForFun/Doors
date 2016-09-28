using ClayApplication.DataAccess;
using ClayApplication.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClayApplication.Service.Extension
{
    public static class ExtensionHelper
    {
        public static UserViewModel GetViewModel(this User userObject)
        {
            var userViewModel = new UserViewModel();
            if (userObject != null)
            {
                userViewModel.Id = userObject.id;
                userViewModel.FirstName = userObject.FirstName;
                userViewModel.LastName = userObject.LastName;
                userViewModel.Login = userObject.login;
                userViewModel.Password = userObject.pwd;
            }

            return userViewModel;
        }

        public static DoorViewModel GetViewModel(this Door doorObject)
        {
            var doorViewModel = new DoorViewModel();
            if (doorObject != null)
            {
                doorViewModel.Id = doorObject.id;
                doorViewModel.Address = doorObject.address;
                doorViewModel.Name = doorObject.name;
                doorViewModel.Owner = doorObject.User.GetViewModel();
            }

            return doorViewModel;
        }

        public static DoorAccessLogViewModel GetViewModel(this DoorAccessLog doorAccessLogObject)
        {
            var doorAccessLogViewModel = new DoorAccessLogViewModel();
            if(doorAccessLogObject != null)
            {
                doorAccessLogViewModel.AccessDenied = doorAccessLogObject.accessdenied ?? true;
                doorAccessLogViewModel.Date = doorAccessLogObject.date ?? DateTime.Now;
                doorAccessLogViewModel.Door = doorAccessLogObject.Door.GetViewModel();
                doorAccessLogViewModel.User = doorAccessLogObject.User.GetViewModel();
                doorAccessLogViewModel.Status = doorAccessLogObject.state ?? true;
            }

            return doorAccessLogViewModel;
        }

        public static AuthorizedUserViewModel GetViewModel(this DoorAccess doorAccessObject)
        {
            var doorAccessViewModel = new AuthorizedUserViewModel();
            if (doorAccessObject != null)
            {
                doorAccessViewModel.IsAuthorized = doorAccessObject.autherized ?? true;
                doorAccessViewModel.Door = doorAccessObject.Door.GetViewModel();
                doorAccessViewModel.User = doorAccessObject.User.GetViewModel();
            }

            return doorAccessViewModel;
        }

        public static AuthorizedUserForEditViewModel GetViewModelForEdit(this DoorAccess doorAccessObject)
        {
            var doorAccessViewModel = new AuthorizedUserForEditViewModel();
            if (doorAccessObject != null)
            {
                doorAccessViewModel.IsAuthorized = doorAccessObject.autherized ?? true;
                doorAccessViewModel.DoorId = doorAccessObject.doorid;
                doorAccessViewModel.UserId = doorAccessObject.userid;
                doorAccessViewModel.UserName = string.Format("{0} {1}", doorAccessObject.User.FirstName, doorAccessObject.User.LastName);
            }

            return doorAccessViewModel;
        }
    }
}
