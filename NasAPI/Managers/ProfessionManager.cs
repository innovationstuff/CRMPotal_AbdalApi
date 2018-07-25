using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class ProfessionManager : BaseManager
    {

        public ProfessionManager() : base(CrmEntityNamesMapping.Profession, "new_professionId", "new_professionenglish", "new_name")
        {

        }

        public virtual IEnumerable<BaseQuickLookup> GetIndividualProfessions(UserLanguage lang)
        {
            string condition = String.Format("Where {0} = 1", DefaultValues.Profession_individualFilterField);
            return GetAllForLookup(lang, true, condition);
        }

    }

}