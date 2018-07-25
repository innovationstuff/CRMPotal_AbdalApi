using Microsoft.Xrm.Sdk;
using NasAPI.Enums;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class HourlyPricingManager : BaseManager, IDisposable
    {
        public PromotionManager PromotionMgr { get; set; }
        public HourlyPricingShiftManager ShiftMgr { get; set; }

        public HourlyPricingManager() : base(CrmEntityNamesMapping.HourlyPricing)
        {
            ShiftMgr = new HourlyPricingShiftManager();
            PromotionMgr = new PromotionManager();
        }

        public IEnumerable<HourlyPricing> GetHourlyPricingByNationalityAndShift(string NationalityId, DayShifts Shift)
        {

            string sql = @"SELECT new_hourlypricingId hourlypricingId,new_name name,new_visitcount visitcount,new_visitprice visitprice,
                        new_discount discount,new_hours [hours],new_noofmonths noofmonths,new_totalvisits totalvisit,
                        new_totalprice totalprice,new_monthvisits monthvisits,versionnumber,new_hourprice,new_shift from new_hourlypricingBase
                        Where new_nationality='@nationalityId' and new_shift=@shift";

            int shifttype = (int)Shift;

            sql = sql.Replace("@nationalityId", NationalityId);
            sql = sql.Replace("@shift", shifttype.ToString());
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            List<HourlyPricing> List = dt.AsEnumerable().Select(dataRow => new HourlyPricing(dataRow)).ToList();

            //List<HourlyPricing> List = new List<HourlyPricing>();
            //for (int i = 0; i < dt.Rows.Count; i++) List.Add(new HourlyPricing()
            //{
            //    HourePrice = dt.Rows[i]["new_hourprice"].ToString(),
            //    hourlypricingId = dt.Rows[i][0].ToString(),
            //    Name = dt.Rows[i][1].ToString(),
            //    VisitCount = dt.Rows[i][2].ToString(),
            //    VisitPrice = dt.Rows[i][3].ToString(),
            //    Discount = dt.Rows[i][4].ToString(),
            //    Hours = dt.Rows[i][5].ToString(),
            //    NoOfMonths = dt.Rows[i][6].ToString(),
            //    TotalVisit = dt.Rows[i][7].ToString(),
            //    TotalPrice = dt.Rows[i][8].ToString(),
            //    MonthVisits = dt.Rows[i][9].ToString(),
            //    VersionNumber = dt.Rows[i][10].ToString()

            //});

            return List;

        }

        public IEnumerable<HourlyPricing> GetPricing(RequestHourlyPricing requestHourlyPricing, UserLanguage Lang)
        {

            //Get HOurlyPricing Of Nationality
            List<HourlyPricing> MorningHP = GetHourlyPricingByNationalityAndShift(requestHourlyPricing.NationalityId, DayShifts.Morning).ToList();
            List<HourlyPricing> EveningHP = GetHourlyPricingByNationalityAndShift(requestHourlyPricing.NationalityId, DayShifts.Evening).ToList();


            int countofdays = requestHourlyPricing.Days.Split(',').Count();

            // Get Promotion

            var promotion = PromotionMgr.GetPromotionByCode(requestHourlyPricing, Lang);

          

            //Get Available Days 
            List<string> MorningAD = ShiftMgr.GetDays(requestHourlyPricing, DayShifts.Morning, countofdays, promotion).ToList();
            List<string> EveningAD = ShiftMgr.GetDays(requestHourlyPricing, DayShifts.Evening, countofdays, promotion).ToList();


            //Check If Days Available Or Not
            bool IsMDAvabilable = !requestHourlyPricing.Days.Split(',').ToList().Except(MorningAD).Any();
            bool IsEDAvabilable = !requestHourlyPricing.Days.Split(',').ToList().Except(EveningAD).Any();

            // key openDays was made in Nas web api to display all days not only district days and this not in availabiltiy business
            //if (ConfigurationManager.AppSettings["OpenDays"] == "true")
            //{
            //    IsMDAvabilable = true;
            //    IsEDAvabilable = true;
            //}


            MorningHP = MorningHP.Select(a => CastToClientHourlyPricing(a, countofdays, requestHourlyPricing.HoursCount, requestHourlyPricing.ContractDuration,requestHourlyPricing.Empcount, IsMDAvabilable, MorningAD, promotion)).ToList();
            EveningHP = EveningHP.Select(a => CastToClientHourlyPricing(a, countofdays, requestHourlyPricing.HoursCount, requestHourlyPricing.ContractDuration, requestHourlyPricing.Empcount, IsEDAvabilable, EveningAD, promotion)).ToList();

            var AllHP = new List<HourlyPricing>();
            AllHP.AddRange(MorningHP);
            AllHP.AddRange(EveningHP);

            return AllHP;
        }

        public Promotion testCheckPromotionValidation(RequestHourlyPricing requestHourlyPricing, UserLanguage lang = UserLanguage.Arabic)
        {
            return PromotionMgr.GetPromotionByCode(requestHourlyPricing, lang);
        }

        public decimal GetDiscount(string HPId, int HoursCount, int totalVisitsCount)
        {

            string SqlDiscount = @"
		   select Top(1) new_discount,new_name,new_hourcount,new_noofmonths,new_daysperweek from new_hourdiscountlist 
		   where new_hourlypricing='@HourlyPricingId'
		     and new_hourcount=@hourscount  and new_daysperweek <= @dayscount
            order by new_daysperweek desc
";

            SqlDiscount = SqlDiscount.Replace("@HourlyPricingId", HPId);
            SqlDiscount = SqlDiscount.Replace("@hourscount", HoursCount.ToString());
            SqlDiscount = SqlDiscount.Replace("@dayscount", totalVisitsCount.ToString());


            DataTable dt = CRMAccessDB.SelectQ(SqlDiscount).Tables[0];
            List<string> List = new List<string>();
            if (dt.Rows.Count == 0)
                return 0m;

            return decimal.Parse(dt.Rows[0]["new_discount"].ToString());
        }

        private HourlyPricing CastToClientHourlyPricing(HourlyPricing pricing, int NoOfVisits, int NoOfHours, int ContractDurationInWeeks,int empCount, bool isAvailable, List<string> AvailableDays, Promotion promotion)
        {
            using (var promotionMgr = new PromotionManager())
            {
                pricing.IsAvailable = isAvailable;
                pricing.NoOfMonths = ContractDurationInWeeks.ToString();
                pricing.VisitCount = NoOfVisits.ToString();
                pricing.Hours = NoOfHours.ToString();
                pricing.Shift = pricing.DayShift.ToString();
                pricing.AvailableDays = AvailableDays;

                var hourlyPricingCost = CalculateHourlyPricingCost(pricing, NoOfHours, NoOfVisits, ContractDurationInWeeks,empCount, promotion);

                pricing.Discount = hourlyPricingCost.Discount.ToString().Replace(".00", "");
                pricing.TotalbeforeDiscount = (hourlyPricingCost.TotalPriceBeforeDiscount).ToString().Replace(".00", "");
                pricing.MonthelyPrice = (hourlyPricingCost.MonthelyPrice).ToString().Replace(".00", "");
                pricing.TotalPrice = hourlyPricingCost.TotalPriceAfterDiscount.ToString().Replace(".00", "");
                pricing.TotalPromotionDiscountAmount = hourlyPricingCost.TotalPromotionDiscountAmount.ToString().Replace(".00", "");
                pricing.TotalPriceAfterPromotion = hourlyPricingCost.TotalPriceAfterPromotion.ToString().Replace(".00", "");

                pricing.VatRate = hourlyPricingCost.VatRate.ToString().Replace(".00", "");
                pricing.VatAmount = hourlyPricingCost.VatAmount.ToString().Replace(".00", "");
                pricing.TotalPriceWithVat = hourlyPricingCost.TotalPriceWithVat.ToString().Replace(".00", "");
                pricing.TotalPrice = hourlyPricingCost.TotalPriceAfterDiscount.ToString().Replace(".00", "");



                var totalVisits = ContractDurationInWeeks * NoOfVisits;
                pricing.PromotionExtraVisits = (promotion.FreeVisitsFactor ?? 0) == 0 ? 0 : (int)Math.Truncate((decimal)totalVisits / promotion.FreeVisitsFactor.Value);
                pricing.PromotionName = promotion.Name;

                return pricing;

            }
        }

        public HourlyPricingCost CalculateHourlyPricingCost(decimal HourRate, decimal Discount, int NoOfHours, int NoOfVisitsPerWeek, int ContractDurationInWeeks,int empCount, Promotion promotion)
        {
            var hourlyPricingCost = new HourlyPricingCost();

            hourlyPricingCost.HourRate = Math.Round(HourRate, 2, MidpointRounding.AwayFromZero);
            hourlyPricingCost.Discount = Math.Round(Discount, 2, MidpointRounding.AwayFromZero);
            hourlyPricingCost.TotalPriceBeforeDiscount = Math.Round((hourlyPricingCost.HourRate * ContractDurationInWeeks * NoOfVisitsPerWeek * NoOfHours * empCount), 0, MidpointRounding.AwayFromZero);
            hourlyPricingCost.MonthelyPrice = Math.Round((hourlyPricingCost.HourRate * 4 * NoOfVisitsPerWeek * NoOfHours * empCount), 0, MidpointRounding.AwayFromZero);
            hourlyPricingCost.DiscountAmount = Math.Round((hourlyPricingCost.TotalPriceBeforeDiscount * hourlyPricingCost.Discount / 100), 0, MidpointRounding.AwayFromZero);
            hourlyPricingCost.TotalPriceAfterDiscount = hourlyPricingCost.TotalPriceBeforeDiscount - hourlyPricingCost.DiscountAmount;

            // calc Promotion
            var promotionFixedDiscountAmount = Math.Round((promotion.FixedDiscount??0), 2, MidpointRounding.AwayFromZero);
            var promotionDiscountAmount = Math.Round((promotion.Discount ?? 0m) * hourlyPricingCost.TotalPriceAfterDiscount / 100, 2, MidpointRounding.AwayFromZero);

            hourlyPricingCost.TotalPromotionDiscountAmount = Math.Round(promotionFixedDiscountAmount + promotionDiscountAmount, 0, MidpointRounding.AwayFromZero);

            hourlyPricingCost.TotalPriceAfterPromotion = hourlyPricingCost.TotalPriceAfterDiscount - hourlyPricingCost.TotalPromotionDiscountAmount;

            hourlyPricingCost.VatRate = decimal.Parse(ConfigurationManager.AppSettings["VatRate"].ToString());
            hourlyPricingCost.VatAmount = Math.Round((hourlyPricingCost.VatRate * hourlyPricingCost.TotalPriceAfterPromotion / 100m), 2, MidpointRounding.AwayFromZero);
            hourlyPricingCost.TotalPriceWithVat = hourlyPricingCost.TotalPriceAfterPromotion + hourlyPricingCost.VatAmount;

            hourlyPricingCost.NetPrice = hourlyPricingCost.TotalPriceWithVat;

            return hourlyPricingCost;
        }

        public HourlyPricingCost CalculateHourlyPricingCost(HourlyPricing pricing, int NoOfHours, int NoOfVisitsPerWeek, int ContractDurationInWeeks,int empCount, Promotion promotion)
        {
            var hourPrice = decimal.Parse(pricing.HourePrice);
            var discount = Math.Round(GetDiscount(pricing.hourlypricingId, NoOfHours, ContractDurationInWeeks * NoOfVisitsPerWeek), 2, MidpointRounding.AwayFromZero);

            return CalculateHourlyPricingCost(hourPrice, discount, NoOfHours, NoOfVisitsPerWeek, ContractDurationInWeeks,empCount, promotion);
        }

        public HourlyPricingCost CalculateHourlyPricingCost(Entity hourlyPricing, int NoOfHours, int NoOfVisitsPerWeek, int ContractDurationInWeeks, int empCount, Promotion promotion)
        {
            var hourPrice = decimal.Parse(hourlyPricing["new_hourprice"].ToString());

            decimal discountval = GetDiscount(hourlyPricing["new_hourlypricingid"].ToString(), NoOfHours, ContractDurationInWeeks * NoOfVisitsPerWeek);
            var discount = Math.Round(discountval, 2, MidpointRounding.AwayFromZero);

            return CalculateHourlyPricingCost(hourPrice, discount, NoOfHours, NoOfVisitsPerWeek,empCount, ContractDurationInWeeks,promotion);
        }


        public void Dispose()
        {
            ShiftMgr.Dispose();
        }
    }
}