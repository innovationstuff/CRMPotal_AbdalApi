using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/BusinessSector")] // en/api/BusinessSector
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BussinessController : ApiController
    {

        [HttpPost]
        public string Create(string Company, int sector, string Description, string phone, int who = 1)
        {
            // select new_nationalityid,new_districtid,new_profrequiredid,new_cityid from lead
            //


            Entity Lead = new Entity("lead");
            Lead["new_sector"] = new OptionSetValue(1);
            Lead["companyname"] = Company;
            Lead["firstname"] = " طلب قطاع اعمال لشركة" + Company;

            Lead["new_company_busienss"] = OptionsController.GetName("account", "industrycode", 1025, sector.ToString());
            // Lead["new_companysector"] = new OptionSetValue(sector);
            Lead["mobilephone"] = phone;
            Lead["description"] = Description;


            Guid id = GlobalCode.Service.Create(Lead);


            return id.ToString();


        }

        [HttpGet]
        public string CreateBussines(string email,string company,string comprep,string mobile,string city,string details, int who = 1)
        {
            //& company=& comprep=& mobile=& city=& details= & who = 1
            Entity PricingReq = new Entity("new_clientattraction");
            PricingReq["new_companyname"] = company;
            PricingReq["new_companyrespperson"] = comprep;
            PricingReq["new_respersonmobileno"] = mobile;
            PricingReq["new_cityid"] = new EntityReference("new_city", new Guid(city));
            PricingReq["new_requestdetails"] = details;
            PricingReq["new_respersonemail"] = email;
            Guid id = GlobalCode.Service.Create(PricingReq);
            return id.ToString();
        }

    }
}
