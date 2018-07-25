using NasAPI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
       [NasAuthorizationFilter]
       [EnableCors(origins: "*", headers: "*", methods: "*")]
       [ApiExplorerSettings(IgnoreApi = true)]

    public class CRMServicesController : ApiController
    {


        [HttpGet]
        private  int UpdateEntity(string Query)
        {

             int result = CRMAccessDB.Update(Query);


            return result;

        }




    }
}
