using NasAPI.Controllers.API;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NasAPI.ApiFilter
{
    public class LocalizationFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as BaseApiController;
            if (controller != null)
            {
                var langRouting = actionContext.RequestContext.RouteData.Values["lang"].ToString().ToLower();
                switch (langRouting)
                {
                    case "en":
                        controller.Language = UserLanguage.English;
                        break;
                    case "ar":
                        controller.Language = UserLanguage.Arabic;
                        break;
                    default:
                        controller.Language = UserLanguage.English;
                        break;
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}