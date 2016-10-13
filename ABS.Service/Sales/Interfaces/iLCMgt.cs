using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models;
using ABS.Models.ViewModel.Sales;
using System.Collections;
using ABS.Models.ViewModel.SystemCommon;


namespace ABS.Service.Sales.Interfaces
{
    public interface iLCMgt
    {
        IEnumerable<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnUser> GetBuyer(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnBank> GetBank(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnBankBranch> GetBankBranchById(int id);
        IEnumerable<vmSalLCDetail> GetLCMaster(vmCmnParameters cmnParam ,out int recordsTotal);
        IEnumerable<vmSalLCDetail> GetPendingPI(int id, int companyID);
        //IEnumerable<vmSalLCDetail> GetLCDetailByID(int id);
        IEnumerable<vmSalLCDetail> GetLCDetailByID(vmCmnParameters cmnParam, out int recordsTotal);
        IEnumerable<vmSalLCDetail> GetLCMasterById(int id);
        string SaveUpdateLC(SalLCMaster itemMaster, List<SalLCDetail> itemDetails, ArrayList fileNames, vmCmnParameters objcmnParam);
        CmnDocumentPath GetUploadPath();
        IEnumerable<vmCmnDocument> GetFileDetailsById(int id);
    }
}
