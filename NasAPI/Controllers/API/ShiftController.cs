using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{


    //[NasAuthorizationFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/Shift")] // en/api/Shift 
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShiftController : ApiController
    {

        //        // GET api/shift?CarId={CarID}&Day={Day}&ShiftType={shiftType}  //Get Locations
        //        public LocationsandRequests Get(string CarId, string Day, string ShiftType)
        //        {

        //            #region Get Contracts and Shifts


        //            System.Data.SqlClient.SqlCommand cmd = new SqlCommand();
        //            if (Day == "5555")
        //            {

        //                //housemade
        //                string sqlrange = @"set dateformat dmy;
        //SELECT        cust.YomiFullName AS 'Customer Name',Contract.new_shift as shifttime,shft.new_hourlyappointmentId as ShiftId, hourRes.new_resourcecode as 'Resource', cust.MobilePhone, Contract.new_ContractNumber AS 'Contract Number', Contract.new_mapurl AS 'Location'
        //,contract.new_shift shiftat,Shft.new_status shiftstatus,Shft.new_employee empId, CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftstart ),103)+' [ '+ FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftstart ) AS DATETIME),'hh:mm tt')+' ]'  ShiftStart,CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftend ),103)+' [ '+ FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftend ) AS DATETIME),'hh:mm tt')+' ]'  ShiftEnd 
        //,shft.new_notes as drivernotes,new_employee.new_IDNumber iqamanum,new_hourlypricing.new_nationalityName,new_hourlypricing.new_name,new_Country.new_NameEnglish
        //,new_Employee.new_name,Contract.statuscode,Contract.new_totalprice_def,Contract.new_HIndvContractId,isnull((select sum(new_amount) from new_receiptvoucher
        //
        //where new_receiptvoucher.new_contracthourid=Contract.new_HIndvContractId),0) as paidamount 
        //,dateadd(hh, 3, Shft.new_shiftstart ) as fullstartdate,dateadd(hh, 3, Shft.new_shiftend ) as fullenddate
        //,CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftstart ),103) as sdate,FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftstart ) AS DATETIME),'hh:mm tt') as stime
        //,CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftend ),103) as edate,FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftend ) AS DATETIME),'hh:mm tt') as etime,Contract.new_ispaid
        //,shft.new_actualshiftstart,shft.new_actualshiftend,new_Employee.new_EmpIdNumber
        //
        //FROM            new_hourlyappointmentBase AS shft INNER JOIN
        //                         new_HIndvContract AS Contract ON shft.new_servicecontractperhour = Contract.new_HIndvContractId INNER JOIN
        //                         new_carhourresourceBase ON shft.new_hourresource = new_carhourresourceBase.new_hourresource INNER JOIN
        //                         new_carresourceBase AS car ON new_carhourresourceBase.new_carresource = car.new_carresourceId LEFT OUTER JOIN
        //                         Contact AS cust ON cust.ContactId = Contract.new_HIndivClintname inner join new_hourresourceBase as hourRes 
        //						 on shft.new_hourresource = hourRes.new_hourresourceId left outer join new_Employee
        //						 on shft.new_employee=new_Employee.new_EmployeeId left outer join new_hourlypricing on Contract.new_houlrypricing =
        //						 new_hourlypricing.new_hourlypricingId left outer join new_Country on new_hourlypricing.new_nationality=new_Country.new_CountryId
        //
        //WHERE        (car.new_carresourceId = @CarId) AND (CONVERT(date, shft.new_shiftstart) >= CONVERT(date, @Date) or CONVERT(date, shft.new_shiftstart) <= CONVERT(date, @ToDate)) AND Contract.new_shift=@shftin    AND Contract.new_contractconfirm=1";



        //                DateTime From = DateTime.ParseExact(DateTime.Now.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                DateTime To = DateTime.ParseExact(DateTime.Now.Date.AddDays(7).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                string s = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //                cmd.CommandText = sqlrange;
        //                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = From;
        //                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = To;
        //                cmd.Parameters.Add("@CarId", SqlDbType.UniqueIdentifier).Value = new Guid(CarId);
        //                cmd.Parameters.Add("@shftin", SqlDbType.Int).Value = ShiftType;


        //            }
        //            else
        //            {
        //                DateTime datetosearch = DateTime.ParseExact(Day, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //                string sql = @"
        //set dateformat dmy;
        //SELECT        cust.YomiFullName AS 'Customer Name',Contract.new_shift as shifttime,shft.new_hourlyappointmentId as ShiftId, hourRes.new_resourcecode as 'Resource', cust.MobilePhone, Contract.new_ContractNumber AS 'Contract Number', Contract.new_mapurl AS 'Location'
        //,contract.new_shift shiftat,Shft.new_status shiftstatus,Shft.new_employee empId, CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftstart ),103)+' [ '+ FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftstart ) AS DATETIME),'hh:mm tt')+' ]'  ShiftStart,CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftend ),103)+' [ '+ FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftend ) AS DATETIME),'hh:mm tt')+' ]'  ShiftEnd 
        //,shft.new_notes as drivernotes,new_employee.new_IDNumber iqamanum,new_hourlypricing.new_nationalityName,new_hourlypricing.new_name,new_Country.new_NameEnglish
        //,new_Employee.new_name,Contract.statuscode,Contract.new_totalprice_def,Contract.new_HIndvContractId,isnull((select sum(new_amount) from new_receiptvoucher
        //
        //where new_receiptvoucher.new_contracthourid=Contract.new_HIndvContractId),0) as paidamount 
        //,dateadd(hh, 3, Shft.new_shiftstart ) as fullstartdate,dateadd(hh, 3, Shft.new_shiftend ) as fullenddate
        //,CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftstart ),103) as sdate,FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftstart ) AS DATETIME),'hh:mm tt') as stime
        //,CONVERT(VARCHAR(20),dateadd(hh, 3, Shft.new_shiftend ),103) as edate,FORMAT(CAST(dateadd(hh, 3, Shft.new_shiftend ) AS DATETIME),'hh:mm tt') as etime,Contract.new_ispaid
        //,shft.new_actualshiftstart,shft.new_actualshiftend,new_Employee.new_EmpIdNumber
        //FROM            new_hourlyappointmentBase AS shft INNER JOIN
        //                         new_HIndvContract AS Contract ON shft.new_servicecontractperhour = Contract.new_HIndvContractId INNER JOIN
        //                         new_carhourresourceBase ON shft.new_hourresource = new_carhourresourceBase.new_hourresource INNER JOIN
        //                         new_carresourceBase AS car ON new_carhourresourceBase.new_carresource = car.new_carresourceId LEFT OUTER JOIN
        //                         Contact AS cust ON cust.ContactId = Contract.new_HIndivClintname inner join new_hourresourceBase as hourRes 
        //						 on shft.new_hourresource = hourRes.new_hourresourceId left outer join new_Employee
        //						 on shft.new_employee=new_Employee.new_EmployeeId left outer join new_hourlypricing on Contract.new_houlrypricing =
        //						 new_hourlypricing.new_hourlypricingId left outer join new_Country on new_hourlypricing.new_nationality=new_Country.new_CountryId
        //WHERE        (car.new_carresourceId = @CarId) AND (CONVERT(date, shft.new_shiftstart) = CONVERT(date, @Date)) AND Contract.new_shift=@shftin  AND Contract.new_contractconfirm=1";

        //                cmd.CommandText = sql;
        //                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = datetosearch;
        //                cmd.Parameters.Add("@CarId", SqlDbType.UniqueIdentifier).Value = new Guid(CarId);
        //                cmd.Parameters.Add("@shftin", SqlDbType.Int).Value = ShiftType;


        //            }



        //            DataTable dt = CRMAccessDB.SelectQ(cmd).Tables[0];
        //            List<Location> List = new List<Location>();
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {

        //            List.Add(new Location()
        //            {
        //                empnum=dt.Rows[i]["new_EmpIdNumber"].ToString(),
        //                CustomerName = dt.Rows[i][0].ToString(),
        //                ShiftType = dt.Rows[i][1].ToString(),
        //                ShiftId = dt.Rows[i][2].ToString(),
        //                Resource = dt.Rows[i][3].ToString(),
        //                MobilePhone = dt.Rows[i][4].ToString(),
        //                ContractNumber = dt.Rows[i][5].ToString(),
        //                Url = dt.Rows[i][6].ToString(),
        //                ShiftAt = dt.Rows[i][7].ToString(),
        //                ShiftStatus = dt.Rows[i][8].ToString(),
        //                EmpId = dt.Rows[i][9].ToString(),
        //                ShiftStart = dt.Rows[i]["new_actualshiftstart"].ToString(),
        //                ShiftEnd = dt.Rows[i]["new_actualshiftend"].ToString(),
        //                DriverNotes = dt.Rows[i]["drivernotes"].ToString(),
        //                IqamaNumber = dt.Rows[i][13].ToString(),
        //                Nationality = dt.Rows[i][16].ToString(),
        //                housemade = dt.Rows[i][17].ToString(),
        //                StatusCode = dt.Rows[i]["statuscode"].ToString(),
        //                StatusName = dt.Rows[i]["new_ispaid"].ToString() == "False" ? "Not Paid" : "Paid",
        //                TotalPrice = dt.Rows[i]["new_totalprice_def"].ToString(),
        //                PaidAmount = Math.Round( decimal.Parse( dt.Rows[i]["paidamount"].ToString()),0,MidpointRounding.AwayFromZero).ToString(),
        //                paidneed = Math.Round((decimal.Parse(dt.Rows[i]["new_totalprice_def"].ToString()) - decimal.Parse(dt.Rows[i]["paidamount"].ToString())), 0, MidpointRounding.AwayFromZero).ToString()

        //                ,sdate = dt.Rows[i]["sdate"].ToString(),
        //                                stime = dt.Rows[i]["stime"].ToString(),
        //                                etime = dt.Rows[i]["etime"].ToString(),


        //            });
        //            }

        //            #endregion

        //            #region Housing Resource Order
        //            if (Day == "5555")
        //            {
        //                string SqlRangeGrouping = @"
        //set dateformat dmy;
        //SELECT        new_Country.new_NameEnglish,new_hourlypricing.new_name,Count(*) Count
        //FROM            new_hourlyappointmentBase AS shft INNER JOIN
        //                         new_HIndvContract AS Contract ON shft.new_servicecontractperhour = Contract.new_HIndvContractId INNER JOIN
        //                         new_carhourresourceBase ON shft.new_hourresource = new_carhourresourceBase.new_hourresource INNER JOIN
        //                         new_carresourceBase AS car ON new_carhourresourceBase.new_carresource = car.new_carresourceId LEFT OUTER JOIN
        //                         Contact AS cust ON cust.ContactId = Contract.new_HIndivClintname inner join new_hourresourceBase as hourRes 
        //						 on shft.new_hourresource = hourRes.new_hourresourceId left outer join new_Employee
        //						 on shft.new_employee=new_Employee.new_EmployeeId left outer join new_hourlypricing on Contract.new_houlrypricing =
        //						 new_hourlypricing.new_hourlypricingId left outer join new_Country on new_hourlypricing.new_nationality=new_Country.new_CountryId
        //WHERE        (car.new_carresourceId = @CarSelectedId) AND (CONVERT(date, shft.new_shiftstart) >= CONVERT(date, @ToDateTime) or CONVERT(date, shft.new_shiftstart) <= CONVERT(date, @ToDateTime)) AND Contract.new_shift=@shftTimein   AND Contract.new_contractconfirm=1
        // 
        //Group by new_Country.new_NameEnglish,new_hourlypricing.new_name
        //";



        //                DateTime FromDate = DateTime.ParseExact(DateTime.Now.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                DateTime Todate = DateTime.ParseExact(DateTime.Now.Date.AddDays(7).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                string s = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //                cmd.CommandText = SqlRangeGrouping;
        //                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
        //                cmd.Parameters.Add("@ToDateTime", SqlDbType.DateTime).Value = Todate;
        //                cmd.Parameters.Add("@CarSelectedId", SqlDbType.UniqueIdentifier).Value = new Guid(CarId);
        //                cmd.Parameters.Add("@shftTimein", SqlDbType.Int).Value = ShiftType;


        //            }
        //            else
        //            {
        //                DateTime datetosearch = DateTime.ParseExact(Day, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //                string SqlRangeGrouping = @"
        //set dateformat dmy;
        //SELECT        new_Country.new_NameEnglish,new_hourlypricing.new_name,Count(*) Count
        //FROM            new_hourlyappointmentBase AS shft INNER JOIN
        //                         new_HIndvContract AS Contract ON shft.new_servicecontractperhour = Contract.new_HIndvContractId INNER JOIN
        //                         new_carhourresourceBase ON shft.new_hourresource = new_carhourresourceBase.new_hourresource INNER JOIN
        //                         new_carresourceBase AS car ON new_carhourresourceBase.new_carresource = car.new_carresourceId LEFT OUTER JOIN
        //                         Contact AS cust ON cust.ContactId = Contract.new_HIndivClintname inner join new_hourresourceBase as hourRes 
        //						 on shft.new_hourresource = hourRes.new_hourresourceId left outer join new_Employee
        //						 on shft.new_employee=new_Employee.new_EmployeeId left outer join new_hourlypricing on Contract.new_houlrypricing =
        //						 new_hourlypricing.new_hourlypricingId left outer join new_Country on new_hourlypricing.new_nationality=new_Country.new_CountryId
        //WHERE        (car.new_carresourceId = @CarSelectedId) AND (CONVERT(date, shft.new_shiftstart) = CONVERT(date, @TheDate)) AND Contract.new_shift=@shftTimein   AND Contract.new_contractconfirm=1
        // Group by new_Country.new_NameEnglish,new_hourlypricing.new_name
        //
        //";

        //                cmd.CommandText = SqlRangeGrouping;
        //                cmd.Parameters.Add("@TheDate", SqlDbType.DateTime).Value = datetosearch;
        //                cmd.Parameters.Add("@CarSelectedId", SqlDbType.UniqueIdentifier).Value = new Guid(CarId);
        //                cmd.Parameters.Add("@shftTimein", SqlDbType.Int).Value = ShiftType;


        //            }



        //            DataTable dt2 = CRMAccessDB.SelectQ(cmd).Tables[0];
        //            List<Requests> ListOfNationalities = new List<Requests>();
        //            for (int i = 0; i < dt2.Rows.Count; i++) ListOfNationalities.Add(new Requests()
        //            {
        //                Count = int.Parse(dt2.Rows[i][2].ToString()),
        //                HourlyPricing = dt2.Rows[i][1].ToString(),
        //                Nationality = dt2.Rows[i][0].ToString()

        //            });



        //            #endregion




        //            LocationsandRequests result = new LocationsandRequests()
        //            {
        //                Locations = List,
        //                Request = ListOfNationalities
        //            };


        //            return result;
        //        }


        //        //Get All Avilable Days 
        //        //http://localhost:50554/api/shift?ContractStartDate=01/03/2017&NoOfMonths=10&Shift=false&DistrictId=34C98E70-37C0-E511-80FB-00505691216D&NationalityId=BE0EF838-292F-E311-B3FD-00155D010303
        //        public IEnumerable<avalibaleDays> Get(string ContractStartDate, int NoOfMonths, bool Shift, string DistrictId, string NationalityId)
        //        {

        //            DateTime startDate = DateTime.ParseExact(ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            DateTime EndDate = startDate.AddMonths(NoOfMonths);
        //            string shift = Shift == false ? "Morning" : "Evening";
        //            string sql = @"Select distinct DayName  from HourContractShifts('@ContractStartDate','@ContractEndDate','@districtid','@ShiftName',4)
        //                        where nationalityId='@nationality'
        //                        group by HourContractShifts.new_resourcecode,DayName 
        //                        having COUNT(*)>=@visitsCount order by DayName";
        //            // 
        //            sql = sql.Replace("@ContractStartDate", startDate.Date.ToString());
        //            sql = sql.Replace("@ContractEndDate", EndDate.Date.ToString());
        //            sql = sql.Replace("@districtid", DistrictId);
        //            sql = sql.Replace("@ShiftName", shift);
        //            sql = sql.Replace("@nationality", NationalityId);
        //            sql = sql.Replace("@visitsCount", (4 * NoOfMonths).ToString());
        //            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

        //            string sql2 = @"select new_days from new_district  where new_districtId='@id'";
        //            sql2 = sql2.Replace("@id", DistrictId);
        //            DataTable dtdistrict = CRMAccessDB.SelectQ(sql2).Tables[0];

        //            List<string> DaysOfWeek = dtdistrict.Rows[0][0].ToString().Split(',').ToList();


        //            //Days Of Weeks
        //            /*


        //           DaysOfWeek.Add("Monday");
        //           DaysOfWeek.Add("Tuesday");
        //           DaysOfWeek.Add("Wednesday");
        //           DaysOfWeek.Add("Thursday");
        //           DaysOfWeek.Add("Friday");
        //           DaysOfWeek.Add("Saturday");

        //            */


        //            //Avilable Days List
        //            List<string> List = new List<string>();
        //            for (int i = 0; i < dt.Rows.Count; i++) List.Add(dt.Rows[i][0].ToString());

        //            //UnAvilable Days
        //            List<string> UnAvilableDays = DaysOfWeek.Except(List).ToList();


        //            //allDays
        //            List<avalibaleDays> Days = new List<avalibaleDays>();
        //            if (List.Count != 0)
        //                for (int i = 0; i < List.Count; i++) Days.Add(new avalibaleDays() { Day = List[i].ToString(), Isavalibale = true });
        //            if (UnAvilableDays.Count != 0)
        //                for (int i = 0; i < UnAvilableDays.Count; i++) Days.Add(new avalibaleDays() { Day = UnAvilableDays[i].ToString(), Isavalibale = false });


        //            return Days;
        //        }


        //        public IEnumerable<string> GetDays(string ContractStartDate, string NoOfMonths, bool Shift, string DistrictId, string NationalityId, int countofdays,int empcount,int weeklyvisits,int hourscount,string cityid)
        //        {

        //            DateTime startDate;
        //            try
        //            {
        //                startDate = DateTime.ParseExact(ContractStartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        //            }
        //            catch (Exception)
        //            {

        //                startDate = DateTime.ParseExact(ContractStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //            }


        //            DateTime EndDate;

        //            EndDate = startDate.AddDays(7* int.Parse(NoOfMonths));
        //            string shift = Shift == false ? "Morning" : "Evening";
        //            #region commented

        //            /* string sql = @"Select distinct DayName  from HourContractShifts('@ContractStartDate','@ContractEndDate','@districtid','@ShiftName',4)
        //                         where nationalityId='@nationality'
        //                         group by HourContractShifts.new_resourcecode,DayName 
        //                         having COUNT(*)>=@visitsCount order by DayName";









        //            // 
        //            sql = sql.Replace("@ContractStartDate", startDate.Date.ToString());
        //            sql = sql.Replace("@ContractEndDate", EndDate.Date.ToString());
        //            sql = sql.Replace("@districtid", DistrictId);
        //            sql = sql.Replace("@ShiftName", shift);
        //            sql = sql.Replace("@nationality", NationalityId);
        //            if (months!=0)
        //            sql = sql.Replace("@visitsCount", (4 * months).ToString());
        //            else
        //                sql = sql.Replace("@visitsCount", (countofdays).ToString());

        //            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

        //            string sql2 = @"select new_days from new_district  where new_districtId='@id'";
        //            sql2 = sql2.Replace("@id", DistrictId);
        //            DataTable dtdistrict = CRMAccessDB.SelectQ(sql2).Tables[0];
        //            */
        //            #endregion
        //            string sql = @" 
        //with allSihfts as  (
        // select  ShiftFrom,  nationalityId,dayname,Count(distinct new_hourresourceId) SeatNo,count(distinct new_employeeid) as NoOfResources from
        //HourContractShiftsV02('@MinDate','@MaxDate','@districtid','@ShiftName','@nationality',@hoursCount,'@cityid')
        //                       --where Dayname in ('Sunday')
        //                    group by dayname,shiftfrom,nationalityId
        //					having Count(distinct new_hourresourceId)>=@EmpCount and count(distinct new_employeeid)>=@EmpCount
        //)
        //
        //select nationalityId,dayname,count(dayname) counts from allSihfts
        //group by nationalityId,dayname
        //having count(dayname) >=@visitsCount";

        //            sql = sql.Replace("@visitsCount", (countofdays / weeklyvisits).ToString());
        //            sql = sql.Replace("@EmpCount", (empcount).ToString());

        //            sql = sql.Replace("@hoursCount", hourscount.ToString());
        //            sql = sql.Replace("@cityid", cityid);


        //            sql = sql.Replace("@MinDate", startDate.AddHours(3).Date.ToString("MM/dd/yyyy"));
        //            sql = sql.Replace("@MaxDate", EndDate.Date.ToString("MM/dd/yyyy"));



        //            sql = sql.Replace("@districtid", DistrictId);

        //            //if (shift == "صباحي") ShiftName = "Morning";
        //            //if (ShiftName == "مسائي") ShiftName = "Evening";
        //            sql = sql.Replace("@ShiftName", shift);
        //            sql = sql.Replace("@nationality", NationalityId);
        //            // SqlCommand CMD = new SqlCommand(sql);
        //            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
        //            string avaDays = string.Empty;


        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                if (avaDays.IndexOf(dt.Rows[i]["DayName"].ToString()) == -1)
        //                    avaDays = avaDays + dt.Rows[i]["DayName"] + ",";
        //            }
        //            avaDays = avaDays.Remove(avaDays.Length - 1);


        //            List<string> DaysOfWeek = avaDays.Split(',').ToList();

        //            //Avilable Days List
        //            //List<string> List = new List<string>();
        //            //for (int i = 0; i < dt.Rows.Count; i++) List.Add(dt.Rows[i][0].ToString());

        //            return DaysOfWeek;
        //        }





        //        //Get All Avilable Nationalities With Price 
        //        //http://localhost:50554/api/shift?ContractStartDate=01/03/2017&NoOfMonths=10&Shift=false&DistrictId=34C98E70-37C0-E511-80FB-00505691216D
        //        public IEnumerable<NationalityPrice> Get(string ContractStartDate, int NoOfMonths, bool Shift, string DistrictId)
        //        {

        //            DateTime startDate = DateTime.ParseExact(ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            DateTime EndDate = startDate.AddMonths(NoOfMonths);
        //            string shift = Shift == false ? "Morning" : "Evening";
        //            int shifttype = Shift == false ? 0 : 1;

        //            string sql = @"Select distinct new_Country.new_name,new_hourlypricing.new_totalprice from HourContractShifts('@ContractStartDate','@ContractEndDate','@districtid','@ShiftName',4) HourContractShifts,new_Country,new_hourlypricing
        //where HourContractShifts.nationalityId=new_Country.new_CountryId and 
        //HourContractShifts.nationalityId=new_hourlypricing.new_nationality
        //and new_hourlypricing.new_shift= @shiftType 
        //                        order by new_name";
        //            sql = sql.Replace("@ContractStartDate", startDate.Date.ToString());
        //            sql = sql.Replace("@ContractEndDate", EndDate.Date.ToString());
        //            sql = sql.Replace("@districtid", DistrictId);
        //            sql = sql.Replace("@ShiftName", shift);
        //            sql = sql.Replace("@shiftType", shifttype.ToString());

        //            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];


        //            //Avilable Nationalities List
        //            List<NationalityPrice> List = new List<NationalityPrice>();
        //            for (int i = 0; i < dt.Rows.Count; i++) List.Add(new NationalityPrice() { Nationality = dt.Rows[i][0].ToString(), Price = dt.Rows[i][1].ToString() });
        //            return List;
        //        }



        //        [HttpPost]

        //        //change Shift Status
        //        public string UpdateShift(string ShiftId, string IqamaNumber, int StatusNumber, string NotesText)
        //        {



        //            Entity Shift = GlobalCode.Service.Retrieve("new_hourlyappointment", new Guid(ShiftId), new ColumnSet(false));



        //            string result = "";


        //            if (StatusNumber == 1 || StatusNumber == 2)
        //            {

        //                string sql = @"select new_HIndvContract.new_HIndivCount from new_hourlyappointment,new_HIndvContract
        //where 
        //new_HIndvContract.new_HIndvContractId=new_hourlyappointment.new_servicecontractperhour and
        //new_hourlyappointmentId='@id'";
        //                sql = sql.Replace("@id", ShiftId);
        //                DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

        //                string sqlemps = @"select new_Employee.new_EmployeeId,new_Employee.new_EmpIdNumber,new_Employee.new_IDNumber,new_Employee.new_name from new_Employee
        //where new_Employee.new_IDNumber='@iqama' or  new_Employee.new_EmpIdNumber='@iqama'";

        //                sqlemps = sqlemps.Replace("@iqama", IqamaNumber);
        //                DataTable dtsqlemps = CRMAccessDB.SelectQ(sqlemps).Tables[0];


        //                if (dtsqlemps.Rows.Count == 0)
        //                    throw new NotImplementedException();



        //                Shift["new_employee"] = new EntityReference("new_employee", new Guid(dtsqlemps.Rows[0]["new_EmployeeId"].ToString()));
        //                if (StatusNumber ==1)
        //                {

        //                    Shift["new_actualshiftstart"] = DateTime.Now;
        //                    int hours = 4;
        //                    bool res = int.TryParse(dt.Rows[0][0].ToString(), out hours);
        //                    Shift["new_actualshiftend"] = (DateTime.Now).AddHours(hours);
        //                }
        //                else
        //                {


        //                    Shift["new_actualshiftend"] = DateTime.Now;
        //                }





        //                result = "#" + dtsqlemps.Rows[0]["new_EmpIdNumber"].ToString() + "#"+ dtsqlemps.Rows[0]["new_IDNumber"].ToString() + "#"+ dtsqlemps.Rows[0]["new_name"].ToString() ;

        //            }




        //            Shift["new_status"] = new OptionSetValue(StatusNumber);
        //            Shift["new_notes"] = NotesText;


        //           // ((DateTime)Shift["new_actualshiftend"]).ToString("hh:mm tt")
        //            GlobalCode.Service.Update(Shift);
        //            if (StatusNumber == 1 )
        //                return ((DateTime)Shift["new_actualshiftend"]).ToString("hh:mm tt") + "#" + ((DateTime)Shift["new_actualshiftstart"]).ToString("hh:mm tt") + result;
        //                else if(StatusNumber == 2)
        //                return ((DateTime)Shift["new_actualshiftend"]).ToString("hh:mm tt") + "#" + "Ok" + result;

        //            else
        //                return "OK";

        //        }



        //        public string Post(string ShiftStrat, string ShiftEnd, string Name, string ContractNumber, string HourResourceCode, string EmployeeId)
        //        {

        //            Entity shift = new Entity("new_hourlyappointment");
        //            shift["new_name"] = Name;
        //            shift["new_shiftstart"] = DateTime.ParseExact(ShiftStrat, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            shift["new_shiftend"] = DateTime.ParseExact(ShiftEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //            Entity Contract = GlobalCode.GetOneEntityBy("new_hindvcontract", "new_contractnumber", ContractNumber);
        //            Entity HourResource = GlobalCode.GetOneEntityBy("new_hourresource", "new_resourcecode", HourResourceCode);
        //            shift["new_servicecontractperhour"] = new EntityReference("new_hindvcontract", new Guid(Contract.Id.ToString()));
        //            shift["new_hourresource"] = new EntityReference("new_hourresource", new Guid(HourResource.Id.ToString()));
        //            if (!string.IsNullOrEmpty(EmployeeId))
        //            {
        //                Entity Employee = GlobalCode.GetOneEntityBy("new_employee", "new_empidnumber", EmployeeId);
        //                shift["new_employee"] = new EntityReference("new_employee", new Guid(Employee.Id.ToString()));
        //            }
        //            shift["new_status"] = new OptionSetValue(0);
        //            Guid ShiftId = GlobalCode.Service.Create(shift);
        //            return ShiftId.ToString();


        //        }

    }


    public class NationalityPrice
    {
        public string Nationality { get; set; }
        public string Price { get; set; }


    }

    public class Location
    {
        public string empnum { get; set; }
        public string sdate { get; set; }
        public string stime { get; set; }
        public string etime { get; set; }
        public string paidneed { get; set; }
        public string PaidAmount { get; set; }
        public string contractsid { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string TotalPrice { get; set; }
        public string CustomerName { get; set; }
        public string ShiftType { get; set; }
        public string ShiftId { get; set; }
        public string Resource { get; set; }
        public string MobilePhone { get; set; }
        public string ContractNumber { get; set; }
        public string Url { get; set; }
        public string ShiftAt { get; set; }
        public string ShiftStatus { get; set; }
        public string EmpId { get; set; }
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public string DriverNotes { get; set; }
        public string IqamaNumber { get; set; }
        public string Nationality { get; set; }
        public string housemade { get; set; }




    }


    public class DriverData
    {
        public string Date { get; set; }
        public string CarId { get; set; }
        public string Day { get; set; }
        public string ShiftType { get; set; }
    }

    public class avalibaleDays
    {
        public string Day { get; set; }
        public bool Isavalibale { get; set; }
    }

    public class Requests
    {

        public string Nationality { get; set; }
        public string HourlyPricing { get; set; }

        public int Count { get; set; }
    }

    public class LocationsandRequests
    {
        public List<Location> Locations { get; set; }
        public List<Requests> Request { get; set; }

    }

}
