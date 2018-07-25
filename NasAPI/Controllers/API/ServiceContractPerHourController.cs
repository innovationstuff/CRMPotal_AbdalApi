using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Filters;
using NasAPI.Inferstructures;
using NasAPI.Managers;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    //  [NasAuthorizationFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/HourlyContract")] // en/api/ServiceContractPerHour 
    public class ServiceContractPerHourController : BaseApiController
    {
        ServiceContractPerHourManager Manager;
        GeneralManager GeneralManager;

        public ServiceContractPerHourController()
        {
            Manager = new ServiceContractPerHourManager();
            GeneralManager = new GeneralManager();
        }

        //        // GET api/servicecontractperhour
        //        [Route("GetUserContracts/{Id}")]
        //        [ApiExplorerSettings(IgnoreApi = true)]
        //        public IEnumerable<ServiceContractPerHour> GetUserContracts(string Id)
        //        {

        //            //Get Customer Contracts
        //            string SQLGetClientContracts = @"select contract.new_HIndvContractId,contract.new_ContractNumber,
        //            contract.new_HIndivClintnameName, 
        //		  contract.new_HIndivClintname,contract.new_cityName
        //		  ,contract.new_city,contract.new_districtName ,contract.new_district,
        //		  contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
        //		  contract.new_contractmonth,contract.new_selecteddays,Contract.new_selectedshifts,Contract.new_visitprice_def
        //		  ,hourlypricing.new_name,hourlypricing.new_nationalityName,
        //		  hourlypricing.new_hourlypricingId,hourlypricing.new_shift
        //		  from new_HIndvContract  contract ,new_hourlypricing hourlypricing
        //		  where contract.new_houlrypricing=hourlypricing.new_hourlypricingId and contract.new_HIndivClintname='@Id' 
        //  order by contract.new_ContractNumber desc
        //";
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
        //                        NoOfMonths = dt.Rows[i]["new_monthvisits_def"].ToString(),
        //                        VisitPrice = dt.Rows[i]["new_visitprice_def"].ToString(),
        //                        NationalityName = dt.Rows[i]["new_nationalityName"].ToString(),
        //                        hourlypricingId = dt.Rows[i][17].ToString(),
        //                        Name = dt.Rows[i]["new_name"].ToString(),
        //                        TotalbeforeDiscount = ((decimal.Parse(dt.Rows[i]["new_visitprice_def"].ToString()) * int.Parse(dt.Rows[i]["new_monthvisits_def"].ToString()) * int.Parse(dt.Rows[i]["new_totalvisits_def"].ToString()) * 4).ToString())


        //                    },
        //                    SelectedDays = dt.Rows[i][12].ToString(),
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
        //		   ,new_shiftend,new_shiftstart
        //		   from new_hourlyappointment shift
        //		  where shift.new_servicecontractperhour in (@IDs)" +
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

        // GET api/servicecontractperhour/SendCode?Mobilenumber=&who=1


        [HttpGet]
        [Route("GetLatestNotPaidContract/{crmUserId}")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage GetLatestNotPaidContract(string crmUserId)
        {

            var result = Manager.GetLatestNotPaidContract(crmUserId);
            return OkResponse(result);
        }


        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(ServiceContractPerHour))]
        public HttpResponseMessage Get(string id)
        {

            var result = Manager.GetHourlyContractDetails(id, Language);
            return OkResponse(result);
        }

        [HttpGet]
        [Route("GetDetails/{id}")]
        [ResponseType(typeof(ServiceContractPerHour))]
        public HttpResponseMessage GetDetails_M(string id)
        {

            var result = Manager.GetHourlyContractDetails(id, Language);
            return OkResponse<ReturnData>(new ReturnData() { State = true, Data = new { contract = result} });
        }

        [HttpGet]
        [Route("SendCode")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string SendCode(string Mobilenumber, int who = 1)
        {

            /*  Random generator = new Random();
              String r = generator.Next(0, 1000000).ToString("D6");

                          //Snd To SMS 
                         Entity SMS = new Entity("new_smsns");
                          string UserName = ConfigurationManager.AppSettings["SMSUserName"];
                          string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
                          string TagName = ConfigurationManager.AppSettings["TagName"];
                          SMSRef.SMSServiceSoapClient sms = new SMSRef.SMSServiceSoapClient();
                          string MobileNumber = Mobilenumber;
                          string Message = r + ":كود تاكيد عملية التسجيل ";
                          Message += " شكرا لاستخدامكم شركةابدال";
                          string result = sms.SendBulkSMS(UserName, SMSPassword, TagName, MobileNumber, Message);

                          if (result == "1")
                              return r;
                          else*/
            return "Error";
        }

        [HttpPost]
        [Route("CreateContract")]
        [ApiExplorerSettings(IgnoreApi = true)]
        private string CreateContract(string CustomerNo, string NationalityId, string HourlyPricingId, string CityId, bool Shift, string DistrictId, string SelectedDays, string ContractStartDate, string lat, string lang, string MonthsCount, int VisitCount, string TotalPrice, int HoursCount, string Discount, string MonthelyPrice, int who = 1)
        {
            //Random generator = new Random();
            String r = new Random().Next(0, 1000000).ToString("D6");


            ContractStartDate = ContractStartDate.Replace('/', '-');



            Entity contract = new Entity("new_hindvcontract");

            contract["new_hindivclintname"] = new EntityReference("contact", new Guid(CustomerNo.ToString()));
            contract["new_nationality"] = new EntityReference("new_country", new Guid(NationalityId));
            contract["new_houlrypricing"] = new EntityReference("new_hourlypricing", new Guid(HourlyPricingId));
            Entity hourlyPricing = GlobalCode.Service.Retrieve("new_hourlypricing", new Guid(HourlyPricingId), new ColumnSet(true));
            contract["new_visitprice_def"] = int.Parse(MathNumber.RoundDeciamlToInt(hourlyPricing["new_hourprice"].ToString())) * HoursCount;
            contract["new_visittotalprice"] = Decimal.Parse(hourlyPricing["new_hourprice"].ToString()) * HoursCount;
            int months = int.Parse(MonthsCount);
            contract["new_visitcount_def"] = SelectedDays.Split(',').Count();

            contract["new_monthvisits_def"] = VisitCount;
            if (months != 0)
                contract["new_monthvisits_def"] = (VisitCount * 4);




            if (months != 0)
                contract["new_contractmonth"] = months;
            //else
            // contract["new_contractmonth"] = 13;
            contract["new_totalvisits_def"] = VisitCount;
            if (months != 0)
                contract["new_totalvisits_def"] = VisitCount * 4 * months;
            //hourlyPricing["new_totalvisits"];
            if (who == 1)
                contract["new_contractsource"] = new OptionSetValue(who);
            else
                contract["new_contractsource"] = new OptionSetValue(2);


            contract["new_hindivcount"] = HoursCount;

            contract["new_contractmonth"] = new OptionSetValue(months);


            contract["new_weeklyvisits"] = new OptionSetValue(VisitCount);

            contract["new_discount_def"] = decimal.Parse(Discount);
            // Math.Round((decimal.Parse(a.TotalbeforeDiscount) - (decimal.Parse(a.TotalbeforeDiscount) * decimal.Parse(Discount) / 100)), 2, MidpointRounding.AwayFromZero).ToString().Replace(".00", "");

            contract["new_totalprice_def"] = int.Parse(Math.Round(decimal.Parse(TotalPrice), 0, MidpointRounding.AwayFromZero).ToString());
            contract["new_new_contractdate"] = DateTime.Now;
            contract["new_city"] = new EntityReference("new_city", new Guid(CityId));
            contract["new_district"] = new EntityReference("new_district", new Guid(DistrictId));
            contract["new_shift"] = Shift == true ? false : true;
            contract["new_contractconfirm"] = false;
            contract["statuscode"] = new OptionSetValue(100000004);



            contract["new_contractstartdate"] = DateTime.ParseExact(ContractStartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            contract["new_customerdays"] = SelectedDays;
            //  contract["new_selecteddays"] = SelectedDays;
            contract["new_latitude"] = lat;
            contract["new_longitude"] = lang;
            contract["new_paymentcode"] = r;
            contract["new_mapurl"] = "http://maps.google.com/maps?q=" + lat + "," + lang + "&z=15";


            Guid contractId = GlobalCode.Service.Create(contract);





            if (!string.IsNullOrEmpty(contractId.ToString()))
            {
                //Send SMS To confirm 

                try
                {



                    string contsql = @"
select new_HIndvContract.new_ContractNumber ,Contact.FirstName from new_HIndvContract,Contact
 where Contact.ContactId =new_HIndvContract.new_HIndivClintname and 
 new_HIndvContract.new_HIndvContractId='@id'";
                    contsql = contsql.Replace("@id", contractId.ToString());
                    System.Data.DataTable dtcontract = CRMAccessDB.SelectQ(contsql).Tables[0];


                    string Sql = @"select    MobilePhone,new_deviceid from Contact where  ContactId='@id'  ";

                    Sql = Sql.Replace("@id", CustomerNo);
                    System.Data.DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];


                    //send Notification 

                    string result = SendNotifications.SendNotify("," + CustomerNo, "عزيزى العميل /" + dtcontract.Rows[0]["FirstName"].ToString() + "   " + "شكرا لاختياركم شركة ناس سيتم التواصل معكم قريبا لتاكيد الموعد للعقد رقم " + dtcontract.Rows[0]["new_ContractNumber"].ToString());

                    string body = "عزيزى العميل /" + dtcontract.Rows[0]["FirstName"].ToString() + "   " + "شكرا لاختياركم شركة ناس سيتم التواصل معكم قريبا لتاكيد الموعد للعقد رقم " + dtcontract.Rows[0]["new_ContractNumber"].ToString();
                    AppleNotifications.SendPushNotification(dt.Rows[0]["new_deviceid"].ToString(), body, "شركة ناس للاستقدام");


                    //Send To SMS 

                    /*  Entity SMS = new Entity("new_smsns");
                      string UserName = ConfigurationManager.AppSettings["SMSUserName"];
                      string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
                      string TagName = ConfigurationManager.AppSettings["TagName"];
                      SMSRef.SMSServiceSoapClient sms = new SMSRef.SMSServiceSoapClient();
                      string MobileNumber = dt.Rows[0][0].ToString();
                      string Message = "https://nasmanpower.com/HourlyServiceContract/Maps.aspx?id=" + contractId + "  برجاء تاكيد الموقع باستخدام الرابط المرسل . شكرا لاستخدامكم شركة ناس   ";
                      string res = sms.SendBulkSMS(UserName, SMSPassword, TagName, MobileNumber, Message);*/
                }
                catch (Exception)
                {
                    return contractId.ToString();

                }



            }

            return contractId.ToString();


        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("GetRequiredColumns")]
        public List<string> GetInvalidColumns()
        {
            return Manager.GetRequiredFields().ToList();
        }

        [HttpPost]
        [Route("")]
        [ResponseType(typeof(RequestServiceContractPerHour))]
        public HttpResponseMessage Post(RequestServiceContractPerHour contract)
        {
            var result = Manager.CreateNewContract(contract);

            return OkResponse<RequestServiceContractPerHour>(result);
        }

        [HttpPost]
        [Route("Create")]
        [ResponseType(typeof(ReturnData))]
        public HttpResponseMessage Create_M(RequestServiceContractPerHour contract)
        {
            var result = Manager.CreateNewContract(contract);

            return OkResponse<ReturnData>(new ReturnData() { State = true, Data = new { contract = result } });
        }


        [HttpPost]
        [Route("ConfirmTerms/{id}")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Post(string id)
        {
            var result = Manager.ConfirmTerms(id);
            return OkResponse<bool>(result);
        }

        [HttpPost]
        [Route("PickCustomerLocation")]
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Post(PickCustomerLocation CustomerLocation)
        {
            var result = Manager.PickCustomerLocation(CustomerLocation);
            return OkResponse<bool>(result);
        }

        [HttpPost]
        [Route("BankTransferStatementFile/{id}")]
        //[ResponseType(typeof(bool))]
        public async Task<HttpResponseMessage> PostBankTransferStatementFile(string id)
        {

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = ConfigurationManager.AppSettings["UploadPath"].ToString();

            var provider = new MultipartFormDataStreamProvider(root);

            try
            {

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                var file = provider.FileData.FirstOrDefault();

                if (file == null)
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                if (String.IsNullOrEmpty(file.Headers.ContentDisposition.FileName))
                    throw new HttpResponseException(HttpStatusCode.Forbidden);

                string fileName = String.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(file.Headers.ContentDisposition.FileName).Replace(" ", "_").Replace("#", "")
                    , DateTime.Now.Ticks, Path.GetExtension(file.Headers.ContentDisposition.FileName));

                File.Move(file.LocalFileName, Path.Combine(root, fileName));

                var attachmentId = Manager.AddtBankTransferStatementAttachment(id, fileName);

                if (!string.IsNullOrEmpty(attachmentId))
                    Manager.UpdateContractStatusAfterUbloadingBankTransferStatement(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        #region optionset

        [Route("Options/Visits")]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetVisits()
        {
            var result = GeneralManager.GetOptionSet_HourlyContract_Visits(Language).Where(v => v.Key < 7).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }

        [Route("Options/Labours")]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetLabours()
        {
            var result = GeneralManager.GetOptionSet_HourlyContract_Labours(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }

        [Route("Options/ContractDuration")]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetContractDuration()
        {
            var result = GeneralManager.GetOptionSet_HourlyContract_ContractDuration(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }

        [Route("Options/Hours")]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetHours()
        {
            var result = GeneralManager.GetOptionSet_HourlyContract_Hours(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }

        [Route("Options/HousingTypes")]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetHousingTypes()
        {
            var result = GeneralManager.GetOptionSet_HourlyContract_HousingTypes(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }

        [Route("Options/HousingFloors")]
        [ResponseType(typeof(List<BaseOptionSet>))]
        public HttpResponseMessage GetHousingFloors()
        {
            var result = GeneralManager.GetOptionSet_HourlyContract_HousingFloors(Language).ToList();
            return OkResponse<List<BaseOptionSet>>(result);
        }

        #endregion

        #region QuickLookups

        [Route("Lookups/DistrictNationalities/{districtId}")]
        [HttpGet]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetDistrictNationalities(string districtId)
        {
            var result = Manager.GetLookups_DistrictNationalities(districtId, Language).ToList();
            return OkResponse<List<BaseQuickLookup>>(result);
        }

        [Route("Lookups/AvailableCities")]
        [HttpGet]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetAvailableCities()
        {
            var result = Manager.GetLookups_AvailableCities(Language).ToList();
            return OkResponse<List<BaseQuickLookup>>(result);
        }

        #endregion
    }






}
