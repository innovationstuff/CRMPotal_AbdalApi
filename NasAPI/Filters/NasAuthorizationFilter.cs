using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using System.Net.Http;
using System.Text;
using NasAPI.Inferstructures;

namespace NasAPI.Filters
{
    public class NasAuthorizationFilter
        :AuthorizationFilterAttribute
    {

     
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //Get Request Authontication Header
            var authHeader = actionContext.Request.Headers.Authorization;


            if (authHeader !=null)
            {
                if(authHeader.Scheme.Equals("basic",StringComparison.OrdinalIgnoreCase) &&
                   !string.IsNullOrWhiteSpace(authHeader.Parameter) )
                {

                    string s;
                    string url = actionContext.Request.RequestUri.AbsoluteUri;
                    url = Uri.UnescapeDataString(url);
                    if (url.Contains("?_="))
                    {
                       url= url.Replace("?_=", ">");
                        url = url.Split('>')[0].ToString();

                    }
                    if (url.Contains("&_="))
                    {
                        url = url.Replace("&_=", ">");
                        url = url.Split('>')[0].ToString();

                    }
                       

                    //url = Uri.UnescapeDataString(url);
                    s = url;
                    string res = GenerateSign.GetSignature(s);

                    var RawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");

                    var usernameandpasswordencoded = RawCredentials.Split('#')[0];
                    var signature = RawCredentials.Split('#')[1];
                    var credentials = encoding.GetString(Convert.FromBase64String(usernameandpasswordencoded));
                    var Split=credentials.Split(':');
                    //APIKEY=UGFzc05BU0FQSUBOYXNBUElVc2VyMTIzQFBhc3M6TmFzQVBJVXNlcjEyM0B1c2Vy#
                    var username = Split[1].ToString();
                    var password = Split[0].ToString();
                    if (username == "NasAPIUser123@user" && password == "PassNASAPI@NasAPIUser123@Pass" && signature==res)
                        return;


                }
            }
            HandelOnAuthorized(actionContext);
        }

         void HandelOnAuthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

        }
    }
}