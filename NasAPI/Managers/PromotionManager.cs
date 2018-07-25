using Microsoft.Xrm.Sdk;
using NasAPI.Inferstructures;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NasAPI.Managers
{
    public class PromotionManager : BaseManager, IDisposable
    {

        public List<string> InvalidProperties { get; set; }

        public PromotionManager() :
            base(CrmEntityNamesMapping.Promotion, "new_promotionslistId", "new_name")
        {

        }

        public void Dispose()
        {

        }

        public Promotion GetPromotionByCode(RequestHourlyPricing requestHourlyPricing, UserLanguage Lang)
        {

            string code = requestHourlyPricing.PromotionCode;
            if (string.IsNullOrEmpty(code))
                code = DefaultValues.ServiceContractPerHour_DefaultPromotionCode;

            string query = String.Format(@"select new_promotionslistId,
                                            new_name,new_code,new_description,new_freevisits,new_discount,
                                            new_fixeddiscount,new_availalbe,new_fromdate,new_todate
                                            from new_promotionslist where new_availalbe = 1 and new_code='{0}'", code);
            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
            if (dt.Rows.Count == 0 && code != DefaultValues.ServiceContractPerHour_DefaultPromotionCode)
            {
                requestHourlyPricing.PromotionCode = DefaultValues.ServiceContractPerHour_DefaultPromotionCode;

                return GetPromotionByCode(requestHourlyPricing, Lang);
            }

            bool checkresult = true;

            PropertyInfo[] properties = typeof(RequestHourlyPricing).GetProperties();

            InvalidProperties = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                string propValue = property.GetValue(requestHourlyPricing) != null ? property.GetValue(requestHourlyPricing).ToString() : "";

                if (!string.IsNullOrEmpty(propValue))
                {
                    bool isPromotionValid = checkpromotionvalidation(property.Name, propValue, dt.Rows[0]["new_promotionslistId"].ToString());
                    checkresult = isPromotionValid && checkresult;
                }
            }

            if (checkresult == false)
            {
                //requestHourlyPricing.PromotionCode = DefaultValues.ServiceContractPerHour_DefaultPromotionCode;

                string query1 = String.Format(@"select new_promotionslistId,
                                            new_name,new_code,new_description,new_freevisits,new_discount,
                                            new_fixeddiscount,new_availalbe,new_fromdate,new_todate
                                            from new_promotionslist where new_code='{0}'", DefaultValues.ServiceContractPerHour_DefaultPromotionCode);
                DataTable dt1 = CRMAccessDB.SelectQ(query1).Tables[0];

                var p = new Promotion(dt1.AsEnumerable().FirstOrDefault());

                //var p = GetPromotionByCode(requestHourlyPricing);
                
                string errorMessage = "";

                if (Lang == UserLanguage.Arabic)
                {
                    errorMessage = "كود الخصم غير مناسب مع الباقة المختارة";
                }
                else
                {
                    errorMessage = "Invalid Promotion With Selected Package. ";
                }
                //if (InvalidProperties.Any())
                //{
                //    InvalidProperties.ForEach(s =>
                //    {
                //        errorMessage += " [ " + s + " ] ";
                //    });
                //}
                p.Name = errorMessage;
                return p;
            }

            var validPromotion = new Promotion(dt.AsEnumerable().FirstOrDefault());
            validPromotion.Name = validPromotion.Description;

            return validPromotion;
            //return dt.AsEnumerable().Select(dataRow => new Promotion(dataRow));
        }

        public bool checkpromotionvalidation(string fieldname, string value, string promoid)
        {

            string query = @"		select *  from new_promovalidation 
											inner join new_promotionslist on new_promovalidation.new_promotionvalidationid=new_promotionslist.new_promotionslistId
                                            where  new_promovalidation.new_promotionvalidationid='@id'  and new_fieldname='@fieldname'";

            query= query.Replace("@id", promoid);
            query= query.Replace("@fieldname", fieldname);


            DataTable dtvalidationvalues = CRMAccessDB.SelectQ(query).Tables[0];
            if (dtvalidationvalues.Rows.Count == 0) return true;


            bool checkresult = true;
            for (int i = 0; i < dtvalidationvalues.Rows.Count; i++)
            {
                decimal resultdecimal = 0;
                decimal valuedecimal = 0;

                if (checkresult == false)
                {
                    InvalidProperties.Add(fieldname);
                    return checkresult;
                }
                
                if (decimal.TryParse(dtvalidationvalues.Rows[i]["new_fieldvalue"].ToString(), out resultdecimal) && decimal.TryParse(value, out valuedecimal))
                {

                    switch (dtvalidationvalues.Rows[i]["new_operator"].ToString())
                    {
                        case "<":
                            checkresult = (valuedecimal < resultdecimal) && checkresult;
                            break;
                        case ">":
                            checkresult = (valuedecimal > resultdecimal) && checkresult;
                            break;
                        case "<=":
                            checkresult = (valuedecimal <= resultdecimal) && checkresult;
                            break;
                        case ">=":
                            checkresult = (valuedecimal >= resultdecimal) && checkresult;
                            break;
                        case "=":
                            checkresult = (valuedecimal == resultdecimal) && checkresult;
                            break;
                        case "==":
                            checkresult = (valuedecimal == resultdecimal) && checkresult;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    checkresult = (value == dtvalidationvalues.Rows[i]["new_fieldvalue"].ToString()) && checkresult;
                }
            }

            return checkresult;

        }

    }
}