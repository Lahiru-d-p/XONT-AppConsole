using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XONT.Ventura.AppConsole.LCSK.Startup1))]

namespace XONT.Ventura.AppConsole.LCSK
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
