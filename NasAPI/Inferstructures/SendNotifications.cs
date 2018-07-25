using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NasAPI.Inferstructures
{
    public class SendNotifications
    {
        //AIzaSyBmE940a7HJF_ITdaG_uFI9nW0TQGv38j0"
        public const string API_KEY = "AIzaSyBmE940a7HJF_ITdaG_uFI9nW0TQGv38j0";
       // public const string id = ",20";
       // public const string MESSAGE = "شكرا لاستخدامك تطبيق ناس";

       public static string SendNotify(string id, string MESSAGE)
        {
            var jGcmData = new JObject();
            var jData = new JObject();

            jData.Add("message", MESSAGE + id);
            jGcmData.Add("to", "/topics/global");
            jGcmData.Add("data", jData);

            string result = "";
            var url = new Uri("https://gcm-http.googleapis.com/gcm/send");
           
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization", "key=" + API_KEY);
                    Task.WaitAll(client.PostAsync(url,
                        new StringContent(jGcmData.ToString(), Encoding.UTF8, "application/json"))
                            .ContinueWith(response =>
                            {
                                return response;
                            }));
                }
                return result;
           
        }
    }
}