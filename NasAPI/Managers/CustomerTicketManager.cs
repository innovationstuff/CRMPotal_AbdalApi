using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NasAPI.DisplayData;
using Microsoft.Xrm.Sdk;
using NasAPI.Enums;
using Microsoft.Xrm.Sdk.Query;
using System.Web.ModelBinding;

namespace NasAPI.Managers
{
    public class CustomerTicketManager : BaseManager
    {

        public CustomerTicketManager()
            : base(CrmEntityNamesMapping.CustomerTicket)
        {

        }


        public override IEnumerable<BaseOptionSet> GetEntityStatusOptions(UserLanguage Language)
        {
            switch (Language)
            {

                case UserLanguage.Arabic:
                    return StaticDisplayLists.CustomerTicket_Status_Arabic;
                default:
                    return StaticDisplayLists.CustomerTicket_Status;
            }
        }

        public Entity CastToCrmEntity(CustomerTicket ticket, CustomerTicketSectorType SectorType, Who? who)
        {
            Entity entity = new Entity(CrmEntityName);
            ColumnSet cols = new ColumnSet(new String[] { "fullname", "mobilephone" });

            Entity Contact = GlobalCode.Service.Retrieve("contact", new Guid(ticket.ContactId), cols);


            if (Contact.Contains("fullname"))
                entity["new_callername"] = Contact["fullname"];
            if (Contact.Contains("mobilephone"))
                entity["new_callno"] = Contact["mobilephone"];


            string Propdetails = "";
            if (int.Parse(ticket.ProblemTypeId) == 100000009)
            {
                Entity Employee = GlobalCode.Service.Retrieve("new_employee", new Guid(ticket.EmployeeId), new ColumnSet(true));
                Propdetails += "  شكوي بخصوص العاملة :   " + Employee["new_name"];
                Propdetails += @"\r/n    الإقامة: " + Employee["new_idnumber"];
                Propdetails += @"\r/n    السبب : " + ticket.ProblemDetails;
                entity["new_problemdetails"] = Propdetails;
            }
            else
            {
                entity["new_problemdetails"] = ticket.ProblemDetails;
            }
            

            int rand = (new Random(1).Next() * 1000) + 1;
            ticket.ClientClosedCode = rand.ToString();

            entity["new_closedno"] = rand;
            entity["new_contracttype"] = new OptionSetValue((int)SectorType);

            entity["new_contact"] = new EntityReference(CrmEntityNamesMapping.CustomerTicket, new Guid(ticket.ContactId));
            if (SectorType == CustomerTicketSectorType.Individual)
                entity["new_IndvContractId"] = new EntityReference(CrmEntityNamesMapping.CustomerTicket, new Guid(ticket.ContractId));
            else if (SectorType == CustomerTicketSectorType.Dalal)
                entity["new_cshindivcontractid"] = new EntityReference(CrmEntityNamesMapping.CustomerTicket, new Guid(ticket.ContractId));
            entity["new_problemcase"] = new OptionSetValue(int.Parse(ticket.ProblemTypeId));
            entity["new_compalinsource"] = new OptionSetValue((int)who);

            return entity;
        }

        #region Dalal

        public IEnumerable<CustomerTicket> GetDalalCustomerTickets(string sectorId, string userId, UserLanguage lang, string statusCode = null)
        {
            string statusCondition = String.IsNullOrEmpty(statusCode) ? string.Empty : String.Format(" and ticket.statuscode={0} ", statusCode);

            //string functionToGetProblemsName = lang == UserLanguage.Arabic ? "getOptionSetDisplay" : "getOptionSetDisplayen";


            string optionSetGetValFn, otherLangOptionSetGetValFn;

            switch (lang)
            {

                case UserLanguage.Arabic:
                    optionSetGetValFn = "dbo.getOptionSetDisplay";
                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplayen";
                    break;
                default:
                    optionSetGetValFn = "dbo.getOptionSetDisplayen";
                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplay";
                    break;

            }

            string query = String.Format(@"select ticket.new_csindvsectorId,ticket.new_name,ticket.new_indvcontractid,ticket.new_problemdetails,ticket.statuscode,ticket.new_empbussid,ticket.new_closedno,
                                            ticket.new_name as contractName,ticket.new_contactName,ticket.CreatedOn,ticket.new_contracttype,ticket.new_problemcase,
                                            Isnull({0}('{1}','{2}', ticket.new_problemcase), {3}('{1}','{2}', ticket.new_problemcase)) as new_problemcaseName,
                                            Isnull({0}('{4}','{2}', ticket.new_contracttype),{3}('{4}','{2}', ticket.new_contracttype)) as new_contracttypeName,
                                            Isnull({0}('{5}','{2}', ticket.statuscode),{3}('{5}','{2}', ticket.statuscode)) as statuscodeName
                                            from new_csindvsector ticket
                                            left outer join contact
                                            on ticket.new_contact = contact.ContactId
                                            where ticket.new_contracttype= {6} and ticket.new_contact = '{7}' {8}", optionSetGetValFn, "new_problemcase", "new_csindvsector", otherLangOptionSetGetValFn, "new_contracttype","statuscode", sectorId, userId, statusCondition);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];

            return dt.AsEnumerable().Select(dataRow => new CustomerTicket(dataRow));
        }


        public CustomerTicket GetDalalTicketById(string ticketId, UserLanguage lang)
        {
            string functionToGetProblemsName = lang == UserLanguage.Arabic ? "getOptionSetDisplay" : "getOptionSetDisplayen";

            string query = String.Format(@"select ticket.new_csindvsectorId,ticket.new_name,ticket.new_indvcontractid,ticket.new_cshindivcontractid,ticket.new_problemdetails,ticket.statuscode,ticket.new_empbussid,ticket.new_closedno,
                                            ticket.new_cshindivcontractidName,ticket.new_indvcontractidName,ticket.new_indvcontractid,ticket.new_contactName,ticket.CreatedOn,ticket.new_contracttype,ticket.new_problemcase,
                                            [dbo].[{0}]('{1}','{2}', ticket.new_problemcase) as new_problemcaseName,
                                            [dbo].[{0}]('{3}','{2}', ticket.new_contracttype) as new_contracttypeName,
                                            [dbo].[{0}]('{4}','{2}', ticket.statuscode) as statuscodeName
                                            from new_csindvsector ticket
                                            left outer join new_HIndvContract contract on ticket.new_indvcontractid= contract.new_HIndvContractId
                                            left outer join contact
                                            on ticket.new_contact = contact.ContactId
                                            where ticket.new_csindvsectorId= '{5}'", functionToGetProblemsName, "new_problemcase", "new_csindvsector", "new_contracttype", "statuscode", ticketId);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
            if (dt.Rows.Count == 0) return null;

            var ticket = new CustomerTicket(dt.Rows[0]);
            return ticket;

        }

        public CustomerTicket GetDalalTicketByNumber(string ticketNumber, UserLanguage lang)
        {
            string functionToGetProblemsName = lang == UserLanguage.Arabic ? "getOptionSetDisplay" : "getOptionSetDisplayen";

            string query = String.Format(@"select ticket.new_csindvsectorId,ticket.new_name,ticket.new_indvcontractid,ticket.new_problemdetails,ticket.statuscode,ticket.new_empbussid,ticket.new_closedno,
                                            contract.new_contractNumber as contractName,ticket.new_contactName,ticket.CreatedOn,ticket.new_contracttype,ticket.new_problemcase,
                                            [dbo].[{0}]('{1}','{2}', ticket.new_problemcase) as new_problemcaseName,
                                            [dbo].[{0}]('{3}','{2}', ticket.new_contracttype) as new_contracttypeName,
                                            [dbo].[{0}]('{4}','{2}', ticket.statuscode) as statuscodeName
                                            from new_csindvsector ticket
                                            left outer join new_HIndvContract contract on ticket.new_indvcontractid= contract.new_HIndvContractId
                                            left outer join contact
                                            on ticket.new_contact = contact.ContactId
                                            where ticket.new_name= '{5}'", functionToGetProblemsName, "new_problemcase", "new_csindvsector", "new_contracttype", "statuscode", ticketNumber);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
            if (dt.Rows.Count == 0) return null;

            var ticket = new CustomerTicket(dt.Rows[0]);
            return ticket;

        }

        public IEnumerable<BaseQuickLookup> GetDalalServedEmployeesByContractId(string contractId)
        {
            var query = String.Format(@"Select new_hourlyappointment.new_employee , 'Iqama:'+new_employee.new_idnumber+'_'+ new_hourlyappointment.new_employeeName   as new_employeeName 
                                        from new_hourlyappointment 
                                        	inner join new_HIndvContract on new_hourlyappointment.new_servicecontractperhour = new_HIndvContract.new_HIndvContractId
											inner join new_employee on new_hourlyappointment.new_employee=new_employee.new_employeeid
                                        	where 
--new_hourlyappointment.new_status in ({0})
                                        	 new_HIndvContract.new_HIndvContractId = '{0}'",
                                                                                       //    , DefaultValues.CustomerTicket_DalalFinishedAppointmentStatus)
               string.Join(",", contractId));

            var result = CRMAccessDB.SelectQ(query).Tables[0].AsEnumerable().Select(dataRow => new BaseQuickLookup(dataRow["new_employee"].ToString(), dataRow["new_employeeName"].ToString()));
            return result;
        }

        public IEnumerable<BaseQuickLookup> GetDalalServedEmployeesByCustomerId(string customerId)
        {
            var query = String.Format(@"Select new_hourlyappointment.new_employee ,    'Iqama:'+new_employee.new_idnumber+'_'+ new_hourlyappointment.new_employeeName  as new_employeeName 
                                        from new_hourlyappointment 
                                        	inner join new_HIndvContract on new_hourlyappointment.new_servicecontractperhour = new_HIndvContract.new_HIndvContractId
	inner join new_employee on new_hourlyappointment.new_employee=new_employee.new_employeeid
                                        	where 
--new_hourlyappointment.new_status in ({0})
                                        	 new_HIndvContract.new_hindivclintname = '{0}'",
                                     string.Join(",", customerId));

            var result = CRMAccessDB.SelectQ(query).Tables[0].AsEnumerable().Select(dataRow => new BaseQuickLookup(dataRow["new_employee"].ToString(), dataRow["new_employeeName"].ToString()));
            return result;
        }

        public IEnumerable<BaseQuickLookup> GetDalalContracts(string customerId)
        {
            var query = String.Format(@"Select new_HIndvContract.new_HIndvContractId , new_HIndvContract.new_ContractNumber
                                        from new_HIndvContract 
                                        	where new_HIndvContract.new_hindivclintname = '{0}'", customerId);

            var result = CRMAccessDB.SelectQ(query).Tables[0].AsEnumerable().Select(dataRow => new BaseQuickLookup(dataRow["new_HIndvContractId"].ToString(), dataRow["new_ContractNumber"].ToString()));
            return result;
        }

        public IEnumerable<BaseOptionSet> GetDalalProblemTypes(UserLanguage Language)
        {
            switch (Language)
            {

                case UserLanguage.Arabic:
                    return StaticDisplayLists.CustomerTicket_DalalProblemTypes_Arabic;
                default:
                    return StaticDisplayLists.CustomerTicket_DalalProblemTypes;
            }
        }

        public CustomerTicket CreateDalalTicket(CustomerTicket ticket, Who? who)
        {
            var entity = CastToCrmEntity(ticket, CustomerTicketSectorType.Dalal, ticket.who);
            Guid ticketId = GlobalCode.Service.Create(entity);
            ticket.Id = ticketId.ToString();
      

            return ticket;
        }
        #endregion

        #region individual

        public IEnumerable<BaseOptionSet> GetIndividualProblemTypes(UserLanguage Language)
        {
            switch (Language)
            {

                case UserLanguage.Arabic:
                    return StaticDisplayLists.CustomerTicket_IndividualProblemTypes_Arabic;
                default:
                    return StaticDisplayLists.CustomerTicket_IndividualProblemTypes;
            }
        }

        public CustomerTicket CreateIndividualTicket(CustomerTicket ticket, Who who)
        {
            var entity = CastToCrmEntity(ticket, CustomerTicketSectorType.Individual, who);
            Guid ticketId = GlobalCode.Service.Create(entity);
            ticket.Id = ticketId.ToString();

            return ticket;
        }
        #endregion
    }
}