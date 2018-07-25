using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace NasAPI.Inferstructures
{
    public class AppleNotifications
    {

        public static void SendPushNotification( string andeoid,string body,string title)
        {

            try
            {

                string applicationID = "AIzaSyBmE940a7HJF_ITdaG_uFI9nW0TQGv38j0";

                string senderId = "465484536200";
                string tovalue = "/topics/global";
                if (!string.IsNullOrEmpty(andeoid))
                    tovalue = andeoid;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    notification = new
                    {
                        body = body,
                        title = title,
                        sound = "Enabled"

                    },
                   
                    to = tovalue,

                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            Console.WriteLine("Message sent: check the client device notification tray.");

        }
    }
}