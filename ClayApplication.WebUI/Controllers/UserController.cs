using ClayApplication.WebUI.Helpers;
using ClayApplication.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClayApplication.Service.ViewModel;

namespace ClayApplication.WebUI.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var user = this.userRepository.Get(id);
            var userViewModel = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

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
                if (userRepository.DoesUserExist(userViewModel.Login, userViewModel.Password))
                {
                    var userObject = userRepository.Get(userViewModel.Login, userViewModel.Password);
                    userViewModel.FirstName = userObject.FirstName;
                    userViewModel.LastName = userObject.LastName;
                    userViewModel.Id = userObject.id;
                    SessionHelper.CurrentUserViewModel = userViewModel;
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
                var user = this.userRepository.Get(userViewModel.Id);
                userViewModel.DoorsHaveAccessTo = new List<DoorViewModel>();
                foreach (var doorAccess in user.DoorAccess)
                {
                    userViewModel.DoorsHaveAccessTo.Add(new DoorViewModel()
                    {
                        Id = doorAccess.Door.id,
                        Name = doorAccess.Door.name,
                        Address = doorAccess.Door.address,
                        Owner = new UserViewModel()
                        {
                            FirstName = doorAccess.Door.User.FirstName,
                            LastName = doorAccess.Door.User.LastName
                        },
                        Locked = doorAccess.Door.locked ?? true
                    });
                }

                foreach (var myDoor in user.Door)
                {
                    userViewModel.DoorsHaveAccessTo.Add(new DoorViewModel()
                    {
                        Id = myDoor.id,
                        Name = myDoor.name,
                        Address = myDoor.address,
                        Owner = new UserViewModel()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName
                        },
                        Locked = myDoor.locked ?? true
                    });
                }

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
                var user = this.userRepository.Get(userViewModel.Id);
                userViewModel.DoorsOwns = new List<DoorViewModel>();
                foreach (var door in user.Door)
                {
                    userViewModel.DoorsOwns.Add(new DoorViewModel()
                    {
                        Id = door.id,
                        Locked = door.locked ?? true,
                        Name = door.name,
                        Address = door.address
                    });
                }

                return View(userViewModel);
            }
            else
            {
                return RedirectToAction("Authentication");
            }
        }
    }
}
