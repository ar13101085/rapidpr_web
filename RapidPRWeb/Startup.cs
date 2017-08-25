using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RapidPRWeb.Startup))]
namespace RapidPRWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
