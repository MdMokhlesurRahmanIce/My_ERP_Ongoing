using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iProductionDDLMgt:IDisposable
    {
        List<vmProductionUOMDropDown> GetAllUnit(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmProductionColorDropDown> GetAllColor(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmProductinItemDropDown> GetAllItem(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmProductinItemDropDown> GetChemicalItemByGroupID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? GroupID);
        List<vmProductinItemDropDown> GetItemMasterByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID);
        List<vmProductinDyingProcessDropDown> GetDyingProcessByProcessID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ProcessID);
        IEnumerable<vmPrdDyingOperationDropDown> GetDyingOperationByProcessID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ProcessID);
        List<vmItemMaster> GetArticals(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId);
        List<vmSetDetail> GetSetNoByArticalNo(int? pageNumber, int? pageSize, int? IsPaging, int? ItemID);
        List<vmOutputUnit> GetCanNoByDeapartmentId(int? pageNumber, int? pageSize, int? IsPaging, int? DepartmentID);
        IEnumerable<vmItemSetSetup> GetDetailsMachine(vmCmnParameters objcmnParam);
        IEnumerable<vmItemSetSetup> GetArticle(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmItemSetSetup> GetItemAsMachine(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmItemSetSetup> GetMachine(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmItemSetSetup> GetDetailOperation(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmSetDetail> GetSetNo(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmSetDetail> GetSetByArticalNo(vmCmnParameters objcmnParam, out int recordsTotal);
        vmProductionUOMDropDown GetUnitSingle(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmProductionUOMDropDown> GetUnit(vmCmnParameters objcmnParam, out int recordsTotal);
        List<vmProductionPrdSetSetupDDL> GetSetNoByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID);
        List<vmProductionDyingSetNoDropDown> GetDyingSetNoByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID);
        List<vmItemMaster> GetDyingItemMachineByItemTypeGroup(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? ItemGroupID);
        List<vmProductionDyingHRMShiftDropDown> GetShift(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);

        IEnumerable<vmBallInfo> GetBeams(vmCmnParameters objcmnParam);
        IEnumerable<vmShiftName> GetShifts(vmCmnParameters objcmnParam);
        IEnumerable<vmOperator> GetOperators(vmCmnParameters objcmnParam);
        IEnumerable<vmBeamQuality> GetBeamQuality(vmCmnParameters objcmnParam);
        IEnumerable<PrdBWSlist> GetLoadMachineStopCauses(vmCmnParameters objcmnParam);
        IEnumerable<PrdBWSlist> GetLoadMachineBrekages(vmCmnParameters objcmnParam);
        IEnumerable<vmWeavingLine> GetWeavingLine(vmCmnParameters objcmnParam);        
        IEnumerable<PrdWeavingMRRMaster> GetWeavingSetNo(vmCmnParameters objcmnParam);
        IEnumerable<PrdFinishingType> GetFinishingType(vmCmnParameters objcmnParam);
        IEnumerable<vmGrade> GetAllGrade(vmCmnParameters objcmnParam);
        IEnumerable<CmnOrganogram> GetDepartment(vmCmnParameters objcmnParam);
        CmnOrganogram GetDepartmentByID(vmCmnParameters objcmnParam);
        IEnumerable<CmnCombo> GetBWSType(vmCmnParameters objcmnParam);
        List<vmPI> GetAllPI(vmCmnParameters objcmnParam);
        IEnumerable<vmCmnBatch> GetAllBatches(vmCmnParameters objcmnParam);
        IEnumerable<PrdFinishingProcess> GetFinishingProcessType(vmCmnParameters objcmnParam);
        List<vmItemMaster> GetMachineByTypeAndGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? ItemTypeID, int? ItemGroupID);
        List<vmItemMaster> GetAllArticalByItemType(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type);
        List<vmLoom> GetLoom(int? LoginCompanyID);
        List<vmWeavingLine> GetLines(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId);
        List<WeavingMachine> GetWeavingMachines(int? pageNumber, int? pageSize, int? IsPaging, int? departmentId);
        List<vmShiftName> getShiftes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId);
        List<vmOperator> getOperators(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type);
       
        IEnumerable<Models.ViewModel.Production.vmDepartment> GetDepartmentByCompayUserID(int? companyID, int? loggedUser);
        List<vmPrdDyingMachinePart> GetMachinePartByMachineID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? MachineID);
        List<vmDyingChemicalConsumptionOperation> GetOperation(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        List<vmStyle> GetStyleNoes(vmCmnParameters objcmnParam);
        List<vmMachineNo> GetMachine(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type);
        List<vmPlate> GetPlates(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type);
        List<vmItemMaster> GetItmeByItemType(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID);
        List<vmCmnBatch> GetBatchByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID);
        List<vmDefectPoint> GetDefectPoints(vmCmnParameters objcmnParam);
        List<vmPrdFinishingType> GetPrdFinishingType(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
      

        List<vmFinishingInspactionDetail> GetInspactionDetailsByIDandDates(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? ItemId, DateTime? FromDate, DateTime? ToDate);
        List<vmPrdWeavingMachinConfig> GetMachine(vmCmnParameters objcmnParam);
        List<vmPrdDyingMachineSetup> GetPrdDyingMachineSetupByItemMachine(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID, int? MechineID);
        List<vmPrdSetSetup> GetDyeingSetAll(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        object GetDyingMachineForChemicalProcess(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);

        object GetDyingReferenceByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID);

        void Dispose();
    }
}
