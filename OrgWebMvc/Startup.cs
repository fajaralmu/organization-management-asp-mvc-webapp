using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrgWebMvc.Startup))]
namespace OrgWebMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
