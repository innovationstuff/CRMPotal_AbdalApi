using Microsoft.Xrm.Sdk;
using NasAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class Contact : BaseModel
    {
        [CrmColumnAttribute("contactid")]
        public string ContactId { get; set; }

        [Required]
        [CrmColumnAttribute("mobilephone")]
        public string MobilePhone { get; set; }

        [Required]
        [CrmColumnAttribute("fullname")]
        public string FullName { get; set; }

        [CrmColumnAttribute("firstname")]
        public string FirstName { get; set; }

        [CrmColumnAttribute("lastname")]
        public string LastName { get; set; }

        [CrmColumnAttribute("emailaddress1")]
        public string Email { get; set; }

        [CrmColumnAttribute("jobtitle")]
        public string JobTitle { get; set; }

        [CrmColumnAttribute("new_contactcity")]
        public string CityId { get; set; }

        [CrmColumnAttribute("new_contactnationality")]
        public string NationalityId { get; set; }

        [CrmColumnAttribute("new_gender")]
        public int? GenderId { get; set; }

        [CrmColumnAttribute("new_idnumer")]
        public string IdNumber { get; set; }

        [CrmColumnAttribute("new_territory")]
        public string RegionId { get; set; }



        //public string UserName { get; set; }
        //public string Password { get; set; }

        public Contact(Entity contactCrmEntity)
        {
            //Helpers.CrmToModelMapper<Contact>.CastFromCrm(contactCrmEntity,this);
            this.ContactId = (contactCrmEntity.Attributes.ContainsKey("contactid") && contactCrmEntity["contactid"] != null) ? contactCrmEntity["contactid"].ToString() : null;
            this.MobilePhone = (contactCrmEntity.Attributes.ContainsKey("mobilephone") && contactCrmEntity["mobilephone"] != null) ? contactCrmEntity["mobilephone"].ToString() : null;
            this.FullName = (contactCrmEntity.Attributes.ContainsKey("fullname") && contactCrmEntity["fullname"] != null) ? contactCrmEntity["fullname"].ToString() : null;
            this.LastName = (contactCrmEntity.Attributes.ContainsKey("lastname") && contactCrmEntity["lastname"] != null) ? contactCrmEntity["lastname"].ToString() : null;
            this.Email = (contactCrmEntity.Attributes.ContainsKey("emailaddress1") && contactCrmEntity["emailaddress1"] != null) ? contactCrmEntity["emailaddress1"].ToString() : null;
            this.JobTitle = (contactCrmEntity.Attributes.ContainsKey("jobtitle") && contactCrmEntity["jobtitle"] != null) ? contactCrmEntity["jobtitle"].ToString() : null;
            this.CityId = (contactCrmEntity.Attributes.ContainsKey("new_contactcity") && contactCrmEntity["new_contactcity"] != null) ? (contactCrmEntity["new_contactcity"] as EntityReference).Id.ToString() : null;
            this.NationalityId = (contactCrmEntity.Attributes.ContainsKey("new_contactnationality") && contactCrmEntity["new_contactnationality"] != null) ? (contactCrmEntity["new_contactnationality"] as EntityReference).Id.ToString() : null;
            this.GenderId = (contactCrmEntity.Attributes.ContainsKey("new_gender") && contactCrmEntity["new_gender"] != null) ? (int?)(contactCrmEntity["new_gender"] as OptionSetValue).Value : null;
            this.IdNumber = (contactCrmEntity.Attributes.ContainsKey("new_idnumer") && contactCrmEntity["new_idnumer"] != null) ? contactCrmEntity["new_idnumer"].ToString() : null;
            this.RegionId = (contactCrmEntity.Attributes.ContainsKey("new_territory") && contactCrmEntity["new_territory"] != null) ? (contactCrmEntity["new_territory"] as EntityReference).Id.ToString() : null;
        }

        public Contact()
        {

        }
    }
}