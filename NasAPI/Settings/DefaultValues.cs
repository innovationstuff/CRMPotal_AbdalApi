using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Settings
{
    public static class DefaultValues
    {
        public const int ServiceContractPerHour_StatusCode = 100000004;
        public const bool ServiceContractPerHour_IsConfirmed = false;

        public const string Nationalitiy_individualFilterField = "new_isindv";
        public const string Profession_individualFilterField = "new_isindv";


        public static string[] CustomerTicket_DalalFinishedAppointmentStatus = { "2", "3" };

        public const string ServiceContractPerHour_DefaultPromotionCode = "- No Promotion";

        public const string DefaultLanguage = "ar";
    }
    public class AppConstants
    {
        public const string DefaultLang = "ar";
        public const string DefaultUserName = "0555555555";
        public const string DefaultUserEmail = "laborAdmin@Innovations.com";
        public const string DefaultPass = "Admin@123456";
        public const string AdminRoleName = "Admin";
        public const string MobilePhone = "0555555555";
        public const string SuperAdminsGroup = "SuperAdmins";
        //public const string MobilePhoneRex = @"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$";
        public const string MobilePhoneRex = @"^(05)(5|0|3|6|4|9|1|8|7)([0-9]{7})$";
        public const string IdNumberRex = @"^([0-9]{10})$";


        #region menu Names
        #region  Agent
        public const string AgentIndexAr = "العملاء";
        public const string AgentIndexEn = "Agents";

        public const string AgentCreateAr = "إنشاء عميل جديد";
        public const string AgentCreateEn = "Create new Agent";

        #endregion
        #endregion

       
    }
}