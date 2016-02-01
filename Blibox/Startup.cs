using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blibox.Startup))]
namespace Blibox
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
