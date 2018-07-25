using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using NasAPI.Helpers;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class ContactManager : BaseManager, IDisposable
    {
        public const string crmGuidColumnName = "contactid";

        public ContactManager()
            : base(CrmEntityNamesMapping.Contact)
        {

        }

        public Entity GetCrmEntityByMobilePhone(string mobilePhone)
        {
            var searchEntities = GlobalCode.GetEntitiesBy(CrmEntityName, "mobilephone", mobilePhone);
            if (searchEntities != null && searchEntities.Entities.Count > 0)
                return searchEntities.Entities.FirstOrDefault();
            return null;
        }

        public Entity UpdateCrmEntityFromRegisteration(Entity entity, Contact contact)
        {
            // Q: if has value and if crm value and portal value are not the same , What to do?
            return SoftUpdateCrmEntity(entity, contact);
        }

        public IEnumerable<BaseOptionSet> GetGendersOptions(UserLanguage language)
        {
            using (var globalmgr = new GlobalCrmManager())
            {
                return globalmgr.GetOptionSetLookup(CrmEntityName, "new_gender", language);
            }
        }

        public Contact RegisterContactInPortal(Contact contact)
        {
            var crmEntity = GetCrmEntityByMobilePhone(contact.MobilePhone);
            if (crmEntity == null)
                crmEntity = AddNewContactToCrm(contact);
            else
                UpdateCrmEntityFromRegisteration(crmEntity, contact);

            contact.ContactId = crmEntity[crmGuidColumnName].ToString();
            return contact;
        }

        public Entity AddNewContactToCrm(Contact contact)
        {
            var entity = new Entity("contact");

            entity["mobilephone"] = contact.MobilePhone;
            //entity["new_username"] = contact.UserName;
            //entity["new_password"] = contact.Password;
            entity["emailaddress1"] = contact.Email;
            entity["fullname"] = contact.FullName;
            var nameSegments = contact.FullName.Trim().Split(' ');
            entity["firstname"] = string.Join(" ", nameSegments.Take(nameSegments.Length - 1));
            entity["lastname"] = nameSegments[nameSegments.Length - 1];
            //entity["lastname"] = contact.FullName;
            var id = GlobalCode.Service.Create(entity);
            entity[crmGuidColumnName] = id;

            return entity;

        }

        public List<string> GetEmptyRequiredFields(string entityId)
        {
            using (GlobalCrmManager crmGlobalMgr = new GlobalCrmManager())
            {
                var list = crmGlobalMgr.GetAllEmptyRequiredFieldsNamesForRecord(CrmEntityName, crmGuidColumnName, entityId).ToList();
                return CrmToModelMapper<Contact>.GetCrmToModelMapper(list).ToList();

            }
        }

        public bool IsProfileCompleted(string id)
        {
            var fields = this.GetEmptyRequiredFields(id);
            return (fields == null || fields.Count == 0 || (fields.Count == 1 && fields[0].ToLower() == "lastname") ||
                (fields.Count == 1 && fields[0].ToLower() == "regionid"));
        }

        public override IEnumerable<string> GetRequiredFields()
        {
            var list = base.GetRequiredFields();
            return CrmToModelMapper<Contact>.GetCrmToModelMapper(list).ToList();
        }

        public Entity UpdateCrmEntityBeforeContract(Contact contact)
        {
            // Q: if has value and if crm value and portal value are not the same , What to do?

            var entity = GetCrmEntity(contact.ContactId);
            return SoftUpdateCrmEntity(entity, contact);
        }
        public bool CheckContactIdNumber(string idNumber)
        {
            string SQL = @"SELECT TOP 1000 [ContactId]  FROM [Abdal_MKH_MSCRM].[dbo].[ContactBase] where new_IdNumer = '@idNumber'";
            SQL = SQL.Replace("@idNumber", idNumber);

            return CRMAccessDB.SelectQ(SQL).Tables[0].Rows.Count > 0 ? true : false;
        }

        public Entity SoftUpdateCrmEntity(Entity entity, Contact contact)
        {
            entity["emailaddress1"] = (!entity.Attributes.ContainsKey("emailaddress1") || entity["emailaddress1"] == null) ? contact.Email : entity["emailaddress1"].ToString();
            entity["jobtitle"] = (!entity.Attributes.ContainsKey("jobtitle") || entity["jobtitle"] == null) ? contact.JobTitle : entity["jobtitle"].ToString();
            if (!string.IsNullOrEmpty(contact.CityId)) entity["new_contactcity"] = new EntityReference(CrmEntityNamesMapping.City, new Guid(contact.CityId));
            if (!string.IsNullOrEmpty(contact.NationalityId)) entity["new_contactnationality"] = new EntityReference(CrmEntityNamesMapping.Nationality, new Guid(contact.NationalityId));
            if (contact.GenderId != null) entity["new_gender"] = new OptionSetValue(contact.GenderId.Value);
            entity["new_idnumer"] = (!entity.Attributes.ContainsKey("new_idnumer") || entity["new_idnumer"] == null) ? contact.IdNumber : entity["new_idnumer"].ToString();
            if (!string.IsNullOrEmpty(contact.RegionId)) entity["new_territory"] = new EntityReference(CrmEntityNamesMapping.Region, new Guid(contact.RegionId));
            entity["fullname"] = (!string.IsNullOrEmpty(contact.FullName) ? contact.FullName : entity["fullname"].ToString());

            GlobalCode.Service.Update(entity);
            return entity;

        }

        public Contact GetContact(string id)
        {
            var entity = GetCrmEntity(id);

            if (entity == null) return null;

            return new Contact(entity);
        }

        public void Dispose()
        {

        }
    }

}