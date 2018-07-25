using NasAPI.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class RequestServiceContractPerHour
    {
        public string ContractNum { get; set; }

        public string ContractId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string NationalityId { get; set; }
        [Required]
        public string HourlyPricingId { get; set; }
        [Required]
        public string CityId { get; set; }
        //public DayShifts Shift { get; set; }
        public string DistrictId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Who { get; set; }
        public int NumOfVisits { get; set; }
        public int NumOfHours { get; set; }
        public string StartDay { get; set; }
        public int ContractDuration { get; set; }  // no of weeks
        public string AvailableDays { get; set; } //SelectedDays
        public int NumOfWorkers { get; set; }

        public string FinalPrice { get; set; }

        public int HouseType { get; set; }
        public string HouseNo { get; set; }
        public int FloorNo { get; set; }
        public string AddressNotes { get; set; }

        public string PromotionCode { get; set; }

        public string PartmentNo { get; set; }

        //string TotalPrice,string Discount,string MonthelyPrice

    }
}