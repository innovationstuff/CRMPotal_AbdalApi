using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class LeadManager : BaseManager
    {

        public LeadManager()
            : base(CrmEntityNamesMapping.Lead)
        {

        }

        public Entity CastToCrmEntity(Lead lead)
        {
            Entity entity = new Entity(CrmEntityName);

            ColumnSet cols = new ColumnSet(new String[] { "firstname", "lastname" });

            Entity Contact = GlobalCode.Service.Retrieve("contact", new Guid(lead.UserId), cols);

            if (Contact.Contains("firstname"))
                entity["firstname"] = Contact["firstname"];
            if (Contact.Contains("lastname"))
                entity["lastname"] = Contact["lastname"];

            entity["new_cityid"] = new EntityReference(CrmEntityNamesMapping.City, new Guid(lead.CityId));
            entity["new_sector"] = new OptionSetValue(Convert.ToInt32(lead.SectorId));
            entity["new_region"] = lead.RegionName != null ? new EntityReference(CrmEntityNamesMapping.Territory, new Guid(lead.RegionName)) : null;
            entity["companyname"] = lead.CompanyName;
            entity["address1_line1"] = lead.Address;
            entity["industrycode"] = new OptionSetValue(Convert.ToInt32(lead.IndustryCode));
            entity["new_salesperson"] = lead.SalesPersonName;
            entity["jobtitle"] = lead.Job;
            entity["mobilephone"] = lead.Mobile;
            entity["emailaddress1"] = lead.Email;
            entity["salesstagecode"] = new OptionSetValue(1);
            entity["statuscode"] = new OptionSetValue(1);

            return entity;
        }


        public Entity CastToCrmEntity(IndividualLead lead)
        {
            Entity entity = new Entity(CrmEntityName);

            entity["new_nationalityid"] = new EntityReference(CrmEntityNamesMapping.Nationality, new Guid(lead.RequiredNationality));
            entity["new_profrequiredid"] = new EntityReference(CrmEntityNamesMapping.Profession, new Guid(lead.RequiredProfession));
            entity["new_medicallead"] = (lead.IsMedicalLead ?? false);
            entity["firstname"] = lead.Name;
            entity["mobilephone"] = lead.Mobile;
            entity["jobtitle"] = lead.JobTitle;
            entity["new_cityid"] = new EntityReference(CrmEntityNamesMapping.City, new Guid(lead.CityId));
            entity["new_sector"] = new OptionSetValue(Convert.ToInt32(SectorsTypeEnum.Individuals));
            entity["new_region"] = !String.IsNullOrEmpty(lead.RegionId ) ? new EntityReference(CrmEntityNamesMapping.Territory, new Guid(lead.RegionId)) : null;
            entity["telephone2"] = lead.HomePhone;
            entity["new_idnumber"] = lead.IdNumber.ToString();
            entity["new_districtid"] = String.IsNullOrEmpty(lead.DistrictId) ? null : new EntityReference(CrmEntityNamesMapping.District, new Guid(lead.DistrictId));
            entity["description"] = lead.Description;
            entity["new_recordsource"] = lead.Source.HasValue  ?  new OptionSetValue((int)lead.Source) : new OptionSetValue(2); 
            return entity;
        }


        public IndividualLead CreateIndividualLead(IndividualLead lead)
        {
            var entity = CastToCrmEntity(lead);
            Guid leadId = GlobalCode.Service.Create(entity);
            lead.Id = leadId.ToString();
            return lead;
        }

        public IEnumerable<Lead> GetLeads(string sectorId, UserLanguage lang)
        {

            string functionToGetProblemsName = lang == UserLanguage.Arabic ? "getOptionSetDisplay" : "getOptionSetDisplayen";

            string query = String.Format(@"select
	                                        cl.LeadId,
	                                        cl.new_cityId,
	                                        cl.new_cityidName,
	                                        cl.new_districtidName,
	                                        cl.new_regionName,
	                                        cl.FirstName,
	                                        cl.LastName,
	                                        cl.companyname,
	                                        cl.new_sector,
	                                        [dbo].[{0}]('new_sector','Lead', cl.new_sector) as sectorName,
	                                        cl.industrycode,
	                                        cl.new_salespersonName,
	                                        cl.new_molfile,
	                                        cl.new_medicallead,
	                                        cl.mobilephone,
	                                        cl.emailaddress1,
	                                        cl.Description,
	                                        cl.StatusCode,
	                                        [dbo].[{0}]('StatusCode','Lead', cl.StatusCode) as statusName,
	                                        cl.new_leadservicetype,
	                                        [dbo].[{0}]('new_leadservicetype','Lead', cl.new_leadservicetype) as serviceTypeName,
                                            cl.address1_line1,
                                            cl.JobTitle
	                                        from Lead cl
                                            where cl.new_sector = {1}", functionToGetProblemsName, sectorId);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];

            return dt.AsEnumerable().Select(dataRow => new Lead(dataRow));
        }
        public IEnumerable<Lead> GetLeadsByMobile(string Mobile, UserLanguage lang)
        {

            string functionToGetProblemsName = lang == UserLanguage.Arabic ? "getOptionSetDisplay" : "getOptionSetDisplayen";

            string query = String.Format(@"select
	                                        cl.LeadId,
	                                        cl.new_cityId,
	                                        cl.new_cityidName,
	                                        cl.new_districtidName,
	                                        cl.new_regionName,
	                                        cl.FirstName,
	                                        cl.LastName,
	                                        cl.companyname,
	                                        cl.new_sector,
	                                        [dbo].[{0}]('new_sector','Lead', cl.new_sector) as sectorName,
	                                        cl.industrycode,
	                                        cl.new_salespersonName,
	                                        cl.new_molfile,
	                                        cl.new_medicallead,
	                                        cl.mobilephone,
	                                        cl.emailaddress1,
	                                        cl.Description,
	                                        cl.StatusCode,
	                                        [dbo].[{0}]('StatusCode','Lead', cl.StatusCode) as statusName,
	                                        cl.new_leadservicetype,
	                                        [dbo].[{0}]('new_leadservicetype','Lead', cl.new_leadservicetype) as serviceTypeName,
                                            cl.address1_line1,
                                            cl.JobTitle,
                                            cl.CreatedOn
	                                        from Lead cl
                                            where cl.mobilephone = '{1}' and
                                            cl.CreatedOn BETWEEN DATEADD(day,DATEDIFF(day,2,GETDATE()),0) and GETDATE()", functionToGetProblemsName, Mobile);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];

            return dt.AsEnumerable().Select(dataRow => new Lead(dataRow));
        }


        public Lead CreateBusinessLead(Lead lead)
        {
            var entity = CastToCrmEntity(lead);
            Guid leadId = GlobalCode.Service.Create(entity);
            lead.Id = leadId.ToString();
            return lead;
        }


    }
}