using System;
using System.Collections.Generic;
using System.Linq;
using ClayApplication.DataAccess;
using ClayApplication.Domain.Abstract;

namespace ClayApplication.Domain.Concrete
{
    public class DoorRepository : IDoorRepository, IDisposable
    {
        private ClayDbEntities clayEntities;

        public DoorRepository()
        {
            clayEntities = new ClayDbEntities();
        }

        public Door Get(int id)
        {
            Door doorObject = null;
            try
            {
                doorObject = clayEntities.Door.Find(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return doorObject;
        }

        public void Create(string address, string name, int ownerId)
        {
            try
            {
                var door = clayEntities.Door.Create();
                var owner = clayEntities.User.Find(ownerId);
                door.name = name;
                door.address = address;
                owner.Door.Add(door);
                this.clayEntities.SaveChanges();
            }
            catch(Exception ex) 
            {
                Console.Write(ex.Message);
            }
        }

        public List<User> GetNotAuthorizedUsersByDoorId(int doorId)
        {
            var notAuthorizedUsers = new List<User>();
            try
            {
                var ownerId = clayEntities.Door.Find(doorId).ownerid;
                var usersNotAuthorized = clayEntities.User.Select(x => x.id)
                    .Except(clayEntities.DoorAccess.Where(x => x.doorid == doorId).Select(x => x.userid));
                notAuthorizedUsers = clayEntities.User.Where(x => usersNotAuthorized.Contains(x.id) && x.id != ownerId).ToList();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return notAuthorizedUsers;
        }

        public void LogAccessToDoorByUser(int doorId, int userId, bool locked, bool accessDenied)
        {
            try
            {
                var doorAccessLog = clayEntities.DoorAccessLog.Create();
                var userObject = clayEntities.User.Find(userId);
                var doorObject = clayEntities.Door.Find(doorId);
                doorAccessLog.state = locked;
                doorAccessLog.date = DateTime.Now;
                doorAccessLog.accessdenied = accessDenied;
                doorAccessLog.Door = doorObject;
                doorAccessLog.User = userObject;
                clayEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public bool UserIsDoorOwner(int doorId, int userId)
        {
            bool isDoorOwner = false;
            try
            {
                var doorObject = clayEntities.Door.Find(doorId);
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

        public bool UserHasAccessToDoor(int doorId, int userId)
        {
            DoorAccess doorAccess = null;
            if (UserIsDoorOwner(doorId, userId) == false)
            {
                try
                {
                    var doorObject = clayEntities.Door.Find(doorId);
                    try
                    {
                        doorAccess = doorObject.DoorAccess.FirstOrDefault(x => x.userid == userId && x.autherized == true);
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
            }
            else
            {
                return true;
            }

            return doorAccess != null;
        }

        public DoorAccess GetDoorAccessByDoorAndUser(int doorId, int userId)
        {
            DoorAccess doorAccessObject = null;
            try
            {
                doorAccessObject = clayEntities.DoorAccess
                    .FirstOrDefault(x => x.doorid == doorId && x.userid == userId);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return doorAccessObject;
        }

        public void AccessToDoor(int doorId, bool locked)
        {
            try
            {
                var doorObject = clayEntities.Door.Find(doorId);
                doorObject.locked = locked;
                clayEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void EditAuthorization(int doorId, int userId, bool authorized)
        {
            try
            {
                var doorAccess = clayEntities.DoorAccess
                    .FirstOrDefault(x => x.doorid == doorId && x.userid == userId);
                if (doorAccess != null)
                {
                    doorAccess.autherized = authorized;
                    clayEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void CreateNewAuthorisation(int doorId, int userId, bool authorized)
        {
            try
            {
                var doorAccess = clayEntities.DoorAccess.Create();
                var door = clayEntities.Door.Find(doorId);
                var user = clayEntities.User.Find(userId);
                doorAccess.autherized = authorized;
                door.DoorAccess.Add(doorAccess);
                user.DoorAccess.Add(doorAccess);
                this.clayEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void Dispose()
        {
            if (clayEntities == null)
            {
                return;
            }
            else
            {
                clayEntities.Dispose();
                clayEntities = null;
            }
        }
    }
}