using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.Planning.Hubs
{
    public class NotificationHubsPlanning : Hub
    {
        public void SendMessagePlanning(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataPlanning")]
        public static void BroadcastDataPlanning(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsPlanning>();
            context.Clients.All.updatedDataPlanning(ent);
        }
    }

}
