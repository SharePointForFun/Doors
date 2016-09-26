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
}