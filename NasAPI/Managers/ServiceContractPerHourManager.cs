using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Inferstructures;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class ServiceContractPerHourManager : BaseManager, IDisposable
    {
        HourlyPricingManager HourlyPricingManager { get; set; }
        PromotionManager PromotionMgr { get; set; }

        public ServiceContractPerHourManager()
            : base(CrmEntityNamesMapping.ServiceContractPerHour, "new_hindvcontractid")
        {
            HourlyPricingManager = new HourlyPricingManager();
            PromotionMgr = new PromotionManager();
        }

        public RequestServiceContractPerHour CreateNewContract(RequestServiceContractPerHour contract)
        {
            var contractEntity = CastToCrmEntity(contract);
            //MIB for making Status waiting paid
            contractEntity["statuscode"] = new OptionSetValue(100000006);

            contractEntity["new_contractconfirm"] = true;
            Guid contractId = GlobalCode.Service.Create(contractEntity);


            contract.ContractId = contractId.ToString();
            Entity CreatedContract = GetCrmEntity(contractId.ToString());
            contract.ContractNum = CreatedContract["new_contractnumber"].ToString();
            contract.FinalPrice = CreatedContract["new_finalprice"].ToString();
            #region MIB //to be changed this for making online payment URL
            try
            {
                Entity Contractupdate = GlobalCode.Service.Retrieve("new_hindvcontract", contractId, new ColumnSet(false));
                //Contractupdate["new_onlinepaymenturl"] = "https://abdal.sa/ar/Payment/DalalShopperResult/" + contractId.ToString();

                Contractupdate["new_onlinepaymenturl"] = String.Format("{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["OnlineAbdalPortalUrl"],
                    GeneralAppSettings.RequestLang, ConfigurationManager.AppSettings["OnlineUserDalalPaymentUrl"], contractId.ToString());

                GlobalCode.Service.Update(Contractupdate);
            }
            catch (Exception)
            {

            }
            #endregion





            return contract;


            //            if (!string.IsNullOrEmpty(contractId.ToString()))
            //            {
            //                //Send SMS To confirm 

            //                try
            //                {



            //                    string contsql = @"
            //            select new_HIndvContract.new_ContractNumber ,Contact.FirstName from new_HIndvContract,Contact
            //             where Contact.ContactId =new_HIndvContract.new_HIndivClintname and 
            //             new_HIndvContract.new_HIndvContractId='@id'";
            //                    contsql = contsql.Replace("@id", contractId.ToString());
            //                    System.Data.DataTable dtcontract = CRMAccessDB.SelectQ(contsql).Tables[0];


            //                    string Sql = @"select    MobilePhone,new_deviceid from Contact where  ContactId='@id'  ";

            //                    Sql = Sql.Replace("@id", CustomerNo);
            //                    System.Data.DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];


            //                    //send Notification 

            //                    string result = SendNotifications.SendNotify("," + CustomerNo, "عزيزى العميل /" + dtcontract.Rows[0]["FirstName"].ToString() + "   " + "شكرا لاختياركم شركة ناس سيتم التواصل معكم قريبا لتاكيد الموعد للعقد رقم " + dtcontract.Rows[0]["new_ContractNumber"].ToString());

            //                    string body = "عزيزى العميل /" + dtcontract.Rows[0]["FirstName"].ToString() + "   " + "شكرا لاختياركم شركة ناس سيتم التواصل معكم قريبا لتاكيد الموعد للعقد رقم " + dtcontract.Rows[0]["new_ContractNumber"].ToString();
            //                    AppleNotifications.SendPushNotification(dt.Rows[0]["new_deviceid"].ToString(), body, "شركة ناس للاستقدام");


            //                    //Send To SMS 

            //                    /*  Entity SMS = new Entity("new_smsns");
            //                      string UserName = ConfigurationManager.AppSettings["SMSUserName"];
            //                      string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
            //                      string TagName = ConfigurationManager.AppSettings["TagName"];
            //                      SMSRef.SMSServiceSoapClient sms = new SMSRef.SMSServiceSoapClient();
            //                      string MobileNumber = dt.Rows[0][0].ToString();
            //                      string Message = "https://nasmanpower.com/HourlyServiceContract/Maps.aspx?id=" + contractId + "  برجاء تاكيد الموقع باستخدام الرابط المرسل . شكرا لاستخدامكم شركة ناس   ";
            //                      string res = sms.SendBulkSMS(UserName, SMSPassword, TagName, MobileNumber, Message);*/
            //                }
            //                catch (Exception)
            //                {
            //                    return contractId.ToString();

            //                }



            //            }

            //return contractId.ToString();


        }

        public Entity CastToCrmEntity(RequestServiceContractPerHour contract)
        {
            Entity contractEntity = new Entity(CrmEntityName);

            contractEntity["new_hindivclintname"] = new EntityReference(CrmEntityNamesMapping.Contact, new Guid(contract.CustomerId.ToString()));
            contractEntity["new_nationality"] = new EntityReference(CrmEntityNamesMapping.Nationality, new Guid(contract.NationalityId));
            contractEntity["new_houlrypricing"] = new EntityReference(CrmEntityNamesMapping.HourlyPricing, new Guid(contract.HourlyPricingId));
            contractEntity["new_city"] = new EntityReference(CrmEntityNamesMapping.City, new Guid(contract.CityId));
            contractEntity["new_district"] = new EntityReference(CrmEntityNamesMapping.District, new Guid(contract.DistrictId));

            RequestHourlyPricing pricingObj = new RequestHourlyPricing();
            pricingObj.PromotionCode = contract.PromotionCode;

            var promotion = PromotionMgr.GetPromotionByCode(pricingObj,UserLanguage.Arabic);

            contractEntity["new_promotionid"] = new EntityReference(CrmEntityNamesMapping.Promotion, new Guid(promotion.Id));
            var totalVisits = contract.ContractDuration * contract.NumOfVisits;
            var extraVisits = (promotion.FreeVisitsFactor ?? 0) == 0 ? 0 : Math.Truncate((decimal)totalVisits / promotion.FreeVisitsFactor.Value);
            var totalPlusExtraVisits = totalVisits + extraVisits;

            contractEntity["new_contractsource"] = new OptionSetValue(contract.Who == 1 ? 1 : 2);  // why?

            contractEntity["new_housetype"] = new OptionSetValue(contract.HouseType);
            contractEntity["new_floorno"] = new OptionSetValue(contract.FloorNo);
            contractEntity["new_partmentnumber"] = contract.PartmentNo;

            contractEntity["new_houseno"] = contract.HouseNo;
            contractEntity["new_notes"] = contract.AddressNotes;


            //if (contract.Who == 1)
            //    contractEntity["new_contractsource"] = new OptionSetValue(contract.Who);
            //else
            //    contractEntity["new_contractsource"] = new OptionSetValue(2);

            contractEntity["new_new_contractdate"] = DateTime.Now;
            contractEntity["new_contractconfirm"] = DefaultValues.ServiceContractPerHour_IsConfirmed;
            contractEntity["statuscode"] = new OptionSetValue(DefaultValues.ServiceContractPerHour_StatusCode);

            contract.StartDay = contract.StartDay.Replace('/', '-');

            contractEntity["new_contractstartdate"] = DateTime.ParseExact(contract.StartDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            contractEntity["new_customerdays"] = contract.AvailableDays;
            contractEntity["new_selecteddays"] = contract.AvailableDays;
            contractEntity["new_latitude"] = contract.Latitude;
            contractEntity["new_longitude"] = contract.Longitude;
            contractEntity["new_mapurl"] = "http://maps.google.com/maps?q=" + contract.Latitude + "," + contract.Longitude + "&z=15";
            contractEntity["new_paymentcode"] = new Random().Next(0, 1000000).ToString("D6");

            Entity hourlyPricing = HourlyPricingManager.GetCrmEntity(contract.HourlyPricingId);
            var hourlyPricingCost = HourlyPricingManager.CalculateHourlyPricingCost(hourlyPricing, contract.NumOfHours, contract.NumOfVisits, contract.ContractDuration, contract.NumOfWorkers, promotion);

            contractEntity["new_shift"] = bool.Parse(hourlyPricing["new_shift"].ToString());

            contractEntity["new_hoursnumber"] = new OptionSetValue(contract.NumOfHours);
            contractEntity["new_visitcount_def"] = contract.NumOfVisits;
            contractEntity["new_weeklyvisits"] = new OptionSetValue(contract.NumOfVisits);
            contractEntity["new_contractmonth"] = new OptionSetValue(contract.ContractDuration);
            contractEntity["new_employeenumber"] = new OptionSetValue(contract.NumOfWorkers);
            contractEntity["new_visitprice_def"] = int.Parse(MathNumber.RoundDeciamlToInt(hourlyPricingCost.HourRate.ToString())) * contract.NumOfHours;
            contractEntity["new_visittotalprice"] = hourlyPricingCost.HourRate * contract.NumOfHours;
            contractEntity["new_monthvisits_def"] = contract.NumOfVisits * 4;

            //  contractEntity["new_totalvisits_def"] = contract.NumOfVisits * contract.ContractDuration; MIB Must be int
            contractEntity["new_totalvisits_def"] = (int)Math.Round(totalPlusExtraVisits, 0);

            contractEntity["new_hindivcount"] = contract.NumOfHours;
            contractEntity["new_discount_def"] = hourlyPricingCost.Discount;
            contractEntity["new_totalprice_def"] = (int)Math.Round(hourlyPricingCost.TotalPriceAfterPromotion, 0);   //decimal value string 180 = 180m as string not using parse
            contractEntity["new_vatrate"] = hourlyPricingCost.VatRate / 100m;
            contractEntity["new_vatamount"] = hourlyPricingCost.VatAmount;
            contractEntity["new_finalprice"] = hourlyPricingCost.TotalPriceWithVat;

            contractEntity["new_onevisitprice"] = hourlyPricingCost.TotalPriceWithVat / (((int)Math.Round(totalPlusExtraVisits, 0)) * contract.NumOfWorkers);

            return contractEntity;
        }

        public string GetLatestNotPaidContract(string crmUserId)
        {
            string sql = string.Format(@"select top 1 {0}  from 
{1} where new_hindivclintname = N'{2}' and statuscode = N'100000006'
order by createdon desc", CrmGuidFieldName,CrmEntityName,crmUserId);


            return Convert.ToString(CRMAccessDB.ExecuteScalar(sql));
        }

        public IEnumerable<BaseQuickLookup> GetLookups_DistrictNationalities(string districtId, UserLanguage language)
        {
            string displayField = (language == UserLanguage.Arabic ? "name" : "NameEnglish");

            string sql = @"SELECT distinct country.new_CountryId CountryId ,country.new_name name,country.new_code code,country.new_isocode isocode,
                        country.new_NameEnglish NameEnglish,country.new_axcode axcode ,country.versionnumber  
                        from new_CountryBase country, new_hourlypricingBase hourPrice,new_carresource,new_district,new_district_carresource
                        where country.new_CountryId =hourPrice.new_nationality
						and new_carresource.new_carresourceId=new_district_carresource.new_carresourceid and new_district.new_districtId=new_district_carresource.new_districtId
						and new_district.new_districtId='@id'  order by country.new_name";

            sql = sql.Replace("@id", districtId);
            return CRMAccessDB.SelectQ(sql).Tables[0].AsEnumerable().Select(dataRow => new BaseQuickLookup(dataRow["CountryId"].ToString(), dataRow[displayField].ToString()));
        }

        public IEnumerable<BaseQuickLookup> GetLookups_AvailableCities(UserLanguage language)
        {
            string displayField = (language == UserLanguage.Arabic ? "new_name" : "new_englsihName");

            string sql = String.Format(@"SELECT distinct city.new_CityId CityId, city.{0} Name, city.versionnumber from new_CityBase city,new_districtBase 
where city.new_CityId=new_districtBase.new_cityid and (new_districtBase.new_days IS NOT NULL AND LEN(new_districtBase.new_days) > 0 
                        AND new_districtBase.new_shifts IS NOT NULL AND LEN(new_districtBase.new_shifts) > 0 )  order by city.{1} ", displayField, displayField);

            return CRMAccessDB.SelectQ(sql).Tables[0].AsEnumerable().Select(dataRow => new BaseQuickLookup(dataRow["CityId"].ToString(), dataRow["Name"].ToString()));
        }


        public void Dispose()
        {

        }

        internal IEnumerable<ServiceContractPerHour> GetUserHourlyContracts(string userId, string statusCode, UserLanguage Language)
        {
            //Get Customer Contracts
            string statusCondition = String.IsNullOrEmpty(statusCode) ? string.Empty : String.Format(" And  contract.statuscode = {0} ", statusCode);
            string optionSetGetValFn, otherLangOptionSetGetValFn;

            switch (Language)
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

            string query = String.Format(@"select contract.new_hindvcontractid,
	   contract.new_contractnumber,
	   hourlypricing.new_name as hourlypricingname,
	    Contract.new_finalprice,
        Contract.CreatedOn,
		Contract.new_contractmonth,
		contract.new_shift,
		Isnull(appointment.userrate,0) as userrate,
		appointment.nextappointment,
        Convert(date, contract.new_contractstartdate) as new_contractstartdate,
        contract.new_latitude,
		contract.new_longitude,
        Isnull({0}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth),{1}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth) ) as durationname,
        Contract.statuscode,
        Isnull({2}('statuscode','new_HIndvContract',Contract.statuscode),{3}('statuscode','new_HIndvContract',Contract.statuscode) ) as statusname
           -- contract.new_HIndivClintnameName, 
		  --contract.new_HIndivClintname,contract.new_cityName
		  --,contract.new_city,contract.new_districtName ,contract.new_district,
		  --contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
		  --contract.new_contractmonth,contract.new_selecteddays,Contract.new_selectedshifts,Contract.new_visitprice_def,
		  --hourlypricing.new_nationalityName,
		  --hourlypricing.new_hourlypricingId,hourlypricing.new_shift
		  from new_HIndvContract  contract inner join new_hourlypricing hourlypricing on contract.new_houlrypricing=hourlypricing.new_hourlypricingId 
		  left outer join (
		  select new_hourlyappointmentBase.new_servicecontractperhour, 
		  IIF(  Count(new_hourlyappointmentBase.new_rate) > 0, ROUND( Sum(Isnull(new_hourlyappointmentBase.new_rate,0)) / Count(new_hourlyappointmentBase.new_rate), 0) , null) as userrate,
		  max(new_hourlyappointmentBase.new_shiftstart) as nextAppointment
		  from new_hourlyappointmentBase
		  where new_hourlyappointmentBase.new_rate is not null and Convert (date, new_shiftstart) <= Convert(date,GetDate())
		  group by new_servicecontractperhour
		  ) as  appointment on new_servicecontractperhour= contract.new_HIndvContractId

		  where contract.new_HIndivClintname = '{4}'  {5}
  order by contract.new_ContractNumber desc", optionSetGetValFn, otherLangOptionSetGetValFn, optionSetGetValFn, otherLangOptionSetGetValFn, userId, statusCondition);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];

            return dt.AsEnumerable().Select(dataRow => new ServiceContractPerHour(dataRow, Language));


            //List<ServiceContractPerHour> List = new List<ServiceContractPerHour>();

            //string ContractIds = "";
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    List.Add(new ServiceContractPerHour()
            //    {

            //        ContractId = dt.Rows[i][0].ToString(),
            //        ContractNum = dt.Rows[i][1].ToString(),
            //        Customer = dt.Rows[i][2].ToString(),
            //        CustomerId = dt.Rows[i][3].ToString(),
            //        City = dt.Rows[i][4].ToString(),
            //        District = dt.Rows[i]["new_districtName"].ToString(),
            //        Nationality = dt.Rows[i]["new_nationalityName"].ToString(),
            //        HourlyPricing = new HourlyPricing()
            //        {
            //            Discount = dt.Rows[i]["new_discount_def"].ToString(),
            //            TotalPrice = dt.Rows[i]["new_totalprice_def"].ToString(),
            //            TotalVisit = dt.Rows[i]["new_totalvisits_def"].ToString(),
            //            MonthVisits = dt.Rows[i][10].ToString(),
            //            NoOfMonths = dt.Rows[i]["new_contractmonth"].ToString(),
            //            VisitPrice = dt.Rows[i]["new_visitprice_def"].ToString(),
            //            NationalityName = dt.Rows[i]["new_nationalityName"].ToString(),
            //            hourlypricingId = dt.Rows[i][17].ToString(),
            //            Name = dt.Rows[i]["new_name"].ToString(),
            //            TotalbeforeDiscount = ((decimal.Parse(dt.Rows[i]["new_visitprice_def"].ToString()) * int.Parse(dt.Rows[i]["new_monthvisits_def"].ToString()) * int.Parse(dt.Rows[i]["new_totalvisits_def"].ToString()) * 4).ToString())


            //        },
            //        SelectedDays = dt.Rows[i]["new_selecteddays"].ToString(),
            //        Shift = dt.Rows[i]["new_shift"].ToString() == "False" ? "Morning" : "Evening",

            //        HourlyAppointments = new List<HourlyAppointment>()
            //    });

            //    ContractIds += "'" + dt.Rows[i][0].ToString() + "'";

            //    if (i != dt.Rows.Count - 1)
            //        ContractIds += ",";

            //}

            //return List;
        }

        public string AddtBankTransferStatementAttachment(string id, string fileName)
        {
            Entity attachment = new Entity(CrmEntityNamesMapping.Attachment);
            attachment["new_attachmentid"] = new EntityReference(CrmEntityNamesMapping.ServiceContractPerHour, new Guid(id)); ;
            attachment["new_attachmentsid"] = Guid.NewGuid();
            attachment["new_attachmentype"] = new OptionSetValue((int)AttachmentTypes.FinancialRequest);
            attachment["new_name"] = fileName;

            var attachmentId = GlobalCode.Service.Create(attachment);
            return attachmentId.ToString();
        }

        public void UpdateContractStatusAfterUbloadingBankTransferStatement(string id)
        {
            Entity contract = new Entity(CrmEntityNamesMapping.ServiceContractPerHour);
            contract[CrmGuidFieldName] = new Guid(id);
            contract["statuscode"] = new OptionSetValue((int) ServiceContractPerHourStatus.PaymentIsPendingConfirmation);
            GlobalCode.Service.Update(contract);
        }

        public ServiceContractPerHour GetUserHourlyContractDetails(string userId, string contractId, UserLanguage Language)
        {

            string optionSetGetValFn, otherLangOptionSetGetValFn;

            switch (Language)
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

            string query = String.Format(@"select contract.new_hindvcontractid,
	   contract.new_contractnumber,
	   hourlypricing.new_name as hourlypricingname,
	    Contract.new_finalprice,
		Contract.new_contractmonth,
		Isnull(appointment.userrate,0) as userrate,
		appointment.nextappointment,
        contract.new_city,
        contract.new_cityName,
        contract.new_district,
        contract.new_districtName ,
        hourlypricing.new_nationalityName,
        hourlypricing.new_nationality,
		contract.new_selecteddays,
		contract.new_shift,
		contract.new_vatrate	as vatrate,


		contract.new_hoursnumber,
		contract.new_weeklyvisits,
		contract.new_totalprice_def as totalprice,
		contract.new_totalprice_def ,
		ISNULL(contract.new_vatrate,0) * 100 as new_vatrate,
		contract.new_vatamount,
		contract.new_discount_def,
		Round(contract.new_weeklyvisits * contract.new_hoursnumber * Contract.new_contractmonth * hourlypricing.new_hourprice,2,2) as totalPriceBeforeDiscount,
		contract.new_employeenumber,
        contract.new_latitude,
		contract.new_longitude,
        Convert(date, contract.new_contractstartdate) as new_contractstartdate,
        Isnull({0}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth),{1}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth) ) as durationname,
        Contract.statuscode,
  Contract.createdon,
        Isnull({2}('statuscode','new_HIndvContract',Contract.statuscode),{3}('statuscode','new_HIndvContract',Contract.statuscode) ) as statusname


    -- contract.new_HIndivClintnameName, 
		  --contract.new_HIndivClintname,
		  --contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
		  --contract.new_contractmonth,,Contract.new_selectedshifts,Contract.new_visitprice_def,
		  --
		  --hourlypricing.new_hourlypricingId,
		  from new_HIndvContract  contract inner join new_hourlypricing hourlypricing on contract.new_houlrypricing=hourlypricing.new_hourlypricingId 
		  left outer join (
		  select new_hourlyappointmentBase.new_servicecontractperhour, 
		  IIF(  Count(new_hourlyappointmentBase.new_rate) > 0, ROUND( Sum(Isnull(new_hourlyappointmentBase.new_rate,0)) / Count(new_hourlyappointmentBase.new_rate), 0) , null) as userrate,
		  max(new_hourlyappointmentBase.new_shiftstart) as nextAppointment
		  from new_hourlyappointmentBase
		  where Convert (date, new_shiftstart) <= Convert(date,GetDate())
		  group by new_servicecontractperhour
		  ) as  appointment on new_servicecontractperhour= contract.new_HIndvContractId

		  where contract.new_HIndivClintname = '{4}' and contract.new_hindvcontractid = '{5}'
  order by contract.new_ContractNumber desc", optionSetGetValFn, otherLangOptionSetGetValFn, optionSetGetValFn, otherLangOptionSetGetValFn, userId, contractId);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
            if (dt.Rows.Count == 0) return null;

            var contract = new ServiceContractPerHour(dt.Rows[0], Language);
            contract.HourlyPricingCost = new HourlyPricingCost(dt.Rows[0]);
            return contract;

        }

        public ServiceContractPerHour GetHourlyContractDetails(string contractId, UserLanguage Language)
        {

            string optionSetGetValFn, otherLangOptionSetGetValFn;

            switch (Language)
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

            string query = String.Format(@"select contract.new_hindvcontractid,
	   contract.new_contractnumber,
	   hourlypricing.new_name as hourlypricingname,
	    Contract.new_finalprice,
		Contract.new_contractmonth,
		Isnull(appointment.userrate,0) as userrate,
		appointment.nextappointment,
        contract.new_city,
        contract.new_cityName,
        contract.new_district,
        contract.new_districtName ,
        hourlypricing.new_nationalityName,
        hourlypricing.new_nationality,
		contract.new_selecteddays,
		contract.new_shift,
		contract.new_vatrate	as vatrate,


		contract.new_hoursnumber,
		contract.new_weeklyvisits,
		contract.new_totalprice_def as totalprice,
		contract.new_totalprice_def ,
		ISNULL(contract.new_vatrate,0) * 100 as new_vatrate,
		contract.new_vatamount,
		contract.new_discount_def,
		Round(contract.new_weeklyvisits * contract.new_hoursnumber * Contract.new_contractmonth * hourlypricing.new_hourprice,2,2) as totalPriceBeforeDiscount,
		contract.new_employeenumber,
        contract.new_latitude,
		contract.new_longitude,
        Convert(date, contract.new_contractstartdate) as new_contractstartdate,
        Isnull({0}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth),{1}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth) ) as durationname,
        Contract.statuscode,
  Contract.createdon,
        Isnull({2}('statuscode','new_HIndvContract',Contract.statuscode),{3}('statuscode','new_HIndvContract',Contract.statuscode) ) as statusname,
    contract.new_HIndivClintnameName,
    contact.mobilephone,
		  contract.new_HIndivClintname

		  --contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
		  --contract.new_contractmonth,,Contract.new_selectedshifts,Contract.new_visitprice_def,
		  --
		  --hourlypricing.new_hourlypricingId,
		  from new_HIndvContract  contract inner join new_hourlypricing hourlypricing on contract.new_houlrypricing=hourlypricing.new_hourlypricingId 
		  left outer join (
		  select new_hourlyappointmentBase.new_servicecontractperhour, 
		  IIF(  Count(new_hourlyappointmentBase.new_rate) > 0, ROUND( Sum(Isnull(new_hourlyappointmentBase.new_rate,0)) / Count(new_hourlyappointmentBase.new_rate), 0) , null) as userrate,
		  max(new_hourlyappointmentBase.new_shiftstart) as nextAppointment
		  from new_hourlyappointmentBase
		  where Convert (date, new_shiftstart) <= Convert(date,GetDate())
		  group by new_servicecontractperhour
		  ) as  appointment on new_servicecontractperhour= contract.new_HIndvContractId
        left outer join contact on contact.contactid = contract.new_HIndivClintname
		  where  contract.new_hindvcontractid = '{4}'
  order by contract.new_ContractNumber desc", optionSetGetValFn, otherLangOptionSetGetValFn, optionSetGetValFn, otherLangOptionSetGetValFn, contractId);

            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
            if (dt.Rows.Count == 0) return null;

            var contract = new ServiceContractPerHour(dt.Rows[0], Language);
            contract.HourlyPricingCost = new HourlyPricingCost(dt.Rows[0]);
            return contract;

        }

        public bool ConfirmTerms(string contractId)
        {
            var sql = String.Format(@"update new_hindvcontract
                    set new_agreeonconditions=1
                    where new_HIndvContractId='{0}'", contractId);
            var result = CRMAccessDB.ExecuteNonQuery(sql);
            return result > 0;

        }

        public bool PickCustomerLocation(PickCustomerLocation customerLocation)
        {
            try
            {
                if (!String.IsNullOrEmpty(customerLocation.ContractId))
                {
                    Entity entity = GlobalCode.Service.Retrieve(CrmEntityName, new Guid(customerLocation.ContractId), new ColumnSet("new_contracttype"));

                    if (!String.IsNullOrEmpty(customerLocation.Latitude))
                    {
                        entity["new_latitude"] = customerLocation.Latitude;
                        entity["new_longitude"] = customerLocation.Longitude;
                        entity["new_mapurl"] = "http://maps.google.com/maps?q=" + customerLocation.Latitude + "," + customerLocation.Longitude + "&z=15";

                        GlobalCode.Service.Update(entity);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return false;
        }


        //        public ServiceContractPerHour GetUserHourlyContractDetails(string userId, string contractId)
        //        {

        //            string optionSetGetValFn, otherLangOptionSetGetValFn;

        //            switch (Language)
        //            {

        //                case UserLanguage.Arabic:
        //                    optionSetGetValFn = "dbo.getOptionSetDisplay";
        //                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplayen";
        //                    break;
        //                default:
        //                    optionSetGetValFn = "dbo.getOptionSetDisplayen";
        //                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplay";
        //                    break;

        //            }

        //            string query = String.Format(@"select contract.new_hindvcontractid,
        //	   contract.new_contractnumber,
        //	   hourlypricing.new_name as hourlypricingname,
        //	    Contract.new_finalprice,
        //		Contract.new_contractmonth,
        //		Isnull(appointment.userrate,0) as userrate,
        //		appointment.nextappointment,
        //        contract.new_city,
        //        contract.new_cityName,
        //        contract.new_district,
        //        contract.new_districtName ,
        //        hourlypricing.new_nationalityName,
        //        hourlypricing.new_nationality,
        //		contract.new_selecteddays,
        //		contract.new_shift,
        //		contract.new_hoursnumber,
        //		contract.new_weeklyvisits,
        //		contract.new_totalprice_def as totalprice,
        //		ISNULL(contract.new_vatrate,0) * 100 as new_vatrate,
        //		contract.new_vatamount,
        //		contract.new_discount_def,
        //		Round(contract.new_weeklyvisits * contract.new_hoursnumber * Contract.new_contractmonth * hourlypricing.new_hourprice,2,2) as totalPriceBeforeDiscount,
        //		contract.new_totalprice_def,
        //		contract.new_employeenumber,
        //        contract.new_latitude,
        //		contract.new_longitude,
        //        Convert(date, contract.new_contractstartdate) as new_contractstartdate,
        //        Isnull({0}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth),{1}('new_contractmonth','new_HIndvContract',Contract.new_contractmonth) ) as durationname,
        //        Contract.statuscode,
        //        Isnull({2}('statuscode','new_HIndvContract',Contract.statuscode),{3}('statuscode','new_HIndvContract',Contract.statuscode) ) as statusname
        //
        //
        //    -- contract.new_HIndivClintnameName, 
        //		  --contract.new_HIndivClintname,
        //		  --contract.new_discount_def,contract.new_totalprice_def,contract.new_totalvisits_def,contract.new_monthvisits_def,
        //		  --contract.new_contractmonth,,Contract.new_selectedshifts,Contract.new_visitprice_def,
        //		  --
        //		  --hourlypricing.new_hourlypricingId,
        //		  from new_HIndvContract  contract inner join new_hourlypricing hourlypricing on contract.new_houlrypricing=hourlypricing.new_hourlypricingId 
        //		  left outer join (
        //		  select new_hourlyappointmentBase.new_servicecontractperhour, 
        //		  IIF(  Count(new_hourlyappointmentBase.new_rate) > 0, ROUND( Sum(Isnull(new_hourlyappointmentBase.new_rate,0)) / Count(new_hourlyappointmentBase.new_rate), 0) , null) as userrate,
        //		  max(new_hourlyappointmentBase.new_shiftstart) as nextAppointment
        //		  from new_hourlyappointmentBase
        //		  where Convert (date, new_shiftstart) <= Convert(date,GetDate())
        //		  group by new_servicecontractperhour
        //		  ) as  appointment on new_servicecontractperhour= contract.new_HIndvContractId
        //
        //		  where contract.new_HIndivClintname = '{4}' and contract.new_hindvcontractid = '{5}'
        //  order by contract.new_ContractNumber desc", optionSetGetValFn, otherLangOptionSetGetValFn, optionSetGetValFn, otherLangOptionSetGetValFn, userId, contractId);

        //            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
        //            if (dt.Rows.Count == 0) return null;

        //            var contract = new ServiceContractPerHour(dt.Rows[0], Language);
        //            contract.HourlyPricingCost = new HourlyPricingCost(dt.Rows[0]);
        //            return contract;

        //        }
    }
}