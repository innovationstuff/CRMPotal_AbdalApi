using Microsoft.Xrm.Sdk;
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
    [RoutePrefix("{lang}/api/Complaints")] // en/api/Complaints 
    [ApiExplorerSettings(IgnoreApi = true)]

    public class ComplaintsController : ApiController
    {
        [Route("GetUserHouseMades/{id}")]

        public IEnumerable<OptionList> GetUserHouseMades(string id)
        {
            string Sql = @"	select distinct  new_hourlyappointment.new_employeeName,new_hourlyappointment.new_employee 
						
						from new_hourlyappointment,new_HIndvContract
						 where 
						 new_hourlyappointment.new_servicecontractperhour=new_HIndvContract.new_HIndvContractId

						and new_hourlyappointment.new_employee is not null 
						 and new_HIndvContract.new_HIndivClintname='@id'";
            Sql = Sql.Replace("@id", id);

            DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];
            List<OptionList> List = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
                List.Add(new OptionList { Id = dt.Rows[i]["new_employee"].ToString(), Name = dt.Rows[i]["new_employeeName"].ToString() });

            return List;
        }


        [Route("GetUserContracts/{id}")]
        public IEnumerable<OptionList> GetUserContracts(string id, int type, int who = 1)
        {
            //قطاع الاعمال1
            //قطاع افراد2
            //عقود بالساعة4
            if(type==4)
            {

            
            string Sql = @"	select distinct  new_HIndvContract.new_HIndvContractId,'('+new_HIndvContract.new_ContractNumber+')/'+new_HIndvContract.new_houlrypricingName as name
						
						from new_HIndvContract
						where 
						new_HIndvContract.new_HIndivClintname='@id'";
            Sql = Sql.Replace("@id", id);

            DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];
            List<OptionList> List = new List<OptionList>();
            for (int i = 0; i < dt.Rows.Count; i++)
                List.Add(new OptionList { Id = dt.Rows[i]["new_HIndvContractId"].ToString(), Name = dt.Rows[i]["name"].ToString() });
           
            return List;
            }
else
            {
                List<OptionList> List = new List<OptionList>();
                return List;
            }
        }

        [HttpPost]
        public string Create(string Category, string Type, string Description, string CustomerNo, string HouseMadeId, string ContractId, int who = 1)   
        {

            ///TODO 1-  Get Customer DATA

            string sql = @"select contact.MobilePhone,contact.FullName,contact.YomiFullName from contact 
                           where contact.ContactId='@id'";

            sql = sql.Replace("@id", CustomerNo);
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            ///TODO 1-  Get Complaints Categories
            ///new_contracttype
            ///1-Bussiness Sector
            ///2-Indiv Sector
            ///4-HIndiv Sector

           ///TODO Categories
            ///new_problemcase
            ///100000005- Others
            ///6-تثبيت خدامة
            ///7-الغاء عقد
            ///8-تجديد عقد
            ///10-شكوى من خدامة

            Entity CutomerServices = new Entity("new_csindvsector");

            if (!string.IsNullOrEmpty(Category)) 

            CutomerServices["new_contracttype"] =new OptionSetValue(int.Parse(Category));
            if (!string.IsNullOrEmpty(Type)) 
            CutomerServices["new_problemcase"] = new OptionSetValue(int.Parse(Type));
           if(who <=1)
            CutomerServices["new_compalinsource"] = new OptionSetValue(who+1);

           if (!string.IsNullOrEmpty(ContractId)) 
            CutomerServices["new_cshindivcontractid"] = new EntityReference("new_hindvcontract", new Guid(ContractId.ToString()));

            string Complaintmessage="";
            //تثبيت خدامة 
            #region شكوى من خدامة او تثبيت خدامة
            if (Type == "6" || Type == "10")
            {
                string sqlhousemade = @"select new_Employee.new_EmpIdNumber Code,new_Employee.new_name as Name,new_employee.new_nationalityIdName Nationality
,new_Employee.new_IDNumber  Iqama
FROM            new_Employee  
where new_EmployeeId='@id'";

                sqlhousemade = sqlhousemade.Replace("@id", HouseMadeId);
                DataTable dthousemade = CRMAccessDB.SelectQ(sqlhousemade).Tables[0];

                if (Type == "6")
                    Complaintmessage = "طلب تثبيت الخدامة /";
                else
                    Complaintmessage = "شكوى الخدامة /";

              Complaintmessage += dthousemade.Rows[0]["Name"].ToString();
              Complaintmessage += "  ورقم اقامتها :  ";
              Complaintmessage += dthousemade.Rows[0]["Iqama"].ToString();
              Complaintmessage += "  ورقم الوظيفى  :  ";
              Complaintmessage += dthousemade.Rows[0]["Code"].ToString();
              Complaintmessage += "  وجنسية   :  ";
              Complaintmessage += dthousemade.Rows[0]["Nationality"].ToString();
              
              Complaintmessage += "       ";

            }
            
            #endregion
        
            Complaintmessage +="    ";
            Complaintmessage +=Description;

            CutomerServices["new_problemdetails"] = Complaintmessage;

          Guid id=  GlobalCode.Service.Create(CutomerServices);



          return id.ToString();
        }



        [Route("GetComplaintsDetails/{id}")]
        public IEnumerable<Complaint> GetComplaintsDetails(string id, string status)
        {
            string Sql = @"select new_name,new_contracttype,new_problemcase,new_compalinsource
,new_cshindivcontractid,new_problemdetails ,new_HIndvContract.new_HIndivClintnameName,new_HIndvContract.new_HIndivClintname
,dateadd  (hh,3,new_csindvsector.CreatedOn)
,CONVERT(VARCHAR(20),dateadd(hh, 3, new_csindvsector.CreatedOn ),103) as edate,FORMAT(CAST(dateadd(hh, 3, new_csindvsector.CreatedOn ) AS DATETIME),'hh:mm tt') as etime,new_csindvsector.statuscode
,new_HIndvContract.new_ContractNumber
from new_csindvsector,new_HIndvContract
where new_csindvsector.new_cshindivcontractid=new_HIndvContract.new_HIndvContractId
and new_HIndvContract.new_HIndivClintname='@id' and new_csindvsector.statuscode='@stat' order by new_csindvsector.CreatedOn desc";

         
            Sql = Sql.Replace("@id", id);
            Sql = Sql.Replace("@stat", status);

            DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];
            List<Complaint> List = new List<Complaint>();
            for (int i = 0; i < dt.Rows.Count; i++)
                List.Add(new Complaint {
                    
                    Code = dt.Rows[i]["new_name"].ToString(),
                    Category = OptionsController.GetName("new_csindvsector", "new_contracttype", 1025, dt.Rows[i]["new_contracttype"].ToString()),
                    Type = OptionsController.GetName("new_csindvsector", "new_problemcase", 1025, dt.Rows[i]["new_problemcase"].ToString()),
                    Date = dt.Rows[i]["edate"].ToString(),
                    Time = dt.Rows[i]["etime"].ToString(),
                    Description = dt.Rows[i]["new_problemdetails"].ToString(),
                    CustomerName = dt.Rows[i]["new_HIndivClintnameName"].ToString(),
                    Status = OptionsController.GetName("new_csindvsector", "statuscode", 1025, dt.Rows[i]["statuscode"].ToString()),
                    ContractNumber = dt.Rows[i]["new_ContractNumber"].ToString(),
                
                
                });

            return List;



        }




    }


    public class Complaint
    {
        public string ContractNumber { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Code { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; }
    }






}
