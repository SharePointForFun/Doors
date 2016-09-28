using ClayApplication.WebUI.Helpers;
using ClayApplication.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClayApplication.Service.ViewModel;
using ClayApplication.Service;

namespace ClayApplication.WebUI.Controllers
{
    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var userViewModel = this.userService.GetUser(id);
            return View(userViewModel);
        }

        public ActionResult Authentication()
        {
            var currentUser = new UserViewModel();
            return View(currentUser);
        }

        [HttpPost]
        public ActionResult Authentication(UserViewModel userViewModel)
        {
            if (userViewModel != null 
                && !string.IsNullOrEmpty(userViewModel.Login)
                && !string.IsNullOrEmpty(userViewModel.Password))
            {
                if (userService.DoesUserExist(userViewModel.Login, userViewModel.Password))
                {
                    var userObject = userService.GetUser(userViewModel.Login, userViewModel.Password);
                    SessionHelper.CurrentUserViewModel = userObject;
                    return RedirectToAction("MyDoors");
                }
                else
                {
                    return View();
                }
            }

            return View();
        }

        public ActionResult DoorsIHaveAccess()
        {
            if (SessionHelper.CurrentUserViewModel != null)
            {
                var userViewModel = SessionHelper.CurrentUserViewModel;
                var otherDoors = userService.DoorsUserHasAccess(userViewModel.Id);
                var myDoors = userService.GetAllDoorsByUserId(userViewModel.Id);
                userViewModel.DoorsIAccess = new List<DoorViewModel>();
                userViewModel.DoorsIAccess = otherDoors.Concat(myDoors).ToList();
                return View(userViewModel);
            }
            else
            {
                return RedirectToAction("Authentication");
            }
        }

        public ActionResult MyDoors()
        {
            if (SessionHelper.CurrentUserViewModel != null)
            {
                var userViewModel = SessionHelper.CurrentUserViewModel;
                userViewModel.MyDoors = userService.GetAllDoorsByUserId(userViewModel.Id).ToList();
                return View(userViewModel);
            }
            else
            {
                return RedirectToAction("Authentication");
            }
        }
    }
}
