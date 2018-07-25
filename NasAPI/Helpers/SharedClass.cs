using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NasAPI.Helpers
{
    public static class SharedClass
    {
        public static string ApiServerUrl => ConfigurationManager.AppSettings["APIServerUrl"];
    }
}