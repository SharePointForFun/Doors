using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ClayApplication.Domain.Abstract;
using ClayApplication.WebUI.Helpers;
using ClayApplication.Service.ViewModel;
using ClayApplication.Service;

namespace ClayApplication.WebUI.Controllers
{
    public class DoorController : Controller
    {
        private IDoorService doorService;

        public DoorController(IDoorService doorService)
        {
            this.doorService = doorService;
        }

        public ActionResult Details(int id)
        {
            var doorObject = doorService.GetDoorAccess(id);
            return View(doorObject);
        }

        public ActionResult ViewLogs(int id)
        {
            var currentUser = SessionHelper.CurrentUserViewModel;
            if (currentUser != null)
            {
                if (doorService.IsUserDoorOwner(id, currentUser.Id))
                {
                    var doorAccessLog = doorService.GetAccessLog(id);
                    return View(doorAccessLog);
                }
                else
                {
                    // user not door owner
                    return RedirectToAction("ErrorPage", "Error", new { message = "User is not door owner" });
                }
            }
            else
            {
                // user not found
                return RedirectToAction("Authentication", "User");
            }
        }
    

        public ActionResult Key(int id)
        {
            var currentUser = SessionHelper.CurrentUserViewModel;
            if (currentUser != null)
            {
                var doorViewModel = doorService.GetDoor(id);
                TempData["doorLocked"] = doorViewModel.Locked;
                if (doorService.UserHasAccessToDoor(doorViewModel, currentUser))
                {
                    return View(doorViewModel);
                }
                else
                {
                    doorService.LogAccessToDoorByUser(doorViewModel.Id, currentUser.Id, doorViewModel.Locked, true);
                    return RedirectToAction("ErrorPage", "Error",  new { message = "Access Denied" });
                }
            }
            else
            {
                // "User could not be found"
                return RedirectToAction("Authentication", "User");
            }
        }

        [HttpPost]
        public ActionResult Key(DoorViewModel doorViewModel)
        {
            try
            {
                var currentUser = SessionHelper.CurrentUserViewModel;
                if (currentUser != null)
                {
                    var doorLocked = (bool)TempData["doorLocked"];
                    doorViewModel.Locked = !doorLocked;
                    //DoorAccessLogViewModel log = new DoorAccessLogViewModel()
                    //{
                    //    User = currentUser,
                    //    Door = doorViewModel,
                    //    Date = DateTime.Now,
                    //    AccessDenied = false
                    //};

                    doorService.LogAccessToDoorByUser(doorViewModel.Id, currentUser.Id, doorViewModel.Locked, false);
                    doorService.AccessToDoor(doorViewModel.Id, doorViewModel.Locked);
                    TempData["doorLocked"] = !doorLocked;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(doorViewModel);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DoorViewModel doorViewModel)
        {
            try
            {
                var currentUser = SessionHelper.CurrentUserViewModel;
                if (currentUser != null)
                {
                    doorService.Create(doorViewModel.Address, doorViewModel.Name, currentUser.Id);
                    return RedirectToAction("MyDoors", "User");
                }

                return RedirectToAction("Authentication", "User");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Door id</param>
        /// <returns></returns>
        public ActionResult AddAccess(int id)
        {
            var doorAccessViewModel = doorService.GetDoorAccessForEditViewModel(id);
            return View(doorAccessViewModel);
        }

        [HttpPost]
        public ActionResult AddAccess(DoorAccessForEditViewModel doorAccessViewModel)
        {
            try
            {
                doorService.CreateNewAuthorisation(doorAccessViewModel);
                return RedirectToAction("Details", new { id = doorAccessViewModel.Door.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }
    }
}