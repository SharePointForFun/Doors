using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using ClayApplication.Domain.Concrete;
using ClayApplication.Domain.Abstract;
using ClayApplication.Service;
using ClayApplication.DataAccess;


namespace SportsStore.WebUI.Infrastructure
{

    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext
            requestContext, Type controllerType)
        {

            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IDoorService>().To<DoorService>();
            ninjectKernel.Bind<IUserService>().To<UserService>();
            ninjectKernel.Bind<IDoorAccessService>().To<DoorAccessService>();
            ninjectKernel.Bind<IDbContextFactory>().To<DbContextFactory>();
            ninjectKernel.Bind<IRepo<Door>>().To<Repo<Door>>();
            ninjectKernel.Bind<IRepo<User>>().To<Repo<User>>();
            ninjectKernel.Bind<IRepo<DoorAccess>>().To<Repo<DoorAccess>>();
            ninjectKernel.Bind<IRepo<DoorAccessLog>>().To<Repo<DoorAccessLog>>();
        }
    }
}
