using Microsoft.Xrm.Sdk;
using NasAPI.Inferstructures;
using NasAPI.Managers;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/IndivContract")] // en/api/IndivContract 
    //[ApiExplorerSettings(IgnoreApi = true)]

    public class IndivContractController : BaseApiController
    {
        IndivContractManager Manager;

        public IndivContractController()
        {
            Manager = new IndivContractManager();
        }


        ///    /api/IndivContract/GetPricingNationalities

        [Route("GetPricingNationalities")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<OptionList> GetPricingNationalities()
        {

            string SQL = @"select distinct new_indvprice.new_nationalityName,new_nationality
  from new_indvprice   where new_indvprice.new_publishedonweb =1

";
            /*    and new_indvprice.new_professionName like '%سائق خاص%'
  and new_indvprice.new_professionName like '%عاملة منزلية%'*/

            DataTable dt = CRMAccessDB.SelectQ(SQL).Tables[0];
            List<OptionList> List = new List<OptionList>();

            for (int i = 0; i < dt.Rows.Count; i++)
                List.Add(new OptionList()
                {
                    Id = dt.Rows[i]["new_nationality"].ToString(),
                    Name = dt.Rows[i]["new_nationalityName"].ToString(),
                });
            
            var index = List.FindIndex(x => x.Id.ToUpper() == "C9DA5D56-A54A-E311-8887-00155D010303");
            if(index !=-1)
            {

          
            var item = List[index];
            List[index] = List[0];
            List[0] = item;
            }

            return List; 
        }

        [Route("GetPricingNationalities/{type}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<OptionList> GetPricingNationalities(int type)
        {

            string SQL = @"select distinct new_indvprice.new_nationalityName,new_nationality
  from new_indvprice   where new_indvprice.new_publishedonweb =1

";
            if(type==3)
            SQL += @" and new_indvprice.new_professionName like '%سائق خاص%'";
            if(type==1)
            SQL += @" and new_indvprice.new_professionName like '%عاملة منزلية%'";


              if(type==5)
            SQL += @" and  new_indvprice.new_professionName like '%مربية%'";
            if(type==4)
                SQL += @"  and    new_indvprice.new_professionName like '%عامل منزلي%'";

             





  //          /*    and new_indvprice.new_professionName like '%عاملة منزلية%'
  //and new_indvprice.new_professionName like '%عاملة منزلية%'*/

            DataTable dt = CRMAccessDB.SelectQ(SQL).Tables[0];
            List<OptionList> List = new List<OptionList>();

            for (int i = 0; i < dt.Rows.Count; i++)
                List.Add(new OptionList()
                {
                    Id = dt.Rows[i]["new_nationality"].ToString(),
                    Name = dt.Rows[i]["new_nationalityName"].ToString(),
                });

            var index = List.FindIndex(x => x.Id.ToUpper() == "C9DA5D56-A54A-E311-8887-00155D010303");
            if (index != -1)
            {


                var item = List[index];
                List[index] = List[0];
                List[0] = item;
            }

            return List;
        }


        ///    /api/IndivContract/GetPricingByNationality?id=CC0DF838-292F-E311-B3FD-00155D010303
        [HttpGet]
        [Route("GetPricingByNationality/{type}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<IndivPricing> GetPricingByNationality(string id)
        {
            string SQL = @"select new_indvprice.new_indvpriceId,new_indvprice.new_pricename,
new_indvprice.new_nationalityName,new_indvprice.new_contractmonths,new_indvprice.new_monthlypaid,new_indvprice.new_periodamount
,new_indvprice.new_everymonth,new_indvprice.new_pricenumber,new_indvprice.new_pricetype,new_nationality,new_prepaid
 from new_indvprice
 where new_nationality='@id'";
            SQL = SQL.Replace("@id", id);

            DataTable dt = CRMAccessDB.SelectQ(SQL).Tables[0];
            List<IndivPricing> List = new List<IndivPricing>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List.Add(new IndivPricing()
                {
                    Id = dt.Rows[i]["new_indvpriceId"].ToString(),
                    Name = dt.Rows[i]["new_pricename"].ToString(),
                    Number = dt.Rows[i]["new_pricenumber"].ToString(),
                    NationalityName = dt.Rows[i]["new_nationalityName"].ToString(),
                    TypeId = dt.Rows[i]["new_pricetype"].ToString(),
                    TypeName = OptionsController.GetName("new_indvprice", "new_pricetype", 1025, dt.Rows[i]["new_pricetype"].ToString()),
                    ContractMonths = MathNumber.RoundDeciaml(dt.Rows[i]["new_contractmonths"].ToString()),
                    PeriodAmount = MathNumber.RoundDeciaml(dt.Rows[i]["new_periodamount"].ToString()),
                    EveryMonth = MathNumber.RoundDeciaml(dt.Rows[i]["new_everymonth"].ToString()),
                    MonthelyPaid = MathNumber.RoundDeciaml(dt.Rows[i]["new_monthlypaid"].ToString()),
                    PrePaid = MathNumber.RoundDeciaml(dt.Rows[i]["new_monthlypaid"].ToString()),

                });
            }
            return List; ;






        }
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string Create(int type,int months, string FullName,string Mobile,string City,string District,string Nationality,int who=1)
        {
				// select new_nationalityid,new_districtid,new_profrequiredid,new_cityid from lead
            //


            Entity Lead = new Entity("lead");
            Lead["new_nationalityid"] = new EntityReference("new_country", new Guid(Nationality));
            Lead["new_cityid"] = new EntityReference("new_city", new Guid(City));
            Lead["new_districtid"] = new EntityReference("new_district", new Guid(District));

            switch (type)
            {
                    //عاملة منزلية
                case  1:
                    {
                        Lead["new_profrequiredid"] = new EntityReference("new_profession", new Guid("39C7F260-292F-E311-B3FD-00155D010303"));

                        break;

                    }
                //عاملة ممرضة

                case 2:
                    {
                        Lead["new_profrequiredid"] = new EntityReference("new_profession", new Guid("5BC7F260-292F-E311-B3FD-00155D010303"));

                        break;

                    }
                //سائق  

                case 3:
                    {
                        Lead["new_profrequiredid"] = new EntityReference("new_profession", new Guid("BFCEF260-292F-E311-B3FD-00155D010303"));

                        break;

                    }
                //عامل منزلى  

                case 4:
                    {
                        Lead["new_profrequiredid"] = new EntityReference("new_profession", new Guid("37C7F260-292F-E311-B3FD-00155D010303"));

                        break;

                    }
                //مربية  
                case 5:
                    {
                        Lead["new_profrequiredid"] = new EntityReference("new_profession", new Guid("E5C6F260-292F-E311-B3FD-00155D010303"));

                        break;

                    }
                    
              
            }




            //firstname_c
            //mobilephone_cl
            //new_leadservicetype_cl
            Lead["firstname"] = FullName;
            Lead["mobilephone"] = Mobile;
            Lead["new_leadservicetype"] = new OptionSetValue(months);



         
            Guid LeadId = GlobalCode.Service.Create(Lead);
            return LeadId.ToString();






        }


        [Route("GetAvailableNumbers")]
        [ResponseType(typeof(IEnumerable<AvailableNumbers>))]
        public HttpResponseMessage GetAvailableNumbers()
        {
            var result = Manager.GetAvailableNumbers().ToList();
            return OkResponse<List<AvailableNumbers>>(result);
        }


        [Route("GetIndivPrices")]
        [ResponseType(typeof(IEnumerable<IndivPricing>))]
        public HttpResponseMessage GetIndivPrices(string nationalityId, string professionId)
        {
            var result = Manager.GetIndivPrices(nationalityId,professionId).ToList();
            return OkResponse<List<IndivPricing>>(result);
        }

        [Route("GetAvailableEmployees")]
        [ResponseType(typeof(IEnumerable<Employee>))]
        public HttpResponseMessage GetAvailableEmployees(string nationalityId, string professionId)
        {
            var result = Manager.GetAvailableEmployees(nationalityId, professionId).ToList();
            return OkResponse<List<Employee>>(result);
        }

    }






}
