using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NasAPI.Inferstructures;
using NasAPI.Filters;
using System.Web.Http.Cors;
using System.Configuration;
using NasAPI.Models;
//using System.Web.Mvc;
using NasAPI.Managers;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    //[NasAuthorizationFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //api/HourlyPricing/GetHours
    [RoutePrefix("{lang}/api/HourlyPricing")] // en/api/HourlyPricing 
    public class HourlyPricingController : BaseApiController
    {
        //Get api/Nationality
        /* public IEnumerable<HourlyPricing> Get()
         {
             string sql = @"SELECT hourPrice.new_hourlypricingId hourlypricingId,hourPrice.new_name name,hourPrice.new_visitcount visitcount,hourPrice.new_visitprice visitprice,
                         hourPrice.new_discount discount,hourPrice.new_hours [hours],hourPrice.new_noofmonths noofmonths,hourPrice.new_totalvisits totalvisit,
                         hourPrice.new_totalprice totalprice,hourPrice.new_monthvisits monthvisits, 
                         CASE WHEN hourPrice.new_shift =0 then 'Morning' else 'Evening' END as [shift],
                         country.new_CountryId nationalityId,country.new_name nationalityname
                         from new_hourlypricingBase hourPrice,new_country country
                         Where hourPrice.new_nationality=country.new_CountryId
                         order by country.new_CountryId,hourPrice.new_shift ";
             DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
             List<HourlyPricing> List = new List<HourlyPricing>();
             for (int i = 0; i < dt.Rows.Count; i++) List.Add(new HourlyPricing()
             {
                 hourlypricingId = dt.Rows[i][0].ToString(),
                 Name = dt.Rows[i][1].ToString(),
                 VisitCount = dt.Rows[i][2].ToString(),
                 VisitPrice = dt.Rows[i][3].ToString(),
                 Discount = dt.Rows[i][4].ToString(),
                 Hours = dt.Rows[i][5].ToString(),
                 NoOfMonths = dt.Rows[i][6].ToString(),
                 TotalVisit = dt.Rows[i][7].ToString(),
                 TotalPrice = dt.Rows[i][8].ToString(),
                 MonthVisits = dt.Rows[i][9].ToString(),
                 Shift = dt.Rows[i][10].ToString(),
                 NationalityId = dt.Rows[i][11].ToString(),
                 NationalityName = dt.Rows[i][12].ToString()
             });

             return List;
         }

         */
        //Get api/Nationality?NationlityId={Id}&Shift={Shift}

        [Route("GetByNationalityAndShift")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<HourlyPricing> Get(string NationalityId, string Shift)
        {


            string sql = @"SELECT new_hourlypricingId hourlypricingId,new_name name,new_visitcount visitcount,new_visitprice visitprice,
                        new_discount discount,new_hours [hours],new_noofmonths noofmonths,new_totalvisits totalvisit,
                        new_totalprice totalprice,new_monthvisits monthvisits,versionnumber,new_hourprice from new_hourlypricingBase
                        Where new_nationality='@nationalityId' and new_shift=@shift";

            int shifttype = 1;
            if (Shift.ToLower() == "morning")
                shifttype = 0;
            sql = sql.Replace("@nationalityId", NationalityId);
            sql = sql.Replace("@shift", shifttype.ToString());
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];


            List<HourlyPricing> List = new List<HourlyPricing>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(new HourlyPricing()
            {
                HourePrice = dt.Rows[i]["new_hourprice"].ToString(),
                hourlypricingId = dt.Rows[i][0].ToString(),
                Name = dt.Rows[i][1].ToString(),
                VisitCount = dt.Rows[i][2].ToString(),
                VisitPrice = dt.Rows[i][3].ToString(),
                Discount = dt.Rows[i][4].ToString(),
                Hours = dt.Rows[i][5].ToString(),
                NoOfMonths = dt.Rows[i][6].ToString(),
                TotalVisit = dt.Rows[i][7].ToString(),
                TotalPrice = dt.Rows[i][8].ToString(),
                MonthVisits = dt.Rows[i][9].ToString(),
                VersionNumber = dt.Rows[i][10].ToString()

            });

            return List;


        }


        [Route("")]
        [ResponseType(typeof(IEnumerable<HourlyPricing>))]
        public HttpResponseMessage GetHourlyPricing([FromUri]RequestHourlyPricing requestHourlyPricing)
        {
            using (var mgr = new HourlyPricingManager())
            {
                var result = mgr.GetPricing(requestHourlyPricing, Language);
                return OkResponse<IEnumerable<HourlyPricing>>(result);
            }
        }

        [Route("TestPromotionValidation")]
        [ResponseType(typeof(Promotion))]
        public HttpResponseMessage testCheckPromotionValidation([FromUri]RequestHourlyPricing requestHourlyPricing)
        {
            using (var mgr = new HourlyPricingManager())
            {
                var result = mgr.testCheckPromotionValidation(requestHourlyPricing);
                return OkResponse<Promotion>(result);
            }
        }


        //        [Route("{all}")]
        //        public IEnumerable<HourlyPricing> Get(bool all)
        //        {
        //            string sql = "";
        //            if (all == false)

        //                sql = @"SELECT hourPrice.new_hourlypricingId hourlypricingId,hourPrice.new_name name,hourPrice.new_visitcount visitcount,hourPrice.new_visitprice visitprice,
        //                        hourPrice.new_discount discount,hourPrice.new_hours [hours],hourPrice.new_noofmonths noofmonths,hourPrice.new_totalvisits totalvisit,
        //                        hourPrice.new_totalprice totalprice,hourPrice.new_monthvisits monthvisits, 
        //                        CASE WHEN hourPrice.new_shift =0 then 'Morning' else 'Evening' END as [shift],
        //                        country.new_CountryId nationalityId,country.new_name nationalityname
        //                        from new_hourlypricingBase hourPrice,new_country country
        //                        Where hourPrice.new_nationality=country.new_CountryId and hourPrice.new_nationality='fa0ef838-292f-e311-b3fd-00155d010303'
        //                        order by country.new_CountryId,hourPrice.new_shift ";

        //            else

        //                sql = @"SELECT hourPrice.new_hourlypricingId hourlypricingId,hourPrice.new_name name,hourPrice.new_visitcount visitcount,hourPrice.new_visitprice visitprice,
        //                        hourPrice.new_discount discount,hourPrice.new_hours [hours],hourPrice.new_noofmonths noofmonths,hourPrice.new_totalvisits totalvisit,
        //                        hourPrice.new_totalprice totalprice,hourPrice.new_monthvisits monthvisits, 
        //                        CASE WHEN hourPrice.new_shift =0 then 'Morning' else 'Evening' END as [shift],
        //                        country.new_CountryId nationalityId,country.new_name nationalityname
        //                        from new_hourlypricingBase hourPrice,new_country country
        //                        Where hourPrice.new_nationality=country.new_CountryId and hourPrice.new_nationality !='fa0ef838-292f-e311-b3fd-00155d010303'
        //                        order by country.new_CountryId,hourPrice.new_shift ";


        //            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
        //            List<HourlyPricing> List = new List<HourlyPricing>();
        //            for (int i = 0; i < dt.Rows.Count; i++) List.Add(new HourlyPricing()
        //            {
        //                hourlypricingId = dt.Rows[i][0].ToString(),
        //                Name = dt.Rows[i][1].ToString(),
        //                VisitCount = dt.Rows[i][2].ToString(),
        //                VisitPrice = dt.Rows[i][3].ToString(),
        //                Discount = dt.Rows[i][4].ToString(),
        //                Hours = dt.Rows[i][5].ToString(),
        //                NoOfMonths = dt.Rows[i][6].ToString(),
        //                TotalVisit = dt.Rows[i][7].ToString(),
        //                TotalPrice = dt.Rows[i][8].ToString(),
        //                MonthVisits = dt.Rows[i][9].ToString(),
        //                VersionNumber = dt.Rows[i][10].ToString()

        //            });

        //            return List;
        //        }

        [Route("GetForAllMonths")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<string> Get(string allmonths)
        {
            string sql = "";


            sql = @"SELECT distinct hourPrice.new_noofmonths noofmonths
                        
                        from new_hourlypricingBase hourPrice,new_country country
                        Where hourPrice.new_nationality=country.new_CountryId 
                        ";
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<string> List = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(
              dt.Rows[i][0].ToString()

            );

            return List;
        }

        [Route("GetMonths")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<string> GetMonths()
        {
            string sql = "";


            sql = @"SELECT distinct hourPrice.new_noofmonths noofmonths
                        
                        from new_hourlypricingBase hourPrice,new_country country
                        Where hourPrice.new_nationality=country.new_CountryId 
                        ";
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<string> List = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(
              dt.Rows[i][0].ToString()

            );

            List.Insert(0, "(اسبوع واحد فقط فيه) ");
            return List;
        }

        //Get Number Of Vistis
        [HttpGet]
        [Route("GetVisits")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<string> GetVisits()
        {

            string sql = "";


            sql = @"SELECT distinct hourPrice.new_visitcount noofmonths
                        
                        from new_hourlypricingBase hourPrice,new_country country
                        Where hourPrice.new_nationality=country.new_CountryId 
                        ";
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<string> List = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(
              dt.Rows[i][0].ToString()

            );

            return List;



        }

        [HttpGet]
        [Route("GetHours")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<string> GetHours()
        {

            return new List<string>() { "4" };
        }


        [Route("ContractDays")]
        [ResponseType(typeof(IEnumerable<string>))]
        public HttpResponseMessage GetContractDays(string ContractStartDate, string NoOfMonths, string Days, int hours, bool shift = false)
        {



            DateTime d1;

            //;= DateTime.ParseExact(ContractStartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);


            // DateTime startDate;
            ContractStartDate = ContractStartDate.Replace('/', '-');

            try
            {
                d1 = DateTime.ParseExact(ContractStartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

                d1 = DateTime.ParseExact(ContractStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            }


            int months = int.Parse(NoOfMonths);
            DateTime d2;



            // if (months != 0)
            //  d2 = d1.AddDays(months*28);
            //else

            d2 = d1.AddDays(7 * months);

            List<string> dateList = new List<string>();
            double days = (d2 - d1).TotalDays - 1;
            // List<string> selecteddays = Days.Split(',').ToList();
            if (shift == true)
            {
                hours = (hours + 8);
                if (hours >= 12)
                    hours -= 12;
                if (hours < 0)
                    hours *= -1;
                if (hours == 0)
                    hours = 12;
            }
            else
            {
                hours = (hours + 4);
                if (hours >= 12)
                    hours -= 12;
                if (hours < 0)
                    hours *= -1;
                if (hours == 0)
                    hours = 12;

            }

            for (int i = 0; i <= days; i++)
            {
                DateTime d = d1.AddDays(i);
                if ((Days.Contains(d.DayOfWeek.ToString())))
                {


                    if (shift == true)
                    {

                        dateList.Add(d.ToString("dddd", new System.Globalization.CultureInfo("ar-AE")) + "    " + d.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("ar-lb")) + "   من 8 صباحاً الى " + hours + " ظهراً");
                    }
                    else
                    {

                        dateList.Add(d.ToString("dddd", new System.Globalization.CultureInfo("ar-AE")) + "    " + d.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("ar-lb")) + "    من 4 مساء الى " + hours + " مساء ");

                    }
                }
            }


            var result = dateList;
            return OkResponse<IEnumerable<string>>(result);

        }



        [Route("GetByShift")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<HourlyPricing> Get(bool all, int? shift)
        {
            string sql = "";
            if (all == false)

                sql = @"SELECT hourPrice.new_hourlypricingId hourlypricingId,hourPrice.new_name name,hourPrice.new_visitcount visitcount,hourPrice.new_visitprice visitprice,
                        hourPrice.new_discount discount,hourPrice.new_hours [hours],hourPrice.new_noofmonths noofmonths,hourPrice.new_totalvisits totalvisit,
                        hourPrice.new_totalprice totalprice,hourPrice.new_monthvisits monthvisits, 
                        CASE WHEN hourPrice.new_shift =0 then 'Morning' else 'Evening' END as [shift],
                        country.new_CountryId nationalityId,country.new_name nationalityname
                        from new_hourlypricingBase hourPrice,new_country country
                        Where hourPrice.new_nationality=country.new_CountryId and
hourPrice.new_nationality='fa0ef838-292f-e311-b3fd-00155d010303' and  hourPrice.new_shift=@shift 
                        order by country.new_CountryId,hourPrice.new_shift   ";

            else

                sql = @"SELECT hourPrice.new_hourlypricingId hourlypricingId,hourPrice.new_name name,hourPrice.new_visitcount visitcount,hourPrice.new_visitprice visitprice,
                        hourPrice.new_discount discount,hourPrice.new_hours [hours],hourPrice.new_noofmonths noofmonths,hourPrice.new_totalvisits totalvisit,
                        hourPrice.new_totalprice totalprice,hourPrice.new_monthvisits monthvisits, 
                        CASE WHEN hourPrice.new_shift =0 then 'Morning' else 'Evening' END as [shift],
                        country.new_CountryId nationalityId,country.new_name nationalityname
                        from new_hourlypricingBase hourPrice,new_country country
                        Where hourPrice.new_nationality=country.new_CountryId 
and hourPrice.new_nationality !='fa0ef838-292f-e311-b3fd-00155d010303' and hourPrice.new_shift=@shift
                        order by country.new_CountryId,hourPrice.new_shift  ";

            sql = sql.Replace("@shift", shift.ToString());

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<HourlyPricing> List = new List<HourlyPricing>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(new HourlyPricing()
            {
                hourlypricingId = dt.Rows[i][0].ToString(),
                Name = dt.Rows[i][1].ToString(),
                VisitCount = dt.Rows[i][2].ToString(),
                VisitPrice = dt.Rows[i][3].ToString(),
                Discount = dt.Rows[i][4].ToString(),
                Hours = dt.Rows[i][5].ToString(),
                NoOfMonths = dt.Rows[i][6].ToString(),
                TotalVisit = dt.Rows[i][7].ToString(),
                TotalPrice = dt.Rows[i][8].ToString(),
                MonthVisits = dt.Rows[i][9].ToString(),
                VersionNumber = dt.Rows[i][10].ToString()

            });

            return List;
        }


        //Post
        // Post api/HourlyPricing?CustomerId=&nationalityId=&hourlyPricingId=&cityId=&districtId=&shift=false&contractStartDate=&selectedDays='Monday'&noofmonths=&noofvistis=&monthvisits=
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(string))]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Post(string CustomerId, string nationalityId, string hourlyPricingId, string cityId, string districtId, bool? shift, string contractStartDate, string selectedDays, int? noofmonths, int? noofvistis, int? monthvisits)
        {

            Entity contract = new Entity("new_hindvcontract");
            // Entity Customer = GlobalCode.GetOneEntityBy("account", "accountnumber",customerNo);
            contract["new_hindivclintname"] = new EntityReference("account", new Guid(CustomerId));
            contract["new_nationality"] = new EntityReference("new_country", new Guid(nationalityId));
            contract["new_houlrypricing"] = new EntityReference("new_hourlypricing", new Guid(hourlyPricingId));
            Entity hourlyPricing = GlobalCode.Service.Retrieve("new_hourlypricing", new Guid(hourlyPricingId), new ColumnSet(true));
            contract["new_visitprice_def"] = hourlyPricing["new_visitprice"];

            //Number of Visits
            if (noofvistis == null || noofvistis == 0)
                contract["new_visitcount_def"] = hourlyPricing["new_visitcount"];
            else
                contract["new_visitcount_def"] = noofvistis;
            //MonthVisits
            if (monthvisits == null || monthvisits == 0)
                contract["new_monthvisits_def"] = hourlyPricing["new_monthvisits"];
            else
                contract["new_monthvisits_def"] = monthvisits;
            //Number Of months
            if (noofmonths == null || noofmonths == 0)
                contract["new_contractmonth"] = hourlyPricing["new_noofmonths"];
            else
                contract["new_contractmonth"] = noofmonths;

            contract["new_totalvisits_def"] = int.Parse(contract["new_visitcount_def"].ToString()) * int.Parse(contract["new_monthvisits_def"].ToString());
            //contract["new_totalvisits_def"] = hourlyPricing["new_totalvisits"];


            contract["new_discount_def"] = hourlyPricing["new_discount"];
            contract["new_totalprice_def"] = (int.Parse(contract["new_totalvisits_def"].ToString()) * int.Parse(hourlyPricing["new_visitprice"].ToString())) - int.Parse(hourlyPricing["new_discount"].ToString());
            contract["new_new_contractdate"] = DateTime.Now;
            contract["new_city"] = new EntityReference("new_city", new Guid(cityId));
            contract["new_district"] = new EntityReference("new_district", new Guid(districtId));
            contract["new_shift"] = shift; //== false ? new OptionSetValue(0) : new OptionSetValue(1);
            contract["new_contractstartdate"] = DateTime.ParseExact(contractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            contract["new_selecteddays"] = selectedDays;
            Guid contractId = GlobalCode.Service.Create(contract);
            var result = contractId.ToString();
            return OkResponse<string>(result);
        }


        // PUT api/Location/5
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/city/5
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Delete(int id)
        {
        }
    }



    //public class HourlyPricing
    //{
    //    public string HourePrice { get; set; }
    //    public string hourlypricingId { get; set; }
    //    public string Name { get; set; }
    //    public string VisitCount { get; set; }
    //    public string VisitPrice { get; set; }
    //    public string Discount { get; set; }
    //    public string Hours { get; set; }
    //    public string NoOfMonths { get; set; }
    //    public string TotalVisit { get; set; }
    //    public string TotalPrice { get; set; }
    //    public string MonthVisits { get; set; }
    //    public string Shift { get; set; }
    //    public string NationalityId { get; set; }
    //    public string NationalityName { get; set; }
    //    public string VersionNumber { get; set; }
    //    public bool? IsAvailable { get; set; }
    //    public string MonthelyPrice { get; set; }
    //    public string TotalbeforeDiscount { get; set; }

    //}
}