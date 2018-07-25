using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class NotificationManager
    {

        public HourlyAppointment GetContactIdByShiftId(string shiftId)
        {

            //            var contactId = CRMAccessDB.ExecuteScalar(@"select new_Contact from new_hourlyappointmentBase
            //inner join ContactBase on ContactBase.ContactId = new_hourlyappointmentBase.new_Contact
            //where new_hourlyappointmentId = N'" + shiftId + @"'");



            //            return contactId != null ? Convert.ToString(contactId) : string.Empty;


            var reader = CRMAccessDB.ExecuteReader(@"select new_hourlyappointmentId,
                                        new_servicecontractperhour,
										new_contractnumber,
										new_Contact,
                                        new_employee,
                                        new_status,
                                        dateadd(hh,3,new_shiftend) as new_shiftend ,
                                        dateadd(hh,3,new_shiftstart) as new_shiftstart ,
                                        dateadd(hh,3,new_actualshiftstart) new_actualshiftstart,
                                        dateadd(hh,3,new_actualshiftend) new_actualshiftend,
                                        Isnull( new_rate,0) as new_rate,
                                        new_carid
                                from new_hourlyappointmentBase inner join 	new_hindvcontract on 	new_hindvcontract.new_HIndvContractId = new_hourlyappointmentBase.new_servicecontractperhour
                                where new_hourlyappointmentBase.new_hourlyappointmentId = N'"+shiftId+@"'
                                order by new_shiftstart");

            HourlyAppointment hourlyAppointment = new HourlyAppointment();

            while (reader.Read())
            {
                hourlyAppointment.new_Contact = reader["new_Contact"].ToString();
                hourlyAppointment.ShiftStart = Convert.ToDateTime(reader["new_shiftstart"]);
                hourlyAppointment.ShiftEnd = Convert.ToDateTime(reader["new_shiftend"]);
                hourlyAppointment.new_contractnumber= Convert.ToString(reader["new_contractnumber"]);
            }
            return hourlyAppointment;




        }

        public HourlyAppointment GetContactIdByCarDeliveryOrderId(string carDeliveryOrderId)
        {

            //            var contactId = CRMAccessDB.ExecuteScalar(@"select new_Contact from new_hourlyappointmentBase
            //inner join ContactBase on ContactBase.ContactId = new_hourlyappointmentBase.new_Contact
            //where new_hourlyappointmentId = N'" + shiftId + @"'");



            //            return contactId != null ? Convert.ToString(contactId) : string.Empty;


            var reader = CRMAccessDB.ExecuteReader(@"select new_hourlyappointmentId,
                                        new_servicecontractperhour,
										new_contractnumber,
										new_Contact,
                                        new_employee,
                                        new_status,
                                        dateadd(hh,3,new_shiftend) as new_shiftend ,
                                        dateadd(hh,3,new_shiftstart) as new_shiftstart ,
                                        dateadd(hh,3,new_actualshiftstart) new_actualshiftstart,
                                        dateadd(hh,3,new_actualshiftend) new_actualshiftend,
                                        Isnull( new_rate,0) as new_rate,
                                        new_carid
                                from new_hourlyappointmentBase inner join 	new_hindvcontract on 	new_hindvcontract.new_HIndvContractId = new_hourlyappointmentBase.new_servicecontractperhour

								
							    inner join new_cardeliveryorder on new_cardeliveryorder.new_cardeliveryorderid = new_hourlyappointmentBase.new_cardeliveryorderhourlyappointmeId
								
								where new_cardeliveryorder.new_cardeliveryorderId = N'"+carDeliveryOrderId+@"' 
                                order by new_shiftstart
");

            HourlyAppointment hourlyAppointment = new HourlyAppointment();

            while (reader.Read())
            {
                hourlyAppointment.new_Contact = reader["new_Contact"].ToString();
                hourlyAppointment.ShiftStart = Convert.ToDateTime(reader["new_shiftstart"]);
                hourlyAppointment.ShiftEnd = Convert.ToDateTime(reader["new_shiftend"]);
                hourlyAppointment.new_contractnumber = Convert.ToString(reader["new_contractnumber"]);
            }
            return hourlyAppointment;




        }


        public void AddUserDevice(string userId, string deviceId)
        {
            int affectedRows = CRMAccessDB.ExecuteNonQueryLaboursdb(@"declare @countId INT 
set @countId = (select count([Id]) from [Devices] where [DeviceId] = N'" + deviceId + @"')
if @countId = 0 
begin

INSERT INTO [dbo].[Devices]
           ([DeviceId]
           ,[UserId]
           ,[IsOnline])
     VALUES
           (N'" + deviceId + @"'
           ,N'" + userId + @"'
           ,N'0')
end");
        }

        public List<Device> SelectAllDevices()
        {
            var dataSet = CRMAccessDB.SelectQlabourdb("select * from Devices");


            List<Device> target = dataSet.Tables[0].AsEnumerable()
                                      .Select(row => new Device
                                      {
                                          Id = row.Field<int>("Id"),
                                          DeviceId = row.Field<string>("DeviceId"),
                                          UserId = row.Field<string>("UserId"),
                                          IsOnline = row.Field<bool>("IsOnline"),

                                      }).ToList();


            return target;
        }

        public List<Device> SelectDevicesByCrmUserId(string crmUserId)
        {
            var dataSet = CRMAccessDB.SelectQlabourdb(@"select * from devices where userid in (select id from AspNetUsers where CrmUserId = N'"+crmUserId+"')" );


            List<Device> target = dataSet.Tables[0].AsEnumerable()
                                      .Select(row => new Device
                                      {
                                          Id = row.Field<int>("Id"),
                                          DeviceId = row.Field<string>("DeviceId"),
                                          UserId = row.Field<string>("UserId"),
                                          IsOnline = row.Field<bool>("IsOnline"),

                                      }).ToList();


            return target;
        }


    }
}