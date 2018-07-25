using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace NasAPI.Models
{
    public class APIResponseModel<T>
    {
        public T Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}