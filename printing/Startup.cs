using System.Linq;
using System.Web;
using printing.Comparers;
using printing.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(printing.Startup))]
namespace printing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
