using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NasAPI.Controllers.API
{
        [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/SMS")] 


    public class SMSController : BaseApiController
    {
        [Route("Send")]
        [HttpGet]
        public HttpResponseMessage Send(string Message, string MobileNumber)
        {
            try
            {
                  //Snd To SMS 
                         Entity SMS = new Entity("new_smsns");
                          string UserName = ConfigurationManager.AppSettings["SMSUserName"];
                          string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
                          string TagName = ConfigurationManager.AppSettings["TagName"];
                          SMSRef.SMSServiceSoapClient sms = new SMSRef.SMSServiceSoapClient();                        
                          string result = sms.SendBulkSMS(UserName, SMSPassword, TagName, MobileNumber, Message);

                          if (result == "1")
                return OkResponse<string>(result);
                          else
                              return NotFoundResponse("Error in Sending SMS Try again PLZ ___________//","faild to send");

            }
            catch (Exception ex)
            {
                return NotFoundResponse("Error in Sending SMS Try again PLZ ___________//", ex.Message);
            }
        }
    }

}
