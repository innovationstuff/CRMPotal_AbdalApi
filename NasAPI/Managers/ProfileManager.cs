using NasAPI.DisplayData;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class ProfileManager : BaseManager
    {
        public IEnumerable<BaseOptionSet> GetHourlyContractStatusForProfile(UserLanguage Language)
        {
            switch (Language)
            {
                case UserLanguage.Arabic:
                    return StaticDisplayLists.HourlyContractStatusForProfile_Arabic;
                default:
                    return StaticDisplayLists.HourlyContractStatusForProfile;
            }
        }
    }
}