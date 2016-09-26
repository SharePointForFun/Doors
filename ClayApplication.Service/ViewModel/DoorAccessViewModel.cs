using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClayApplication.Service.ViewModel
{
    public class DoorAccessViewModel
    {
        public DoorViewModel Door { get; set; }
        public List<AuthorizedUserViewModel> AuthorizedUsers { get; set; }
    }

    public class AuthorizedUserViewModel
    {
        public UserViewModel User { get; set; }
        public DoorViewModel Door { get; set; }
        public bool IsAuthorized { get; set; }
    }

    public class DoorAccessForEditViewModel
    {
        public DoorViewModel Door { get; set; }
        public List<UserViewModel> UsersListSelection { get; set; }
        public int SelectedUserId { get; set; }
        public string SelectedUserName { get; set; }
        public bool IsSelectedUserAuthorized { get; set; }
    }

    public class AuthorizedUserForEditViewModel
    {
        public int UserId { get; set; }
        public int DoorId { get; set; }
        public string UserName { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
