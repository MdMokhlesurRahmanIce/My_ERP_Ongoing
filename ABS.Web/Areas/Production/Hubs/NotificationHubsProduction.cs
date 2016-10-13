using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.Production.Hubs
{
    public class NotificationHubsProduction : Hub
    {
        public void SendMessageProduction(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataProduction")]
        public static void BroadcastDataProduction(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsProduction>();
            context.Clients.All.updatedDataProduction(ent);
        }
    }

}
