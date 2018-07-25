using NasAPI.Enums;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class ServiceContractPerHour : BaseModel
    {
        public string ContractId { get; set; }
        public string ContractNum { get; set; }
        public string CreatedOn { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? NumOfVisits { get; set; }
        public int? NumOfHours { get; set; }
        //public string StartDay { get; set; }
        public DateTime? StartDay { get; set; }
        public int? ContractDuration { get; set; }  // no of weeks
        public string ContractDurationName { get; set; }  // no of weeks
        public string SelectedDays { get; set; } //SelectedDays
        public int? NumOfWorkers { get; set; }
        public DayShifts? Shift { get; set; }
        public DayShiftsAR? ShiftAR { get; set; }
        public string ShiftName { get; set; }
        public decimal? FinalPrice { get; set; }
        public int? UserRate { get; set; }
        public string NationalityId { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string CustomerId { get; set; }
        public string HourlyPricingId { get; set; }
        public DateTime? NextAppointment { get; set; }

        public string HourlyPricingName { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        public string City { get; set; }
        public string Nationality { get; set; }
        public string District { get; set; }
        public string totalpricewithoutvat { get; set; }
        public string vatrate { get; set; }
        public string vatamount { get; set; }

        public string Customer { get; set; }
        public HourlyPricing HourlyPricing { get; set; }
        public Status Status { get; set; }
        public List<HourlyAppointment> HourlyAppointments { get; set; }
        public HourlyPricingCost HourlyPricingCost { get; set; }

        public string CustomerMobilePhone { get; set; }

        public ServiceContractPerHour()
        {

        }


        public ServiceContractPerHour(DataRow dataRow, UserLanguage lang = UserLanguage.English)
        {
            this.ContractId = dataRow.Table.Columns.Contains("new_hindvcontractid") ? dataRow["new_hindvcontractid"].ToString() : null;
            this.ContractNum = dataRow.Table.Columns.Contains("new_contractnumber") ? dataRow["new_contractnumber"].ToString() : null;
            this.CreatedOn = dataRow.Table.Columns.Contains("createdon") ? dataRow["createdon"].ToString() : null;
            this.FinalPrice = (dataRow.Table.Columns.Contains("new_finalprice") && dataRow["new_finalprice"] != null && dataRow["new_finalprice"] != DBNull.Value) ? (decimal?)dataRow["new_finalprice"] : null;
            this.ContractDuration = (dataRow.Table.Columns.Contains("new_contractmonth") && dataRow["new_contractmonth"] != null) ? (int?)dataRow["new_contractmonth"] : null;
            this.UserRate = (dataRow.Table.Columns.Contains("userrate") && dataRow["userrate"] != null) ? (int?)dataRow["userrate"] : null;
            this.NextAppointment = (dataRow.Table.Columns.Contains("nextappointment") && dataRow["nextappointment"] != DBNull.Value) ? (DateTime?)dataRow["nextappointment"] : null;
            this.HourlyPricingName = (dataRow.Table.Columns.Contains("hourlypricingname")) ? dataRow["hourlypricingname"].ToString() : null;
            this.CityId = (dataRow.Table.Columns.Contains("new_city")) ? dataRow["new_city"].ToString() : null;
            this.City = (dataRow.Table.Columns.Contains("new_cityName")) ? dataRow["new_cityName"].ToString() : null;
            this.DistrictId = (dataRow.Table.Columns.Contains("new_district")) ? dataRow["new_district"].ToString() : null;
            this.District = (dataRow.Table.Columns.Contains("new_districtName")) ? dataRow["new_districtName"].ToString() : null;
            this.Nationality = (dataRow.Table.Columns.Contains("new_nationalityName")) ? dataRow["new_nationalityName"].ToString() : null;
            this.NationalityId = (dataRow.Table.Columns.Contains("new_nationality")) ? dataRow["new_nationality"].ToString() : null;
            this.StatusCode = (dataRow.Table.Columns.Contains("statuscode")) ? dataRow["statuscode"].ToString() : null;
            this.StatusName = (dataRow.Table.Columns.Contains("statusname")) ? dataRow["statusname"].ToString() : null;

            if (lang == UserLanguage.Arabic)
                this.ShiftAR = (dataRow.Table.Columns.Contains("new_shift") && dataRow["new_shift"] != DBNull.Value) ? (DayShiftsAR?)Convert.ToInt32(((bool?)dataRow["new_shift"]).Value) : null;
            else
                this.Shift = (dataRow.Table.Columns.Contains("new_shift") && dataRow["new_shift"] != DBNull.Value) ? (DayShifts?)Convert.ToInt32(((bool?)dataRow["new_shift"]).Value) : null;

            if (lang == UserLanguage.Arabic)
                this.ShiftName = this.ShiftAR == null ? null : this.ShiftAR.ToString();
            else
                this.ShiftName = this.Shift == null ? null : this.Shift.ToString();

            this.NumOfHours = (dataRow.Table.Columns.Contains("new_hoursnumber") && dataRow["new_hoursnumber"] != DBNull.Value) ? (int?)dataRow["new_hoursnumber"] : null;
            this.NumOfVisits = (dataRow.Table.Columns.Contains("new_weeklyvisits") && dataRow["new_weeklyvisits"] != DBNull.Value) ? (int?)dataRow["new_weeklyvisits"] : null;
            this.NumOfWorkers = (dataRow.Table.Columns.Contains("new_employeenumber") && dataRow["new_employeenumber"] != DBNull.Value) ? (int?)dataRow["new_employeenumber"] : null;
            this.StartDay = (dataRow.Table.Columns.Contains("new_contractstartdate") && dataRow["new_contractstartdate"] != DBNull.Value) ? (DateTime?)dataRow["new_contractstartdate"] : null;

            this.Longitude = (dataRow.Table.Columns.Contains("new_longitude")) ? dataRow["new_longitude"].ToString() : null;
            this.Latitude = (dataRow.Table.Columns.Contains("new_latitude")) ? dataRow["new_latitude"].ToString() : null;
            this.ContractDurationName = (dataRow.Table.Columns.Contains("durationname")) ? dataRow["durationname"].ToString() : null;

            this.ContractDurationName = (dataRow.Table.Columns.Contains("durationname")) ? dataRow["durationname"].ToString() : null;

            this.totalpricewithoutvat = (dataRow.Table.Columns.Contains("totalprice")) ? dataRow["totalprice"].ToString() : null;
            this.vatrate = (dataRow.Table.Columns.Contains("vatrate")) ? dataRow["vatrate"].ToString() : null;
            this.vatamount = (dataRow.Table.Columns.Contains("new_vatamount")) ? dataRow["new_vatamount"].ToString() : null;
            this.SelectedDays = (dataRow.Table.Columns.Contains("new_selecteddays")) ? dataRow["new_selecteddays"].ToString() : null;
            this.Customer = (dataRow.Table.Columns.Contains("new_HIndivClintnameName")) ? dataRow["new_HIndivClintnameName"].ToString() : null;
            this.CustomerMobilePhone = (dataRow.Table.Columns.Contains("mobilephone")) ? dataRow["mobilephone"].ToString() : null;
            this.CustomerId = (dataRow.Table.Columns.Contains("new_HIndivClintname")) ? dataRow["new_HIndivClintname"].ToString() : null;

        }
    }
}