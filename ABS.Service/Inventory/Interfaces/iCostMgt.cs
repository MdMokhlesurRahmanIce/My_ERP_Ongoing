using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;



namespace ABS.Service.Inventory.Interfaces
{
    public interface iCostMgt
    {

        IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnAddressCountry> GetLocation(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<PurchasePOMaster> GetPurchaseOrderList(vmCmnParameters objcmnParam); 

        IEnumerable<PurchaseTax> GetTaxTypeByCategoryId(vmCmnParameters objcmnParam); 

        IEnumerable<PurchaseTaxCategory> GetTaxCategory(vmCmnParameters objcmnParam);

        IEnumerable<vmCost> GetCostAccessmentMaster(vmCmnParameters objcmnParam);


        IEnumerable<vmCost> GetCostAccessmentDetailByCostAccessmentId(vmCmnParameters objcmnParam);

        IEnumerable<vmCost> GetCostInfoByPOID(vmCmnParameters objcmnParam);

        string SaveUpdateAccessmentCostMasterNdetails(PurchaseCostAccessmentMaster itemMaster, List<PurchaseCostAccessmentDetail> itemDetails, int menuID);


        //--------------------- Cost Clearing -------------------------------------------------------------------------------

        IEnumerable<PurchaseConsumerChargeType> GetConsumerChargeType(vmCmnParameters objcmnParam);
        
        string SaveUpdateClearingCostMasterNdetails(PurchaseCostClearingMaster itemMaster, List<PurchaseCostClearingDetail> itemDetails, int menuID);

        IEnumerable<vmCost> GetCostClearingMaster(vmCmnParameters objcmnParam);

        IEnumerable<vmCost> GetCostClearingDetailByCostClearingId(vmCmnParameters objcmnParam);

        

        //----------------------------------------------- Cost Transport ------------------------------------------------------

        IEnumerable<PurchaseConsumerChargeType> GetVehicles(vmCmnParameters objcmnParam);

        string SaveUpdateTransportCostMasterNdetails(PurchaseCostTransportMaster itemMaster, List<PurchaseCostTransportDetail> itemDetails, int menuID);

        IEnumerable<vmCost> GetCostTransportMaster(vmCmnParameters objcmnParam);

        IEnumerable<vmCost> GetCostTransportDetailByCostTransportId(vmCmnParameters objcmnParam);



    }
}
