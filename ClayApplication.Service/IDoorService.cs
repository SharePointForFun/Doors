using ClayApplication.DataAccess;
using ClayApplication.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClayApplication.Service
{
    public interface IDoorService
    {
        void Create(string address, string name, int ownerId);
        DoorAccessViewModel GetDoorAccess(int id);
        DoorViewModel GetDoor(int id);
        List<DoorAccessLogViewModel> GetAccessLog(int doorId);
        void LogAccessToDoorByUser(int doorId, int userId, bool locked, bool accessDenied);
        void AccessToDoor(int doorId, bool locked);
        List<UserViewModel> GetNotAuthorizedUsersByDoorId(int doorId);
        DoorAccessForEditViewModel GetDoorAccessForEditViewModel(int doorId);
        void CreateNewAuthorisation(DoorAccessForEditViewModel authorizedUser);
        bool UserHasAccessToDoor(DoorViewModel door, UserViewModel user);
        bool IsUserDoorOwner(int doorId, int userId);
    }
}
