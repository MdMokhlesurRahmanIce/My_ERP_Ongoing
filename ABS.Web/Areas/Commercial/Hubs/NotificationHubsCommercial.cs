using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.Commercial.Hubs
{
    public class NotificationHubsCommercial : Hub
    {
        public void SendMessageCommercial(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataCommercial")]
        public static void BroadcastDataCommercial(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsCommercial>();
            context.Clients.All.updatedDataCommercial(ent);
        }
    }

}
