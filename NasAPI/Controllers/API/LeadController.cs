using NasAPI.Managers;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/Lead")] // en/api/Lead
    public class LeadController : BaseApiController
    {

        LeadManager Manager;
        GeneralManager GeneralManager;

        public LeadController()
        {
            Manager = new LeadManager();
            GeneralManager = new GeneralManager();
           
        }

        [Route("Business/GetAll")]
        [HttpGet]
        [ResponseType(typeof(List<Lead>))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage GetAllLeads(string sectorId)
        {
            var result = Manager.GetLeads(sectorId, Language).ToList();
            return OkResponse(result);
        }


        [Route("Business/Create")]
        [HttpPost]
        [ResponseType(typeof(Lead))]
        public HttpResponseMessage CreateLead(Lead lead)
        {
            try
            {
                var result = Manager.CreateBusinessLead(lead);
                return OkResponse<Lead>(result);
            }
            catch (Exception ex)
            {
                return NotFoundResponse("Error in create lead", ex.Message);
            }
        }



        [Route("Individual/Create")]
        [HttpPost]
        [ResponseType(typeof(IndividualLead))]
        public HttpResponseMessage CreateIndividualLead(IndividualLead Lead)
        {
            var result = Manager.CreateIndividualLead(Lead);
            return OkResponse<IndividualLead>(result);
        }


        [Route("Individual/GetLeadsByMobile")]
        [HttpPost]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage GetLeadsByMobile(IndividualLead Lead)
        {
            var leadManager = new LeadManager();
            return OkResponse<bool>(leadManager.GetLeadsByMobile(Lead.Mobile, Language).Count() > 1);
        }

        [Route("Individual/CreateAndCompleteProfile")]
        [HttpPost]
        [ResponseType(typeof(IndividualLead))]
        public HttpResponseMessage CreateIndividualLeadWithCompleteProfile(IndividualLead Lead)
        {
            var re = Request;
            var headers = re.Headers;
            string Source = "";
            if (headers.Contains("source"))
            {
                Source = headers.GetValues("source").First();
            }
            var LeadManager = new LeadManager();
           
                if(LeadManager.GetLeadsByMobile(Lead.Mobile,Language).Count() > 1)
                {
                    return OkResponse<ReturnData>(new ReturnData()
                    {
                        State = false,
                        Data = new { message = Language == UserLanguage.Arabic ? "هذا العميل لديه طلبـات سابقة" : "This client has prior requests", user = Lead }
                    });
                }

          

            using (var ContactManager = new ContactManager())
            {
                if (!ContactManager.IsProfileCompleted(Lead.ContactId))
                {
                    Contact contact = ContactManager.GetContact(Lead.ContactId);
                    contact.CityId = Lead.CityId ?? contact.CityId;
                    contact.IdNumber = Lead.IdNumber == null?  contact.IdNumber : Lead.IdNumber.ToString();
                    contact.JobTitle = Lead.JobTitle ?? contact.JobTitle;
                    contact.RegionId = Lead.RegionId ?? contact.RegionId;
                    contact.Email = Lead.Email ?? contact.Email;
                    contact.NationalityId = Lead.NationalityId ?? contact.NationalityId;
                    contact.GenderId = Lead.GenderId ?? contact.GenderId;

                    ContactManager.UpdateCrmEntityBeforeContract(contact);
                }

            }
            Lead.Source = Source == "1" ? Enums.RecordSource.Mobile : Enums.RecordSource.Web;
            var result = Manager.CreateIndividualLead(Lead);
            return OkResponse<ReturnData>(new ReturnData()
            {
                State = true,
                Data = new { user = result }
            }); 
        }



        #region  lookups

        [Route("Options/IndustryCodes")]
        [HttpGet]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetIndustryCodes()
        {
            var result = GeneralManager.GetOptionSet_Lead_Industrycodes(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }

        [Route("Options/Regions")]
        [HttpGet]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetAllRegions()
        {
            var result = GeneralManager.GetAllRegions(Language).ToList();
            return OkResponse<List<BaseQuickLookup>>(result);
        }

        #endregion
    }
}