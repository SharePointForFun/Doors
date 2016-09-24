using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClayApplication.Service.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<DoorViewModel> DoorsHaveAccessTo { get; set; }
        public List<DoorViewModel> DoorsOwns { get; set; }
    }
}