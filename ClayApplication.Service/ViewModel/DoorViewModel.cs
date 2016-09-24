using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClayApplication.Service.ViewModel
{
    public class DoorViewModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public UserViewModel Owner { get; set; }
        public List<UserViewModel> AuthorizedUsers { get; set; }
        public bool Locked { get; set; }
    }

    public class DoorAccessViewModel
    {
        public int DoorId { get; set; }
        public string DoorName { get; set; }
        public string DoorAddress { get; set; }
        public List<UserViewModel> UsersForAccess { get; set; }
        public List<AuthorizedUserViewModel> AuthorizedUsers { get; set; }
        public int SelectedUserId { get; set; }
        public string SelectedUserName { get; set; }
        public bool Authorized { get; set; }
    }

    public class AuthorizedUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool Authorized { get; set; }
        public int DoorId { get; set; }
    }
}