using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyAdmin.Startup))]
namespace MyAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
