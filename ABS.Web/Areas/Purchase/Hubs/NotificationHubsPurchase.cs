using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.Purchase.Hubs
{
    public class NotificationHubsPurchase : Hub
    {
        public void SendMessagePurchase(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataPurchase")]
        public static void BroadcastDataPurchase(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsPurchase>();
            context.Clients.All.updatedDataPurchase(ent);
        }
    }

}
