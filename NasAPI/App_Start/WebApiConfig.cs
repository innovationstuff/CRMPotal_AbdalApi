using NasAPI.ApiFilter;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace NasAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Enable the Cors Domain 
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ApiControllerActionId",
                routeTemplate: "{lang}/api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional, lang = "en"}

            );


            config.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "{lang}/api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional, lang = "en" }
          );


            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
         
            // =============Filters==========

            config.Filters.Add(new LocalizationFilter());
            config.Filters.Add(new ActionValidationFilter());
            config.Filters.Add(new ExceptionFilter());

        }
    }
}
