using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NasAPI.Controllers.API;
using System.Data;

namespace NasAPI.Managers
{
    public class CityManager : BaseManager
    {

        public CityManager() : base(CrmEntityNamesMapping.City, "new_cityid", "new_englsihName", "new_name")
        {

        }

        internal IEnumerable<District> GetDistrictsByCity(string id)
        {
            string sql = @"SELECT new_districtId districtId,new_name name,new_days days,new_shifts shifts,versionnumber from new_districtBase 
                        where new_cityid='@cityId' AND new_days IS NOT NULL AND LEN(new_days) > 0 
                        AND new_shifts IS NOT NULL AND LEN(new_shifts) > 0  order by new_name ";
            sql = sql.Replace("@cityId", id);

            List<District> newDistricts = CRMAccessDB.SelectQ(sql).Tables[0].AsEnumerable().Select(dataRow => new District(dataRow)).ToList();

            newDistricts.ForEach(t =>
            {
                if (t.days.IndexOf(",Friday") > -1) t.days = t.days.Replace(",Friday", "");
                if (t.days.IndexOf("Friday") > -1) t.days = t.days.Replace("Friday", "");
            });

            return newDistricts.Select(t => t);
        }
    }
}