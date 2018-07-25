using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Filters;
using NasAPI.Inferstructures;
using NasAPI.Managers;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/DomesticInvoice")] // en/api/DomesticInvoice 
    public class DomesticInvoiceController : BaseApiController
    {
        DomesticInvoiceManager Manager;

        public DomesticInvoiceController()
        {
            Manager = new DomesticInvoiceManager();
        }

        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(DomesticInvoice))]
        public HttpResponseMessage Get(string id)
        {

            var result = Manager.GetDomesticInvoiceDetails(id, Language);
            return OkResponse(result);
        }

        [HttpGet]
        [Route("GetUserInvoices/{userId}")]
        [ResponseType(typeof(DomesticInvoice))]
        public HttpResponseMessage GetUserInvoices(string userId)
        {

            var result = Manager.GetDomesticInvoices(userId, Language);
            return OkResponse(result);
        }

    }
}
