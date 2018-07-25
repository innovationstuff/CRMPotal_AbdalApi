using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NasAPI.Settings;
using NasAPI.Models;
using System.Data;

namespace NasAPI.Managers
{
    public class GeneralManager : BaseManager
    {
        GlobalCrmManager GlobalCrmManager { get; set; }

        public GeneralManager()
            : base(null)
        {
            GlobalCrmManager = new GlobalCrmManager();
        }

        public IEnumerable<BaseQuickLookup> GetAllRegionsForLookup(UserLanguage language)
        {
            var displayField = (language == UserLanguage.Arabic ? "name" : "new_nameenglish");
            return GlobalCrmManager.GetQuickLookup(CrmEntityNamesMapping.Region, "territoryid", displayField);
        }

        public IEnumerable<BaseOptionSet> GetOptionSet_HourlyContract_Visits(UserLanguage language)
        {
            return GlobalCrmManager.GetOptionSetLookup("new_HIndvContract", "new_weeklyvisits", language);
        }

        public IEnumerable<BaseOptionSet> GetOptionSet_HourlyContract_Labours(UserLanguage language)
        {


            return GlobalCrmManager.GetOptionSetLookup("new_HIndvContract", "new_employeenumber", language).OrderBy(a => a.Key);

        }

        public IEnumerable<BaseOptionSet> GetOptionSet_HourlyContract_ContractDuration(UserLanguage language)
        {
            return GlobalCrmManager.GetOptionSetLookup("new_HIndvContract", "new_contractmonth", language).OrderBy(a => a.Key);

        }

        public IEnumerable<BaseOptionSet> GetOptionSet_HourlyContract_Hours(UserLanguage language)
        {
            IEnumerable<BaseOptionSet> result = GlobalCrmManager.GetOptionSetLookup("new_HIndvContract", "new_hoursnumber", language);

            result = result.Where(t => t.Key != 5);

            return result;
        }

        public IEnumerable<BaseOptionSet> GetOptionSet_Lead_Industrycodes(UserLanguage language)
        {
            return GlobalCrmManager.GetOptionSetLookup("lead", "industrycode", language);
        }

        public IEnumerable<BaseQuickLookup> GetAllRegions(UserLanguage language)
        {
            var displayField = (language == UserLanguage.Arabic ? "name" : "new_nameenglish");
            var displayFieldOpposite = (language == UserLanguage.Arabic ? "new_nameenglish" : "name");
            return GlobalCrmManager.GetQuickLookup(CrmEntityNamesMapping.Region, "territoryid", displayField, displayFieldOpposite);
        }

        public IEnumerable<BaseOptionSet> GetOptionSet_HourlyContract_HousingTypes(UserLanguage language)
        {
            return GlobalCrmManager.GetOptionSetLookup("new_HIndvContract", "new_housetype", language); ;
        }

        public IEnumerable<BaseOptionSet> GetOptionSet_HourlyContract_HousingFloors(UserLanguage language)
        {
            return GlobalCrmManager.GetOptionSetLookup("new_HIndvContract", "new_floorno", language);

        }

    }
}