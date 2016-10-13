using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ABS.Models;

namespace ABS.Web.Areas.HRPayroll.Hubs
{
    public class NotificationHubsHRPayroll : Hub
    {
        public void SendMessageHRPayroll(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("broadcastDataHRPayroll")]
        public static void BroadcastDataHRPayroll(NotificationEntity ent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHubsHRPayroll>();
            context.Clients.All.updatedDataHRPayroll(ent);
        }
    }

}
