using ClayApplication.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClayApplication.Domain.Abstract
{
    public interface IDoorRepostory
    {
        void Create(string address, string name, int ownerId);
        Door Get(int id);
        void LogAccessToDoorByUser(int doorId, int userId, bool locked, bool accessDenied);
        void AccessToDoor(int doorId, bool locked);
        List<User> GetNotAuthorizedUsersByDoorId(int doorId);
        void CreateNewAuthorisation(int doorId, int userId, bool authorized);
        bool UserHasAccessToDoor(int doorId, int userId);
        bool UserIsDoorOwner(int doorId, int userId);
        DoorAccess GetDoorAccessByDoorAndUser(int doorId, int userId);
        void EditAuthorization(int doorId, int userId, bool authorized);
    }
}
