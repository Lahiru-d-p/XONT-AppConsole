using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace XONT.Ventura.AppConsole
{
    public class NotificationHub : Hub
    {
        public void NotifyAllClients(string msg)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.displayNotification(msg);
        }


    }
}