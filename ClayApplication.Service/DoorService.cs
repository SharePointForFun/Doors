using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ClayApplication.DataAccess;
using ClayApplication.Service.ViewModel;
using ClayApplication.Domain.Abstract;
using ClayApplication.Service.Extension;
using Omu.ValueInjecter;

namespace ClayApplication.Service
{
    public class DoorService: IDoorService
    {
        private readonly IRepo<Door> doorRepo;
        private readonly IRepo<User> userRepo;
        private readonly IRepo<DoorAccessLog> doorAccessLogRepo;
        private readonly IRepo<DoorAccess> doorAccessRepo;
        
        public DoorService(
            IRepo<Door> doorRepo, 
            IRepo<User> userRepo, 
            IRepo<DoorAccessLog> doorAccessLogRepo, 
            IRepo<DoorAccess> doorAccessRepo)
        {
            this.doorRepo = doorRepo;
            this.userRepo = userRepo;
            this.doorAccessLogRepo = doorAccessLogRepo;
            this.doorAccessRepo = doorAccessRepo;
        }

        // Door repo
        public DoorViewModel GetDoor(int id)
        {
            var doorObject = doorRepo.Get(id);
            var doorViewModel = doorObject.GetViewModel();
            return doorViewModel;
        }

        // Door repo
        public void Create(string address, string name, int ownerId)
        {
            try
            {
                var door = new Door()
                {
                    address = address,
                    name = name,
                    User = userRepo.Get(ownerId)
                };

                doorRepo.Insert(door);
                doorRepo.Save();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        // Door repo
        public void AccessToDoor(int doorId, bool locked)
        {
            try
            {
                var doorObject = doorRepo.Get(doorId);
                doorObject.locked = locked;
                doorRepo.Save();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        // doorAccessRepo
        public void CreateNewAuthorisation(DoorAccessForEditViewModel authorizedUser)
        {
            try
            {
                var doorAccess = new DoorAccess()
                {
                    userid = authorizedUser.SelectedUserId,
                    doorid = authorizedUser.Door.Id,
                    autherized = authorizedUser.IsSelectedUserAuthorized
                };

                doorAccessRepo.Insert(doorAccess);
                doorAccessRepo.Save();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        // doorRepo
        // doorAccessRepo
        public DoorAccessForEditViewModel GetDoorAccessForEditViewModel(int doorId)
        {
            DoorAccessForEditViewModel doorAccessViewModel = null;
            doorAccessViewModel = new DoorAccessForEditViewModel()
            {
                Door = doorRepo.Get(doorId).GetViewModel(),
                UsersListSelection = GetNotAuthorizedUsersByDoorId(doorId)
            };

            return doorAccessViewModel;
        }

        // DoorAccessLog repo
        public void LogAccessToDoorByUser(int doorId, int userId, bool locked, bool accessDenied)
        {
            try
            {
                var doorAccessLog = new DoorAccessLog() 
                {
                    date = DateTime.Now,
                    accessdenied = accessDenied,
                    state = locked,
                    userid = userId,
                    doorid = doorId
                };

                doorAccessLogRepo.Insert(doorAccessLog);
                doorAccessLogRepo.Save();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


        // Door repo
        public DoorAccessViewModel GetDoorAccess(int id)
        {
            var doorObject = doorRepo.Get(id);
            var doorAccess = new DoorAccessViewModel()
            {
                Door = doorObject.GetViewModel(),
            };

            doorAccess.AuthorizedUsers = new List<AuthorizedUserViewModel>();
            foreach (var access in doorObject.DoorAccess)
            {
                doorAccess.AuthorizedUsers.Add(new AuthorizedUserViewModel()
                {
                    User = userRepo.Get(access.userid).GetViewModel(),
                    IsAuthorized = access.autherized ?? false
                });
            }

            return doorAccess;
        }

        // doorRepo
        public List<DoorAccessLogViewModel> GetAccessLog(int doorId)
        {
            var doorObject = doorRepo.Get(doorId);
            var doorAccessLog = new List<DoorAccessLogViewModel>();
            foreach (var log in doorObject.DoorAccessLog)
            {
                doorAccessLog.Add(log.GetViewModel());
            }

            return doorAccessLog;
        }

        // doorRepo
        // doorAccessRepo
        public List<UserViewModel> GetNotAuthorizedUsersByDoorId(int doorId)
        {
            var notAuthorizedUsers = new List<UserViewModel>();
            try
            {
                var ownerId = doorRepo.Get(doorId).ownerid;
                var usersIds = doorAccessRepo.Where(x => x.doorid == doorId).Select(x => x.userid);
                if (usersIds != null)
                {
                    var usersNotAuthorized = userRepo.GetAll().Select(x => x.id)
                        .Except(usersIds.ToList());
                    var notAuthorizedUsersTmp = userRepo.Where(x => usersNotAuthorized.Contains(x.id) && x.id != ownerId);
                    if (notAuthorizedUsersTmp != null)
                    {
                        var notAuthorizedUsersList = notAuthorizedUsersTmp.ToList();
                        foreach (var authUser in notAuthorizedUsersList)
                        {
                            notAuthorizedUsers.Add(authUser.GetViewModel());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return notAuthorizedUsers;
        }

        // doorAccessRepo
        public bool UserHasAccessToDoor(DoorViewModel door, UserViewModel user)
        {
            DoorAccess doorAccess = null;
            if (IsUserDoorOwner(door.Id, user.Id))
            {
                return true;
            }

            try
            {
                try
                {
                    var doorAccessQueryable = doorAccessRepo.Where(x => x.userid == user.Id && x.autherized == true);
                    doorAccess = (DoorAccess)doorAccessQueryable.First();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return doorAccess != null;
        }

        // doorRepo
        public bool IsUserDoorOwner(int doorId, int userId)
        {
            bool isDoorOwner = false;
            try
            {
                var doorObject = doorRepo.Get(doorId);
                if (doorObject != null)
                {
                    isDoorOwner = doorObject.User.id == userId;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return isDoorOwner;
        }
    }
}
