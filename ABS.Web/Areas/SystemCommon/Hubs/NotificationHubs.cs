using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.SystemCommon.Hubs
{
    public class NotificationHubs : Hub
    {
        public void SendMessage(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastData")]
        public static void BroadcastData(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubs>();
            context.Clients.All.updatedData(ent);
        }
    }

}
