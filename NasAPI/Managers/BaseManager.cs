using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class BaseManager
    {
        public string CrmEntityName { get; set; }
        public string CrmGuidFieldName { get; set; }
        public string CrmDisplayFieldName { get; set; }
        public string CrmDisplayFieldName_Arabic { get; set; }
        public string StatusFieldName { get; set; }

        public BaseManager()
            : this(null)
        {

        }
        public BaseManager(string crmEntityName)
            : this(crmEntityName, null)
        {
        }
        public BaseManager(string crmEntityName, string crmGuidFieldName)
            : this(crmEntityName, crmGuidFieldName, null)
        {

        }
        public BaseManager(string crmEntityName, string crmGuidFieldName, string crmDisplayFieldName)
            : this(crmEntityName, crmGuidFieldName, crmDisplayFieldName, null)
        {
        }
        public BaseManager(string crmEntityName, string crmGuidFieldName, string crmDisplayFieldName, string crmDisplayFieldName_Arabic)
        {
            this.CrmEntityName = crmEntityName;
            this.CrmGuidFieldName = crmGuidFieldName;
            this.CrmDisplayFieldName = crmDisplayFieldName;
            this.CrmDisplayFieldName_Arabic = crmDisplayFieldName_Arabic;
            this.StatusFieldName = "statuscode";
        }

        public virtual Entity GetCrmEntity(string id)
        {
            using (var globalmgr = new GlobalCrmManager())
                return globalmgr.GetCrmEntity(CrmEntityName, id);
        }

        public virtual Entity GetCrmEntity(string id, string[] columns)
        {
            using (var globalmgr = new GlobalCrmManager())
                return globalmgr.GetCrmEntity(CrmEntityName, id, columns);
        }

        //public virtual IEnumerable<BaseQuickLookup> GetAllForLookup(UserLanguage lang)
        //{
        //    using (var globalmgr = new GlobalCrmManager())
        //    {
        //        var displayField = (lang == UserLanguage.Arabic ? CrmDisplayFieldName_Arabic : CrmDisplayFieldName);
        //        return globalmgr.GetQuickLookup(CrmEntityName, CrmGuidFieldName, displayField);
        //    }
        //}

        public virtual IEnumerable<BaseQuickLookup> GetAllForLookup(UserLanguage lang, bool withOppositeDisplayLangIfNull = true, string filterWhereCondition = "")
        {
            using (var globalmgr = new GlobalCrmManager())
            {
                string displayField = string.Empty, oppositeDisplayField = string.Empty;
                switch (lang)
                {
                    case UserLanguage.Arabic:
                        displayField = CrmDisplayFieldName_Arabic;
                        oppositeDisplayField = (withOppositeDisplayLangIfNull ? CrmDisplayFieldName : null);
                        break;
                    default:
                        displayField = CrmDisplayFieldName;
                        oppositeDisplayField = (withOppositeDisplayLangIfNull ? CrmDisplayFieldName_Arabic : null);
                        break;
                }
                return globalmgr.GetQuickLookup(CrmEntityName, CrmGuidFieldName, displayField, oppositeDisplayField, filterWhereCondition);
            }
        }

        public virtual IEnumerable<string> GetRequiredFields()
        {
            using (GlobalCrmManager crmGlobalMgr = new GlobalCrmManager())
            {
                return crmGlobalMgr.GetRequiredFieldsNamesForEntity(CrmEntityName).ToList();
            }
        }

        public virtual IEnumerable<BaseOptionSet> GetEntityStatusOptions(UserLanguage lang)
        {
            using (var globalCrmManager = new GlobalCrmManager())
            {
                return globalCrmManager.GetOptionSetLookup(CrmEntityName, "statuscode", lang);
            }
        }



    }
}