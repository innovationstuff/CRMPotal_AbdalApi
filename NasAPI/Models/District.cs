using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class District
    {
        //,new_days days,new_shifts shifts,versionnumber from new_districtBase 
        public string districtId { get; set; }
        public string name { get; set; }
        public string days { get; set; }
        public string shifts { get; set; }

        public District()
        {

        }

        public District(DataRow row)
        {
            districtId = row[0].ToString();
            name = row[1].ToString();
            days = row[2].ToString();
            shifts = row[3].ToString();
        }

    }
}