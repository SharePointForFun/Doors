using ClayApplication.DataAccess;
using ClayApplication.Service.ViewModel;

namespace ClayApplication.Service
{
    public interface IDoorAccessService
    {
        void CreateNewAuthorisation(AuthorizedUserViewModel authorizedUser);
        AuthorizedUserForEditViewModel GetDoorAccessByDoorAndUser(int doorId, int userId);
        void EditAuthorization(AuthorizedUserForEditViewModel authorizedUser);
    }
}
