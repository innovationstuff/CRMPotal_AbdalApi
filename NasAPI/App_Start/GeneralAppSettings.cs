using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NasAPI
{
    public static class GeneralAppSettings
    {
        public static string RequestLang
        {
            get
            {
                //var lang = HttpContext.Current.Request.RequestContext.RouteData.Values["lang"].ToString().ToLower();
                //return !string.IsNullOrEmpty(lang) ? lang : DefaultValues.DefaultLanguage;
                return DefaultValues.DefaultLanguage;
            }
        }
    }

}