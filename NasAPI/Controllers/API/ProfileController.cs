using NasAPI.Helpers;
using NasAPI.Inferstructures;
using NasAPI.Managers;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Profile;

namespace NasAPI.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix("{lang}/api/Profile")] // en/api/Profile 
    public class ProfileController : BaseApiController
    {
        NasAPI.Managers.ProfileManager Manager { get; set; }
        //public OptionsController options { get; set; }
        public ProfileController()
        {
            Manager = new NasAPI.Managers.ProfileManager();
            //options = new OptionsController();
        }



        #region HourlyContracts

        /// <summary>
        /// Get All Hourly Contracts Basic data for user
        /// </summary>
        /// <remarks> Get All Hourly Contracts Basic data for user </remarks>
        /// <returns>IEnumerable<ServiceContractPerHour></returns>
        [HttpGet]
        [Route("HourlyContract/All")]
        [ResponseType(typeof(List<ServiceContractPerHour>))]
        public HttpResponseMessage GetHourlyContracts(string userId, string statusCode = null)
        {
            using (var contractMgr = new ServiceContractPerHourManager())
            {
                var result = contractMgr.GetUserHourlyContracts(userId, statusCode, Language).ToList();
                return OkResponse(result);
            }
        }
        [HttpGet]
        [Route("HourlyContract/All_M")]
        [ResponseType(typeof(ReturnData))]
        public HttpResponseMessage GetHourlyContracts_M(string userId, int pageSize, int pageNumber, string statusCode = null)
        {
            using (var contractMgr = new ServiceContractPerHourManager())
            {
                var result = contractMgr.GetUserHourlyContracts(userId, statusCode, Language).ToList();
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

                return OkResponse<ReturnData>(new ReturnData() { State = true, Data = new { contracts = items } });
            }
        }

        [HttpGet]
        [Route("HourlyContract/Details/{contractId}")]
        [ResponseType(typeof(List<ServiceContractPerHour>))]
        public HttpResponseMessage GetHourlyContractDetails(string userId, string contractId)
        {
            using (var contractMgr = new ServiceContractPerHourManager())
            {
                var result = contractMgr.GetUserHourlyContractDetails(userId, contractId, Language);
                return OkResponse(result);
            }
        }

        [HttpGet]
        [Route("HourlyContract/Appointments/{contractId}")]
        [ResponseType(typeof(List<HourlyAppointment>))]
        public HttpResponseMessage GetHourlyContractAppointments(string userId, string contractId)
        {
            using (var contractMgr = new HourlyAppointmentManager())
            {
                var result = contractMgr.GetHourlyAppointments(contractId, Language).ToList();
                return OkResponse(result);
            }
        }

        //[HttpGet]
        //[Route("HourlyContract/{id}")]
        //[ResponseType(typeof(IEnumerable<ServiceContractPerHour>))]
        //public HttpResponseMessage GetHourlyContract(string id, string userId)
        //{

        //}


        #endregion


        #region Options

        [ResponseType(typeof(IEnumerable<BaseOptionSet>))]
        [Route("Options/HourlyContractStatus")]
        public HttpResponseMessage GetUserContractStatusOptions()
        {
            var result = Manager.GetHourlyContractStatusForProfile(Language);
            return OkResponse(result);
        }

        #endregion


        // old actions

        //        [HttpGet]
        //        // GET api/Profile/GetUserContracts
        //        public IEnumerable<ServiceContractPerHour> GetUserContracts(string Id)
        //        {

        //            //Get Customer Contracts
        //            string SQLGetClientContracts = @"select contract.new_HIndvContractId,contract.new_ContractNumber,
        //                    contract.new_HIndivClintnameName, 
        //        		  contract.new_HIndivClintname,contract.new_cityName
        //        		  ,contract.new_city,contract.new_districtName ,contract.new_district,
        //        		  contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
        //        		  contract.new_contractmonth,contract.new_selecteddays,Contract.new_selectedshifts,Contract.new_visitprice_def
        //        		  ,hourlypricing.new_name,hourlypricing.new_nationalityName,
        //        		  hourlypricing.new_hourlypricingId,hourlypricing.new_shift
        //        		  from new_HIndvContract  contract ,new_hourlypricing hourlypricing
        //        		  where contract.new_houlrypricing=hourlypricing.new_hourlypricingId and contract.new_HIndivClintname='@Id' 
        //          order by contract.new_ContractNumber desc
        //        ";
        //            SQLGetClientContracts = SQLGetClientContracts.Replace("@Id", Id);

        //            DataTable dt = CRMAccessDB.SelectQ(SQLGetClientContracts).Tables[0];
        //            List<ServiceContractPerHour> List = new List<ServiceContractPerHour>();

        //            string ContractIds = "";
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                List.Add(new ServiceContractPerHour()
        //                {

        //                    ContractId = dt.Rows[i][0].ToString(),
        //                    ContractNum = dt.Rows[i][1].ToString(),
        //                    Customer = dt.Rows[i][2].ToString(),
        //                    CustomerId = dt.Rows[i][3].ToString(),
        //                    City = dt.Rows[i][4].ToString(),
        //                    District = dt.Rows[i]["new_districtName"].ToString(),
        //                    Nationality = dt.Rows[i]["new_nationalityName"].ToString(),
        //                    HourlyPricing = new HourlyPricing()
        //                    {
        //                        Discount = dt.Rows[i]["new_discount_def"].ToString(),
        //                        TotalPrice = dt.Rows[i]["new_totalprice_def"].ToString(),
        //                        TotalVisit = dt.Rows[i]["new_totalvisits_def"].ToString(),
        //                        MonthVisits = dt.Rows[i][10].ToString(),
        //                        NoOfMonths = dt.Rows[i]["new_contractmonth"].ToString(),
        //                        VisitPrice = dt.Rows[i]["new_visitprice_def"].ToString(),
        //                        NationalityName = dt.Rows[i]["new_nationalityName"].ToString(),
        //                        hourlypricingId = dt.Rows[i][17].ToString(),
        //                        Name = dt.Rows[i]["new_name"].ToString(),
        //                        TotalbeforeDiscount = ((decimal.Parse(dt.Rows[i]["new_visitprice_def"].ToString()) * int.Parse(dt.Rows[i]["new_monthvisits_def"].ToString()) * int.Parse(dt.Rows[i]["new_totalvisits_def"].ToString()) * 4).ToString())


        //                    },
        //                    SelectedDays = dt.Rows[i]["new_selecteddays"].ToString(),
        //                    Shift = dt.Rows[i]["new_shift"].ToString() == "False" ? "Morning" : "Evening",

        //                    HourlyAppointments = new List<HourlyAppointment>()
        //                });

        //                ContractIds += "'" + dt.Rows[i][0].ToString() + "'";

        //                if (i != dt.Rows.Count - 1)
        //                    ContractIds += ",";

        //            }

        //            if (dt.Rows.Count > 0)
        //            {
        //                //Get  Contracts Hourly AppointMent 
        //                string SqlShifts = @"select new_servicecontractperhourName,new_servicecontractperhour,new_employeeName,new_employee,new_status,new_notes
        //        		   ,new_shiftend,new_shiftstart
        //        		   from new_hourlyappointment shift
        //        		  where shift.new_servicecontractperhour in (@IDs)" +
        //             " order by new_servicecontractperhourName,new_shiftstart";

        //                SqlShifts = SqlShifts.Replace("@IDs", ContractIds);

        //                DataTable sqldt = CRMAccessDB.SelectQ(SqlShifts).Tables[0];
        //                List<HourlyAppointment> ShiftsList = new List<HourlyAppointment>();

        //                for (int i = 0; i < sqldt.Rows.Count; i++)

        //                    ShiftsList.Add(new HourlyAppointment()
        //                    {

        //                        ContractCode = sqldt.Rows[i][0].ToString(),
        //                        ContractId = sqldt.Rows[i][1].ToString(),
        //                        HouseMadeName = sqldt.Rows[i][2].ToString(),
        //                        HouseMadeId = sqldt.Rows[i][3].ToString(),
        //                        Status = sqldt.Rows[i][4].ToString(),
        //                        Notes = sqldt.Rows[i][5].ToString(),
        //                        EndDate = sqldt.Rows[i][6].ToString(),
        //                        StartDate = sqldt.Rows[i][7].ToString()
        //                    });


        //                //Set every Hourly Appointment To it's Contact 
        //                for (int i = 0; i < List.Count; i++)
        //                    List[i].HourlyAppointments.AddRange(ShiftsList.Where(a => a.ContractCode == List[i].ContractNum));



        //            }

        //            return List;
        //        }


        //        [HttpGet]
        //        //GET  api/Profile/ContractStatus
        //        public IEnumerable<Status> ContractStatus()
        //        {
        //            return StaticLists.contractstatus();
        //        }


        //        [HttpGet]
        //        //GET api/Profile/HourlyContracts
        //        public IEnumerable<ServiceContractPerHour> HourlyContracts(string Id, string Status)
        //        {

        //            //Get Customer Contracts
        //            string SQLGetClientContracts = @"select contract.new_HIndvContractId,contract.new_ContractNumber,
        //                    contract.new_HIndivClintnameName, 
        //        		  contract.new_HIndivClintname,contract.new_cityName
        //        		  ,contract.new_city,contract.new_districtName ,contract.new_district,
        //        		  contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
        //        		  contract.new_contractmonth,contract.new_selecteddays,Contract.new_selectedshifts,Contract.new_visitprice_def
        //        		  ,hourlypricing.new_name,hourlypricing.new_nationalityName,
        //        		  hourlypricing.new_hourlypricingId,hourlypricing.new_shift,contract.statuscode ,CONVERT(nvarchar(10),dateadd(hh, 3,contract.new_contractstartdate),103) as startdate,new_contractmonth
        //        ,new_customerdays		  
        //        from new_HIndvContract  contract ,new_hourlypricing hourlypricing
        //        		  where contract.new_houlrypricing=hourlypricing.new_hourlypricingId and contract.new_HIndivClintname='@Id' and contract.statuscode='@status'
        //           order by contract.new_ContractNumber desc
        //        ";


        //            SQLGetClientContracts = SQLGetClientContracts.Replace("@Id", Id);
        //            SQLGetClientContracts = SQLGetClientContracts.Replace("@status", Status);

        //            DataTable dt = CRMAccessDB.SelectQ(SQLGetClientContracts).Tables[0];
        //            List<ServiceContractPerHour> List = new List<ServiceContractPerHour>();

        //            string ContractIds = "";
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string numbermonthes = "";
        //                numbermonthes = dt.Rows[i]["new_contractmonth"].ToString();
        //                List.Add(new ServiceContractPerHour()
        //                {
        //                    ContractDate = dt.Rows[i]["startdate"].ToString(),
        //                    ContractId = dt.Rows[i][0].ToString(),
        //                    ContractNum = dt.Rows[i][1].ToString(),
        //                    Customer = dt.Rows[i][2].ToString(),
        //                    CustomerId = dt.Rows[i][3].ToString(),
        //                    City = dt.Rows[i][4].ToString(),
        //                    District = dt.Rows[i]["new_districtName"].ToString(),
        //                    Nationality = dt.Rows[i]["new_nationalityName"].ToString(),
        //                    HourlyPricing = new HourlyPricing()
        //                    {

        //                        //  VisitCount = options.GetVisits(0)[int.Parse(dt.Rows[i]["new_visitcount_def"].ToString()) - 1].Name.ToString(),
        //                        Discount = dt.Rows[i]["new_discount_def"].ToString(),
        //                        TotalPrice = dt.Rows[i]["new_totalprice_def"].ToString(),
        //                        TotalVisit = dt.Rows[i]["new_totalvisits_def"].ToString(),
        //                        MonthVisits = dt.Rows[i][10].ToString(),
        //                        NoOfMonths = OptionsController.GetName("new_HIndvContract", "new_contractmonth", 1025, numbermonthes),
        //                        // NoOfMonths = options.GetMonths(0)[int.Parse(numbermonthes)].Name.ToString(),
        //                        VisitPrice = dt.Rows[i]["new_visitprice_def"].ToString(),
        //                        NationalityName = dt.Rows[i]["new_nationalityName"].ToString(),
        //                        hourlypricingId = dt.Rows[i][17].ToString(),
        //                        Name = dt.Rows[i]["new_name"].ToString(),
        //                        TotalbeforeDiscount = ((decimal.Parse(dt.Rows[i]["new_visitprice_def"].ToString()) * int.Parse(dt.Rows[i]["new_monthvisits_def"].ToString()) * int.Parse(dt.Rows[i]["new_totalvisits_def"].ToString()) * 4).ToString()),
        //                        Shift = dt.Rows[i]["new_shift"].ToString() == "False" ? "true" : "false",
        //                        //Shift = dt.Rows[i]["new_shift"].ToString() == "False" ? "صباحا" : "مساء",

        //                    },
        //                    SelectedDays = dt.Rows[i]["new_customerdays"].ToString().Replace("Sunday", "الاحد").Replace("Monday", "الاثنين")
        //                    .Replace("Tuesday", "الثلاثاء").Replace("Wednesday", "الاربعاء").Replace("Thursday", "الخميس").Replace("Friday", "الجمعة").Replace("Saturday", "السبت"),
        //                    Shift = dt.Rows[i]["new_shift"].ToString() == "False" ? "صباحا" : "مساء",

        //                    HourlyAppointments = new List<HourlyAppointment>()
        //                });

        //                ContractIds += "'" + dt.Rows[i][0].ToString() + "'";

        //                if (i != dt.Rows.Count - 1)
        //                    ContractIds += ",";

        //            }

        //            return List;
        //        }
        //        [HttpGet]
        //        //GET api/Profile/HourlyContractDetails
        //        public IEnumerable<ServiceContractPerHour> HourlyContractDetails(string Id)
        //        {

        //            //Get Customer Contracts
        //            string SQLGetClientContracts = @"select contract.new_HIndvContractId,contract.new_ContractNumber,
        //                    contract.new_HIndivClintnameName, 
        //        		  contract.new_HIndivClintname,contract.new_cityName
        //        		  ,contract.new_city,contract.new_districtName ,contract.new_district,
        //        		  contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
        //        		  contract.new_contractmonth,contract.new_selecteddays,Contract.new_selectedshifts,Contract.new_visitprice_def
        //        		  ,hourlypricing.new_name,hourlypricing.new_nationalityName,
        //        		  hourlypricing.new_hourlypricingId,hourlypricing.new_shift,contract.statuscode ,CONVERT(nvarchar(10),dateadd(hh, 3,contract.new_contractstartdate),103) as startdate,Contract.new_latitude,Contract.new_longitude
        //        		  
        //                         ,Contract.new_visitcount_def,Contract.new_visitprice_def,new_contractmonth,new_customerdays
        //                   from new_HIndvContract  contract ,new_hourlypricing hourlypricing
        //        		  where contract.new_houlrypricing=hourlypricing.new_hourlypricingId and contract.new_HIndvContractId='@Id' 
        //          order by contract.new_ContractNumber desc
        //        ";
        //            SQLGetClientContracts = SQLGetClientContracts.Replace("@Id", Id);

        //            DataTable dt = CRMAccessDB.SelectQ(SQLGetClientContracts).Tables[0];
        //            List<ServiceContractPerHour> List = new List<ServiceContractPerHour>();

        //            string ContractIds = "";
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                //string numbermonthes = "اسبوع واحد فقط ";
        //                //if (!string.IsNullOrEmpty(dt.Rows[i]["new_contractmonth"].ToString()))
        //                string numbermonthes = dt.Rows[i]["new_contractmonth"].ToString();
        //                List.Add(new ServiceContractPerHour()
        //                {
        //                    Statuscode = dt.Rows[i]["statuscode"].ToString(),
        //                    StatusName = dt.Rows[i]["statuscode"].ToString() == "100000000" ? "منتهى"
        //                    : dt.Rows[i]["statuscode"].ToString() == "100000004" ? "لم يتم الدفع" : dt.Rows[i]["statuscode"].ToString() == "100000002" ? "قادمة" : "جارى الان",
        //                    Longitude = dt.Rows[i]["new_longitude"].ToString(),
        //                    Latitude = dt.Rows[i]["new_latitude"].ToString(),
        //                    ContractDate = dt.Rows[i]["startdate"].ToString(),
        //                    ContractId = dt.Rows[i][0].ToString(),
        //                    ContractNum = dt.Rows[i][1].ToString(),
        //                    Customer = dt.Rows[i][2].ToString(),
        //                    CustomerId = dt.Rows[i][3].ToString(),
        //                    City = dt.Rows[i][4].ToString(),
        //                    District = dt.Rows[i]["new_districtName"].ToString(),
        //                    Nationality = dt.Rows[i]["new_nationalityName"].ToString(),
        //                    HourlyPricing = new HourlyPricing()
        //                    {

        //                        Shift = dt.Rows[i]["new_shift"].ToString() == "False" ? "True" : "False",
        //                        Discount = dt.Rows[i]["new_discount_def"].ToString(),
        //                        TotalPrice = dt.Rows[i]["new_totalprice_def"].ToString(),
        //                        TotalVisit = dt.Rows[i]["new_totalvisits_def"].ToString(),
        //                        MonthVisits = dt.Rows[i][10].ToString(),
        //                        NoOfMonths = OptionsController.GetName("new_HIndvContract", "new_contractmonth", 1025, numbermonthes),
        //                        // NoOfMonths = options.GetMonths(0)[int.Parse(numbermonthes)].Name.ToString(),
        //                        VisitPrice = dt.Rows[i]["new_visitprice_def"].ToString(),
        //                        VisitCount = OptionsController.GetName("new_HIndvContract", "new_weeklyvisits", 1025, dt.Rows[i]["new_visitcount_def"].ToString()),
        //                        //  VisitCount = options.GetVisits(0)[int.Parse(dt.Rows[i]["new_visitcount_def"].ToString())-1].Name.ToString(),
        //                        NationalityName = dt.Rows[i]["new_nationalityName"].ToString(),
        //                        hourlypricingId = dt.Rows[i][17].ToString(),
        //                        Name = dt.Rows[i]["new_name"].ToString(),
        //                        TotalbeforeDiscount = ((decimal.Parse(dt.Rows[i]["new_visitprice_def"].ToString()) * int.Parse(dt.Rows[i]["new_monthvisits_def"].ToString()) * int.Parse(dt.Rows[i]["new_totalvisits_def"].ToString()) * 4).ToString()),
        //                        MonthelyPrice = (int.Parse(dt.Rows[i]["new_visitprice_def"].ToString().ToString()) * 1 * int.Parse(dt.Rows[i]["new_totalvisits_def"].ToString()) * 4).ToString()

        //                    },
        //                    SelectedDays = dt.Rows[i]["new_customerdays"].ToString().Replace("Sunday", "الاحد").Replace("Monday", "الاثنين")
        //                    .Replace("Tuesday", "الثلاثاء").Replace("Wednesday", "الاربعاء").Replace("Thursday", "الخميس").Replace("Friday", "الجمعة").Replace("Saturday", "السبت"),
        //                    Shift = dt.Rows[i]["new_shift"].ToString() == "False" ? "صباحا" : "مساء",


        //                    HourlyAppointments = new List<HourlyAppointment>()
        //                });

        //                ContractIds += "'" + dt.Rows[i][0].ToString() + "'";

        //                if (i != dt.Rows.Count - 1)
        //                    ContractIds += ",";

        //            }

        //            if (dt.Rows.Count > 0)
        //            {
        //                //Get  Contracts Hourly AppointMent 
        //                string SqlShifts = @"select shift.new_name,new_servicecontractperhourName,new_servicecontractperhour,new_employeeName,new_employee,new_status,new_notes
        //        		   , CONVERT(VARCHAR(20),dateadd(hh, 3, shift.new_shiftstart ),103)+' [ '+ FORMAT(CAST(dateadd(hh, 3, shift.new_shiftstart ) AS DATETIME),'hh:mm tt')+' ]'  ShiftStart,CONVERT(VARCHAR(20),dateadd(hh, 3, shift.new_shiftend ),103)+' [ '+ FORMAT(CAST(dateadd(hh, 3, shift.new_shiftend ) AS DATETIME),'hh:mm tt')+' ]'  ShiftEnd ,shift.new_hourlyappointmentId
        //        		   ,CONVERT(VARCHAR(20),dateadd(hh, 3, shift.new_shiftstart ),103) shiftdatestart ,FORMAT(CAST(dateadd(hh, 3, shift.new_shiftstart ) AS DATETIME),'hh:mm tt') shifttimestart,FORMAT(CAST(dateadd(hh, 3, shift.new_shiftend ) AS DATETIME),'hh:mm tt') shifttimeend
        //         from new_hourlyappointment shift
        //        		  where shift.new_servicecontractperhour in (@IDs)" +
        //             " order by new_servicecontractperhourName,new_shiftstart";

        //                SqlShifts = SqlShifts.Replace("@IDs", ContractIds);

        //                DataTable sqldt = CRMAccessDB.SelectQ(SqlShifts).Tables[0];
        //                List<HourlyAppointment> ShiftsList = new List<HourlyAppointment>();

        //                for (int i = 0; i < sqldt.Rows.Count; i++)
        //                {
        //                    DateTime d1 = DateTime.ParseExact(sqldt.Rows[i]["shiftdatestart"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //                    ShiftsList.Add(new HourlyAppointment()
        //                    {
        //                        ShiftId = sqldt.Rows[i]["new_hourlyappointmentId"].ToString(),
        //                        ContractCode = sqldt.Rows[i]["new_servicecontractperhourName"].ToString(),
        //                        ContractId = sqldt.Rows[i]["new_servicecontractperhour"].ToString(),
        //                        HouseMadeName = sqldt.Rows[i]["new_employeeName"].ToString(),
        //                        HouseMadeId = sqldt.Rows[i]["new_employee"].ToString(),
        //                        Status = sqldt.Rows[i]["new_status"].ToString() == "0" ? "جديد" :
        //                                 sqldt.Rows[i]["new_status"].ToString() == "1" ? "بداالان" :
        //                                 sqldt.Rows[i]["new_status"].ToString() == "2" ? "انتهى" :
        //                                 sqldt.Rows[i]["new_status"].ToString() == "3" ? "رفض" : "تم التاجيل",
        //                        Notes = sqldt.Rows[i]["new_notes"].ToString(),
        //                        StartDate = sqldt.Rows[i]["ShiftStart"].ToString(),
        //                        EndDate = sqldt.Rows[i]["ShiftEnd"].ToString(),
        //                        StatusCode = sqldt.Rows[i]["new_status"].ToString(),
        //                        ShiftCode = sqldt.Rows[i]["new_name"].ToString(),
        //                        Dayname = d1.ToString("dddd", new System.Globalization.CultureInfo("ar-AE")),
        //                        TimeFrom = sqldt.Rows[i]["shifttimestart"].ToString(),
        //                        TimeTo = sqldt.Rows[i]["shifttimeend"].ToString(),
        //                        Daydate = sqldt.Rows[i]["shiftdatestart"].ToString(),

        //                    });
        //                }

        //                //Set every Hourly Appointment To it's Contact 
        //                for (int i = 0; i < List.Count; i++)
        //                    List[i].HourlyAppointments.AddRange(ShiftsList.Where(a => a.ContractCode == List[i].ContractNum));



        //            }

        //            return List;
        //        }


        #region individual

        [HttpGet]
        [Route("DomesticInvoice/Details/{Id}")]
        [ResponseType(typeof(DomesticInvoice))]
        public HttpResponseMessage GetDomesticInvoiceDetails(string userId, string Id)
        {
            using (var domesticInvoiceMgr = new DomesticInvoiceManager())
            {
                var result = domesticInvoiceMgr.GetDomesticInvoiceDetails(Id, userId, Language);
                return OkResponse<DomesticInvoice>(result);
            }
        }

        #endregion
    }
}
