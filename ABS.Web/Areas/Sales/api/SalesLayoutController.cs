using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.MenuMgt;
using ABS.Web.Areas.Sales.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using ABS.Utility;
using System.Threading.Tasks;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using ABS.Web.Areas.SystemCommon.Hubs;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/SalesLayout")]
    public class SalesLayoutController : ApiController
    {
        private iMenuMgt objService = null;

        public SalesLayoutController()
        {
            this.objService = new MenuMgt();
        }
        
        [Route("GetBreadCrums/{menuID:int}")]
        [HttpGet]
        public List<vmBreadCrums> GetBreadCrums(int? menuID)
        {
            var type = objService.GetBreadCrums(menuID);
            return type;
        }

        [Route("GetSideMenu/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]
        [HttpGet]
        public object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            objService = new MenuMgt();
            var type = objService.GetSideMenu(companyID, loggedUser, ModuleID);


            return type;
        }    

        [Route("GetTopMenu/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]       
        [HttpGet]
        public List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
           var type = objService.GetTopMenu(companyID, loggedUser, ModuleID);
            return type;
        }

        [HttpPost]
        public object GetMenuPermission(object[] data)
        {
            vmApplicationTokenModel menu = JsonConvert.DeserializeObject<vmApplicationTokenModel>(data[0].ToString());
            string menuPath = menu.MenuPath;
            objService = new MenuMgt();
            var menuist = objService.GetMenuPermission(menu);
            return menuist;
        }

       
        [Route("GetNotificationInfo/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]
        [HttpGet]
        public dynamic GetNotificationInfo(int? companyID, int? loggedUser, int? ModuleID)
        {
            var type = objService.GetNotificationInfoes(companyID, loggedUser, ModuleID);
            return type;
        }
    
        [HttpPost]
        public async Task<dynamic> ApproveNotification(NotificationEntity model)
        {
            int returnValue = 0;
            try
            {              
                int WorkFlowStatus = 1;
                if (WorkFlowStatus == 1)
                {
                    returnValue = new WorkFLowMgt().ApproveProcess(model);
                    List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(model, "");
                    foreach (var item in nModel)
                    {
                        returnValue = await new EmailService().NotificationMail(item);
                    }
                    //NotificationHubsSales.BroadcastDataSales(new NotificationEntity());
                    NotificationHubs.BroadcastData(new NotificationEntity());
                    returnValue = 200;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return returnValue;
        }
        [HttpPost]
        public async Task<dynamic> DeclainedNotification(NotificationEntity model)
        {
            int returnValue = 0;
            try
            {                
                #region New
                returnValue = new WorkFLowMgt().DeclinedProcess(model);
                List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectListDeclined(model, "");
                foreach (var item in nModel)
                {
                    returnValue = await new EmailService().NotificationMail(item);
                }
                //NotificationHubsSales.BroadcastDataSales(new NotificationEntity());
                NotificationHubs.BroadcastData(new NotificationEntity());
                returnValue = 201;
                #endregion New
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return returnValue;
        }       

    }
}