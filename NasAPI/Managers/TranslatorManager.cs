using Microsoft.Xrm.Sdk;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class TranslatorManager
    {
        public string GetTerms(UserLanguage lang)
        {
            try
            {


                //1-Getting all RV that dident saved in CRM 
                string sql = @" select * FROM [LaborServices].[dbo].[Localizations]
  where [ResourceId] like 'TermsData' and [LocaleId] like '" + (lang == UserLanguage.Arabic ? "ar" : "en") + "'";
                DataTable dt = CRMAccessDB.SelectQlabourdb(sql).Tables[0];
                if (dt.Rows.Count > 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string terms = dt.Rows[i]["Value"].ToString();
                        return terms;
                    }
            }
            catch (Exception e)
            {
                return "";
            }

            return "";
        }

        internal string GetTermsForMobile(UserLanguage lang)
        {
            try
            {


                //1-Getting all RV that dident saved in CRM 
                string sql = @" select * FROM [LaborServices].[dbo].[Localizations]
  where [ResourceId] like 'TermsDataForMobile' and [LocaleId] like '" + (lang == UserLanguage.Arabic ? "ar" : "en") + "'";
                DataTable dt = CRMAccessDB.SelectQlabourdb(sql).Tables[0];
                if (dt.Rows.Count > 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string terms = dt.Rows[i]["Value"].ToString();
                        return terms;
                    }
            }
            catch (Exception e)
            {
                return "";
            }

            return "";
        }

        public string GetDalalAdditionalWarning(UserLanguage lang)
        {
            return Translate("DalalAdditionalWarning", lang);
        }

        protected string Translate(string ResourceId, UserLanguage lang)
        {

            try
            {


                //1-Getting all RV that dident saved in CRM 
                string sql = String.Format(@" select [Value] FROM [LaborServices].[dbo].[Localizations]
                                                where [ResourceId] like '{0}' and [LocaleId] like '{1}'", ResourceId, (lang == UserLanguage.Arabic ? "ar" : "en"));
                DataTable dt = CRMAccessDB.SelectQlabourdb(sql).Tables[0];

                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["Value"].ToString();

                return string.Empty;

            }
            catch (Exception e)
            {
                return string.Empty;
            }

        }
    }
}