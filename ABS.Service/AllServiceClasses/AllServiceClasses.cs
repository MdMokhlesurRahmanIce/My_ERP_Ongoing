using ABS.Data.BaseFactories;
using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.Purchase;
using ABS.Models.ViewModel.LocalSales;


namespace ABS.Service.AllServiceClasses
{
    #region Sales Entities
    public class SalPIMaster_EF : GenericFactory_EF<ERP_Entities, SalPIMaster> { }
    public class SalBookingMaster_EF : GenericFactory_EF<ERP_Entities, SalBookingMaster> { }

    public class SalPIDetail_GF : GenericFactory<ERP_Entities, SalPIDetail> { }
    public class SalPIDetail_EF : GenericFactory_EF<ERP_Entities, SalPIDetail> { }
    public class SalBookingDetail_EF : GenericFactory_EF<ERP_Entities, SalBookingDetail> { }

    public class SalLCMaster_EF : GenericFactory_EF<ERP_Entities, SalLCMaster> { }
    public class SalIncoterm_EF : GenericFactory_EF<ERP_Entities, SalIncoterm> { }
    public class SalLCDetail_EF : GenericFactory_EF<ERP_Entities, SalLCDetail> { }
    public class vmSalLCDetail_GF : GenericFactory<ERP_Entities, vmSalLCDetail> { }

    public class SalHDOMaster_EF : GenericFactory_EF<ERP_Entities, SalHDOMaster> { }
    public class SalHDOMaster_GF : GenericFactory<ERP_Entities, SalHDOMaster> { }
    public class SalHDODetail_EF : GenericFactory_EF<ERP_Entities, SalHDODetail> { }

    public class SalFDOMaster_EF : GenericFactory_EF<ERP_Entities, SalFDOMaster> { }
    public class SalFDODetail_EF : GenericFactory_EF<ERP_Entities, SalFDODetail> { }

    public class SalDCMaster_EF : GenericFactory_EF<ERP_Entities, SalDCMaster> { }
    public class SalDCDetail_EF : GenericFactory_EF<ERP_Entities, SalDCDetail> { }
    public class CmnDocument_EF : GenericFactory_EF<ERP_Entities, CmnDocument> { }
    public class SalPPBillingMaster_GF : GenericFactory<ERP_Entities, SalPPBillingMaster> { }
    public class SalPPBillingMaster_EF : GenericFactory_EF<ERP_Entities, SalPPBillingMaster> { }
    public class SalPPBAMaster_EF : GenericFactory_EF<ERP_Entities, SalPPBAMaster> { }
    public class SalPPBADetail_EF : GenericFactory_EF<ERP_Entities, SalPPBADetail> { }
    public class SalCmnCustomCode_EF : GenericFactory_EF<ERP_Entities, CmnCustomCode> { }
    public class vmPIDetail_GF : GenericFactory<ERP_Entities, vmPIDetail> { }
    #endregion

    #region System Common Entities
    public class CmnUserWiseCompany_EF : GenericFactory_EF<ERP_Entities, CmnUserWiseCompany> { }
    public class CmnCombo_EF : GenericFactory_EF<ERP_Entities, CmnCombo> { }

    public class CmnACCIntegration_EF : GenericFactory_EF<ERP_Entities, CmnACCIntegration> { } 

    public class CmnItemCount_EF : GenericFactory_EF<ERP_Entities, CmnItemCount> { }

    public class CmnItemMaster_EF : GenericFactory_EF<ERP_Entities, CmnItemMaster> { }
    public class CmnUser_EF : GenericFactory_EF<ERP_Entities, CmnUser> { }
    public class HRMShift_EF : GenericFactory_EF<ERP_Entities, HRMShift> { }
    public class FabricDevelopment_EF : GenericFactory<ERP_Entities, vmFinishGood> { }
   
    public class ArticleDeails_EF : GenericFactory<ERP_Entities, vmArticleDetail> { }
  
    public class Team_EF : GenericFactory<ERP_Entities, vmTeam> { }

    public class FabricConsumption_EF : GenericFactory<ERP_Entities, vmConsumption> { }
    public class CmnCompany_EF : GenericFactory_EF<ERP_Entities, CmnCompany> { }
    public class CmnCompany_GF : GenericFactory<ERP_Entities, CmnCompany> { }

    public class CmnTransactionType_GF : GenericFactory<ERP_Entities, CmnTransactionType> { }
    public class CmnBank_EF : GenericFactory_EF<ERP_Entities, CmnBank> { }
    public class CmnBankBranch_EF : GenericFactory_EF<ERP_Entities, CmnBankBranch> { }
    public class CmnBankAdvising_EF : GenericFactory_EF<ERP_Entities, CmnBankAdvising> { }
    public class CmnCustomCode_EF : GenericFactory_EF<ERP_Entities, CmnCustomCode> { }
    public class CmnCustomCodeDetail_EF : GenericFactory_EF<ERP_Entities, CmnCustomCodeDetail> { }
    public class RndConsumptionMaster_EF : GenericFactory_EF<ERP_Entities, RndConsumptionMaster> { }
    public class RndConsumptionDetail_EF : GenericFactory_EF<ERP_Entities, RndConsumptionDetail> { }


    public partial class CmnItemGroup_EF : GenericFactory_EF<ERP_Entities, CmnItemGroup> { }
    public class vmCmnMenu_GF : GenericFactory<ERP_Entities, vmCmnMenu> { }
    public class CmnModule_EF : GenericFactory_EF<ERP_Entities, CmnModule> { }
    public partial class vmCmnMenuPermission_GF : GenericFactory<ERP_Entities, vmCmnMenuPermission> { }
    public class CmnMenuPermission_EF : GenericFactory_EF<ERP_Entities, CmnMenuPermission> { }
    public class vmCmnModule_GF : GenericFactory<ERP_Entities, vmCmnModule> { }
    public class CmnModulePermission_EF : GenericFactory_EF<ERP_Entities, CmnModulePermission> { }
    public class vmCmnOrganogram_GF : GenericFactory<ERP_Entities, vmCmnOrganogram> { }

    public class CmnOrganogram_EF : GenericFactory_EF<ERP_Entities, CmnOrganogram> { }
    public class vmNotification_GF : GenericFactory<ERP_Entities, vmNotification> { }
    public class vmAuthenticatedUser_GF : GenericFactory<ERP_Entities, vmAuthenticatedUser> { }
    public class vmUser_GF : GenericFactory<ERP_Entities, vmUser> { }
    public class vmUserGroup_GF : GenericFactory<ERP_Entities, vmUserGroup> { }
    public class CmnUserWiseComapny_EF : GenericFactory_EF<ERP_Entities, CmnUserWiseCompany> { }

    public class CmnUserUserAuthentication_EF : GenericFactory_EF<ERP_Entities, CmnUserAuthentication> { }

    public class vmItemGroup_GF : GenericFactory<ERP_Entities, vmItemGroup> { }
    public class vmUserType_GF : GenericFactory<ERP_Entities, vmUserType> { }
    public class CmnItemColor_EF : GenericFactory_EF<ERP_Entities, CmnItemColor> { }
    public class CmnGrade_EF : GenericFactory_EF<ERP_Entities, CmnItemGrade> { }

    public class CmnItemColor_GF : GenericFactory<ERP_Entities, CmnItemColor> { }
    public class CmnItemColor_VM : GenericFactory<ERP_Entities, vmColor> { }
    public class CmnItemSize_EF : GenericFactory_EF<ERP_Entities, CmnItemSize> { }

    public class CmnStatu_GF : GenericFactory<ERP_Entities, CmnStatu> { }
    public class CmnStatu_EF : GenericFactory_EF<ERP_Entities, CmnStatu> { }
    public class CmnMenu_GF : GenericFactory<ERP_Entities, CmnMenu> { }
    public class CmnMenu_EF : GenericFactory_EF<ERP_Entities, CmnMenu> { }
    public class SystemDate_EF : GenericFactory_EF<ERP_Entities, Object> { }
    public class CmnMenuType_GF : GenericFactory<ERP_Entities, CmnMenuType> { }
    // public class CmnOrganogram_EF : GenericFactory_EF<ERP_Entities, CmnOrganogram> { }
    public class CmnUserGroup_EF : GenericFactory_EF<ERP_Entities, CmnUserGroup> { }

    public class CmnUserType_EF : GenericFactory_EF<ERP_Entities, CmnUserType> { }
    public class CmnItemType_EF : GenericFactory_EF<ERP_Entities, CmnItemType> { }

    public class CmnUOM_EF : GenericFactory_EF<ERP_Entities, CmnUOM> { }
    public class CmnUOMGroup_EF : GenericFactory_EF<ERP_Entities, CmnUOMGroup> { }

    public class CmnItemBrand_EF : GenericFactory_EF<ERP_Entities, ABS.Models.CmnItemBrand> { }
    public class CmnItemModel_EF : GenericFactory_EF<ERP_Entities, CmnItemModel> { }

    public class CmnLot_EF : GenericFactory_EF<ERP_Entities, CmnLot> { }
    public class CmnBatch_EF : GenericFactory_EF<ERP_Entities, CmnBatch> { }
    public class CmnWorkFlowMaster_EF : GenericFactory_EF<ERP_Entities, CmnWorkFlowMaster> { }
    public class CmnWorkFlowDetail_EF : GenericFactory_EF<ERP_Entities, CmnWorkFlowDetail> { }

    public class InvStockMaster_Sale_EF : GenericFactory_EF<ERP_Entities, InvStockMaster> { }
    public class InvStocTransit_Sale_EF : GenericFactory_EF<ERP_Entities, InvStockTransit> { }

    public class CmnDocumentPath_EF : GenericFactory_EF<ERP_Entities, CmnDocumentPath> { }
    public class CmnItemFinishingWeight_EF : GenericFactory_EF<ERP_Entities, CmnItemFinishingWeight> { }
    public class SalSalesInvoiceMaster_EF : GenericFactory_EF<ERP_Entities, SalSalesInvoiceMaster> { }
    public class SalSalesInvoiceDetail_EF : GenericFactory_EF<ERP_Entities, SalSalesInvoiceDetail> { }

    #endregion

    #region Inventory Entities

    public class InvStockMaster_EF : GenericFactory_EF<ERP_Entities, InvStockMaster> { }
    public class InvStockDetail_EF : GenericFactory_EF<ERP_Entities, InvStockDetail> { }
    public class InvStockTransit_EF : GenericFactory_EF<ERP_Entities, InvStockTransit> { }
    public class InvStockMaster_GF : GenericFactory<ERP_Entities, InvStockMaster> { }
    public class vmStockMaster_GF : GenericFactory<ERP_Entities, vmStockMaster> { }


    public class InvGrrMaster_EF : GenericFactory_EF<ERP_Entities, InvGrrMaster> { }
    public class InvGrrMaster_GF : GenericFactory<ERP_Entities, vmChallan> { }


    public class InvGrrDetail_EF : GenericFactory_EF<ERP_Entities, InvGrrDetail> { }
    public class InvRequisitionMaster_EF : GenericFactory_EF<ERP_Entities, InvRequisitionMaster> { }
    public class InvRequisitionDetail_EF : GenericFactory_EF<ERP_Entities, InvRequisitionDetail> { }
    public class vmGrr_GF : GenericFactory<ERP_Entities, vmGrr> { }

    public class vmChallan_GF : GenericFactory<ERP_Entities, vmChallan> { }
    public class vmQC_GF : GenericFactory<ERP_Entities, vmQC> { }
    public class InvIssueMaster_EF : GenericFactory_EF<ERP_Entities, InvIssueMaster> { }
    public class InvIssueDetail_EF : GenericFactory_EF<ERP_Entities, InvIssueDetail> { }

    public class PurchaseQuotationMaster_EF : GenericFactory_EF<ERP_Entities, PurchaseQuotationMaster> { }

    public class PurchaseQuotationMaster_GF : GenericFactory<ERP_Entities, PurchaseQuotationMaster> { }
    public class PurchaseQuotationDetail_EF : GenericFactory_EF<ERP_Entities, PurchaseQuotationDetail> { }

    public class PurchaseFR_EF : GenericFactory_EF<ERP_Entities, PurchaseFR> { }

    public class vmSPR_GF : GenericFactory<ERP_Entities, vmSPR> { }
    public class vmRequisition_GF : GenericFactory<ERP_Entities, vmRequisition> { }
    public class InvRequisitionMaster_GF : GenericFactory<ERP_Entities, InvRequisitionMaster> { }

    public class CmnItemGroup_GF : GenericFactory<ERP_Entities, CmnItemGroup> { }
    public class CmnItemMaster_GF : GenericFactory<ERP_Entities, CmnItemMaster> { }

    public class vmIssueMaster_GF : GenericFactory<ERP_Entities, vmIssueMaster> { }


    public class InvRChallanMaster_GF : GenericFactory<ERP_Entities, vmChallan> { }
    public class InvRChallanMaster_EF : GenericFactory_EF<ERP_Entities, InvRChallanMaster> { }


    public class InvIssueMaster_GF : GenericFactory<ERP_Entities, InvIssueMaster> { }

    public class InvIssueDetail_GF : GenericFactory<ERP_Entities, InvIssueDetail> { }

    public class vmRequisitionDetails_GF : GenericFactory<ERP_Entities, vmRequisitionDetails> { }

    public class InvRequisitionDetail_GF : GenericFactory<ERP_Entities, InvRequisitionDetail> { }
    public class PurchaseFR_GF : GenericFactory<ERP_Entities, PurchaseFR> { }

    public class vmCost_GF : GenericFactory<ERP_Entities, vmCost> { }

    public class PurchaseTaxCategory_GF : GenericFactory<ERP_Entities, PurchaseTaxCategory> { }
    public class PurchaseTaxCategory_EF : GenericFactory_EF<ERP_Entities, PurchaseTaxCategory> { }
    public class PurchaseTax_EF : GenericFactory_EF<ERP_Entities, PurchaseTax> { }
    public class PurchaseTax_GF : GenericFactory<ERP_Entities, PurchaseTax> { }

    public class PurchasePOMaster_GF : GenericFactory<ERP_Entities, PurchasePOMaster> { }

    public class PurchaseConsumerChargeType_GF : GenericFactory<ERP_Entities, PurchaseConsumerChargeType> { }









    
    #endregion

    #region Purchase Entities
    public class vmComparativeStatement_GF : GenericFactory<ERP_Entities, vmComparativeStatement> { }
  
    public class vmQuotation_GF : GenericFactory<ERP_Entities, vmQuotation> { }

    #endregion

    #region Production Entities
    public class PrdDyingConsumptionMaster_EF : GenericFactory_EF<ERP_Entities, PrdDyingConsumptionMaster> { }
    public class PrdDyingConsumptionDetail_EF : GenericFactory_EF<ERP_Entities, PrdDyingConsumptionDetail> { }
    public class PrdDyingConsumptionChemicalM_EF : GenericFactory_EF<ERP_Entities, PrdDyingConsumptionChemicalM> { }
    public class PrdDyingConsumptionChemical_EF : GenericFactory_EF<ERP_Entities, PrdDyingConsumptionChemical> { }

    public class RndYarn_EF : GenericFactory_EF<ERP_Entities, RndYarnCR> { }
    public class RndYarnDetail_EF : GenericFactory_EF<ERP_Entities, RndYarnCRDetail> { }
    public class PrdFinishingType_EF : GenericFactory_EF<ERP_Entities, PrdFinishingType> { }
    //public partial class PrdLineSetupFactory_EF : GenericFactory_EF<ERP_Entities, PrdCmnLineSetup> { }
    public partial class PrdBWSlist_EF : GenericFactory_EF<ERP_Entities, PrdBWSlist> { }
    public partial class PrdBWSlist_EF : GenericFactory_EF<ERP_Entities, PrdBWSlist> { }
    public partial class PrdDefectType_EF : GenericFactory_EF<ERP_Entities, PrdDefectType> { }
    public partial class PrdDefectList_EF : GenericFactory_EF<ERP_Entities, PrdDefectList> { }

    public partial class PrdOutputUnit_EF : GenericFactory_EF<ERP_Entities, PrdOutputUnit> { }

    public class PrdDyingChemicalSetup_EF : GenericFactory_EF<ERP_Entities, PrdDyingChemicalSetup> { }
    public class CmnUserTeam_EF : GenericFactory_EF<ERP_Entities, CmnUserTeam> { }
    public class CmnuserTeamDetail_EF : GenericFactory_EF<ERP_Entities, CmnuserTeamDetail> { }
    public class PrdDyingOperationSetup_EF : GenericFactory_EF<ERP_Entities, PrdDyingOperationSetup> { }
    public class PrdDyingProcess_EF : GenericFactory_EF<ERP_Entities, PrdDyingProcess> { }
    public class PrdDyingChemicalSetupDetail_EF : GenericFactory_EF<ERP_Entities, PrdDyingChemicalSetupDetail> { }
    public class PrdDyingOperation_EF : GenericFactory_EF<ERP_Entities, PrdDyingOperation> { }



    public class PrdSetMaster_EF : GenericFactory_EF<ERP_Entities, PrdSetMaster> { }
    public class PrdWeavingMachinConfig_EF : GenericFactory_EF<ERP_Entities, PrdWeavingMachinConfig> { }

    public class PrdWeavingMRRMaster_EF : GenericFactory_EF<ERP_Entities, PrdWeavingMRRMaster> { }
    public class PrdWeavingMachineSetup_EF : GenericFactory_EF<ERP_Entities, PrdWeavingMachineSetup> { }
    public class PrdWeavingMachineBook_EF : GenericFactory_EF<ERP_Entities, PrdWeavingMachineBook> { }
    public class PrdSetMaster_VM : GenericFactory<ERP_Entities, vmSetMaster> { }
    public class PrdFabricInspection_VM : GenericFactory<ERP_Entities, vmFabricInspection> { }

    public class PrdSetSetup_EF : GenericFactory_EF<ERP_Entities, PrdSetSetup> { }
    public class PrdWeavingLine_EF : GenericFactory_EF<ERP_Entities, PrdWeavingLine> { }
    public class PrdWeavingLine_VM : GenericFactory<ERP_Entities, vmWeavingLine> { }
    public class PrdFabricInspectionMaster_VM : GenericFactory<ERP_Entities, vmFabricInspectionMaster> { }
    public class vmFinishingInspactionDetail_VM : GenericFactory<ERP_Entities, vmFinishingInspactionDetail> { }
    public class vmSelectedItemDataSetSetup_GF : GenericFactory<ERP_Entities, vmSelectedItemDataSetSetup> { }

    public class vmPrdWeavingMachineSetup_GF : GenericFactory<ERP_Entities, vmWeavingMachineSetup> { }

    public class PrdDyingMachineSetup_EF : GenericFactory_EF<ERP_Entities, PrdDyingMachineSetup> { }
    public class PrdDyingMachineSetupDetail_EF : GenericFactory_EF<ERP_Entities, PrdDyingMachineSetupDetail> { }
    public class PrdSetSetupMasterDetail_VM : GenericFactory<ERP_Entities, vmSetSetupMasterDetail> { }
    public class PrdSizeBeamIssue_VM : GenericFactory<ERP_Entities, vmSizeBeamIssue> { }
    public class preWeavingMachineGriage_VM : GenericFactory<ERP_Entities, vmWeavingGriage> { }

    public class prdIssueDetail : GenericFactory<ERP_Entities, vmIssueDetail> { }
    public class PrdBallMachineSetup_EF : GenericFactory_EF<ERP_Entities, PrdBallMachineSetup> { }
    public class vmBallWarpingInformation_GF : GenericFactory<ERP_Entities, vmBallWarpingInformation> { }
    public class vmBallMachineStopAndBrekage_GF : GenericFactory<ERP_Entities, vmBallMachineStopAndBrekage> { }

    public class PrdBallMRRMaster_EF : GenericFactory_EF<ERP_Entities, PrdBallMRRMaster> { }
    public class PrdBallMRRDeatail_EF : GenericFactory_EF<ERP_Entities, PrdBallMRRDetail> { }
    public class PrdBallMRRBreakage_EF : GenericFactory_EF<ERP_Entities, PrdBallMRRBreakage> { }
    public class PrdBallMRRBreakageM_EF : GenericFactory_EF<ERP_Entities, PrdBallMRRBreakageM> { }
    public class PrdBallMRRConsumption_EF : GenericFactory_EF<ERP_Entities, PrdBallMRRConsumption> { }
    public class PrdBallMRRMachineStop_EF : GenericFactory_EF<ERP_Entities, PrdBallMRRMachineStop> { }
    public class PrdBallMRRMachineStopM_EF : GenericFactory_EF<ERP_Entities, PrdBallMRRMachineStopM> { }

    public class vmRecoveryUser_GF : GenericFactory<ERP_Entities, vmRecoverUser> { }
    public class PrdInternalIssue_EF : GenericFactory_EF<ERP_Entities, PrdInternalIssue> { }
    public class PrdFinishingInspactionMaster_EF : GenericFactory_EF<ERP_Entities, PrdFinishingInspactionMaster> { }
    public class PrdFinishingInspactionDetail_EF : GenericFactory_EF<ERP_Entities, PrdFinishingInspactionDetail> { }
    public class PrdSizingBeamIssue_EF : GenericFactory_EF<ERP_Entities, PrdSizingBeamIssue> { }
    public class PrdSizingBeamIssueDetail_EF : GenericFactory_EF<ERP_Entities, PrdSizingBeamIssueDetail> { }

    public class PrdInternalIssueDetails_EF : GenericFactory_EF<ERP_Entities, PrdInternalIssueDetail> { }

    public class PrdSizingChemicalSetup_EF : GenericFactory_EF<ERP_Entities, PrdSizingChemicalSetup> { }
    public class PrdSizingChemicalSetupDetail_EF : GenericFactory_EF<ERP_Entities, PrdSizingChemicalSetupDetail> { }
    public class dynamic_EF : GenericFactory<ERP_Entities, object> { }
    public class vmPrdDyingOperationSetupGenericFactory : GenericFactory<ERP_Entities, vmPrdDyingOperationSetup> { }
    public class PrdDyingMRRSet_EF : GenericFactory_EF<ERP_Entities, PrdDyingMRRSet> { }
    public class PrdDyingMRRSetDetail_EF : GenericFactory_EF<ERP_Entities, PrdDyingMRRSetDetail> { }
    public class PrdDyingMRRMaster_EF : GenericFactory_EF<ERP_Entities, PrdDyingMRRMaster> { }
    public class PrdDyingMRRDetail_EF : GenericFactory_EF<ERP_Entities, PrdDyingMRRDetail> { }
    public class PrdSizingMRRMaster_EF : GenericFactory_EF<ERP_Entities, PrdSizingMRRMaster> { }
    public class PrdSizingMRRDetail_EF : GenericFactory_EF<ERP_Entities, PrdSizingMRRDetail> { }
    public class PrdSizingMRRMachineStopM_EF : GenericFactory_EF<ERP_Entities, PrdSizingMRRMachineStopM> { }
    public class PrdSizingMRRMachineStop_EF : GenericFactory_EF<ERP_Entities, PrdSizingMRRMachineStop> { }
    public class PrdSizingMRRBreakageM_EF : GenericFactory_EF<ERP_Entities, PrdSizingMRRBreakageM> { }
    public class PrdSizingMRRBreakage_EF : GenericFactory_EF<ERP_Entities, PrdSizingMRRBreakage> { }
    public class PrdSizingMRRMaster_VM : GenericFactory<ERP_Entities, vmPrdSizingMRRMaster> { }

    public class PrdLCBMRRMaster_EF : GenericFactory_EF<ERP_Entities, PrdLCBMRRMaster> { }
    public class PrdLCBMRRDetail_EF : GenericFactory_EF<ERP_Entities, PrdLCBMRRDetail> { }
    public class PrdLCBMRRMachineStopM_EF : GenericFactory_EF<ERP_Entities, PrdLCBMRRMachineStopM> { }
    public class PrdLCBMRRMachineStop_EF : GenericFactory_EF<ERP_Entities, PrdLCBMRRMachineStop> { }
    public class PrdLCBMRRBreakageM_EF : GenericFactory_EF<ERP_Entities, PrdLCBMRRBreakageM> { }
    public class PrdLCBMRRBreakage_EF : GenericFactory_EF<ERP_Entities, PrdLCBMRRBreakage> { }
    public class PrdLCBMRRMasterDetail_VM : GenericFactory<ERP_Entities, vmPrdLCBMRRMasterDetail> { }

    public class PrdWeavingLoomData_EF : GenericFactory_EF<ERP_Entities, PrdWeavingLoomData> { }
    public class PrdWeavingLoomDetailData_EF : GenericFactory_EF<ERP_Entities, PrdWeavingLoomDetailData> { }
    public class PrdWeavingLoomStop_EF : GenericFactory_EF<ERP_Entities, PrdWeavingLoomStop> { }
    public class PrdWeavingLoomStopDetail_EF : GenericFactory_EF<ERP_Entities, PrdWeavingLoomStopDetail> { }
    public class PrdFinishingConsumptionDetail_EF : GenericFactory_EF<ERP_Entities, PrdFinishingConsumptionDetail> { }
    public class PrdFinishingConsumptionMaster_EF : GenericFactory_EF<ERP_Entities, PrdFinishingConsumptionMaster> { }
    public class vmWeavingLoomDataMasterDetail_VM : GenericFactory<ERP_Entities, vmWeavingLoomDataMasterDetail> { }

    public class PrdFinishingMRRMaster_EF : GenericFactory_EF<ERP_Entities, PrdFinishingMRRMaster> { }
    public class PrdFinishingMRRShrinkage_EF : GenericFactory_EF<ERP_Entities, PrdFinishingMRRShrinkage> { }
    public class vmPrdFinishingMRRMasterShrinkage_VM : GenericFactory<ERP_Entities, vmPrdFinishingMRRMasterShrinkage> { }
    public class vmChemicalSetupMasterDetail_VM : GenericFactory<ERP_Entities, vmChemicalSetupMasterDetail> { }

    public class PrdWastage_EF : GenericFactory_EF<ERP_Entities, PrdWastage> { }
    public class PrdWastageDetail_EF : GenericFactory_EF<ERP_Entities, PrdWastageDetail> { }
    public class vmWastageMasterDetail_VM : GenericFactory<ERP_Entities, vmWastageMasterDetail> { }

    public class vmBallConsumption_GF : GenericFactory<ERP_Entities, vmBallConsumption> { }

    public class PrdFinishingProcess_EF : GenericFactory_EF<ERP_Entities, PrdFinishingProcess> { }

    public class vmFinishingChemicalPreparation_GF : GenericFactory<ERP_Entities, vmFinishingChemicalPreparation> { }
    public class PrdFinishingChemicalSetup_EF : GenericFactory_EF<ERP_Entities, PrdFinishingChemicalSetup> { }
    public class PrdFinishingChemicalSetupDetail_EF : GenericFactory_EF<ERP_Entities, PrdFinishingChemicalSetupDetail> { }

    public class CmnWorkFlowTransaction_EF : GenericFactory_EF<ERP_Entities, CmnWorkFlowTransaction> { }
    public class CmnEmailTracking_EF : GenericFactory_EF<ERP_Entities, CmnEmailTracking> { }

    public class PrdWeavingMachineConfigDetail_EF : GenericFactory_EF<ERP_Entities, PrdWeavingMachineConfigDetail> { }
    public class vmPrdWeavingMachineConfigMasterDetail_GF : GenericFactory<ERP_Entities, vmPrdWeavingMachineConfigMasterDetail> { }

    public class MntMachineMaintenanceOrder_EF : GenericFactory_EF<ERP_Entities, MntMachineMaintenanceOrder> { }

    public class PrdSizingChemicalConsumption_EF : GenericFactory_EF<ERP_Entities, PrdSizingChemicalConsumption> { }
    public class PrdSizingChemicalconsumptionDetail_EF : GenericFactory_EF<ERP_Entities, PrdSizingChemicalconsumptionDetail> { }
    public class vmChemicalSetupMasterDetail_GF : GenericFactory<ERP_Entities, vmChemicalSetupMasterDetail> { }

    public class PrdFinishingQAMaster_EF : GenericFactory_EF<ERP_Entities, PrdFinishingQAMaster> { }
    public class PrdFinishingQADetail_EF : GenericFactory_EF<ERP_Entities, PrdFinishingQADetail> { }

    public class PrdFinishingPackingList_EF : GenericFactory_EF<ERP_Entities, PrdFinishingPackingList> { }
    public class PrdFinishingPackingListDetail_EF : GenericFactory_EF<ERP_Entities, PrdFinishingPackingListDetail> { }
    public class vmFinishingPackingListDetail_GF : GenericFactory<ERP_Entities, vmFinishingPackingListMasterDetail> { }
    public class vmBallInfo_GF : GenericFactory<ERP_Entities, vmBallInfo> { }
    public class VmSLCurrentStock_VM : GenericFactory<ERP_Entities, vmLSCurrentStock> { }
    public class VmItemType_VM : GenericFactory_EF<ERP_Entities, CmnItemType> { }
    public class PrdDyingMachinePart_EF : GenericFactory_EF<ERP_Entities, PrdDyingMachinePart> { }
    #endregion

    #region db Sample Entities
    //public class tbl_Sales_EF : GenericFactory_EF<dbSampleEntities, tbl_Sales> { }
    #endregion

    #region Commented Entities
    //public class PIBuyerGFactory_EF : GenericFactory_EF<ERP_Entities, CmnUser> { }
    //public class PICompanyGFactory_EF : GenericFactory_EF<ERP_Entities, CmnCompany> { }
    //public class ItemColorGFactory_EF : GenericFactory_EF<ERP_Entities, CmnItemColor> { }
    //public class PISampleNoGFactory_EF : GenericFactory_EF<ERP_Entities, CmnItemMaster> { }
    //public class PIComboGFactory_EF : GenericFactory_EF<ERP_Entities, CmnCombo> { }
    //public class CompanyFactory_EF : GenericFactory_EF<ERP_Entities, CmnCompany> { }
    //public class BuyerFactory_EF : GenericFactory_EF<ERP_Entities, CmnUser> { }
    //public class BankFactory_EF : GenericFactory_EF<ERP_Entities, CmnBank> { }
    //public class BankBranchFactory_EF : GenericFactory_EF<ERP_Entities, CmnBankBranch> { }
    //public class MenuPermissionMgtGFactory1 : GenericFactory<ERP_Entities, vmCmnMenuPermission> { }
    //public class CompanyTemp : GenericFactory_EF<ERP_Entities, CmnCompany> { }
    //public class ModuleDDL_GF : GenericFactory<ERP_Entities, vmCmnModule> { }
    //public class Company : GenericFactory_EF<ERP_Entities, CmnCompany> { }
    //public class ModulePMgtGFactory_EF : GenericFactory_EF<ERP_Entities, CmnModule> { }
    //public class CompanyDDL_EF : GenericFactory_EF<ERP_Entities, CmnCompany> { }
    //public class Organogram_GF : GenericFactory<ERP_Entities, vmCmnOrganogram> { }
    //public class CmnUOMFactory_EF : GenericFactory_EF<ERP_Entities, CmnUOM> { }
    //public class CmnColorFactory_EF : GenericFactory_EF<ERP_Entities, CmnItemColor> { }
    //public class CmnSizeFactory_EF : GenericFactory_EF<ERP_Entities, CmnItemSize> { }
    //public class ItemGroupFF_ddl : GenericFactory_EF<ERP_Entities, CmnItemGroup> { }
    //public partial class CmnItemMaster_EF : GenericFactory_EF<ERP_Entities, CmnItemMaster> { }
    #endregion

}
