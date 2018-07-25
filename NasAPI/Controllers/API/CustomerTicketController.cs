using NasAPI.Enums;
using NasAPI.Helpers;
using NasAPI.Managers;
using NasAPI.Models;
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
    [RoutePrefix("{lang}/api/CustomerTicket")] // en/api/CustomerTicket
    public class CustomerTicketController : BaseApiController
    {
        CustomerTicketManager Manager;

        public CustomerTicketController()
        {
            Manager = new CustomerTicketManager();
        }

        [Route("Dalal/GetTickets")]
        [HttpGet]
        [ResponseType(typeof(List<CustomerTicket>))]
        public HttpResponseMessage GetAllTickets(string sectorId, string userId, string statusCode = null)
        {
            var result = Manager.GetDalalCustomerTickets(sectorId, userId, Language, statusCode).ToList();
            return OkResponse(result);
        }

        [Route("Dalal/GetTicket/{id}")]
        [HttpGet]
        [ResponseType(typeof(CustomerTicket))]
        public HttpResponseMessage GetTicketById(string id)
        {
            var result = Manager.GetDalalTicketById(id, Language);

            return OkResponse(result);
        }

        [Route("Dalal/GetTicketByNumber")]
        [HttpGet]
        [ResponseType(typeof(CustomerTicket))]
        public HttpResponseMessage GetDalalTicketByNumber(string ticketNumber)
        {
            var result = Manager.GetDalalTicketByNumber(ticketNumber, Language);

            return OkResponse(result);
        }


        [Route("Options/StatusList")]
        [HttpGet]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetStatusList()
        {
            var result = Manager.GetEntityStatusOptions(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }


        [Route("Dalal/GetTickets_M")]
        [HttpGet]
        [ResponseType(typeof(ReturnData))]
        public HttpResponseMessage GetAllTickets_M(string sectorId, string userId, int pageSize, int pageNumber, string statusCode = null)
        {
            var result = Manager.GetDalalCustomerTickets(sectorId, userId, Language, statusCode).ToList();
            // Get's No of Rows Count   
            int count = result.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = result.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };
            
            return OkResponse<ReturnData>(new ReturnData() { State = true, Data = new { tickets = items } });
        }

        #region ========= Dalal ===============

        [Route("Dalal/ServedEmployeesForContract/{contractId}")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<BaseQuickLookup>))]
        public HttpResponseMessage GetDalalServedEmployeesByContractId(string contractId)
        {
            var result = Manager.GetDalalServedEmployeesByContractId(contractId).ToList();
            return OkResponse(result);
        }

        [Route("Dalal/ServedEmployees")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<BaseQuickLookup>))]
        public HttpResponseMessage GetDalalServedEmployeesByCustomerId(string userId)
        {
            var result = Manager.GetDalalServedEmployeesByCustomerId(userId).ToList();
            return OkResponse(result);
        }

        [Route("Dalal/Contracts")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<BaseQuickLookup>))]
        public HttpResponseMessage GetDalalContracts(string userId)
        {
            var result = Manager.GetDalalContracts(userId).ToList();
            return OkResponse(result);
        }

        [Route("Dalal/ProblemTypes")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<BaseOptionSet>))]
        public HttpResponseMessage GetDalalProblemTypes()
        {
            var result = Manager.GetDalalProblemTypes(Language).ToList();
            return OkResponse(result);
        }

        [Route("Dalal/Create")]
        [HttpPost]
        [ResponseType(typeof(CustomerTicket))]
        public HttpResponseMessage CreateDalalTicket(CustomerTicket ticket)
        {
            try
            {
                var result = Manager.CreateDalalTicket(ticket, ticket.who);
                return OkResponse<CustomerTicket>(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("contact With Id"))
                {
                    string message = string.IsNullOrEmpty(ex.Message) ? "Contact Not Found" : ex.Message;
                    return NotFoundResponse("Contact Not Found", message);
                }
                else
                {
                    return NotFoundResponse("Error in create dalal ticket", ex.Message);
                }
            }
        }

        [Route("Dalal/Create_M")]
        [HttpPost]
        [ResponseType(typeof(ReturnData))]
        public HttpResponseMessage CreateDalalTicket_M(CustomerTicket ticket)
        {
            try
            {
                ticket.who = Who.Mobile;
                var result = Manager.CreateDalalTicket(ticket, Who.Mobile);
                return OkResponse<ReturnData>(new ReturnData() { State = true, Data = new { ticket = result } });
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("contact With Id"))
                {
                    string message = string.IsNullOrEmpty(ex.Message) ? "Contact Not Found" : ex.Message;
                    return OkResponse<ReturnData>(new ReturnData() { State = true, Data = "Contact Not Found : " + message });
                }
                else
                {
                    return OkResponse<ReturnData>(new ReturnData() { State = true, Data = "Error in create dalal ticket : " + ex.Message });
                }
            }
        }
        #endregion


        #region ================ Individual ===================

        [Route("Individual/ProblemTypes")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<BaseOptionSet>))]
        public HttpResponseMessage GetIndividualProblemTypes()
        {
            var result = Manager.GetIndividualProblemTypes(Language).ToList();
            return OkResponse(result);
        }

        [Route("Individual/Create")]
        [HttpPost]
        [ResponseType(typeof(CustomerTicket))]
        public HttpResponseMessage CreateIndividualTicket(CustomerTicket ticket, Who who = Who.Web)
        {
            var result = Manager.CreateIndividualTicket(ticket, who);
            return OkResponse<CustomerTicket>(result);
        }

        #endregion
    }
}