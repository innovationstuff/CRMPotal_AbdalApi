using NasAPI.Managers;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace NasAPI.Controllers.API
{
    public class BaseApiController : ApiController
    {
        public UserLanguage Language { get; set; }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        protected HttpResponseMessage NotFoundResponse(string key, string message)
        {
            ModelState.AddModelError(key, message);
            return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);
        }

        protected HttpResponseMessage NotFoundResponse()
        {
            return NotFoundResponse("NotFound", "Not Found");
        }

        protected HttpResponseMessage OkResponse<T>(T result)
        {
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        protected HttpResponseMessage OkResponse()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}