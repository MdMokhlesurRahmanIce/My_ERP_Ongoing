using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.Sales.Hubs
{
    public class NotificationHubsSales : Hub
    {
        public void SendMessageSales(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataSales")]
        public static void BroadcastDataSales(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsSales>();
            context.Clients.All.updatedDataSales(ent);
        }
    }

}
