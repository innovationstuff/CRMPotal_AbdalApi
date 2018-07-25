//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace NasAPI.Controllers.API
//{
//    public class NotificationController : Controller
//    {
//        // GET: Notification
//        public ActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using Microsoft.AspNet.Identity;
using NasAPI.Controllers.API;
using NasAPI.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("{lang}/api/Notification")]
    public class NotificationController : BaseApiController
    {

        NotificationManager notificationManager;
        OneSignalLibrary.OneSignalClient client;
        public NotificationController()
        {
            notificationManager = new NotificationManager();
            client = new OneSignalLibrary.OneSignalClient(System.Configuration.ConfigurationManager.AppSettings["appKey"],
                                                        System.Configuration.ConfigurationManager.AppSettings["resetKey"],
                                                        System.Configuration.ConfigurationManager.AppSettings["userAuth"]);
        }



        [HttpPost]
        [Authorize]
        [Route("AddDevice/{deviceId}")]
        public IHttpActionResult AddDevice(string deviceId)
        {
            try
            {

                string userName = User.Identity.GetUserName();
                string userId = User.Identity.GetUserId();

                if (!string.IsNullOrEmpty(userId))
                {
                    notificationManager.AddUserDevice(userId, deviceId);

                    return Ok();
                }
                else
                {
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SendNotification/{message}")]
        public IHttpActionResult SendNotification(string message)
        {
            try
            {
                var Devices = notificationManager.SelectAllDevices();
                var DevicesList = Devices.ToList().Select(x => x.DeviceId);
                var RecepientDevices = new OneSignalLibrary.Posting.Device(new HashSet<string>(DevicesList));

                Dictionary<string, string> NotificationCOntent = new Dictionary<string, string>();
                //NotificationCOntent.Add("ar", arabicMessage);
                //NotificationCOntent.Add("message",arabicMessage);
                NotificationCOntent.Add("en", message);
                var content = new OneSignalLibrary.Posting.ContentAndLanguage(NotificationCOntent);

                try
                {
                    client.SendNotification(RecepientDevices, content, null, null);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private  string ToDateTimeString( DateTime? dateTime)
        {
            return dateTime?.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en")) ?? "";
        }

        [HttpGet]
        [Route("SendShiftNotification/{shiftId}")]
        public IHttpActionResult SendShiftNotification(string shiftId)
        {
            try
            {
                var hourlyAppointment = notificationManager.GetContactIdByShiftId(shiftId);
                var Devices = notificationManager.SelectDevicesByCrmUserId(hourlyAppointment.new_Contact);
                var DevicesList = Devices.ToList().Select(x => x.DeviceId);
                var RecepientDevices = new OneSignalLibrary.Posting.Device(new HashSet<string>(DevicesList));

                Dictionary<string, string> NotificationCOntent = new Dictionary<string, string>();
                var message = " يوجد لديك زيارة غداً بخصوص العقد رقم {0} من {1} الى {2} ";

                message = String.Format(message, hourlyAppointment.new_contractnumber
                    , hourlyAppointment.ShiftStart.HasValue ? hourlyAppointment.ShiftStart.Value.ToShortTimeString(): ""
                    , hourlyAppointment.ShiftEnd.HasValue ?  hourlyAppointment.ShiftEnd.Value.ToShortTimeString() : "");

                NotificationCOntent.Add("en", message);
                var content = new OneSignalLibrary.Posting.ContentAndLanguage(NotificationCOntent);

                try
                {
                    client.SendNotification(RecepientDevices, content, null, null);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SendCarDeliveryOrderNotification/{carDeliveryOrderId}/{message}")]
        public IHttpActionResult SendCarDeliveryOrderNotification(string carDeliveryOrderId,string message)
        {
            try
            {
                var hourlyAppointment = notificationManager.GetContactIdByCarDeliveryOrderId(carDeliveryOrderId);
                var Devices = notificationManager.SelectDevicesByCrmUserId(hourlyAppointment.new_Contact);
                var DevicesList = Devices.ToList().Select(x => x.DeviceId);
                var RecepientDevices = new OneSignalLibrary.Posting.Device(new HashSet<string>(DevicesList));

                Dictionary<string, string> NotificationCOntent = new Dictionary<string, string>();
                //var message = " يوجد لديك زيارة غداً بخصوص العقد رقم {0} من {1} الى {2} ";

                //message = String.Format(message, hourlyAppointment.new_contractnumber
                //    , hourlyAppointment.ShiftStart.HasValue ? hourlyAppointment.ShiftStart.Value.ToShortTimeString() : ""
                //    , hourlyAppointment.ShiftEnd.HasValue ? hourlyAppointment.ShiftEnd.Value.ToShortTimeString() : "");

                NotificationCOntent.Add("en", message);
                var content = new OneSignalLibrary.Posting.ContentAndLanguage(NotificationCOntent);

                try
                {
                    client.SendNotification(RecepientDevices, content, null, null);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
