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
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class SizeBeamIssueMgt : iSizeBeamIssueMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory<vmSizeBeamIssue> GFactory_VM_SetDetail = null;
        private iGenericFactory_EF<PrdSizingBeamIssue> GenericFactory_EFSizingBeamIssue = null;
        private iGenericFactory_EF<PrdSizingBeamIssueDetail> GenericFactory_EFSizingBeamIssueDetail = null;
        private iGenericFactory_EF<PrdWeavingMachineBook> GenericFactory_EFWeavingMachineBook = null;
        private iGenericFactory_EF<PrdWeavingMachinConfig> GenericFactory_EFPrdWeavingMachinConfig = null;
        public vmSizeBeamIssue GetSetDeatailBySetNo(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo, int? LoginCompanyID)
        {
            GFactory_VM_SetDetail = new PrdSizeBeamIssue_VM();

            vmSizeBeamIssue _vmSetDetails = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", LoginCompanyID);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("SetID", SetNo);
                    spQuery = "[GetPrdIssueDeailsBySetNoForSizeBeam]";
                    _vmSetDetails = GFactory_VM_SetDetail.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmSetDetails;
        }



        public vmSizeBeamIssue GetSizeBeamIssuemasterDetailByBeamIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? BeamIssueId, int? LoginCompanyID)
        {
            GFactory_VM_SetDetail = new PrdSizeBeamIssue_VM();

            vmSizeBeamIssue _vmSetDetails = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", LoginCompanyID);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("BeamIssueID", BeamIssueId);
                    spQuery = "[Get_PrdSizeBeamIssuemasterDetailByBeamIssueID]";
                    _vmSetDetails = GFactory_VM_SetDetail.ExecuteQuery(spQuery, ht).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmSetDetails;
        }
        //-------------
        public List<vmSizeBeamIssue> GetSizingMRRMasterDetailByBeamIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? BeamIssueId, int? LoginCompanyID)
        {
            GFactory_VM_SetDetail = new PrdSizeBeamIssue_VM();

            List<vmSizeBeamIssue> _vmSetDetails = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", LoginCompanyID);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("BeamIssueID", BeamIssueId);
                    spQuery = "[Get_PrdSizingMRRMasterDetailByBeamIssueID]";
                    _vmSetDetails = GFactory_VM_SetDetail.ExecuteQuery(spQuery, ht).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmSetDetails;
        }

        public List<vmSizeBeamIssue> GetSizingMRRMasterDetailBySetID(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo, int? LoginCompanyID)
        {
            GFactory_VM_SetDetail = new PrdSizeBeamIssue_VM();

            List<vmSizeBeamIssue> _vmSetDetails = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", LoginCompanyID);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    ht.Add("SetID", SetNo);
                    spQuery = "[GetSizingMRRMasterDetailBySetID]";
                    _vmSetDetails = GFactory_VM_SetDetail.ExecuteQuery(spQuery, ht).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmSetDetails;
        }
        public List<vmSizeBeamIssue> GetSizeBeamIssueDetails(int? pageNumber, int? pageSize, int? IsPaging, int? LoginCompanyID)
        {
            GFactory_VM_SetDetail = new PrdSizeBeamIssue_VM();

            List<vmSizeBeamIssue> _vmSetDetails = null;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", LoginCompanyID);
                    ht.Add("LoggedUser", 0);
                    ht.Add("PageNo", 0);
                    ht.Add("RowCountPerPage", 0);
                    ht.Add("IsPaging", 0);
                    spQuery = "[GetSizeBeamIssueDetails]";
                    _vmSetDetails = GFactory_VM_SetDetail.ExecuteQuery(spQuery, ht).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return _vmSetDetails;
        }

        public int SaveSizeBeamIssue(List<vmSizeBeamIssue> _objSizeIssueDetails, vmSizeBeamIssue _objSizeIssueMaster)
        {
            int result = 0;
            // using (TransactionScope transaction = new TransactionScope())
            //{

            try
            {
                if (_objSizeIssueMaster.BeamIssueID == 0)
                {
                    Int64 BeamIssueID = SaveSizeBeamIssueMaster(_objSizeIssueMaster);
                    if (BeamIssueID > 0)
                    {

                        result = SaveSizeBeamIssueMasterDetails(_objSizeIssueDetails, BeamIssueID, _objSizeIssueMaster);
                    }
                }
                else
                {

                    Int64 BeamIssueID = UpdateSizeBeamIssueMaster(_objSizeIssueMaster);
                    if (BeamIssueID > 0)
                    {
                        result = UpdateSizeBeamIssueMasterDetails(_objSizeIssueDetails, BeamIssueID, _objSizeIssueMaster);
                        result = SaveWeavingMachineBook(_objSizeIssueDetails, _objSizeIssueMaster);
                        result = UpdateWeavingMachineConfig(_objSizeIssueDetails);
                    }



                }
            }
            catch (Exception)
            {

                throw;
            }

            // }
            return result;
        }

        private int UpdateWeavingMachineConfig(List<vmSizeBeamIssue> _objSizeIssueDetails)
        {
            GenericFactory_EFPrdWeavingMachinConfig = new PrdWeavingMachinConfig_EF();
            foreach(vmSizeBeamIssue aitem in _objSizeIssueDetails)
            {
                PrdWeavingMachinConfig _objWeavingMachineConfig = GenericFactory_EFPrdWeavingMachinConfig.FindBy(x => x.MachineConfigID == aitem.LoomID).FirstOrDefault();
                _objWeavingMachineConfig.IsBook = true;
                GenericFactory_EFPrdWeavingMachinConfig.Update(_objWeavingMachineConfig);
                GenericFactory_EFPrdWeavingMachinConfig.Save();
            }
            return 1;
        }

        private int SaveWeavingMachineBook(List<vmSizeBeamIssue> _objSizeIssueDetails, vmSizeBeamIssue _objSizeIssueMaster)
        {

            try
            {
                Int64 NextId = 0;
                GenericFactory_EFWeavingMachineBook = new PrdWeavingMachineBook_EF();
                foreach (vmSizeBeamIssue aitem in _objSizeIssueDetails)
                {
                    NextId = GenericFactory_EFWeavingMachineBook.getMaxVal_int64("MachineBookID", "PrdWeavingMachineBook");
                    PrdWeavingMachineBook _prdWeavingMachineBook = new PrdWeavingMachineBook();
                    _prdWeavingMachineBook.MachineBookID = NextId;
                    _prdWeavingMachineBook.MachineConfigID = Convert.ToInt64(aitem.LoomID);
                    _prdWeavingMachineBook.BeamIssueDetailID = aitem.BeamIssueDetailID;
                    _prdWeavingMachineBook.ItemID = _objSizeIssueMaster.ItemID;
                    _prdWeavingMachineBook.SetID = _objSizeIssueMaster.SetID;
                    _prdWeavingMachineBook.SizeMRRID = _objSizeIssueMaster.SizeMRRID;
                    if (aitem.BFDate != null) { _prdWeavingMachineBook.BookingDate = Convert.ToDateTime(aitem.BFDate).AddDays(1); } else { _prdWeavingMachineBook.BookingDate = null; }
                    _prdWeavingMachineBook.Remarks = aitem.Remarks;
                    _prdWeavingMachineBook.CompanyID = aitem.CompanyID;
                    _prdWeavingMachineBook.IsDeleted = _objSizeIssueMaster.IsDeleted;
                    GenericFactory_EFWeavingMachineBook.Insert(_prdWeavingMachineBook);
                    GenericFactory_EFWeavingMachineBook.Save();
                }

                return 1;

            }
            catch (Exception)
            {

                throw;
            }

        }

        private int UpdateSizeBeamIssueMasterDetails(List<vmSizeBeamIssue> _objSizeIssueDetails, long BeamIssueID, vmSizeBeamIssue _objSizeIssueMaster)
        {
            try
            {
                GenericFactory_EFSizingBeamIssueDetail = new PrdSizingBeamIssueDetail_EF();

                foreach (vmSizeBeamIssue aitem in _objSizeIssueDetails)
                {
                    PrdSizingBeamIssueDetail _prdSizingBeamIssueDetail = GenericFactory_EFSizingBeamIssueDetail.FindBy(x => x.BeamIssueDetailID == aitem.BeamIssueDetailID).FirstOrDefault();

                    _prdSizingBeamIssueDetail.BeamIssueID = BeamIssueID;
                    _prdSizingBeamIssueDetail.BeamID = aitem.OutputID;// OutputID is the beamID
                    _prdSizingBeamIssueDetail.Length = aitem.LengthYds;
                    if (aitem.BSDate != null) { _prdSizingBeamIssueDetail.BSDate = Convert.ToDateTime(aitem.BSDate).AddDays(1); } else { _prdSizingBeamIssueDetail.BSDate = null; }
                    if (aitem.BMDate != null) { _prdSizingBeamIssueDetail.BMDate = Convert.ToDateTime(aitem.BMDate).AddDays(1); } else { _prdSizingBeamIssueDetail.BMDate = null; }
                    _prdSizingBeamIssueDetail.LoomID = aitem.LoomID;
                    _prdSizingBeamIssueDetail.Totalfabric = aitem.Totalfabric ?? 0;
                    if (aitem.BFDate != null) { _prdSizingBeamIssueDetail.BFDate = Convert.ToDateTime(aitem.BFDate).AddDays(1); } else { _prdSizingBeamIssueDetail.BFDate = null; }
                    _prdSizingBeamIssueDetail.UpdateBy = _objSizeIssueMaster.CreateBy;
                    // _prdSizingBeamIssueDetail.
                    GenericFactory_EFSizingBeamIssueDetail.Update(_prdSizingBeamIssueDetail);
                    GenericFactory_EFSizingBeamIssueDetail.Save();
                }

            }
            catch (Exception)
            {

                throw;
            }
            return 1;
        }

        private Int64 UpdateSizeBeamIssueMaster(vmSizeBeamIssue _objSizeIssueMaster)
        {
            GenericFactory_EFSizingBeamIssue = new PrdSizingBeamIssue_EF();
            PrdSizingBeamIssue _objPrdSizingBeamIssue = GenericFactory_EFSizingBeamIssue.FindBy(x => x.BeamIssueID == _objSizeIssueMaster.BeamIssueID).FirstOrDefault();

            _objPrdSizingBeamIssue.TransactionTypeID = _objSizeIssueMaster.TransactionTypeID;
            _objPrdSizingBeamIssue.ItemID = _objSizeIssueMaster.ItemID;
            _objPrdSizingBeamIssue.Setlength = _objSizeIssueMaster.SetLength;
            _objPrdSizingBeamIssue.SetID = _objSizeIssueMaster.SetID;
            _objPrdSizingBeamIssue.SizeMRRID = _objSizeIssueMaster.SizeMRRID;
            _objPrdSizingBeamIssue.WeavingDepartmentID = _objSizeIssueMaster.WeavingDepartmentID;
            _objPrdSizingBeamIssue.WeavingReceiveDate= _objSizeIssueMaster.WeavingReceiveDate;
            _objPrdSizingBeamIssue.WeavingReceiveBy = _objSizeIssueMaster.WeavingReceiveBy;
            _objPrdSizingBeamIssue.WeavingReceivedRemarks = _objSizeIssueMaster.WeavingReceivedRemarks;
            _objPrdSizingBeamIssue.IsReceivedWeaving=_objSizeIssueMaster.IsReceivedWeaving;

            //_objPrdSizingBeamIssue.SizeDepartmentID = _objSizeIssueMaster.SizeDepartmentID ?? 0;
            //_objPrdSizingBeamIssue.IsIssuedSize = _objSizeIssueMaster.IsIssuedSize;
            //_objPrdSizingBeamIssue.SizeIssueDate = _objSizeIssueMaster.SizeIssueDate;
            //_objPrdSizingBeamIssue.SizeIssueBy = _objSizeIssueMaster.SizeIssueBy;
            //_objPrdSizingBeamIssue.SizeIssueRemarks = _objSizeIssueMaster.SizeIssueRemarks;
            //_objPrdSizingBeamIssue.UpdateBy = _objSizeIssueMaster.CreateBy;
           // _objPrdSizingBeamIssue.IsDeleted = _objSizeIssueMaster.IsDeleted;
            GenericFactory_EFSizingBeamIssue.Update(_objPrdSizingBeamIssue);
            GenericFactory_EFSizingBeamIssue.Save();
            return _objSizeIssueMaster.BeamIssueID;

        }

        private int SaveSizeBeamIssueMasterDetails(List<vmSizeBeamIssue> _objSizeIssueDetails, Int64 BeamIssueID, vmSizeBeamIssue _objSizeIssueMaster)
        {
            int status = 0;
            GenericFactory_EFSizingBeamIssueDetail = new PrdSizingBeamIssueDetail_EF();


            try
            {
                Int64 NextId = 0;

                foreach (vmSizeBeamIssue aitem in _objSizeIssueDetails)
                {

                    NextId = GenericFactory_EFSizingBeamIssueDetail.getMaxVal_int64("BeamIssueDetailID", "PrdSizingBeamIssueDetail");
                    PrdSizingBeamIssueDetail _prdSizingBeamIssueDetail = new PrdSizingBeamIssueDetail();
                    _prdSizingBeamIssueDetail.BeamIssueDetailID = NextId;
                    _prdSizingBeamIssueDetail.BeamIssueID = BeamIssueID;
                    _prdSizingBeamIssueDetail.BeamID = aitem.OutputID;// OutputID is the beamID
                    _prdSizingBeamIssueDetail.Length = aitem.LengthYds;
                    if (aitem.BSDate != null) { _prdSizingBeamIssueDetail.BSDate = Convert.ToDateTime(aitem.BSDate).AddDays(1); } else { _prdSizingBeamIssueDetail.BSDate = null; }
                    if (aitem.BMDate != null) { _prdSizingBeamIssueDetail.BMDate = Convert.ToDateTime(aitem.BMDate).AddDays(1); } else { _prdSizingBeamIssueDetail.BMDate = null; }
                    _prdSizingBeamIssueDetail.LoomID = aitem.LoomID;
                    _prdSizingBeamIssueDetail.Totalfabric = aitem.Totalfabric ?? 0;
                    if (aitem.BFDate != null) { _prdSizingBeamIssueDetail.BFDate = Convert.ToDateTime(aitem.BFDate).AddDays(1); } else { _prdSizingBeamIssueDetail.BFDate = null; }
                    _prdSizingBeamIssueDetail.CompanyID = _objSizeIssueMaster.CompanyID;
                    _prdSizingBeamIssueDetail.CreateBy = _objSizeIssueMaster.CreateBy;
                    _prdSizingBeamIssueDetail.IsDeleted = _objSizeIssueMaster.IsDeleted;


                    GenericFactory_EFSizingBeamIssueDetail.Insert(_prdSizingBeamIssueDetail);
                    GenericFactory_EFSizingBeamIssueDetail.Save();
                }

                status = 1;
            }
            catch
            {

            }
            return status;

        }

        private Int64 SaveSizeBeamIssueMaster(vmSizeBeamIssue _objSizeIssueMaster)
        {
            Int64 NextId = 0;
            GenericFactory_EFSizingBeamIssue = new PrdSizingBeamIssue_EF();

            NextId = GenericFactory_EFSizingBeamIssue.getMaxVal_int64("BeamIssueID", "PrdSizingBeamIssue");
            PrdSizingBeamIssue _prdSizingBeamIssue = new PrdSizingBeamIssue();
            _prdSizingBeamIssue.BeamIssueID = NextId;
            _prdSizingBeamIssue.TransactionTypeID = _objSizeIssueMaster.TransactionTypeID;
            _prdSizingBeamIssue.BeamIssueNo = NextId.ToString();// BeamIssueno is BeamIssueID
            _prdSizingBeamIssue.ItemID = _objSizeIssueMaster.ItemID;
            _prdSizingBeamIssue.Setlength = _objSizeIssueMaster.SetLength;
            _prdSizingBeamIssue.SetID = _objSizeIssueMaster.SetID;
            _prdSizingBeamIssue.SizeMRRID = _objSizeIssueMaster.SizeMRRID;
            _prdSizingBeamIssue.SizeDepartmentID = _objSizeIssueMaster.SizeDepartmentID ?? 0;
            _prdSizingBeamIssue.IsIssuedSize = _objSizeIssueMaster.IsIssuedSize;
            _prdSizingBeamIssue.SizeIssueDate = _objSizeIssueMaster.SizeIssueDate;
            _prdSizingBeamIssue.SizeIssueBy = _objSizeIssueMaster.SizeIssueBy;
            _prdSizingBeamIssue.SizeIssueRemarks = _objSizeIssueMaster.SizeIssueRemarks;
            _prdSizingBeamIssue.CompanyID = _objSizeIssueMaster.CompanyID;
            _prdSizingBeamIssue.CreateBy = _objSizeIssueMaster.CreateBy;
            _prdSizingBeamIssue.IsDeleted = _objSizeIssueMaster.IsDeleted;
            _prdSizingBeamIssue.Shade = _objSizeIssueMaster.Shade;
            _prdSizingBeamIssue.GPL = _objSizeIssueMaster.GPL;
            GenericFactory_EFSizingBeamIssue.Insert(_prdSizingBeamIssue);
            GenericFactory_EFSizingBeamIssue.Save();


            return NextId;
        }
    }
}
