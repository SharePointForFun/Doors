using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClayApplication.Domain.Abstract;
using ClayApplication.WebUI.Helpers;
using ClayApplication.Service.ViewModel;

namespace ClayApplication.WebUI.Controllers
{
    public class DoorController : Controller
    {
        private IDoorRepostory doorRepository;

        public DoorController(IDoorRepostory doorRepository)
        {
            this.doorRepository = doorRepository;
        }

        public ActionResult ErrorPage(string message)
        {
            var errorMessage = new ErrorMessage() { Message = message };
            return View(errorMessage);
        }

        public ActionResult Details(int id)
        {
            var doorObject = doorRepository.Get(id);
            var doorAccess = new DoorAccessViewModel()
            {
                DoorId = doorObject.id,
                DoorName = doorObject.name,
                DoorAddress = doorObject.address
            };

            doorAccess.AuthorizedUsers = new List<AuthorizedUserViewModel>();
            foreach (var access in doorObject.DoorAccess)
            {
                doorAccess.AuthorizedUsers.Add(new AuthorizedUserViewModel()
                { 
                    UserId = access.userid,
                    DoorId = doorObject.id,
                    UserName = string.Format("{0} {1}", access.User.FirstName, access.User.LastName),
                    Authorized = access.autherized ?? false
                });
            }

            return View(doorAccess);
        }

        public ActionResult EditAuthorisation(int doorId, int userId)
        {
            var authorizedUser = new AuthorizedUserViewModel();
            try
            {
                var doorAccessObject = doorRepository.GetDoorAccessByDoorAndUser(doorId, userId);
                if (doorAccessObject != null)
                {
                    authorizedUser.UserId = userId;
                    authorizedUser.DoorId = doorId;
                    authorizedUser.Authorized = doorAccessObject.autherized ?? false;
                    authorizedUser.UserName = string.Format("{0} {1}", doorAccessObject.User.FirstName, doorAccessObject.User.LastName);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return View(authorizedUser);
        }

        [HttpPost]
        public ActionResult EditAuthorisation(AuthorizedUserViewModel user)
        {
            try
            {
                if (user != null)
                {
                    doorRepository.EditAuthorization(user.DoorId, user.UserId, user.Authorized);
                    return RedirectToAction("Details", new { id = user.DoorId }); 
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return RedirectToAction("ErrorPage", new { message = "Something went wrong" });
        }

        public ActionResult ViewLogs(int id)
        {
            var currentUser = SessionHelper.CurrentUserViewModel;
            if (currentUser != null)
            {
                if (doorRepository.UserIsDoorOwner(id, currentUser.Id))
                {
                    var doorObject = doorRepository.Get(id);
                    if (doorObject != null)
                    {
                        var doorAccessLog = new List<DoorAccessLogViewModel>();
                        foreach (var log in doorObject.DoorAccessLog)
                        {
                            doorAccessLog.Add(new DoorAccessLogViewModel()
                            {
                                Date = log.date ?? DateTime.MinValue,
                                UserName = string.Format("{0} {1}", log.User.FirstName, log.User.LastName),
                                UserId = log.User.id,
                                DoorId = log.Door.id,
                                Status = log.state ?? true,
                                AccessDenied = log.accessdenied ?? false
                            });
                        }

                        return View(doorAccessLog);
                    }
                    else
                    {
                        // door not found
                        return RedirectToAction("ErrorPage", new { message = "Door could not be found"});
                    }
                }
                else
                {
                    // user not door owner
                    return RedirectToAction("ErrorPage", new { message = "User is not door owner" });
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
            var doorViewModel = new DoorViewModel();
            if (currentUser != null)
            {
                var doorObject = doorRepository.Get(id);
                if (doorObject != null)
                {
                    doorViewModel.Id = doorObject.id;
                    doorViewModel.Name = doorObject.name;
                    doorViewModel.Address = doorObject.address;
                    doorViewModel.Owner = new UserViewModel()
                    {
                        Id = doorObject.User.id,
                        FirstName = doorObject.User.FirstName,
                        LastName = doorObject.User.LastName
                    };

                    doorViewModel.Locked = doorObject.locked ?? true;
                    TempData["doorLocked"] = doorViewModel.Locked;
                    if (doorRepository.UserHasAccessToDoor(id, currentUser.Id))
                    {
                        return View(doorViewModel);
                    }
                    else
                    {
                        doorRepository.LogAccessToDoorByUser(doorViewModel.Id, currentUser.Id, doorViewModel.Locked, true);
                        return RedirectToAction("ErrorPage", new { message = "Access Denied" });
                    }
                }
                else
                {
                    // "Door could not be found"
                    return RedirectToAction("ErrorPage", new { message = "Door could not found" });
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
                    doorRepository.LogAccessToDoorByUser(doorViewModel.Id, currentUser.Id, doorViewModel.Locked, false);
                    doorRepository.AccessToDoor(doorViewModel.Id, doorViewModel.Locked);
                    TempData["doorLocked"] = !doorLocked;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(doorViewModel);
        }

        public ActionResult ManageAccess(int id)
        {
            var notAuthorizedUsers = doorRepository.GetNotAuthorizedUsersByDoorId(id);
            var doorAccessViewModel = new DoorAccessViewModel() { DoorId = id };
            doorAccessViewModel.UsersForAccess = new List<UserViewModel>();
            foreach (var userObject in notAuthorizedUsers)
            {
                doorAccessViewModel.UsersForAccess.Add(new UserViewModel()
                {
                    Id = userObject.id,
                    FirstName = userObject.FirstName,
                    LastName = userObject.LastName
                });
            }
            
            return View(doorAccessViewModel);
        }

        [HttpPost]
        public ActionResult ManageAccess(DoorAccessViewModel doorAccessViewModel)
        {
            try
            {
                var currentUser = SessionHelper.CurrentUserViewModel;
                if (currentUser != null)
                {
                    doorRepository.CreateNewAuthorisation(doorAccessViewModel.DoorId, doorAccessViewModel.SelectedUserId, doorAccessViewModel.Authorized);
                    return RedirectToAction("Details", new { id = doorAccessViewModel.DoorId });
                }
                else
                {
                    return RedirectToAction("Authentication", "User");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
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
                    doorRepository.Create(doorViewModel.Address, doorViewModel.Name, currentUser.Id);
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
    }
}