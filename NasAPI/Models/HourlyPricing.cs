using NasAPI.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class HourlyPricing
    {
        public string HourePrice { get; set; }
        public string hourlypricingId { get; set; }
        public string Name { get; set; }
        public string VisitCount { get; set; }
        public string VisitPrice { get; set; }
        public string Discount { get; set; }
        public string Hours { get; set; }
        public string NoOfMonths { get; set; }
        public string TotalVisit { get; set; }
        public string TotalPrice { get; set; }
        public string MonthVisits { get; set; }
        public string Shift { get; set; }
        public string NationalityId { get; set; }
        public string NationalityName { get; set; }
        public string VersionNumber { get; set; }
        public bool? IsAvailable { get; set; }
        public string MonthelyPrice { get; set; }
        public string TotalbeforeDiscount { get; set; }

        public DayShifts DayShift { get; set; }

        public string VatRate { get; set; }
        public string VatAmount { get; set; }
        public string TotalPriceWithVat { get; set; }

        public string TotalPromotionDiscountAmount { get; set; }
        public string TotalPriceAfterPromotion { get; set; }

        public int PromotionExtraVisits { get; set; }
        public string PromotionName { get; set; }

        public List<String> AvailableDays { get; set; }

        public HourlyPricing()
        {

        }

        public HourlyPricing(DataRow dataRow)
        {
            bool isEveningShift = Convert.ToBoolean(dataRow["new_shift"].ToString());
            HourePrice = dataRow["new_hourprice"].ToString();
            hourlypricingId = dataRow[0].ToString();
            Name = dataRow[1].ToString();
            VisitCount = dataRow[2].ToString();
            VisitPrice = dataRow[3].ToString();
            Discount = dataRow[4].ToString();
            Hours = dataRow[5].ToString();
            NoOfMonths = dataRow[6].ToString();
            TotalVisit = dataRow[7].ToString();
            TotalPrice = dataRow[8].ToString();
            MonthVisits = dataRow[9].ToString();
            VersionNumber = dataRow[10].ToString();
            //DayShift = ((DayShifts)dataRow["new_shift"]);
            DayShift = (DayShifts)Enum.Parse(typeof(DayShifts), isEveningShift ? "1" : "0");
            Shift = DayShift.ToString();
        }
    }

}