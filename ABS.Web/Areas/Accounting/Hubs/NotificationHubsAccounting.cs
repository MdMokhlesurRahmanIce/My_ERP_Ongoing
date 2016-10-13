using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.Accounting.Hubs
{
    public class NotificationHubsAccounting : Hub
    {
        public void SendMessageAccounting(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataAccounting")]
        public static void BroadcastDataAccounting(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsAccounting>();
            context.Clients.All.updatedDataAccounting(ent);
        }
    }

}
