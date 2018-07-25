using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class NationalityManager : BaseManager
    {

        public NationalityManager() : base(CrmEntityNamesMapping.Nationality, "new_countryid", "new_nameenglish", "new_name")
        {

        }


        public virtual IEnumerable<BaseQuickLookup> GetIndividualNationalities(UserLanguage lang)
        {
            string condition = String.Format( "Where {0} = 1", DefaultValues.Nationalitiy_individualFilterField);
            return GetAllForLookup(lang, true, condition);
        }


    }

}