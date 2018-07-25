using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class HourlyAppointment
    {

        public string Id { get; set; }
        public string StatusCode { get; set; }
        public string ContractId { get; set; }
        public string new_Contact { get; set; }
        public string new_contractnumber { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string Notes { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? ShiftStart { get; set; }
        public DateTime? ShiftEnd { get; set; }
        public DateTime? ActualShiftStart { get; set; }
        public DateTime? ActualShiftEnd { get; set; }
        public int? Rate { get; set; }
        public string CarName { get; set; }
        public string CarId { get; set; }
       

        public HourlyAppointment()
        {
                
        }

        public HourlyAppointment(DataRow dataRow)
        {
            this.Id = dataRow.Table.Columns.Contains("new_hourlyappointmentId") ? dataRow["new_hourlyappointmentId"].ToString() : null;
            this.ContractId = dataRow.Table.Columns.Contains("new_servicecontractperhour") ? dataRow["new_servicecontractperhour"].ToString() : null;
            this.EmpName = dataRow.Table.Columns.Contains("new_employeeName") ? dataRow["new_employeeName"].ToString() : null;
            this.EmpId = dataRow.Table.Columns.Contains("new_employee") ? dataRow["new_employee"].ToString() : null;
            this.Status = dataRow.Table.Columns.Contains("new_status") ? dataRow["new_status"].ToString() : null;
            this.StatusName = dataRow.Table.Columns.Contains("statusName") ? dataRow["statusName"].ToString() : null;
            this.Notes = dataRow.Table.Columns.Contains("new_notes") ? dataRow["new_notes"].ToString() : null;
            this.ShiftEnd = (dataRow.Table.Columns.Contains("new_shiftend") && dataRow["new_shiftend"] != DBNull.Value) ? (DateTime?)dataRow["new_shiftend"] : null;
            this.ShiftStart = (dataRow.Table.Columns.Contains("new_shiftstart") && dataRow["new_shiftstart"] != DBNull.Value) ? (DateTime?)dataRow["new_shiftstart"] : null;
            this.ActualShiftStart = (dataRow.Table.Columns.Contains("new_actualshiftstart") && dataRow["new_actualshiftstart"] != DBNull.Value) ? (DateTime?)dataRow["new_actualshiftstart"] : null;
            this.ActualShiftEnd = (dataRow.Table.Columns.Contains("new_actualshiftend") && dataRow["new_actualshiftend"] != DBNull.Value) ? (DateTime?)dataRow["new_actualshiftend"] : null;
            this.Rate = (dataRow.Table.Columns.Contains("new_rate") && dataRow["new_rate"] != DBNull.Value) ? (int?)dataRow["new_rate"] : null;
            this.CarName = dataRow.Table.Columns.Contains("new_caridName") ? dataRow["new_caridName"].ToString() : null;
            this.CarId = dataRow.Table.Columns.Contains("new_carid") ? dataRow["new_carid"].ToString() : null;
        }
    }
}