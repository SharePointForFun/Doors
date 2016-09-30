using ClayApplication.DataAccess;
using ClayApplication.Domain.Abstract;
using ClayApplication.Service.ViewModel;
using ClayApplication.Service.Extension;
using System;
using System.Linq;

namespace ClayApplication.Service
{
    public class DoorAccessService : IDoorAccessService
    {
        private readonly IRepo<DoorAccess> doorAccessRepo;

        public DoorAccessService(IRepo<DoorAccess> doorAccessRepo)
        {
            this.doorAccessRepo = doorAccessRepo;
        }

        public AuthorizedUserForEditViewModel GetDoorAccessByDoorAndUser(int doorId, int userId)
        {
            DoorAccess doorAccessObject = null;
            try
            {
                var doorAccessQueryable = doorAccessRepo.Where(x => x.doorid.Equals(doorId) && x.userid.Equals(userId));
                doorAccessObject = (DoorAccess)doorAccessQueryable.First();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return doorAccessObject.GetViewModelForEdit();
        }

        public void EditAuthorization(AuthorizedUserForEditViewModel authorizedUser)
        {
            try
            {
                var doorAccessObject = doorAccessRepo.Where(x => x.Door.id == authorizedUser.DoorId
                                                        && x.userid == authorizedUser.UserId);
                if(doorAccessObject != null)
                {
                    DoorAccess doorAccess = (DoorAccess)doorAccessObject.First();
                    if (doorAccess != null)
                    {
                        doorAccess.autherized = authorizedUser.IsAuthorized;
                        doorAccessRepo.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void CreateNewAuthorisation(AuthorizedUserViewModel authorizedUser)
        {
            try
            {
                var doorAccess = new DoorAccess()
                {
                    userid = authorizedUser.User.Id,
                    doorid = authorizedUser.Door.Id,
                    autherized = authorizedUser.IsAuthorized
                };

                doorAccessRepo.Insert(doorAccess);
                doorAccessRepo.Save();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
