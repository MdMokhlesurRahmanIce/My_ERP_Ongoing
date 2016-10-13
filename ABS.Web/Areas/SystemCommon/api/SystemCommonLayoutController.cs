using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.MenuMgt;
using ABS.Web.Areas.SystemCommon.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using ABS.Utility;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ABS.Web.Areas.SystemCommon.api
{
     [RoutePrefix("SystemCommon/api/SystemCommonLayout")]
    public class SystemCommonLayoutController : ApiController
    {
        private iMenuMgt objService = null;

        public SystemCommonLayoutController()
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
        public object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            //  Thread.Sleep(100);
            objService = new MenuMgt();
            var type = objService.GetSideMenu(companyID, loggedUser, ModuleID);
            return type;
        }

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
            objService = new MenuMgt();
            var menuist = objService.GetMenuPermission(menu);
            return menuist;
        }


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
                #region  old
                // Required CompnayID, IsApproved = 1, Comments, Isupdate false,isDelete false ,StatusID = 1
                //String Message = System.Enum.GetName(typeof(workFlowTranEnum_MessageName), (int)workFlowTranEnum_MessageName.Appoved);
                //int WorkFlowStatus = new WorkFLowMgt().ExecuteWorkFlowTransactionProcess(new WorkFLowMgt().SetProcedureParamForApprove(model, (int)model.LoggedCompanyID, (int)workFlowTranEnum_IsApproved.True, model.Comments, (int)workFlowTranEnum_IsUpdate.False, (int)workFlowTranEnum_IsDelete.False, (int)workFlowTranEnum_Status.Active, model.CustomCode, (int)workFlowTranEnum_IsDeclained.False, Message));
                //if (WorkFlowStatus == 0)
                //{
                //    vmNotificationMail nModel = new WorkFLowMgt().GetNotificationMailObject(model, Message);
                //    returnValue = await new EmailService().NotificationMail(nModel);
                //    // if(returnValue==1)
                //    NotificationHubs.BroadcastData(new NotificationEntity());
                //    returnValue = 200;
                //}
                #endregion old

                #region  New

                int WorkFlowStatus = 1;
                if (WorkFlowStatus == 1)
                {
                    returnValue = new WorkFLowMgt().ApproveProcess(model);
                   List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectList(model, "");
                    foreach (var item in nModel)
                    {
                       returnValue = await new EmailService().NotificationMail(item);
                    }
                    NotificationHubs.BroadcastData(new NotificationEntity());
                    returnValue = 200;
                }

                #endregion New

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
                #region Old 
                //Required CompnayID,IsApproved=0,Comments,Isupdate false,isDelete false ,StatusID=1
                //String MessageName = System.Enum.GetName(typeof(workFlowTranEnum_MessageName), (int)workFlowTranEnum_MessageName.Declined);
                //int WorkFlowStatus = new WorkFLowMgt().ExecuteWorkFlowTransactionProcess(new WorkFLowMgt().SetProcedureParamForDeclained(model, (int)model.LoggedCompanyID, (int)workFlowTranEnum_IsApproved.False, model.Comments, (int)workFlowTranEnum_IsUpdate.False, (int)workFlowTranEnum_IsDelete.False, (int)workFlowTranEnum_Status.Active, model.CustomCode, (int)workFlowTranEnum_IsDeclained.True, MessageName));
                //if (WorkFlowStatus == 0)
                //{
                //    NotificationHubs.BroadcastData(new NotificationEntity());
                //    returnValue = 201;
                //}
                #endregion old
                #region New 
                returnValue = new WorkFLowMgt().DeclinedProcess(model);
                List<vmNotificationMail> nModel = new WorkFLowMgt().GetNotificationMailObjectListDeclined(model, "");
                foreach (var item in nModel)
                {
                    returnValue = await new EmailService().NotificationMail(item);
                }
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