using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClayApplication.Service.ViewModel
{
    public class DoorAccessLogViewModel
    {
        public int DoorId { get; set; }
        public int UserId { get; set; }
        public string DoorName { get; set; }
        public string DoorAddress { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public bool AccessDenied { get; set; }
    }
}