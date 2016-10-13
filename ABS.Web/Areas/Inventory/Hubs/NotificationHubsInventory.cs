using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.Inventory.Hubs
{
    public class NotificationHubsInventory : Hub
    {
        public void SendMessageInventory(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataInventory")]
        public static void BroadcastDataInventory(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsInventory>();
            context.Clients.All.updatedDataInventory(ent);
        }
    }

}
