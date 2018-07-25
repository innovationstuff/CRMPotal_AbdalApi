using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class RequestHourlyPricing
    {

        public string NationalityId { get; set; }
        public string Days { get; set; }
        public string ContractStartDate { get; set; }
        public int ContractDuration { get; set; }
        public string DistrictId { get; set; }
        public int NoOfVisits { get; set; }
        public int HoursCount { get; set; }
        public int Empcount { get; set; }
        public int Weeklyvisits { get; set; }
        public string CityId { get; set; }

        public string PromotionCode { get; set; }
    }
}