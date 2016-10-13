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
using ABS.Web.Areas.Inventory.Hubs;
using ABS.Web.Areas.SystemCommon.Hubs;

namespace ABS.Web.Areas.Inventory.api
{
    [RoutePrefix("Inventory/api/InventoryLayout")]
    public class InventoryLayoutController : ApiController
    {
        private iMenuMgt objService = null;

        public InventoryLayoutController()
        {
            this.objService = new MenuMgt();
        }
        
        // GET: GetBreadCrums/1 
        [Route("GetBreadCrums/{menuID:int}")]
        [HttpGet]
        public List<vmBreadCrums> GetBreadCrums(int? menuID)
        {
            //  Thread.Sleep(100);
            var type = objService.GetBreadCrums(menuID);
            return type;
        }
        #region GetAll Layout Menus
        // GET: GetMenuPermissionByParam/1/10/0
        [Route("GetSideMenu/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]
        // [ResponseType(typeof(vmCmnCustomCode))]
        [HttpGet]
        public dynamic GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            //  Thread.Sleep(100);
            var type = objService.GetSideMenu(companyID, loggedUser, ModuleID);
            return type;
        }

        #region GetAll Layout Menus       

        [Route("GetTopMenu/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]
       
        [HttpGet]
        public List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            //  Thread.Sleep(100);
           var type = objService.GetTopMenu(companyID, loggedUser, ModuleID);
            return type;
        }

        [HttpPost]
        public object GetMenuPermission(object[] data)
        {
            vmApplicationTokenModel menu = JsonConvert.DeserializeObject<vmApplicationTokenModel>(data[0].ToString());
            string menuPath = menu.MenuPath;
            var menuist = objService.GetMenuPermission(menu);
            return menuist;
        }


        #endregion  GetAll Layout Menus



        #endregion  GetAll Layout Menus
        #region GetAll Top Navigation List
        // GET: GetMenuPermissionByParam/1/10/0
        [Route("GetNotificationInfo/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]
        // [ResponseType(typeof(vmCmnCustomCode))]
        [HttpGet]
        public dynamic GetNotificationInfo(int? companyID, int? loggedUser, int? ModuleID)
        {
            //  Thread.Sleep(100);
            //var type = objService.GetNotificationInfo(companyID, loggedUser, ModuleID);
            var type = objService.GetNotificationInfoes(companyID, loggedUser, ModuleID);
            return type;
        }
        #endregion   GetAll Top Navigation List        
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
                    //NotificationHubsInventory.BroadcastDataInventory(new NotificationEntity());
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
               // NotificationHubsInventory.BroadcastDataInventory(new NotificationEntity());
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