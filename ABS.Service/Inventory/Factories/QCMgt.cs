using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using ABS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Inventory.Factories
{
    public class QCMgt : iQCMgt
    {
        private ERP_Entities _ctxCmn = null; 
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo = null;
        private iGenericFactory<InvRequisitionMaster> GFactory_GF_Requisition = null;
        private iGenericFactory_EF<CmnDocument> GenericFactory_CmnDocument = null;
        private iGenericFactory_EF<CmnDocumentPath> GenericFactory_CmnDocumentPath = null;

        /// No CompanyID Provided
        //public List<InvGrrMaster> GetGRRNo(int? pageNumber, int? pageSize, int? IsPaging)
        //{

        //    List<InvGrrMaster> lstInvGrrMaster = null;
        //    using (_ctxCmn = new ERP_Entities())
        //    {
        //        try
        //        {
        //            lstInvGrrMaster = (from grr in _ctxCmn.InvGrrMasters.Where(m => m.IsDeleted == false && m.IsMrrCompleted == false && m.IsQcCompleted == false)
        //                               select new { GrrID = grr.GrrID, GrrNo = grr.GrrNo, IsDeleted = grr.IsDeleted }).ToList()
        //                               .Select(m => new InvGrrMaster { GrrID = m.GrrID, GrrNo = m.GrrNo, IsDeleted = m.IsDeleted })
        //                               .OrderByDescending(m => m.GrrID).ToList();  
        //        }
        //        catch (Exception e)
        //        {
        //            e.ToString();
        //        }
        //    }
        //    return lstInvGrrMaster;
        //}

        public IEnumerable<vmQC> GetGRRNo(vmCmnParameters objcmnParam, Int32 TransTypeID, out int recordsTotal)
        {
            IEnumerable<vmQC> objGrrNoWithoutPaging = null; 
            IEnumerable<vmQC> lstInvGrrMaster = null;
            recordsTotal = 0;

            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    objGrrNoWithoutPaging = (from grr in _ctxCmn.InvGrrMasters.Where(m => m.IsDeleted == false && m.IsMrrCompleted == false && m.IsQcCompleted == false && m.TransactionTypeID==TransTypeID)
                                                 join spr in _ctxCmn.InvRequisitionMasters on grr.RequisitionID equals spr.RequisitionID
                                                 join comp in _ctxCmn.CmnCompanies on spr.ToCompanyID equals comp.CompanyID
                                                 select new 
                                                 { 
                                                     GrrID = grr.GrrID, GrrNo = grr.GrrNo, IsDeleted = grr.IsDeleted, FromCompanyID=spr.ToCompanyID, FromCompanyName= comp.CompanyName
                                                 }).ToList()
                                                 .Select(m => new vmQC { GrrID = m.GrrID, GrrNo = m.GrrNo, IsDeleted = m.IsDeleted, FromCompanyID = m.FromCompanyID, FromCompanyName=m.FromCompanyName }).ToList();

                    lstInvGrrMaster = objGrrNoWithoutPaging.OrderByDescending(x => x.GrrID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = objGrrNoWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstInvGrrMaster;
        }

        public IEnumerable<InvRequisitionMaster> GetSPR(vmCmnParameters objcmnParam)
        {
            GFactory_GF_Requisition = new InvRequisitionMaster_GF();
            IEnumerable<InvRequisitionMaster> lstSPR = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("pageNumber", objcmnParam.pageNumber);
                ht.Add("pageSize", objcmnParam.pageSize);
                ht.Add("IsPaging", objcmnParam.IsPaging);
                ht.Add("CompanyID", objcmnParam.loggedCompany);
                spQuery = "Get_QuotationSPRNo";
                lstSPR = GFactory_GF_Requisition.ExecuteQuery(spQuery, ht);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstSPR;
        }
        public IEnumerable<InvMrrQcMaster> ChkDuplicateNo(vmCmnParameters objcmnParam, string Mno)
        {
            IEnumerable<InvMrrQcMaster> lstMNo = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstMNo = (from rm in _ctxCmn.InvMrrQcMasters.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.QCCertificateNo == Mno)
                              select new
                              {
                                  MrrQcID = rm.MrrQcID,
                                  QCCertificateNo = rm.QCCertificateNo

                              }).ToList().Select(x => new InvMrrQcMaster
                              {
                                  MrrQcID = x.MrrQcID,
                                  QCCertificateNo = x.QCCertificateNo
                              }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstMNo;
        }


        public List<CmnCombo> GetSPRPOLCType(int? pageNumber, int? pageSize, int? IsPaging)
        { 
            List<CmnCombo> lstSPRPOLCType = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstSPRPOLCType = _ctxCmn.CmnComboes.Where(m => m.ComboType == "SPL" && m.IsDeleted == false).ToList().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstSPRPOLCType;
        }
        public List<CmnCombo> GetChallanInvoiceReceiptTypes(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnCombo> lstChallanInvoiceReceiptTypes = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstChallanInvoiceReceiptTypes = _ctxCmn.CmnComboes.Where(m => m.ComboType == "CIR" && m.IsDeleted == false).ToList().Select(m => new CmnCombo { ComboID = m.ComboID, ComboName = m.ComboName, ComboType = m.ComboType, IsDefault = m.IsDefault, IsDeleted = m.IsDeleted }).ToList();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstChallanInvoiceReceiptTypes;
        }

        public IEnumerable<vmChallan> GetQCList()
        {
            IEnumerable<vmChallan> lstQCMaster = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    lstQCMaster = _ctxCmn.InvMrrQcMasters.Where(m => m.IsDeleted == false).Select(m => new vmChallan { MrrQcID = m.MrrQcID, MrrQcNo = m.MrrQcNo, ManualQCNoRpt = m.QCCertificateNo + "||" + m.MrrQcNo, CompanyID = m.CompanyID, CreateBy = m.CreateBy }).ToList();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstQCMaster;
        }

        public IEnumerable<vmSPRPOLCNo> GetSPRPOLCNoByID(vmCmnParameters objcmnParam, int SPRPOLCTypeID, out int recordsTotal)
        {
            IEnumerable<vmSPRPOLCNo> lstSPRPOLCNo = null;
            IEnumerable<vmSPRPOLCNo> lstvmSPRPOLCNoWithoutPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    if (SPRPOLCTypeID == 21)
                    {
                        lstvmSPRPOLCNoWithoutPaging = (from rm in _ctxCmn.InvRequisitionMasters

                                                       select new
                                                       {
                                                           SPRPOLCID = rm.RequisitionID,
                                                           SPRPOLCNo = rm.RequisitionNo,
                                                           IsDeleted = rm.IsDeleted

                                                       }).Select(x => new vmSPRPOLCNo
                                                       {
                                                           SPRPOLCID = x.SPRPOLCID,
                                                           SPRPOLCNo = x.SPRPOLCNo,
                                                           IsDeleted = x.IsDeleted
                                                       }).Where(m => m.IsDeleted == false).ToList();

                        lstSPRPOLCNo = lstvmSPRPOLCNoWithoutPaging.OrderBy(x => x.SPRPOLCID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                        recordsTotal = lstvmSPRPOLCNoWithoutPaging.Count();
                    } 
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstSPRPOLCNo;
        }

        public IEnumerable<vmChallanInvoiceReceipt> GetChallanInvoiceReceiptNoByID(vmCmnParameters objcmnParam, int CIRTypeID, out int recordsTotal)
        {

            IEnumerable<vmChallanInvoiceReceipt> lstChallanInvoiceReceiptNo = null;
            IEnumerable<vmChallanInvoiceReceipt> lstChallanInvoiceReceiptNoWOPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    if (CIRTypeID == 24)
                    {
                        lstChallanInvoiceReceiptNoWOPaging = (from rm in _ctxCmn.InvRChallanMasters.Where(m => m.IsDeleted == false)

                                                              select new
                                                              {
                                                                  ChallanInvoiceReceiptID = rm.CHID,
                                                                  ChallanInvoiceReceiptNo = rm.CHNo,
                                                                  IsDeleted = rm.IsDeleted

                                                              }).Select(x => new vmChallanInvoiceReceipt
                                                              {
                                                                  ChallanInvoiceReceiptID = x.ChallanInvoiceReceiptID,
                                                                  ChallanInvoiceReceiptNo = x.ChallanInvoiceReceiptNo,
                                                                  IsDeleted = x.IsDeleted
                                                              }).ToList();

                        lstChallanInvoiceReceiptNo = lstChallanInvoiceReceiptNoWOPaging.OrderBy(x => x.ChallanInvoiceReceiptID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                        recordsTotal = lstChallanInvoiceReceiptNoWOPaging.Count();
                    }

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstChallanInvoiceReceiptNo;

        }

        public vmSPRPOLCNo GetSPRPOLCDateByNo(int SprpolcType, Int64 SPRPOLCNo)
        {
            vmSPRPOLCNo objDate = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    if (SprpolcType == 21) // for SPR
                    {
                        objDate = (from rm in _ctxCmn.InvRequisitionMasters.Where(m => m.RequisitionID == SPRPOLCNo && m.IsDeleted == false) select rm).Select(t => new vmSPRPOLCNo { SPRPOLCDate = t.RequisitionDate }).FirstOrDefault();
                    }

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objDate;
        }

        public vmChallanInvoiceReceipt GetCIRDateByNo(int CIRType, Int64 CIRNo)
        {
            vmChallanInvoiceReceipt objDate = null; 
            Int64 CIRID = Convert.ToInt64(CIRNo);
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    if (Convert.ToInt16(CIRType) == 24) // for Challan
                    {
                        objDate = (from rm in _ctxCmn.InvRChallanMasters.Where(m => m.CHID == CIRID && m.IsDeleted == false) select rm).ToList().Select(t => new vmChallanInvoiceReceipt { ChallanInvoiceReceiptDate = t.CHDate }).FirstOrDefault();
                    }

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return objDate;
        }

        public List<vmQC> GetQCMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            List<vmQC> lstQC = null;
            List<vmQC> lstQCWOPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstQCWOPaging = (from qcm in _ctxCmn.InvMrrQcMasters.Where(m => m.IsDeleted == false && m.CompanyID == objcmnParam.loggedCompany && m.IsMrrCompleted == false && m.TransactionTypeID==objcmnParam.tTypeId)
                                     join suplr in _ctxCmn.CmnUsers on qcm.SupplierID equals suplr.UserID
                                     into xsuplr
                                     from qsuplr in xsuplr.DefaultIfEmpty()

                                     join usr in _ctxCmn.CmnUsers on qcm.CreateBy equals usr.UserID
                                     into xusr
                                     from qusr in xusr.DefaultIfEmpty()

                                     join grr in _ctxCmn.InvGrrMasters on qcm.GrrID equals grr.GrrID
                                     into xgrr
                                     from qgrr in xgrr.DefaultIfEmpty()

                                     join po in _ctxCmn.PurchasePOMasters on qcm.POID equals po.POID
                                     into xpo
                                     from qpo in xpo.DefaultIfEmpty()

                                     join pi in _ctxCmn.PurchasePIMasters on qcm.PIID equals pi.PIID
                                     into xpi
                                     from qpi in xpi.DefaultIfEmpty()

                                     join spr in _ctxCmn.InvRequisitionMasters on qcm.RequisitionID equals spr.RequisitionID
                                     into xspr
                                     from qspr in xspr.DefaultIfEmpty()

                                     join Frmcomp in _ctxCmn.CmnCompanies on qspr.ToCompanyID equals Frmcomp.CompanyID
                                     into xFrmcomp
                                     from qFrmcomp in xFrmcomp.DefaultIfEmpty()


                                     join comp in _ctxCmn.CmnCompanies on qcm.CompanyID equals comp.CompanyID
                                     into xcomp
                                     from qcomp in xcomp.DefaultIfEmpty()
                                    

                                     select new
                                     {
                                        
                                         MrrQcID = qcm.MrrQcID,
                                         MrrQcNo = qcm.MrrQcNo,
                                         GrrID = qcm.GrrID,
                                         GrrNo = qcm.GrrNo,
                                         DocURL = qcm.DocURL,
                                        
                                         IsDeleted = qcm.IsDeleted,
                                       
                                         SPLID = (long?)qcm.SPLID,
                                         SPLTypeID = qcm.SPLTypeID,
                                        
                                         SPLNoName = "",
                                         SPLDate = DateTime.Now,//GetSPRPOLCDateByNo(),//GetSPLDate(qcm.SPLTypeID, qcm.SPLID),
                                         CIRDate = DateTime.Now,//GetCIRDate(qcm.SPLTypeID, qcm.SPLID), 
                                         CIRID = (long?)qcm.CIRID,
                                         CIRNoName = "",
                                         CIRTypeID = qcm.CIRTypeID,
                                         // CIRTypeName = qcirTypeName.ComboName,
                                         MrrQcDate = qcm.MrrQcDate == null ? DateTime.Now : qcm.MrrQcDate,
                                         SupplierID = (long?)qcm.SupplierID,
                                         SupplierName = qsuplr.UserFullName,
                                         CompanyName = qcomp.CompanyName,
                                         CompanyID = qcm.CompanyID,
                                         UserID = qusr.UserID,
                                         UserFullName = qusr.UserFullName,
                                         GrrDate = qgrr.GrrDate == null ? DateTime.Now : qgrr.GrrDate,
                                         POID = (long?)qpo.POID,
                                         PONo = qpo.PONo,
                                         LCNO = qpo.LCorVoucherorLcafNo,
                                         PIID = (long?)qpi.PIID,
                                         PINo = qpi.PINo,
                                         RequisitionID = (long?)qspr.RequisitionID,
                                         RequisitionNo = qspr.RequisitionNo,
                                         FromCompanyID = qFrmcomp.CompanyID,
                                         FromCompanyName= qFrmcomp.CompanyName,

                                         RefCHNo = qgrr.RefCHNo,
                                         Remarks = qcm.Remarks,
                                         Description = qcm.Description,
                                         QCCertificateNo = qcm.QCCertificateNo,

                                         QtyWithoutQc = 0.0m,// GetQtyWithoutQc(qcm.GrrID, qcm.SPLID, qcm.MrrQcID),
                                         QtyWithQc = 0.0m//GetQtyWithQc(qcm.GrrID, qcm.SPLID, qcm.MrrQcID)

                                     }).ToList().Select(x => new vmQC
                                     {
                                         MrrQcID = x.MrrQcID,
                                         MrrQcNo = x.MrrQcNo,
                                         GrrID = x.GrrID,
                                         GrrNo = x.GrrNo,
                                         GrrDate = x.GrrDate,
                                         DocURL = x.DocURL,
                                         IsDeleted = x.IsDeleted,
                                         SPLID = (long?)x.SPLID,
                                         SPLTypeID = x.SPLTypeID,
                                         //  SPLTypeName = x.SPLTypeName,
                                         CIRID = (long?)x.CIRID,
                                         CIRTypeID = x.CIRTypeID,
                                         //  CIRTypeName = x.CIRTypeName,
                                         SPLDate = x.SPLDate,//GetSPRPOLCDateByNo(x.SPLTypeID, x.SPLID).SPRPOLCDate,
                                         CIRDate = x.CIRDate,//GetSPRPOLCDateByNo(x.SPLTypeID, x.SPLID).SPRPOLCDate,
                                         SPLNoName = x.SPLNoName,// GetSPLNoName(x.SPLTypeID, x.SPLID).SPLNoName,
                                         CIRNoName = x.CIRNoName,//GetCIRNoName(x.CIRTypeID, x.CIRID).CIRNoName,
                                         MrrQcDate = x.MrrQcDate,
                                         SupplierID = x.SupplierID,
                                         SupplierName = x.SupplierName,
                                         CompanyName = x.CompanyName,
                                         CompanyID = x.CompanyID,
                                         UserID = x.UserID,
                                         UserFullName = x.UserFullName,
                                         POID = (long?)x.POID,
                                         PONo = x.PONo,
                                         LCNO = x.LCNO,
                                         PIID = (long?)x.PIID,
                                         PINo = x.PINo,
                                         RefCHNo = x.RefCHNo,
                                         RequisitionID = (long?)x.RequisitionID,
                                         RequisitionNo = x.RequisitionNo,

                                         FromCompanyID = x.FromCompanyID,
                                         FromCompanyName = x.FromCompanyName,


                                         Remarks = x.Remarks,
                                         Description = x.Description,
                                         QCCertificateNo = x.QCCertificateNo,

                                         QtyWithoutQc = x.QtyWithoutQc, // GetQtyWithoutQc(x.GrrID, x.SPLID, x.MrrQcID).QtyWithoutQc,
                                         QtyWithQc = x.QtyWithQc //GetQtyWithQc(x.GrrID, x.SPLID, x.MrrQcID).QtyWithQc
                                     }).ToList();

                    lstQC = lstQCWOPaging.OrderByDescending(x => x.MrrQcID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstQCWOPaging.Count();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstQC;
        }

        public List<vmQC> GetItemDetailByGrrNo(vmCmnParameters objcmnParam, Int64 grrID, out int recordsTotal)
        {

            List<vmQC> lstQC = null;
            List<vmQC> lstQCWOPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstQCWOPaging = (from grm in _ctxCmn.InvGrrMasters.Where(r => r.GrrID == grrID && r.IsDeleted == false && r.CompanyID == objcmnParam.loggedCompany && r.IsMrrCompleted == false) //&& r.IsQcCompleted == false
                                     join rm in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == grrID && m.Qty >= m.QcRemainingQty) on grm.GrrID equals rm.GrrID
                                     join item in _ctxCmn.CmnItemMasters on rm.ItemID equals item.ItemID
                                     into xitem
                                     from qitem in xitem.DefaultIfEmpty()
                                     join batch in _ctxCmn.CmnBatches on rm.BatchID equals batch.BatchID
                                     into xbatch
                                     from qbatch in xbatch.DefaultIfEmpty()
                                     join lot in _ctxCmn.CmnLots on rm.LotID equals lot.LotID
                                     into xlot
                                     from qlot in xlot.DefaultIfEmpty()
                                     join uom in _ctxCmn.CmnUOMs on rm.UnitID equals uom.UOMID
                                     into xuom
                                     from quom in xuom.DefaultIfEmpty()

                                     join po in _ctxCmn.PurchasePOMasters on grm.POID equals po.POID
                                     into xpo
                                     from qpo in xpo.DefaultIfEmpty()

                                     join pi in _ctxCmn.PurchasePIMasters on grm.PIID equals pi.PIID
                                     into xpi
                                     from qpi in xpi.DefaultIfEmpty()

                                     join spr in _ctxCmn.InvRequisitionMasters on grm.RequisitionID equals spr.RequisitionID
                                     into xspr
                                     from qspr in xspr.DefaultIfEmpty()

                                     join spplr in _ctxCmn.CmnUsers on grm.SupplierID equals spplr.UserID
                                     into xspplr
                                     from qspplr in xspplr.DefaultIfEmpty()

                                     //join qcm in _ctxCmn.InvMrrQcMasters on rm.GrrID equals qcm.GrrID
                                     //into xqcm from yqcm in xqcm.DefaultIfEmpty()
                                     //join qcd in _ctxCmn.InvMrrQcDetails on yqcm.MrrQcID equals 


                                     select new
                                     {
                                         MrrQcDetailID = 0,
                                         MrrQcID = 0,
                                         MrrQcNo = "",
                                         GrrID = rm.GrrID,
                                         GrrDate = grm.GrrDate,
                                         GrrDetailID = rm.GrrDetailID,
                                         IsDeleted = rm.IsDeleted,
                                         IsQCCompleted = rm.IsQcCompleted,
                                         ItemID = qitem.ItemID,
                                         ItemName = qitem.ItemName,
                                         ItemCode = qitem.ArticleNo,
                                         GrrQty = rm.Qty,

                                         PassQty = rm.QcRemainingQty,
                                         RejectQty = 0.0m,

                                         AdditionalGrrQty = rm.AdditionalQty,
                                         AdditionalPassQty = rm.QcRemainingAdditionalQty == null ? 0.0m : rm.QcRemainingAdditionalQty,
                                         AdditionalRejectQty = 0.00m,

                                         UnitID = (int?)rm.UnitID,
                                         UOMName = quom.UOMName,
                                         LotID = (long?)qlot.LotID,
                                         LotNo = qlot.LotNo,
                                         BatchID = (long?)qbatch.BatchID,
                                         BatchNo = qbatch.BatchNo,
                                         POID = (long?)qpo.POID,
                                         PONo = qpo.PONo,
                                         LCNO = qpo.LCorVoucherorLcafNo,
                                         PIID = (long?)qpi.PIID,
                                         PINo = qpi.PINo,
                                         UserID = (int?)qspplr.UserID,
                                         UserFullName = qspplr.UserFullName,
                                         RequisitionID = (long?)qspr.RequisitionID,
                                         RequisitionNo = qspr.RequisitionNo,
                                         RefCHNo = grm.RefCHNo,
                                         Remarks = "",

                                         QcRemainingQty = rm.QcRemainingQty,
                                         QcValidQty = rm.QcRemainingQty,

                                         QcRemainingAdditionalQty = rm.QcRemainingAdditionalQty == null ? 0.0m : rm.QcRemainingAdditionalQty,
                                         QcValidAdditionalQty = rm.QcRemainingAdditionalQty == null ? 0.0m : rm.QcRemainingAdditionalQty
                                          

                                     }).Select(x => new vmQC
                                     {
                                         MrrQcDetailID = x.MrrQcDetailID,
                                         MrrQcID = x.MrrQcID,
                                         MrrQcNo = x.MrrQcNo,
                                         GrrID = x.GrrID,
                                         GrrDate = x.GrrDate,
                                         GrrDetailID = x.GrrDetailID,
                                         IsDeleted = x.IsDeleted,
                                         IsQCCompleted = x.IsQCCompleted,
                                         ItemID = x.ItemID,
                                         ItemName = x.ItemName,
                                         ItemCode = x.ItemCode,
                                         GrrQty = x.GrrQty,
                                         PassQty = x.PassQty,
                                         RejectQty = x.RejectQty,

                                         AdditionalGrrQty = x.AdditionalGrrQty,
                                         AdditionalPassQty = x.AdditionalPassQty,
                                         AdditionalRejectQty = x.AdditionalRejectQty,

                                         UnitID = x.UnitID,
                                         UOMName = x.UOMName,
                                         LotID = (long?)x.LotID,
                                         LotNo = x.LotNo,
                                         BatchID = (long?)x.BatchID,
                                         BatchNo = x.BatchNo,
                                         POID = (long?)x.POID,
                                         PONo = x.PONo,
                                         LCNO = x.LCNO,
                                         PIID = (long?)x.PIID,
                                         PINo = x.PINo,
                                         RequisitionID = (long?)x.RequisitionID,
                                         RequisitionNo = x.RequisitionNo,
                                         UserID = x.UserID,
                                         UserFullName = x.UserFullName,
                                         RefCHNo = x.RefCHNo,
                                         Remarks = x.Remarks,

                                         QcRemainingQty = x.QcRemainingQty,
                                         QcValidQty = x.QcValidQty,

                                         QcRemainingAdditionalQty = x.QcRemainingAdditionalQty,
                                         QcValidAdditionalQty = x.QcValidAdditionalQty

                                     }).ToList();

                    lstQC = lstQCWOPaging.OrderBy(x => x.GrrID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstQCWOPaging.Count();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstQC;
        }

        public List<vmQC> GetQCDetailsListByQCMasterID(vmCmnParameters vmCmnParam, Int64 id)
        {
            List<vmQC> lstQC = null;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstQC = (from qcm in _ctxCmn.InvMrrQcMasters.Where(k => k.MrrQcID == id && k.CompanyID == vmCmnParam.loggedCompany && k.IsDeleted == false && k.IsMrrCompleted == false)
                             join qcd in _ctxCmn.InvMrrQcDetails on qcm.MrrQcID equals qcd.MrrQcID
                             join item in _ctxCmn.CmnItemMasters on qcd.ItemID equals item.ItemID
                             //into xitem
                             //from qitem in xitem.DefaultIfEmpty()
                             join batch in _ctxCmn.CmnBatches on qcd.BatchID equals batch.BatchID
                             into xbatch
                             from qbatch in xbatch.DefaultIfEmpty()
                             join lot in _ctxCmn.CmnLots on qcd.LotID equals lot.LotID
                             into xlot
                             from qlot in xlot.DefaultIfEmpty()
                             join uom in _ctxCmn.CmnUOMs on qcd.UnitID equals uom.UOMID
                             into xuom
                             from quom in xuom.DefaultIfEmpty()

                             select new
                             { 
                                 MrrQcID = qcm.MrrQcID,
                                 MrrQcNo = qcm.MrrQcNo,
                                 MrrQcDetailID = qcd.MrrQcDetailID,
                                 GrrID = qcm.GrrID,
                                 GrrDetailID = 0,
                                 IsDeleted = qcd.IsDeleted,
                                 // IsQCCompleted = qcm.,
                                 ItemID = qcd.ItemID,
                                 ItemName = item.ItemName,
                                 ItemCode = item.ArticleNo, 
                                 GrrQty = qcd.GrrQty, 
                                 PassQty = qcd.PassQty,
                                 RejectQty = qcd.RejectQty, 
                                 AdditionalPassQty = qcd.AdditionalPassQty,
                                 AdditionalRejectQty = qcd.AdditionalRejectQty, 
                                 UnitID = qcd.UnitID,
                                 UOMName = quom.UOMName,
                                 LotID = (long?)qlot.LotID,
                                 LotNo = qlot.LotNo,
                                 BatchID = (long?)qbatch.BatchID,
                                 BatchNo = qbatch.BatchNo,
                                 Remarks = qcd.Remarks,
                                 QcRemainingQty = 0.0m,
                                 QcValidQty = 0.0m, 
                                 QcRemainingAdditionalQty = 0.0m,
                                 QcValidAdditionalQty = 0.0m

                             }).ToList().Select(x => new vmQC
                             {
                                 MrrQcDetailID = x.MrrQcDetailID,
                                 MrrQcNo = x.MrrQcNo,
                                 MrrQcID = x.MrrQcID,
                                 GrrID = x.GrrID,
                                 GrrDetailID = x.GrrDetailID,
                                 IsDeleted = x.IsDeleted,
                                 // IsQCCompleted = x.IsQCCompleted,
                                 ItemID = x.ItemID,
                                 ItemName = x.ItemName,
                                 ItemCode = x.ItemCode,
                                 GrrQty = x.GrrQty, 
                                 PassQty = x.PassQty,
                                 RejectQty = x.RejectQty, 
                                 AdditionalPassQty = x.AdditionalPassQty,
                                 AdditionalRejectQty = x.AdditionalRejectQty, 
                                 UnitID = x.UnitID,
                                 UOMName = x.UOMName,
                                 LotID = (long?)x.LotID,
                                 LotNo = x.LotNo,
                                 BatchID = (long?)x.BatchID,
                                 BatchNo = x.BatchNo,
                                 Remarks = x.Remarks, 
                                 QcRemainingQty = (from qcdeatil in _ctxCmn.InvGrrDetails.Where(a => a.GrrID == x.GrrID && a.ItemID == x.ItemID && a.IsDeleted == false)
                                                   select qcdeatil.QcRemainingQty).FirstOrDefault(),
                                 QcValidQty = x.PassQty + x.RejectQty + (from qcdeatil in _ctxCmn.InvGrrDetails.Where(a => a.GrrID == x.GrrID && a.ItemID == x.ItemID && a.IsDeleted == false)
                                                                         select qcdeatil.QcRemainingQty).FirstOrDefault(),

                                 QcRemainingAdditionalQty = (from qcdeatil in _ctxCmn.InvGrrDetails.Where(a => a.GrrID == x.GrrID && a.ItemID == x.ItemID && a.IsDeleted == false)
                                                             select qcdeatil.QcRemainingAdditionalQty).FirstOrDefault(),

                                 QcValidAdditionalQty = x.AdditionalPassQty + x.AdditionalRejectQty + (from qcdeatil in _ctxCmn.InvGrrDetails.Where(a => a.GrrID == x.GrrID && a.ItemID == x.ItemID && a.IsDeleted == false)
                                                                                                       select qcdeatil.QcRemainingAdditionalQty).FirstOrDefault()
 
                             }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstQC;
        }
        public string SaveUpdateQCMasterNdetails(InvMrrQcMaster qcMaster, List<InvMrrQcDetail> qcDetails, int menuID)
        {

            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                if (qcMaster.MrrQcID > 0)
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            Int64 mrrQcID = qcMaster.MrrQcID;
                            IEnumerable<InvMrrQcMaster> lstInvMrrQcMaster = (from qcm in _ctxCmn.InvMrrQcMasters.Where(m => m.MrrQcID == mrrQcID) select qcm).ToList();
                            InvMrrQcMaster objInvMrrQcMaster = new InvMrrQcMaster();
                            foreach (InvMrrQcMaster qcms in lstInvMrrQcMaster)
                            {
                                qcms.IsMrrCompleted = false;
                                qcms.POID = qcMaster.POID;
                                qcms.PONo = qcMaster.PONo;
                                qcms.RequisitionID = qcMaster.RequisitionID;
                                qcms.QCCertificateNo = qcMaster.QCCertificateNo;
                                qcms.TransactionTypeID = qcMaster.TransactionTypeID;
                                qcms.DocURL = qcMaster.DocURL;
                                qcms.Remarks = qcMaster.Remarks;
                                qcms.Description = qcMaster.Description;
                                qcms.DepartmentID = qcMaster.DepartmentID;
                                qcms.UpdateBy = qcMaster.CreateBy;
                                qcms.UpdateOn = DateTime.Now;
                                qcms.UpdatePc = HostService.GetIP();
                                qcms.CIRID = qcMaster.CIRID;
                                qcms.CIRTypeID = qcMaster.CIRTypeID;
                                qcms.CompanyID = qcMaster.CompanyID;
                                qcms.GrrID = qcMaster.GrrID;
                                qcms.GrrNo = qcMaster.GrrNo;
                                qcms.MrrQcDate = qcMaster.MrrQcDate;
                                qcms.SPLID = qcMaster.SPLID;
                                qcms.SPLTypeID = qcMaster.SPLTypeID;
                                qcms.SupplierID = qcMaster.SupplierID;

                                objInvMrrQcMaster = qcms;

                                //  for one save then qccomplete
                                InvGrrMaster objInvGrrMaster = (from spr in _ctxCmn.InvGrrMasters.Where(m => m.GrrID == qcms.GrrID && m.CompanyID == qcms.CompanyID) select spr).FirstOrDefault();
                                objInvGrrMaster.IsQcCompleted = true;

                            }
                            List<InvMrrQcDetail> lstInvMrrQcDetail = new List<InvMrrQcDetail>();
                            foreach (InvMrrQcDetail qcdt in qcDetails)
                            {
                                InvMrrQcDetail objInvMrrQcDetail = (from qcdetl in _ctxCmn.InvMrrQcDetails.Where(m => m.MrrQcDetailID == qcdt.MrrQcDetailID) select qcdetl).FirstOrDefault();
                                //start for exist passed n reject qty 
                                decimal? prePassedRejectQty = objInvMrrQcDetail.PassQty + objInvMrrQcDetail.RejectQty;
                                decimal? prePassedAdditionalRejectQty = objInvMrrQcDetail.AdditionalPassQty + objInvMrrQcDetail.AdditionalRejectQty;
                                //end for exist passed n reject qty 

                                objInvMrrQcDetail.PassQty = qcdt.PassQty;
                                objInvMrrQcDetail.RejectQty = qcdt.RejectQty;
                                objInvMrrQcDetail.AdditionalPassQty = qcdt.AdditionalPassQty;
                                objInvMrrQcDetail.AdditionalRejectQty = qcdt.AdditionalRejectQty;
                                objInvMrrQcDetail.Remarks = qcdt.Remarks;
                                objInvMrrQcDetail.IsDeleted = false;
                                objInvMrrQcDetail.GrrQty = qcdt.GrrQty;
                                objInvMrrQcDetail.UnitID = qcdt.UnitID;
                                objInvMrrQcDetail.UpdateBy = qcMaster.CreateBy;
                                objInvMrrQcDetail.UpdateOn = DateTime.Now;
                                objInvMrrQcDetail.UpdatePc = HostService.GetIP();
                                lstInvMrrQcDetail.Add(objInvMrrQcDetail);

                                InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                                objInvGrrDetail.QcRemainingQty = (objInvGrrDetail.QcRemainingQty + prePassedRejectQty) - (qcdt.PassQty + qcdt.RejectQty);

                                InvGrrDetail objInvGrrDetailAdditional = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == qcdt.ItemID) select grrd).FirstOrDefault();
                                objInvGrrDetailAdditional.QcRemainingAdditionalQty = (objInvGrrDetailAdditional.QcRemainingAdditionalQty + prePassedAdditionalRejectQty) - (qcdt.AdditionalPassQty + qcdt.AdditionalRejectQty);
                            }
                            _ctxCmn.SaveChanges();

                            ////**********----------------------Start File Upload----------------------**********

                            //GenericFactory_CmnDocument = new CmnDocument_EF();
                            //int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                            //List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                            //for (int i = 1; i <= fileNames.Count; i++)
                            //{
                            //    CmnDocument objCmnDocument = new CmnDocument();
                            //    objCmnDocument.DocumentID = DocumentID;
                            //    objCmnDocument.DocumentPahtID = 3;
                            //    //objCmnDocument.DocumentName = fileNames[i].ToString();
                            //    string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                            //    objCmnDocument.DocumentName = qcMaster.MrrQcNo + "_Doc_" + i + extension;
                            //    objCmnDocument.TransactionID = qcMaster.MrrQcID;
                            //    objCmnDocument.TransactionTypeID = 19;
                            //    objCmnDocument.CompanyID = qcMaster.CompanyID;
                            //    objCmnDocument.CreateBy = Convert.ToInt16(qcMaster.CreateBy);
                            //    objCmnDocument.CreateOn = DateTime.Now;
                            //    objCmnDocument.CreatePc =  HostService.GetIP();
                            //    objCmnDocument.IsDeleted = false;

                            //    objCmnDocument.IsDeleted = false;
                            //    lstCmnDocument.Add(objCmnDocument);

                            //    DocumentID++;
                            //}

                            //GenericFactory_CmnDocument.InsertList(lstCmnDocument);
                            //GenericFactory_CmnDocument.Save();
                            //GenericFactory_CmnDocument.updateMaxID("CmnDocument", Convert.ToInt64(DocumentID - 1));

                            ////**********----------------------File upload completed----------------------**********



                            transaction.Complete();
                            result = qcMaster.MrrQcNo.ToString();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                            result = "";
                        }
                    }
                }
                else
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            //...........START  new maxId........//
                            long NextId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvMrrQcMaster"));
                            long FirstDigit = 0;
                            long OtherDigits = 0;
                            long nextQCDetailId = Convert.ToInt16(GenericFactory_EF_CmnCombo.getMaxID("InvMrrQcDetail"));
                            FirstDigit = Convert.ToInt64(nextQCDetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(nextQCDetailId.ToString().Substring(1, nextQCDetailId.ToString().Length - 1));
                            //..........END new maxId.........//


                            //......... START for custom code........... //
                            string customCode = "";
                            string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, DateTime.Now, qcMaster.CompanyID, 1, 1);

                            if (CustomNo != null)
                            {
                                customCode = CustomNo;
                            }
                            else
                            {
                                customCode = NextId.ToString();
                            }
                            //.........END for custom code............ //

                            string newMrrQcNo = customCode;
                            qcMaster.MrrQcID = NextId;
                            qcMaster.CreateOn = DateTime.Now;
                            qcMaster.CreatePc = HostService.GetIP();
                            qcMaster.MrrQcNo = newMrrQcNo;
                            qcMaster.IsDeleted = false;
                            qcMaster.IsMrrCompleted = false;
                             
                            //  for one save then qccomplete
                            InvGrrMaster objInvGrrMaster = (from spr in _ctxCmn.InvGrrMasters.Where(m => m.GrrID == qcMaster.GrrID && m.CompanyID == qcMaster.CompanyID) select spr).FirstOrDefault();
                            objInvGrrMaster.IsQcCompleted = true;
                             
                            // itemMaster.IsHDOCompleted = false;
                            //  itemMaster.IsLcCompleted = false;
                            List<InvMrrQcDetail> lstqcDetail = new List<InvMrrQcDetail>();
                            foreach (InvMrrQcDetail sdtl in qcDetails)
                            {
                                InvMrrQcDetail objqcDetail = new InvMrrQcDetail();
                                objqcDetail.MrrQcDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits);//nextQCDetailId;
                                objqcDetail.MrrQcID = NextId;
                                objqcDetail.ItemID = sdtl.ItemID;
                                objqcDetail.GrrQty = sdtl.GrrQty;
                                objqcDetail.IsDeleted = false;//sdtl.IsDeleted;
                                objqcDetail.PassQty = sdtl.PassQty;
                                objqcDetail.RejectQty = sdtl.RejectQty;

                                objqcDetail.AdditionalPassQty = sdtl.AdditionalPassQty;
                                objqcDetail.AdditionalRejectQty = sdtl.AdditionalRejectQty;
                                 
                                objqcDetail.Remarks = sdtl.Remarks;
                                objqcDetail.UnitID = sdtl.UnitID;
                                objqcDetail.LotID = sdtl.LotID;
                                objqcDetail.BatchID = sdtl.BatchID;
                                objqcDetail.CreateBy = qcMaster.CreateBy;//sdtl.CreateBy;
                                objqcDetail.CreateOn = DateTime.Now;
                                objqcDetail.CreatePc = HostService.GetIP();
                                // objSalPIDetail.IsCICompleted = false;
                                lstqcDetail.Add(objqcDetail);
                                //nextQCDetailId++;
                                OtherDigits++;

                                InvGrrDetail objInvGrrDetail = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == sdtl.ItemID) select grrd).FirstOrDefault();
                                objInvGrrDetail.QcRemainingQty = objInvGrrDetail.QcRemainingQty - (sdtl.PassQty + sdtl.RejectQty);

                                InvGrrDetail objInvGrrDetailAdditional = (from grrd in _ctxCmn.InvGrrDetails.Where(m => m.GrrID == qcMaster.GrrID && m.ItemID == sdtl.ItemID) select grrd).FirstOrDefault();
                                objInvGrrDetailAdditional.QcRemainingAdditionalQty = objInvGrrDetailAdditional.QcRemainingAdditionalQty - (sdtl.AdditionalPassQty + sdtl.AdditionalRejectQty);

                            }

                            _ctxCmn.InvMrrQcMasters.Add(qcMaster);

                            //............Update MaxID.................//
                            GenericFactory_EF_CmnCombo.updateMaxID("InvMrrQcMaster", Convert.ToInt64(NextId));
                            //............Update CustomCode.............//
                            GenericFactory_EF_CmnCombo.updateCustomCode(menuID, DateTime.Now, qcMaster.CompanyID, 1, 1);

                            _ctxCmn.InvMrrQcDetails.AddRange(lstqcDetail);

                            //............Update MaxID.................//
                            GenericFactory_EF_CmnCombo.updateMaxID("InvMrrQcDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            _ctxCmn.SaveChanges();


                            ////**********----------------------Start File Upload----------------------**********

                            //GenericFactory_CmnDocument = new CmnDocument_EF();
                            //int DocumentID = Convert.ToInt16(GenericFactory_CmnDocument.getMaxID("CmnDocument"));
                            //List<CmnDocument> lstCmnDocument = new List<CmnDocument>();

                            //for (int i = 1; i <= fileNames.Count; i++)
                            //{
                            //    CmnDocument objCmnDocument = new CmnDocument();
                            //    objCmnDocument.DocumentID = DocumentID;
                            //    objCmnDocument.DocumentPahtID = 3;
                            //    //objCmnDocument.DocumentName = fileNames[i].ToString();
                            //    string extension = System.IO.Path.GetExtension(fileNames[i - 1].ToString());
                            //    objCmnDocument.DocumentName = qcMaster.MrrQcNo + "_Doc_" + i + extension;
                            //    objCmnDocument.TransactionID = qcMaster.MrrQcID;
                            //    objCmnDocument.TransactionTypeID = qcMaster.TransactionTypeID;
                            //    objCmnDocument.CompanyID = qcMaster.CompanyID;
                            //    objCmnDocument.CreateBy = Convert.ToInt16(qcMaster.CreateBy);
                            //    objCmnDocument.CreateOn = DateTime.Now;
                            //    objCmnDocument.CreatePc =  HostService.GetIP();
                            //    objCmnDocument.IsDeleted = false;

                            //    objCmnDocument.IsDeleted = false;
                            //    lstCmnDocument.Add(objCmnDocument);

                            //    DocumentID++;
                            //}

                            //GenericFactory_CmnDocument.InsertList(lstCmnDocument);
                            //GenericFactory_CmnDocument.Save();
                            //GenericFactory_CmnDocument.updateMaxID("CmnDocument", Convert.ToInt64(DocumentID - 1));

                            ////**********----------------------File upload completed----------------------**********


                            transaction.Complete();
                            result = newMrrQcNo;
                        }
                        catch (Exception e)
                        {
                            result = "";
                        }
                    }

                }
            }
            return result;
        }

        public CmnDocumentPath GetUploadPath(int TransTypeID)
        {
            GenericFactory_CmnDocumentPath = new CmnDocumentPath_EF();
            CmnDocumentPath objUploadPath = null;
            try
            {
                objUploadPath = GenericFactory_CmnDocumentPath.GetAll().Select(m => new
                CmnDocumentPath
                {
                    TransactionTypeID = m.TransactionTypeID,
                    PhysicalPath = m.PhysicalPath
                })
                    .Where(m => m.TransactionTypeID == TransTypeID).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUploadPath;
        }
        public IEnumerable<vmCmnDocument> GetFileDetailsById(int qcID, int TransTypeID)
        {
            GenericFactory_CmnDocument = new CmnDocument_EF();
            IEnumerable<vmCmnDocument> objFileInfo = null;
            string fullFilePath = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //  var transactionName;
                    var virtualPath = _ctxCmn.CmnDocumentPaths.Where(m => m.TransactionTypeID == TransTypeID && m.IsDeleted == false).ToList().
                                     Select(m => new CmnDocumentPath
                                     {
                                         VirtualPath = m.VirtualPath
                                     }).FirstOrDefault();

                    var transactionName = _ctxCmn.CmnTransactionTypes.Where(m => m.TransactionTypeID == TransTypeID && m.IsDeleted == false).ToList().
                                     Select(m => new CmnTransactionType
                                     {
                                         TransactionTypeName = m.TransactionTypeName
                                     }).FirstOrDefault();


                    objFileInfo = _ctxCmn.CmnDocuments.Where(m => m.TransactionID == qcID).ToList().
                                Select(m => new vmCmnDocument
                                {
                                    DocumentID = m.DocumentID,
                                    DocumentName = m.DocumentName,
                                    TransactionID = m.TransactionID,
                                    FullDocumentPath = virtualPath.VirtualPath + transactionName.TransactionTypeName + "/" + m.DocumentName
                                }).ToList();
                     
                }
            }
            catch (Exception e)
            {
                e.ToString();
            } 
            return objFileInfo;
        }
    }
}
