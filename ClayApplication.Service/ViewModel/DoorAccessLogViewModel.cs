using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClayApplication.Service.ViewModel
{
    public class DoorAccessLogViewModel
    {
        public DoorViewModel Door { get; set; }
        public UserViewModel User { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public bool AccessDenied { get; set; }
    }
}