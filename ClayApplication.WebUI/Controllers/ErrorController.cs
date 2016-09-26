using ClayApplication.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClayApplication.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult ErrorPage(string message)
        {
            var errorMessage = new ErrorMessage() { Message = message };
            return View(errorMessage);
        }

    }
}
