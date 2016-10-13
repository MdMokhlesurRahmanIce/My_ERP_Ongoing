using ABS.Models;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/TransactionTypes")]
    public class TransactionTypesController : ApiController
    {
        private iCmnTransactionTypeMgt objComService = null;

        public TransactionTypesController()
        {
            this.objComService = new CmnTransactionTypesMgt();
        }

        // GET: GetCustomers/0/10/0
        [Route("GetTransactionTypes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnTransactionType))]
        [HttpGet]
        public IEnumerable<CmnTransactionType> GetTransactionTypes(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnTransactionType> objListCompanies = null;
            try
            {
                objListCompanies = objComService.GetTransactionTypes(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompanies;
        }

       
    }
}
