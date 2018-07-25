using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class PickCustomerLocation
    {
        public string ContractId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}