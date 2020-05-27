using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ApartmentsRUS.Startup))]
namespace ApartmentsRUS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
