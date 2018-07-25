using NasAPI.Enums;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class HourlyPricingShiftManager : IDisposable
    {
        public void Dispose()
        {

        }

        public IEnumerable<string> GetDays(RequestHourlyPricing requestHourlyPricing, DayShifts Shift, int countOfDays, Promotion promotion)
        {
            requestHourlyPricing.ContractStartDate = requestHourlyPricing.ContractStartDate.Replace('/', '-');
            DateTime startDate;
            try
            {
                startDate = DateTime.ParseExact(requestHourlyPricing.ContractStartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {

                startDate = DateTime.ParseExact(requestHourlyPricing.ContractStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            }

            var totalVisits = requestHourlyPricing.ContractDuration * requestHourlyPricing.Weeklyvisits;
            var extraVisits = (promotion.FreeVisitsFactor ?? 0) == 0 ? 0 : Math.Truncate((decimal)totalVisits / promotion.FreeVisitsFactor.Value);
            var totalPlusExtraVisits = totalVisits + extraVisits;

            DateTime EndDate;

            var contractDurationAfterPromotionInWeeks = (int)Math.Ceiling(totalPlusExtraVisits / requestHourlyPricing.Weeklyvisits);


            if (contractDurationAfterPromotionInWeeks <= 3)
                EndDate = startDate.AddDays(6 * contractDurationAfterPromotionInWeeks);
            else
            {
                EndDate = startDate.AddDays(7 * contractDurationAfterPromotionInWeeks);
                EndDate = EndDate.AddDays(-1);
            }


            string shift = Shift.ToString();
            #region commented

            /* string sql = @"Select distinct DayName  from HourContractShifts('@ContractStartDate','@ContractEndDate','@districtid','@ShiftName',4)
                         where nationalityId='@nationality'
                         group by HourContractShifts.new_resourcecode,DayName 
                         having COUNT(*)>=@visitsCount order by DayName";

   




           


            // 
            sql = sql.Replace("@ContractStartDate", startDate.Date.ToString());
            sql = sql.Replace("@ContractEndDate", EndDate.Date.ToString());
            sql = sql.Replace("@districtid", DistrictId);
            sql = sql.Replace("@ShiftName", shift);
            sql = sql.Replace("@nationality", NationalityId);
            if (months!=0)
            sql = sql.Replace("@visitsCount", (4 * months).ToString());
            else
                sql = sql.Replace("@visitsCount", (countofdays).ToString());

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            string sql2 = @"select new_days from new_district  where new_districtId='@id'";
            sql2 = sql2.Replace("@id", DistrictId);
            DataTable dtdistrict = CRMAccessDB.SelectQ(sql2).Tables[0];
            */
            #endregion


            //            string sql = @" 
            //with allSihfts as  (
            // select  ShiftFrom,  nationalityId,dayname,Count(distinct new_hourresourceId) SeatNo,count(distinct new_employeeid) as NoOfResources from
            //HourContractShiftsV02('@MinDate','@MaxDate','@districtid','@ShiftName','@nationality',@hoursCount,'@cityid')
            //                       --where Dayname in ('Sunday')
            //                    group by dayname,shiftfrom,nationalityId
            //					having Count(distinct new_hourresourceId)>=@EmpCount and count(distinct new_employeeid)>=@EmpCount
            //)

            //select nationalityId,dayname,count(dayname) counts from allSihfts
            //group by nationalityId,dayname
            //having count(dayname) >=@visitsCount";

            string sql = @"exec dbo.HourContractShifts_ValidationV01 
                                '@MinDate','@MaxDate','@districtid','@ShiftName','@nationality',@hoursCount,'@cityid' , @empCount, @visitsCount
                          ";

            //sql = sql.Replace("@visitsCount", (countOfDays / requestHourlyPricing.Weeklyvisits).ToString());

            sql = sql.Replace("@visitsCount", (contractDurationAfterPromotionInWeeks).ToString());

            sql = sql.Replace("@empCount", (requestHourlyPricing.Empcount).ToString());

            sql = sql.Replace("@hoursCount", requestHourlyPricing.HoursCount.ToString());
            sql = sql.Replace("@cityid", requestHourlyPricing.CityId);


            sql = sql.Replace("@MinDate", startDate.AddHours(3).Date.ToString("MM/dd/yyyy"));
            sql = sql.Replace("@MaxDate", EndDate.Date.ToString("MM/dd/yyyy"));



            sql = sql.Replace("@districtid", requestHourlyPricing.DistrictId);

            //if (shift == "صباحي") ShiftName = "Morning";
            //if (ShiftName == "مسائي") ShiftName = "Evening";
            sql = sql.Replace("@ShiftName", shift);
            sql = sql.Replace("@nationality", requestHourlyPricing.NationalityId);
            // SqlCommand CMD = new SqlCommand(sql);
            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            string avaDays = string.Empty;


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (avaDays.IndexOf(dt.Rows[i]["DayName"].ToString()) == -1)
                    avaDays = avaDays + dt.Rows[i]["DayName"] + ",";
            }
            if (!string.IsNullOrEmpty(avaDays))
                avaDays = avaDays.Remove(avaDays.Length - 1);


            List<string> DaysOfWeek = avaDays.Split(',').ToList();

            //Avilable Days List
            //List<string> List = new List<string>();
            //for (int i = 0; i < dt.Rows.Count; i++) List.Add(dt.Rows[i][0].ToString());

            return DaysOfWeek;
        }

    }
}