using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClayApplication.WebUI.Startup))]
namespace ClayApplication.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
