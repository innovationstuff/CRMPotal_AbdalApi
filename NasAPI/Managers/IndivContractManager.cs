using NasAPI.Controllers.API;
using NasAPI.Inferstructures;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class IndivContractManager : BaseManager
    {

        public IEnumerable<AvailableNumbers> GetAvailableNumbers()
        {
            string SQL = @"select employee.new_nationalityId,employee.new_nationalityIdName as new_nationalityName ,employee.new_professionId,employee.new_professionIdName as new_professionName,
	  count(distinct employee.new_EmployeeId) as availablecount from new_employee employee
	  inner join new_indvprice on new_nationality = employee.new_nationalityId
and new_profession = employee.new_professionId  
              where employee.statecode = 0
              and employee.new_employeetype = 3
              and  employee.statuscode in(279640012,1,279640001,279640000)
              and employee.new_indivcontract is null
			  and new_indvprice.statecode = 0 and new_indvprice.new_forweb = 1
	          group by employee.new_nationalityId,employee.new_nationalityIdName,employee.new_professionId,employee.new_professionIdName";
            DataTable dt = CRMAccessDB.SelectQ(SQL).Tables[0];
            List<AvailableNumbers> List = new List<AvailableNumbers>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!List.Any(a => a.Nationality == dt.Rows[i]["new_nationalityName"].ToString()))
                {
                    AvailableNumbers n = new AvailableNumbers();
                    n.ProfessionCounts = new List<ProfessionCount>();
                    n.Nationality = dt.Rows[i]["new_nationalityName"].ToString();
                    n.NationalityId = dt.Rows[i]["new_nationalityId"].ToString();
                    n.ProfessionCounts.Add(new ProfessionCount()
                    {
                        Profession = dt.Rows[i]["new_professionName"].ToString(),
                        ProfessionId = dt.Rows[i]["new_professionId"].ToString(),
                        Count = dt.Rows[i]["availablecount"].ToString(),
                    });
                    List.Add(n);
                }
                else
                {
                    List.FirstOrDefault(a => a.Nationality == dt.Rows[i]["new_nationalityName"].ToString()).ProfessionCounts.Add(new ProfessionCount()
                    {
                        Profession = dt.Rows[i]["new_professionName"].ToString(),
                        ProfessionId = dt.Rows[i]["new_professionId"].ToString(),
                        Count = dt.Rows[i]["availablecount"].ToString(),
                    });
                }
            }

            return List;
        }

        public IEnumerable<IndivPricing> GetIndivPrices(string nationalityId, string professionId)
        {

            string SQL = @"select distinct  * from new_indvprice 
where
new_nationality = '@nationalityId'
and new_profession = '@professionId' and statecode = 0 and new_forweb = 1";
            SQL = SQL.Replace("@nationalityId", nationalityId);
            SQL = SQL.Replace("@professionId", professionId);

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
                    NationalityId = nationalityId,
                    ProfessionId = professionId,

                });
            }
            return List;
        }

        public IEnumerable<Employee> GetAvailableEmployees(string nationalityId, string professionId)
        {

            string SQL = @"select employee.new_nationalityIdName as new_nationalityName ,employee.new_professionIdName as new_professionName ,
candidate.new_cancareold
      ,candidate.new_cancook
      ,candidate.new_candowithchildren
      ,candidate.new_canspeakarabic
      ,candidate.new_canspeakenglish
,employee.* from new_employee employee, new_candidate candidate
where 
employee.new_CandidateIld = candidate.new_candidateId
and employee.new_nationalityId = '@nationalityId'
and employee.new_professionId = '@professionId'
and employee.statecode = 0
and employee.new_employeetype = 3
    and  employee.statuscode in(279640012,1,279640001,279640000)
      and employee.new_indivcontract is null";
            SQL = SQL.Replace("@nationalityId", nationalityId);
            SQL = SQL.Replace("@professionId", professionId);

            DataTable dt = CRMAccessDB.SelectQ(SQL).Tables[0];
            List<Employee> List = new List<Employee>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List.Add(new Employee()
                {
                    Name = dt.Rows[i]["new_name"].ToString(),
                    IdNumber = dt.Rows[i]["new_empidnumber"].ToString(),
                    EmployeeId = dt.Rows[i]["new_idnumber"].ToString(),
                    JobNumber = dt.Rows[i]["new_EmployeeId"].ToString(),
                    JobTitle = dt.Rows[i]["new_professionName"].ToString(),
                    Nationality = dt.Rows[i]["new_nationalityName"].ToString(),
                    Region = dt.Rows[i]["new_religion"].ToString(),
                    Skills = (dt.Rows[i]["new_cancook"].ToString() == "True" ? "تجيد الطبخ والتنظيف " : "") + (dt.Rows[i]["new_candowithchildren"].ToString() == "True" ? "/n تجيد معاملة الأطفال " : "") + (dt.Rows[i]["new_cancareold"].ToString() == "True" ? " /n تجيد معاملة كبار السن" : "") + (dt.Rows[i]["new_canspeakarabic"].ToString() == "True" ? " /n تجيد اللغة العربية" : "") + (dt.Rows[i]["new_canspeakenglish"].ToString() == "True" ? " /n تجيد اللغة الإنجليزية" : ""),
                    Image = dt.Rows[i]["new_fullpicture"].ToString() == "" ? "Imagess/manmajhol.png" : "Images/" + dt.Rows[i]["new_fullpicture"].ToString(),

                });
            }
            return List;
        }


        public void CreateContract()
        {

        }

    }
}