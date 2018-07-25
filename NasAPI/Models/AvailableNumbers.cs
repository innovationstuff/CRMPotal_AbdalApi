using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class AvailableNumbers
    {
        public string NationalityId { get; set; }
        public string Nationality { get; set; }
        public List<ProfessionCount> ProfessionCounts { get; set; }
    }
    public class ProfessionCount
    {
        public string ProfessionId { get; set; }
        public string Profession { get; set; }
        public string Count { get; set; }
    }
}