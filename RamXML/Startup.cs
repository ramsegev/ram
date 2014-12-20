using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RamXML.Startup))]
namespace RamXML
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
