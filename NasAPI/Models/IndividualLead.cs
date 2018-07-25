using NasAPI.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class IndividualLead : BaseModel
    {
        public string Id { get; set; }
        public string RequiredNationality { get; set; }
        public string RequiredNationalityName { get; set; }
        public string RequiredProfession { get; set; }
        public string RequiredProfessionName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string JobTitle { get; set; }
        public string HomePhone { get; set; }
        public long? IdNumber { get; set; }
        public bool? IsMedicalLead { get; set; }
        public string SectorId { get; set; }
        public string SalesPersonName { get; set; }
        public string SalesPersonId { get; set; }
        public string Description { get; set; }

        public string Email { get; set; }
        public string NationalityId { get; set; }
        public int? GenderId { get; set; }

        public string ContactId { get; set; }

        public RecordSource? Source { get; set; }
    }
}