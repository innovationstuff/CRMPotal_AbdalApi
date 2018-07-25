using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Mvc;
using NasAPI.Models;
using NasAPI.Managers;
using System.Web.Http.Description;
using System.Configuration;

namespace NasAPI.Controllers.API
{
    [RoutePrefix("{lang}/api/contact")] // en/api/contact 
    public class ContactController : BaseApiController
    {

        ContactManager Manager { get; set; }

        public ContactController()
        {
            Manager = new ContactManager();
        }

        /// <summary>
        /// Post Contact to CRM.
        /// </summary>
        /// <remarks>
        /// Register new Contact in Portal DB >> then add the contact to CRM 
        /// it will search by mobile phone in CRM then if exists, it returns the object with the GUID  or 
        /// if not exists, Add to CRM DB
        /// </remarks>
        /// <param name="contact"> contact </param>
        /// <returns> the Same Contact Object with GUID </returns>
        /// <response>code:200 the Same Contact Object with GUID  </response>
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(Contact))]
        public HttpResponseMessage Post(Contact contact)
        {
            contact = Manager.RegisterContactInPortal(contact);
            return OkResponse<Contact>(contact);
        }

        [Route("GetInvalidColumns/{id}")]
        [ResponseType(typeof(List<string>))]
        public HttpResponseMessage GetInvalidColumns(string id)
        {
            var result =  Manager.GetEmptyRequiredFields(id);
            return OkResponse<List<string>>(result);
            //return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("Genders")]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetAllGenders()
        {
            var result = Manager.GetGendersOptions(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
            //return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile(Contact contact)
        {
            Manager.UpdateCrmEntityBeforeContract(contact);
            return OkResponse();
            //return Request.CreateResponse(HttpStatusCode.OK);
        }


        [Route("UpdateProfile_M")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile_M(Contact contact)
        {
            if(Manager.CheckContactIdNumber(contact.IdNumber))
                return OkResponse(new ReturnData() { State = false, Data = new { message = "رقم الهوية مدخل من قبل" } });
            Manager.UpdateCrmEntityBeforeContract(contact);
            return OkResponse(new ReturnData() { State = true, Data = new { contact = contact } });
            //return Request.CreateResponse(HttpStatusCode.OK);
        }

    

        /// <summary>
        /// Get Contact data by CRM GUID
        /// </summary>
        /// <remarks>Get Contact data by CRM GUID</remarks>
        /// <param name="id">CRM GUID</param>
        /// <returns>Contact</returns>
        [Route("{id}")]
        [ResponseType(typeof(Contact))]
        public HttpResponseMessage Get(string id)
        {
            var contact = Manager.GetContact(id);
            if (contact == null) return NotFoundResponse();
            return OkResponse<Contact>(contact);
            //return Request.CreateResponse(HttpStatusCode.OK, contact);
        }

        /// <summary>
        /// Get Contact data by CRM GUID
        /// </summary>
        /// <remarks>Get Contact data by CRM GUID</remarks>
        /// <param name="id">CRM GUID</param>
        /// <returns>Contact</returns>
        [Route("GetDetails/{id}")]
        [ResponseType(typeof(ReturnData))]
        public HttpResponseMessage GetDetails(string id)
        {
            var contact = Manager.GetContact(id);
            if (contact == null) return NotFoundResponse();
            return OkResponse<ReturnData>(new ReturnData() { State = true, Data = new { contact = contact } });
            //return Request.CreateResponse(HttpStatusCode.OK, contact);
        }

        //[Route("Test/{id}")]
        //public string Test(int id)
        //{
        //    return String.Format("{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["OnlineAbdalPortalUrl"],
        //            GeneralAppSettings.RequestLang, ConfigurationManager.AppSettings["OnlineUserDalalPaymentUrl"], id.ToString());
        //}

        [Route("IsProfileCompleted/{id}")]
        [HttpGet]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage IsProfileCompleted(string id)
        {
            var result = Manager.IsProfileCompleted(id);
            return OkResponse<bool>(result);
        }

        [Route("IsProfileCompleted_M/{id}")]
        [HttpGet]
        [ResponseType(typeof(ReturnData))]
        public HttpResponseMessage IsProfileCompleted_M(string id)
        {
            var result = Manager.IsProfileCompleted(id);
            return OkResponse<ReturnData>(new ReturnData() { State = true, Data =new { completed = result } });
        }

    }
}
