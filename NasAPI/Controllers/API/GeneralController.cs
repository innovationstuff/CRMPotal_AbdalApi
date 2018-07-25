using NasAPI.Managers;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    [RoutePrefix("{lang}/api/General")] // en/api/Nationality 
    public class GeneralController : BaseApiController
    {
        GeneralManager Manager { get; set; }

        public GeneralController()
        {
            Manager = new GeneralManager();
        }

        [Route("Regions")]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetAllRegions()
        {
            var result = Manager.GetAllRegionsForLookup(Language).ToList();
            return OkResponse<List<BaseQuickLookup>>(result);
        }

        //[Route("GetPromotions")]
        //[ResponseType(typeof(List<Promotion>))]
        //public HttpResponseMessage GetPromotions(string code)
        //{
        //    var result = Manager.GetPromotions(code).ToList();
        //    return OkResponse<List<Promotion>>(result);
        //}



    }
}
