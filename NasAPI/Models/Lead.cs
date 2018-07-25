using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class Lead : BaseModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CityId { get; set; }
        //public string CityName { get; set; }
        //public string DistrictName { get; set; }
        public string RegionName { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string SectorId { get; set; }
        //public string SectorName { get; set; }
        public string IndustryCode { get; set; }
        public string SalesPersonName { get; set; }
        //public string MolFile { get; set; }
        //public string MedicalLead { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        //public string StatusId { get; set; }
        //public string StatusName { get; set; }
        //public string ServiceTypeId { get; set; }
        //public string ServiceTypeName { get; set; }
        public string Address { get; set; }
        public string Job { get; set; }

        public Lead()
        {

        }

        public Lead(DataRow dataRow)
        {
            this.Id = dataRow.Table.Columns.Contains("leadid") ? dataRow["leadid"].ToString() : null;
            this.CityId = dataRow.Table.Columns.Contains("new_cityId") ? dataRow["new_cityId"].ToString() : null;
            //this.CityName = dataRow.Table.Columns.Contains("new_cityidName") ? dataRow["new_cityidName"].ToString() : null;
            //this.DistrictName = dataRow.Table.Columns.Contains("new_districtidName") ? dataRow["new_districtidName"].ToString() : null;
            this.RegionName = dataRow.Table.Columns.Contains("new_regionName") ? dataRow["new_regionName"].ToString() : null;
            //this.FirstName = dataRow.Table.Columns.Contains("FirstName") ? dataRow["FirstName"].ToString() : null;
            //this.LastName = dataRow.Table.Columns.Contains("LastName") ? dataRow["LastName"].ToString() : null;
            this.CompanyName = dataRow.Table.Columns.Contains("companyname") ? dataRow["companyname"].ToString() : null;
            this.SectorId = dataRow.Table.Columns.Contains("new_sector") ? dataRow["new_sector"].ToString() : null;
            //this.SectorName = dataRow.Table.Columns.Contains("sectorName") ? dataRow["sectorName"].ToString() : null;
            this.IndustryCode = dataRow.Table.Columns.Contains("industrycode") ? dataRow["industrycode"].ToString() : null;
            this.SalesPersonName = dataRow.Table.Columns.Contains("new_salespersonName") ? dataRow["new_salespersonName"].ToString() : null;
            //this.MolFile = dataRow.Table.Columns.Contains("new_molfile") ? dataRow["new_molfile"].ToString() : null;
            //this.MedicalLead = dataRow.Table.Columns.Contains("new_medicallead") ? dataRow["new_medicallead"].ToString() : null;
            this.Mobile = dataRow.Table.Columns.Contains("mobilephone") ? dataRow["mobilephone"].ToString() : null;
            this.Email = dataRow.Table.Columns.Contains("emailaddress1") ? dataRow["emailaddress1"].ToString() : null;
            this.Description = dataRow.Table.Columns.Contains("description") ? dataRow["description"].ToString() : null;
            //this.StatusId = dataRow.Table.Columns.Contains("statuscode") ? dataRow["statuscode"].ToString() : null;
            //this.StatusName = dataRow.Table.Columns.Contains("statusName") ? dataRow["statusName"].ToString() : null;
            //this.ServiceTypeId = dataRow.Table.Columns.Contains("new_leadservicetype") ? dataRow["new_leadservicetype"].ToString() : null;
            //this.ServiceTypeName = dataRow.Table.Columns.Contains("serviceTypeName") ? dataRow["serviceTypeName"].ToString() : null;
            this.Address = dataRow.Table.Columns.Contains("address1_line1") ? dataRow["address1_line1"].ToString() : null;
            this.Job = dataRow.Table.Columns.Contains("jobtitle") ? dataRow["jobtitle"].ToString() : null;

        }
    }
}