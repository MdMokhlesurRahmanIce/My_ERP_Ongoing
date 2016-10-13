using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Service.AllServiceClasses;
using ABS.Service.Production.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Factories
{
    public class InternalIssueMgt : iInternalIssue
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmSetSetupMasterDetail> GFactory_VM_PrdSetSetupMasterDetail = null;
        private iGenericFactory<vmIssueDetail> GFactory_VM_prdIssueDetail = null;
        private iGenericFactory_EF<PrdInternalIssue> GenericFactory_EFInternalIssue = null;
        private iGenericFactory_EF<PrdInternalIssueDetail> GenericFactory_EFInternalIssueDetails = null;
        public InternalIssueMgt()
        {
            GFactory_VM_PrdSetSetupMasterDetail = new PrdSetSetupMasterDetail_VM();
            GFactory_VM_prdIssueDetail = new prdIssueDetail();
          
        }
        public vmSetSetupMasterDetail GetSetDetailsBySetNo(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo)
        {
            vmSetSetupMasterDetail _vmSetSetupMasterDetails = null;
            string spQuery = string.Empty;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", 0);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("SetID", SetNo);
                    spQuery = "[GetPrdSetDetails]";
                    _vmSetSetupMasterDetails = GFactory_VM_PrdSetSetupMasterDetail.ExecuteQuery(spQuery, ht).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmSetSetupMasterDetails;


        }
      public  List<vmIssueDetail> GetIssueDetailBySetNO(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo)
        {
            List<vmIssueDetail> _vmIssueDetails = null;
            string spQuery = string.Empty;

            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", 1);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("SetID", SetNo);
                    spQuery = "[GetPrdIssueDeails]";
                    _vmIssueDetails = GFactory_VM_prdIssueDetail.ExecuteQuery(spQuery, ht).ToList();


                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmIssueDetails;
        }

       public List<vmIssueDetail> GetIssueDetailByIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? IssueID)
      {

          List<vmIssueDetail> _vmIssueDetails = null;
          string spQuery = string.Empty;

          try
          {
              using (_ctxCmn = new ERP_Entities())
              {
                  Hashtable ht = new Hashtable();
                  ht.Add("CompanyID", 1);
                  ht.Add("LoggedUser", 0);
                  ht.Add("PageNo", 0);
                  ht.Add("RowCountPerPage", 0);
                  ht.Add("IsPaging", 0);
                  ht.Add("IssueID", IssueID);
                  spQuery = "[GetPrdIssueDeailsByIssueID]";
                  _vmIssueDetails = GFactory_VM_prdIssueDetail.ExecuteQuery(spQuery, ht).ToList();


              }
          }
          catch (Exception e)
          {
              e.ToString();
          }

          return _vmIssueDetails;
      }


       public List<vmIssueDetail> GetInternalIssueDetial(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, bool IsIssuedBall, bool IsReceivedDy, bool IsIssuedDy, bool IsReceivedLCB)
      {
          List<vmIssueDetail> _vmIssueDetails = null;
          string spQuery = string.Empty;

          try
          {
              using (_ctxCmn = new ERP_Entities())
              {
                  Hashtable ht = new Hashtable();
                  ht.Add("CompanyID", CompanyId);
                  ht.Add("LoggedUser", 0);
                  ht.Add("PageNo", 0);
                  ht.Add("RowCountPerPage", 0);
                  ht.Add("IsPaging", 0);
                  ht.Add("IsIssuedBall", IsIssuedBall);
                  ht.Add("IsReceivedDy", IsReceivedDy);
                  ht.Add("IsIssuedDy", IsIssuedDy);
                  ht.Add("IsReceivedLCB", IsReceivedLCB);

                  spQuery = "[GetPrdInternalIssueDetails]";
                  _vmIssueDetails = GFactory_VM_prdIssueDetail.ExecuteQuery(spQuery, ht).ToList();


              }
          }
          catch (Exception e)
          {
              e.ToString();
          }

          return _vmIssueDetails;

      }
      public int SaveInternalIssue(List<vmIssueDetail> _objIssueDetails, vmInternalIssue _objInternalIssue)
      {
          int result = 0;
          try
          {
              if (_objInternalIssue.IssueID > 0)
              {
                  result = UpdateInteralIssueMasterAndIDetails(_objIssueDetails, _objInternalIssue);

              }
              else
              {

                  Int64 IssueID = SaveInteralIssueMaster(_objInternalIssue);
                  if (IssueID > 0)
                  {
                      SaveInternalIssueDetails(_objIssueDetails, IssueID, _objInternalIssue);
                  }
                  result = 1;
              }

            
          }
          catch (Exception)
          {
              result = 0;
              
          }
          return result;
      }

      private int UpdateInteralIssueMasterAndIDetails(List<vmIssueDetail> _objIssueDetails, vmInternalIssue _objInternalIssue)
      {
          int result = 0;
         
          try
          {
             result= UpdateInternalIssueMaster(_objInternalIssue);
              if(result>0)
              {
                  UpdateInternalIssuDetails(_objIssueDetails);
              }
          }
          catch (Exception)
          {
              
             
          }
          return result;
      }

      private void UpdateInternalIssuDetails(List<vmIssueDetail> _objIssueDetails)
      {
          try
          {
              GenericFactory_EFInternalIssueDetails = new PrdInternalIssueDetails_EF();
              foreach(vmIssueDetail aitem in _objIssueDetails)
              {
                  PrdInternalIssueDetail _InternalIssueDetailobj = GenericFactory_EFInternalIssueDetails.GetAll().Where(x => x.IssueDetailID == aitem.IssueDetailID).FirstOrDefault();
                  if(_InternalIssueDetailobj!=null)
                  {
                      _InternalIssueDetailobj.RopeNumber = aitem.RopeNumber;
                      _InternalIssueDetailobj.CanID = aitem.CanID;

                      GenericFactory_EFInternalIssueDetails.Update(_InternalIssueDetailobj);
                      GenericFactory_EFInternalIssueDetails.Save();

                  }


              }

          }
          catch (Exception)
          {
              
              
          }
      }

      private int UpdateInternalIssueMaster(vmInternalIssue _objInternalIssue)
      {
          try
          {
              GenericFactory_EFInternalIssue = new PrdInternalIssue_EF();
              PrdInternalIssue _internalIssueobj = GenericFactory_EFInternalIssue.GetAll().Where(x => x.IssueID == _objInternalIssue.IssueID).FirstOrDefault();
              if(_objInternalIssue.DepartmentID==7)// Ball 
              {
                  _internalIssueobj.IssBallDate = _objInternalIssue.IssBallDate;
                  _internalIssueobj.IssBallRemarks = _objInternalIssue.IssBallRemarks;


              }
              else if(_objInternalIssue.DepartmentID==8) //Dying
              {
                  if(_objInternalIssue.IssDyDate==null)
                  {
                      _internalIssueobj.IsReceivedDy = _objInternalIssue.IsReceivedDy;
                      _internalIssueobj.ReceivedDyDate = _objInternalIssue.ReceivedDyDate;
                      _internalIssueobj.ReceivedDyRemarks = _objInternalIssue.ReceivedDyRemarks;
                  }
                 else
                  {
                    _internalIssueobj.IsIssuedDy=_objInternalIssue.IsIssuedDy;
                    _internalIssueobj.IssDyDate = _objInternalIssue.IssDyDate;
                    _internalIssueobj.IssDyRemarks = _objInternalIssue.IssDyRemarks;


                  }

              }
              else if(_objInternalIssue.DepartmentID==9)// LCB
              {
                  _internalIssueobj.IsReceivedLCB = _objInternalIssue.IsReceivedLCB;
                  _internalIssueobj.ReceivedLCBDate = _objInternalIssue.ReceivedLCBDate;
                  _internalIssueobj.ReceivedLCBRemarks = _objInternalIssue.ReceivedLCBRemarks;              

              }


              GenericFactory_EFInternalIssue.Update(_internalIssueobj);
              GenericFactory_EFInternalIssue.Save();

             
          }
          catch (Exception)
          {
              
         
          }
          return 1;
      }

      private void SaveInternalIssueDetails(List<vmIssueDetail> _objIssueDetails, Int64 IssueId, vmInternalIssue _objInternalIssue)
      {
          Int64 NextId = 0;
          GenericFactory_EFInternalIssueDetails = new PrdInternalIssueDetails_EF();
        
          if (IssueId > 0)
          {
              foreach (vmIssueDetail _aitem in _objIssueDetails)
              {
                  NextId = GenericFactory_EFInternalIssueDetails.getMaxVal_int64("IssueDetailID", "PrdInternalIssueDetail");
                  PrdInternalIssueDetail _InternalIssueDetail = new PrdInternalIssueDetail();
                  _InternalIssueDetail.IssueDetailID = NextId;
                  _InternalIssueDetail.IssueID = IssueId;
                  _InternalIssueDetail.BallID = _aitem.BallID;
                  _InternalIssueDetail.RopeNumber = _aitem.RopeNumber;
                  _InternalIssueDetail.CanID = _aitem.CanID;
                  _InternalIssueDetail.IsDeleted = false;
                  _InternalIssueDetail.OperatorID = _aitem.OperatorID;
                  _InternalIssueDetail.CompanyID = _aitem.CompanyID;
                  _InternalIssueDetail.MachineID = _aitem.MachineID;

                  GenericFactory_EFInternalIssueDetails.Insert(_InternalIssueDetail);
                  GenericFactory_EFInternalIssueDetails.Save();

              }
          }
      }

      private Int64 SaveInteralIssueMaster(vmInternalIssue _objInternalIssue)
       {
          
          Int64 NextId =0;
           try
           {
               GenericFactory_EFInternalIssue = new PrdInternalIssue_EF();
               NextId = GenericFactory_EFInternalIssue.getMaxVal_int64("IssueID", "PrdInternalIssue");
               PrdInternalIssue _objInternalIssueMaster = new PrdInternalIssue();
               _objInternalIssueMaster.IssueID = NextId;
               _objInternalIssueMaster.TransactionTypeID = _objInternalIssue.TransactionTypeID;
               _objInternalIssueMaster.TransactionTypeID = _objInternalIssue.TransactionTypeID;
               _objInternalIssueMaster.IssueNo = NextId.ToString();
               _objInternalIssueMaster.ItemID = _objInternalIssue.ItemID;
               _objInternalIssueMaster.SetID = _objInternalIssue.SetID;
               _objInternalIssueMaster.DepartmentID = _objInternalIssue.DepartmentID;
               _objInternalIssueMaster.IsIssuedBall = _objInternalIssue.IsIssuedBall;
               _objInternalIssueMaster.IssBallBy = _objInternalIssue.CreateBy;
               _objInternalIssueMaster.CompanyID = _objInternalIssue.CompanyID;
               _objInternalIssueMaster.IsIssuedBall = _objInternalIssue.IsIssuedBall;
               _objInternalIssueMaster.IssBallRemarks = _objInternalIssue.IssBallRemarks;
               _objInternalIssueMaster.BalMRRID = _objInternalIssue.BalMRRID;
               _objInternalIssueMaster.IsReceivedDy = false;
               _objInternalIssueMaster.IsIssuedDy = false;
               _objInternalIssueMaster.IsReceivedLCB = false;

               _objInternalIssueMaster.IssBallDate = _objInternalIssue.IssBallDate;
               _objInternalIssueMaster.IsDeleted = false;

               GenericFactory_EFInternalIssue.Insert(_objInternalIssueMaster);
               GenericFactory_EFInternalIssue.Save();
               
                   
           }
           catch (Exception)
           {
               
              
           }
          return NextId;
       }

        public  vmSetSetupMasterDetail GetSetDetailsByIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? IssueID)
      {

          vmSetSetupMasterDetail _vmSetSetupMasterDetails = null;
          string spQuery = string.Empty;

          try
          {
              using (_ctxCmn = new ERP_Entities())
              {
                  Hashtable ht = new Hashtable();
                  ht.Add("CompanyID", 0);
                  ht.Add("LoggedUser", 0);
                  ht.Add("PageNo", 0);
                  ht.Add("RowCountPerPage", 0);
                  ht.Add("IsPaging", 0);
                  ht.Add("IssueId", IssueID);
                  spQuery = "[GetPrdSetDetailsByIssueID]";
                  _vmSetSetupMasterDetails = GFactory_VM_PrdSetSetupMasterDetail.ExecuteQuery(spQuery, ht).FirstOrDefault();


              }
          }
          catch (Exception e)
          {
              e.ToString();
          }

          return _vmSetSetupMasterDetails;
      }
    }
}
