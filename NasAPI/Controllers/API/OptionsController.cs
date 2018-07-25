using NasAPI.Filters;
using NasAPI.Managers;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{

    //[NasAuthorizationFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/Options")] // en/api/Options 
    [ApiExplorerSettings(IgnoreApi = true)]

    public class OptionsController : BaseApiController
    {
        GeneralManager Mananger;

        public OptionsController()
        {
            this.Mananger = new GeneralManager();
        }




        #region Old Actions

        public string SqlQuery
        {
            get
            {

                return @"select    AttributeValue,Value from StringMap s inner join EntityLogicalView e on s.ObjectTypeCode = e.ObjectTypeCode 
                                   where e.Name = '@entityname' and s.AttributeName = '@optionname'  and LangId=@lang"


                    ;
            }
        }

        public List<BaseOptionSet> GetVisits()
        {
            return Mananger.GetOptionSet_HourlyContract_Visits(Language).ToList();

            //string sql = SqlQuery.Replace("@entityname", "new_HIndvContract").Replace("@optionname", "new_weeklyvisits")
            //    .Replace("@lang", lang==0 ?"1025":"1033");

            //DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            //List<OptionList> list = new List<OptionList>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //    list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString() });

            //return list.ToList();
        }
       // api/HourlyContract/options/Labours
        public List<OptionList> GetLabours(int lang = 0)
        {

            string sql = SqlQuery.Replace("@entityname", "new_HIndvContract").Replace("@optionname", "new_employeenumber")
                  .Replace("@lang", lang == 0 ? "1025" : "1033");

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<OptionList> list = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
                list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString()+"عامل" });

            return list.ToList();
        }

        public List<OptionList> GetMonths(int lang = 0)
        {

            string sql = SqlQuery.Replace("@entityname", "new_HIndvContract").Replace("@optionname", "new_contractmonth")
                  .Replace("@lang", lang == 0 ? "1025" : "1033");
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<OptionList> list = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
                list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString() });

            return list.ToList();
        }

        public List<OptionList> GetHours(int lang = 0)
        {
            List<OptionList> list = new List<OptionList>();
            list.Add(new OptionList() { Id = "4", Name = " ساعات 4" });
            list.Add(new OptionList() { Id = "5", Name = " ساعات 5" });



            return list;
        }

        public List<OptionList> GetComplaintsCategories(int lang = 0)
        {

            string sql = SqlQuery.Replace("@entityname", "new_csindvsector").Replace("@optionname", "new_contracttype")
                  .Replace("@lang", lang == 0 ? "1025" : "1033");
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<OptionList> list = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["AttributeValue"].ToString() == "3") continue;
                list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString() });
            }

            return list.Where(a => a.Id != "6").ToList();

        }


        public List<OptionList> GetComplaintsTypes(int lang = 0)
        {

            string sql = SqlQuery.Replace("@entityname", "new_csindvsector").Replace("@optionname", "new_problemcase")
                  .Replace("@lang", lang == 0 ? "1025" : "1033");
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<OptionList> list = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.Parse(dt.Rows[i]["AttributeValue"].ToString()) <= 100 || dt.Rows[i]["AttributeValue"].ToString() == "100000005")
                    list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString() });
            }

            return list.Where(a => a.Id != "6").ToList();

        }

        public List<OptionList> GetPricingList(int lang = 0)
        {

            string sql = SqlQuery.Replace("@entityname", "new_indvprice").Replace("@optionname", "new_pricetype")
                  .Replace("@lang", lang == 0 ? "1025" : "1033");

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<OptionList> list = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString() });
            }
            list = list.Where(a => a.Id != "1" && a.Id != "60" && a.Name != "اخرى").ToList();
            return list;

        }


        public List<OptionList> GetComplaintsStatus(int lang = 0)
        {

            string sql = SqlQuery.Replace("@entityname", "new_csindvsector").Replace("@optionname", "statuscode")
                  .Replace("@lang", lang == 0 ? "1025" : "1033");

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<OptionList> list = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString() });
            }

            return list.Where(a => a.Id != "1" && a.Id != "2").ToList();

        }

        public List<OptionList> GetSectors(int lang = 0)
        {

            //new_company_busienss
            string sql = SqlQuery.Replace("@entityname", "account").Replace("@optionname", "industrycode")
                  .Replace("@lang", lang == 0 ? "1025" : "1033");

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<OptionList> list = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                list.Add(new OptionList() { Id = dt.Rows[i]["AttributeValue"].ToString(), Name = dt.Rows[i]["Value"].ToString() });
            }

            return list;

        }

        //new_companysector

        public static string GetName(string EntityName, string FieldName, int language, string Value)
        {


            string SQL = @"select   Value from StringMap s inner join EntityLogicalView e on s.ObjectTypeCode = e.ObjectTypeCode 
                                   where e.Name = '@entityname' and s.AttributeName = '@optionname'  and LangId=@lang
        						   and s.AttributeValue='@value'";
            SQL = SQL.Replace("@entityname", EntityName).Replace("@optionname", FieldName).Replace("@lang", language.ToString()).Replace("@value", Value);

            DataTable dt = CRMAccessDB.SelectQ(SQL).Tables[0];
            return dt.Rows[0]["Value"].ToString();
        }


        //statuscode

        #endregion
    }









    public class OptionList
    {

        public string Id { get; set; }
        public string Name { get; set; }

    }
}
