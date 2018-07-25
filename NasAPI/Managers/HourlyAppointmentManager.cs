using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class HourlyAppointmentManager : BaseManager, IDisposable
    {
        public HourlyAppointmentManager()
            : base(CrmEntityNamesMapping.HourlyAppointment, "new_hourlyappointmentid")
        {
        }

        public void Dispose()
        {

        }

        public IEnumerable<HourlyAppointment> GetHourlyAppointments(string contractId,UserLanguage lang)
        {
            string functionToGetProblemsName = lang == UserLanguage.Arabic ? "getOptionSetDisplay" : "getOptionSetDisplayen";

            string SqlShifts = String.Format(@"select 
                                        new_hourlyappointmentId,
                                        new_servicecontractperhour,
                                        new_employeeName,
                                        new_employee,
                                        new_status,
                                        [dbo].[{0}]('new_status','new_hourlyappointment', new_hourlyappointment.new_status) as statusName,
                                        new_hourlyappointment.new_notes,
                                        dateadd(hh,3,new_shiftend) as new_shiftend ,
                                        dateadd(hh,3,new_shiftstart) as new_shiftstart ,
                                        dateadd(hh,3,new_actualshiftstart) new_actualshiftstart,
                                        dateadd(hh,3,new_actualshiftend) new_actualshiftend,
                                        Isnull( new_rate,0) as new_rate,
                                        new_carid,
                                        new_caridName
                                from new_hourlyappointment inner join new_HIndvContract on new_HIndvContract.new_HIndvContractId = new_hourlyappointment.new_servicecontractperhour
                                where new_hourlyappointment.new_servicecontractperhour = '{1}'
                                order by new_shiftstart", functionToGetProblemsName, contractId);

            var result = CRMAccessDB.SelectQ(SqlShifts).Tables[0].AsEnumerable().Select(dataRow => new HourlyAppointment(dataRow));

            return result;
        }
    }
}