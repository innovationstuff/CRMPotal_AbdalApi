using NasAPI.Controllers.API;
using NasAPI.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace NasAPI.Controllers.LaborServicesApi
{
    [RoutePrefix("{lang}/api/Translator")]
    public class TranslatorController : BaseApiController
    {
        [HttpGet]
        [Route("GetTerms")]
        [ResponseType(typeof(object))]
        public HttpResponseMessage GetTerms()
        {
            var TranslatorMgr = new TranslatorManager();
            var result = TranslatorMgr.GetTerms(Language);
            return OkResponse(result);

        }

        [HttpGet]
        [Route("GetTermsForMobile")]
        [ResponseType(typeof(object))]
        public HttpResponseMessage GetTermsForMobile()
        {
            var TranslatorMgr = new TranslatorManager();
            var result = TranslatorMgr.GetTermsForMobile(Language);
            return OkResponse(result);

        }

        [HttpGet]
        [Route("GetDalalAdditionalWarning")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage GetDalalAdditionalWarning()
        {
            var TranslatorMgr = new TranslatorManager();
            var result = TranslatorMgr.GetDalalAdditionalWarning(Language);
            return OkResponse<string>(result);

        }
    }
}