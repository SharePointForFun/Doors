using ClayApplication.DataAccess;
using ClayApplication.Domain.Abstract;
using ClayApplication.Service.ViewModel;
using ClayApplication.Service.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClayApplication.Service
{
    public class UserService: IUserService
    {
        private readonly IRepo<User> userRepo;
        private readonly IRepo<DoorAccess> doorAccessRepo;
        public UserService(IRepo<User> userRepo, IRepo<DoorAccess> doorAccessRepo)
        {
            this.userRepo = userRepo;
            this.doorAccessRepo = doorAccessRepo;
        }

        public UserViewModel GetUser(string login, string pwd)
        {
            var user = userRepo.Where(x => x.login.Equals(login) && x.pwd.Equals(pwd));
            if(user != null)
            {
                return user.First().GetViewModel();
            }
            
            return null;
        }

        public UserViewModel GetUser(int id)
        {
            var user = userRepo.Get(id);
            return user.GetViewModel();
        }

        public bool DoesUserExist(string login, string pwd)
        {
            bool found = false;
            try
            {
                var userQueryable = userRepo.Where(x => x.login == login && x.pwd == pwd);
                found = userQueryable != null;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return found;
        }

        public ICollection<UserViewModel> GetAll()
        {
            var users = new List<UserViewModel>();
            try
            {
                var usersObjects = userRepo.GetAll();
                if (usersObjects != null)
                {
                    foreach (var userObject in usersObjects)
                    {
                        users.Add(userObject.GetViewModel());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return users;
        }

        public IEnumerable<DoorViewModel> GetAllDoorsByUserId(int userId)
        {
            var doors = new List<DoorViewModel>();
            try
            {
                var user = userRepo.Get(userId);
                if (user != null)
                {
                    foreach (var door in user.Door)
                    {
                        doors.Add(door.GetViewModel());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            
            return doors;
        }

        public IEnumerable<DoorViewModel> DoorsUserHasAccess(int id)
        {
            var doors = new List<DoorViewModel>();
            try
            {
                var doorAccessObjects = doorAccessRepo.Where(x => x.userid == id);
                var doorAccessObjectList = doorAccessObjects.ToList();
                if (doorAccessObjects != null)
                {
                    foreach (var doorAccessObject in doorAccessObjectList)
                    {
                        doors.Add(doorAccessObject.Door.GetViewModel());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return doors;
        }
    }
}
