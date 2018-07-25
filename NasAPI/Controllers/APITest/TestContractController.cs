using NasAPI.Controllers.API;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Microsoft.Xrm.Sdk;
using NasAPI.Managers;
using NasAPI.Settings;

namespace NasAPI.Controllers.APITest
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/TestContract")] // en/api/TestContract
    public class TestContractController : BaseApiController
    {
        // GET: TestContract
        [Route("GetAll")]
        [HttpGet]
        [ResponseType(typeof(List<ServiceContractPerHour>))]
        public HttpResponseMessage GetAllContracts()
        {
            string query = "select new_hindvcontractid , new_contractnumber ," +
                           "new_districtName from new_hindvcontract";

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];

            var result = dt.AsEnumerable().Select(dataRow => new ServiceContractPerHour(dataRow)).ToList();

            return OkResponse(result);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        [ResponseType(typeof(List<ServiceContractPerHour>))]
        public HttpResponseMessage GetContractById(Guid id)
        {
            string query = string.Format("select new_hindvcontractid , new_contractnumber ," +
                                         "new_districtName,new_cityName from new_hindvcontract where new_hindvcontractid= '{0}'",
                id);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];

            var result = dt.AsEnumerable().Select(dataRow => new ServiceContractPerHour(dataRow)).ToList();

            return OkResponse(result);
        }

        [Route("CreateNew")]
        [HttpPost]
        [ResponseType(typeof(RequestServiceContractPerHour))]
        public HttpResponseMessage CreateNewContract(RequestServiceContractPerHour contract)
        {
            ServiceContractPerHourManager manager=new ServiceContractPerHourManager();
            Entity contractEntity = manager.CastToCrmEntity(contract);
            Guid newContractGuid = GlobalCode.Service.Create(contractEntity);
            Entity newEntity = manager.GetCrmEntity(newContractGuid.ToString());
            return OkResponse(contract);
        }
    }
}