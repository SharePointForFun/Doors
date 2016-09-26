using ClayApplication.Service;
using ClayApplication.Service.ViewModel;
using ClayApplication.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClayApplication.WebUI.Controllers
{
    public class DoorAccessController : Controller
    {
        private IDoorAccessService doorAccessService;

        public DoorAccessController(IDoorAccessService doorRepository)
        {
            this.doorAccessService = doorRepository;
        }


        public ActionResult EditAuthorisation(int doorId, int userId)
        {
            var userForEdit = doorAccessService.GetDoorAccessByDoorAndUser(doorId, userId);
            if (userForEdit != null)
            {
                return View(userForEdit);
            }

            return RedirectToAction("ErrorPage", "Error", new { message = "Something went wrong" });
        }

        [HttpPost]
        public ActionResult EditAuthorisation(AuthorizedUserForEditViewModel userForEdit)
        {
            try
            {
                if (userForEdit != null)
                {
                    doorAccessService.EditAuthorization(userForEdit);
                    return RedirectToAction("Details", "Door", new { id = userForEdit.DoorId });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return RedirectToAction("ErrorPage", "Error", new { message = "Something went wrong" });
        }
    }
}
