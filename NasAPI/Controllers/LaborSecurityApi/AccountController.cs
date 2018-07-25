using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Controllers.API;
using NasAPI.Filters;
using NasAPI.Identity;
using NasAPI.Models;
using NasAPI.Settings;
using NasAPI.Web.Helpers;
using NasAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.LaborSecurityApi
{
    //  [NasAuthorizationFilter]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix("{lang}/api/Account")]
    public class AccountController : BaseApiController
    {
        private AuthRepository _repo = null;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]

        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            ReturnData data = await _repo.RegisterUser(userModel);

            return OkResponse<ReturnData>(data );
        }

        [AllowAnonymous]
        [Route("VerifyCode")]
        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> VerifyCode(VerifyRegisterCodeViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(
            "<strong>Invalid Model</strong>",
            Encoding.UTF8,
            "text/html")
                };
            }

            ReturnData result = await _repo.VerifyCode(userModel);


            return OkResponse<ReturnData>(result);
        }

        [AllowAnonymous]
        [Route("Login")]
        [ResponseType(typeof(object))]
        public async Task<HttpResponseMessage> Login(LoginModel model)
        {

            string userMobile = model.UserName.RemoveWhiteSpace();
            ApplicationUser user = await _repo.GetUser(model);
            if (user != null && user.PhoneNumberConfirmed == true)
            {
                var request = HttpContext.Current.Request;
                var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/Token";
                using (var client = new HttpClient())
                {
                    var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", user.UserName),
                new KeyValuePair<string, string>("password", model.Password)
            };
                    var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    var responseCode = tokenServiceResponse.StatusCode;
                    var responseMsg = new HttpResponseMessage(responseCode)
                    {
                        Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                    };
                    if (responseCode == HttpStatusCode.OK)
                    {
                        return OkResponse<object>(new { Token = responseString, LoginData = model, User = user });
                    }
                    else
                    {
                        return OkResponse<object>(new { Token = responseString, LoginData = model });
                    }
                }
            }
            else
            {
                return OkResponse<ReturnData>(new ReturnData()
                {
                    State = false,
                    Data = new { Message = "the account is not confirmed" , User = user}
                });
            }
        }

        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> ForgotPassword(ForgotPasswordViewModel model, bool IsSMSEnabled)
        {
            if (ModelState.IsValid)
            {
                ReturnData code = await _repo.ForgotPassword(model, IsSMSEnabled, Language);
                return OkResponse<ReturnData>( code);
            }

            // If we got this far, something failed, redisplay form
            return NotFoundResponse("Error","Invalid Data");
        }

        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> ForgotPasswordConfirmation(ResetPasswordByPhoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                ReturnData result = await _repo.ForgotPasswordConfirmation(model);
                return OkResponse<ReturnData>(result);
            }
            return NotFoundResponse("Error", "Invalid Data");
        }

        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> ReGenrateCode(RegenrateCodeViewModel model, bool IsSMSEnabled)
        {
            if (string.IsNullOrEmpty(model.PhoneNumber))
                return NotFoundResponse("Error", "هناك خطأ فى اعادة توليد الرمز الجديد");
            ReturnData result = await _repo.ReGenrateCode(model, IsSMSEnabled, Language);
            return OkResponse<ReturnData>(result);
        }

        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ReturnData result = await _repo.ResetPassword(model);
                return OkResponse<ReturnData>(result);
            }
            return NotFoundResponse("Error", "Invalid Data");
        }

        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFoundResponse("Error", "Invalid Data");
            }
            ReturnData result = await _repo.ChangePassword(model);
            return OkResponse<ReturnData>(result);
          
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        [Authorize]
        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> GetProfile(string UserId,string CrmUserId)
        {
            ReturnData result = await _repo.GetProfile(UserId, Language, CrmUserId);
            return OkResponse<ReturnData>(result);
        }

        [Authorize]
        [ResponseType(typeof(ReturnData))]
        public async Task<HttpResponseMessage> EditProfile(string UserId, ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                ReturnData result = await _repo.EditProfile(UserId, model, Language);
                return OkResponse<ReturnData>(result);
            }
            return NotFoundResponse("Error", "Invalid Data");
        }
    }
}

#region commented

/*



        // GET api/account?username=&password=
        
          public IEnumerable<AccountModel> Get(string username, string password, string who = "1")
        {

            string sql = @" select fullname,new_IdNumer,mobilephone,emailaddress1,new_username,ContactId from Contact
where ( new_username='@username' or new_IdNumer='@username' ) and new_password='@password' ";

            sql = sql.Replace("@username", username);
            sql = sql.Replace("@password", password);
            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<AccountModel> AccountList = new List<AccountModel>();

            if (dt.Rows.Count > 0)
                AccountList.Add(new AccountModel()
                {
                    FullName = dt.Rows[0][0].ToString(),
                    IdNumber = dt.Rows[0][1].ToString(),
                    MobileNumber = dt.Rows[0][2].ToString(),
                    Email = dt.Rows[0][3].ToString()
                    ,
                    Username = dt.Rows[0][4].ToString(),
                    CustomerId = dt.Rows[0][5].ToString()
                });

            return AccountList;
        }





          [HttpGet]
          public IEnumerable<AccountModel> Login(string username, string password,string deviceid, string who = "1")
          {

              string sql = @" select fullname,new_IdNumer,mobilephone,emailaddress1,new_username,ContactId from Contact
where ( new_username='@username' or new_IdNumer='@username' ) and new_password='@password' ";

              sql = sql.Replace("@username", username);
              sql = sql.Replace("@password", password);
              System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
              List<AccountModel> AccountList = new List<AccountModel>();

              if (dt.Rows.Count > 0)
                  sql = @"update  Contact set new_deviceid='@deviceids' where ContactId='@id'";
                  sql = sql.Replace("@deviceids", deviceid);
                  sql = sql.Replace("@id", dt.Rows[0][5].ToString());
                 int i = CRMAccessDB.Update(sql);

                  AccountList.Add(new AccountModel()
                  {
                      FullName = dt.Rows[0][0].ToString(),
                      IdNumber = dt.Rows[0][1].ToString(),
                      MobileNumber = dt.Rows[0][2].ToString(),
                      Email = dt.Rows[0][3].ToString()
                      ,
                      Username = dt.Rows[0][4].ToString(),
                      CustomerId = dt.Rows[0][5].ToString()
                  });

              return AccountList;
          }
         [HttpGet] 
          //Get User Profile
          public IEnumerable<AccountModel> GetUserProfile(string id)
          {
              string sql = @" select fullname,new_IdNumer,mobilephone,emailaddress1,new_username,ContactId,new_contactnationalityName
		   ,new_gender from Contact
 where ContactId='@id' ";

              sql = sql.Replace("@id", id);
              System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
              List<AccountModel> AccountList = new List<AccountModel>();

              if (dt.Rows.Count > 0)
                  AccountList.Add(new AccountModel()
                  {
                      FullName = dt.Rows[0][0].ToString(),
                      IdNumber = dt.Rows[0][1].ToString(),
                      MobileNumber = dt.Rows[0][2].ToString(),
                      Email = dt.Rows[0][3].ToString()
                      ,
                      Username = dt.Rows[0][4].ToString(),
                      CustomerId = dt.Rows[0][5].ToString(),
                      Nationality = dt.Rows[0]["new_contactnationalityName"].ToString(),
                      Gender = dt.Rows[0]["new_gender"].ToString(),
                  });

              return AccountList;




          }


        // GET api/account/5

        // POST /api/account?FullName=basma&Nationality=3a822eeb-5f77-e311-8f5a-00155d010303&Username=Ahmed&IdNumber=1234&Password=123456&MobileNumber=01245678&Gender=2&Email=basma@yahoo.com",
        //string FullName,String Nationality,string IdNumber,string Password,string MobileNumber,bool? gender,string Email ,

        [HttpPost]

          public RegisterResult Register(string FullName, string Nationality, string IdNumber, string Password, string Username, string MobileNumber, int? Gender, string Email, int who = 1)
        {

            #region Erroes List
            RegisterResult regresult = new RegisterResult();
            regresult.Errors = new List<Errors>();




            var ISAccountWithUserName = CheckUserNameExist(Username);
            var ISAccountWithEmail = CheckEmailExist(Email);
            var ISAccountWithMobile = CheckMobileExist(MobileNumber);
            var ISAccountWithIdNumber = CheckIdNumberExist(IdNumber);
            int id = 1;

            if (ISAccountWithUserName == true)
            {
                regresult.Errors.Add(new Errors() { Id = id, Message = "Username Is Exist" });
                id++;
            }

            if (ISAccountWithEmail == true)
            {
                regresult.Errors.Add(new Errors() { Id = id, Message = "Email Is Exist" });

                id++;
            }
            if (ISAccountWithMobile == true)
            {
                regresult.Errors.Add(new Errors() { Id = id, Message = "Mobile Is Exist" });

                id++;
            }

            if (ISAccountWithIdNumber == true)
            {
                regresult.Errors.Add(new Errors() { Id = id, Message = "IDNumber Is Exist" });
                id++;
            }

            if (ISAccountWithUserName == true && ISAccountWithEmail == true && ISAccountWithIdNumber == true && ISAccountWithMobile == true)
            {
                regresult.Errors.RemoveAll(a => a.Id >= 1);
                regresult.Errors.Add(new Errors() { Id = 1, Message = "User Registered Before.Go To Forget Passowrd..." });
                return regresult;
            }
        

            if (ISAccountWithUserName == true )
                return regresult;


            if (  ISAccountWithIdNumber == true && ISAccountWithMobile == true)
            {
                //EMailAddress1='@Email'
                string Sql = @"select  ContactId from Contact where
		  new_IdNumer='@IDNumber'   and MobilePhone='@Mobile' and new_username is null and new_password is null ";

                Sql = Sql.Replace("@Email", Email);
                Sql = Sql.Replace("@Mobile", MobileNumber);
                Sql = Sql.Replace("@IDNumber", IdNumber);
                System.Data.DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];

                if(dt.Rows.Count>0)
                {
                    string Sqlupdate = @"update Contact set new_username='@username' , new_password='@pass',EMailAddress1='@Email'  where ContactId='@custID'";


                    Sqlupdate = Sqlupdate.Replace("@custID", dt.Rows[0][0].ToString());
                    Sqlupdate = Sqlupdate.Replace("@username", Username);
                    Sqlupdate = Sqlupdate.Replace("@pass", Password);
                    Sqlupdate = Sqlupdate.Replace("@Email", Email);

                    int res = CRMAccessDB.Update(Sqlupdate);
                    if (res > 0)
                    {
                             regresult.CustomerId = dt.Rows[0][0].ToString();
                             regresult.Errors.RemoveAll(a => a.Id >= 1);
                           return regresult;
                    }
                    return regresult;

                }



            }

            #endregion

            if (ISAccountWithUserName == true || ISAccountWithEmail == true || ISAccountWithIdNumber == true || ISAccountWithMobile == true)
            {
                return regresult;
            }


            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
            }


            Entity contact = new Entity("contact");

            var names = FullName.Split(' ');
            contact["firstname"] = FullName.Split(' ')[0];
            if (names.Length > 1)
                contact["lastname"] = FullName.Split(' ')[1];
            else
                contact["lastname"] = FullName.Split(' ')[0];

            contact["fullname"] = FullName;
            contact["new_idnumer"] = IdNumber;
            contact["mobilephone"] = MobileNumber;
            contact["emailaddress1"] = Email;
            contact["new_username"] = Username;
            contact["new_password"] = Password;
            contact["new_gender"] = Gender == 1 ? new OptionSetValue(1) : new OptionSetValue(2);
            contact["new_contactnationality"] = new EntityReference("new_country", new Guid(Nationality));


            Guid contactId = GlobalCode.Service.Create(contact);
            regresult.CustomerId = contactId.ToString();
            return regresult;


            #region Http Posted File Saved


            //if (httpPostedFile != null)
            //{
            //    // Validate the uploaded image(optional)

            //    // Get the complete file path
            //    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

            //    // Save the uploaded file to "UploadedFiles" folder
            //    httpPostedFile.SaveAs(fileSavePath);
            //}

            #endregion






        }


        [HttpGet]
        public bool HaveNameAndPass(string Mobile, string Email,String IDNumber)
        {

            string Sql = @"select  ContactId from Contact where
		  new_IdNumer='@IDNumber'  and EMailAddress1='@Email' and MobilePhone='@Mobile' and new_username is null and new_password is null ";

            Sql = Sql.Replace("@Email", Email);
            Sql = Sql.Replace("@Mobile", Mobile);
            Sql = Sql.Replace("@IDNumber", IDNumber);
            System.Data.DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];

            if (dt.Rows.Count > 0)
                return false;


            return true;
        }

        //Send Code 

        [HttpGet]
        public string SendCode(string EmailORMobile, int? type = 0, int who = 1)
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");

       
            string Sql = @"update   Contact set new_resetcode='@code'  where EMailAddress1='@Email'  or MobilePhone='@Email'";

            Sql = Sql.Replace("@Email", EmailORMobile);
            Sql = Sql.Replace("@code", r);
            int i = CRMAccessDB.Update(Sql);
          
            if (i > 0)
            {
                if (type == 0)
                {
                    Sql = @"select    MobilePhone from Contact where  new_resetcode='@code'  and ( EMailAddress1='@Email'  or MobilePhone='@Email')";

                    Sql = Sql.Replace("@Email", EmailORMobile);
                    Sql = Sql.Replace("@code", r);
                    System.Data.DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //Snd To SMS 
                        Entity SMS = new Entity("new_smsns");

                        string UserName = ConfigurationManager.AppSettings["SMSUserName"];
                        string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
                        string TagName = ConfigurationManager.AppSettings["TagName"];
                        SMSRef.SMSServiceSoapClient sms = new SMSRef.SMSServiceSoapClient();
                        string MobileNumber = dt.Rows[0][0].ToString();
                        string Message = r + ":الكود تغير كلمة السر";
                        string result = sms.SendBulkSMS(UserName, SMSPassword, TagName, MobileNumber, Message);

                        if (result == "1")
                            return EmailORMobile + "/لقد تم ارسال الكود الخاص بتغير كلمة السر الخاصة بك على الجوال التالى";
                        else
                            return "Error";
                    }
                    else
                    {
                        return "Error";
                    }


                }
                else
                {



                    if (!EmailORMobile.Contains('@'))
                        return "Error";
                    MailSender.SendEmail02(EmailORMobile, "", "الكود الخاص بتغير كلمة السر", r + "/الكود الخاص بتغير كلمة السر الخاصة بك", false,"");


                    return EmailORMobile + "/لقد تم ارسال الكود الخاص بتغير كلمة السر الخاصة بك على البريد التالى";
                }
            }
            else
            {
                return "Error";

            }



        }



        [HttpGet]
        public string CheckCode(string code, string EmailORMobile, int who = 1)
        {

            string Sql = @"select ContactId from Contact where new_resetcode ='@code' and 
(EMailAddress1='@mobile' or MobilePhone='@mobile')";


            Sql = Sql.Replace("@mobile", EmailORMobile);
            Sql = Sql.Replace("@code", code);

            System.Data.DataTable dt = CRMAccessDB.SelectQ(Sql).Tables[0];

            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();

            return "Error";
        }



        [HttpPost]
        public bool ResetPassword(string code, string EmailORMobile, string AccountId, string Password, int who = 1)
        {

            string Sql = @"update  Contact set new_password='@password'   where new_resetcode ='@code' and 
 (EMailAddress1='@mobile' or MobilePhone='@mobile') and ContactId='@AccountId'";


            Sql = Sql.Replace("@mobile", EmailORMobile);
            Sql = Sql.Replace("@code", code);
            Sql = Sql.Replace("@AccountId", AccountId);
            Sql = Sql.Replace("@password", Password);
            int result = CRMAccessDB.Update(Sql);

            if (result >= 1)
            {

               
                Random generator = new Random();
                String r = generator.Next(0, 1000000).ToString("D6");

                string SqlCode = @"update   Contact set new_resetcode='@code'  where  ContactId='@AccountId'";

                SqlCode = SqlCode.Replace("@AccountId", AccountId);
                SqlCode = SqlCode.Replace("@code", r);
                int i = CRMAccessDB.Update(SqlCode);

                return true;
            }
                


            return false;
        }


        [HttpPost]
        public account ResetPasswordandLogin(string code, string EmailORMobile, string AccountId, string Password, int who = 1)
        {

            string Sql = @"update  Contact set new_password='@password'   where new_resetcode ='@code' and 
 (EMailAddress1='@mobile' or MobilePhone='@mobile') and ContactId='@AccountId'";


            Sql = Sql.Replace("@mobile", EmailORMobile);
            Sql = Sql.Replace("@code", code);
            Sql = Sql.Replace("@AccountId", AccountId);
            Sql = Sql.Replace("@password", Password);
            int result = CRMAccessDB.Update(Sql);

            if (result >= 1)
            {


                Random generator = new Random();
                String r = generator.Next(0, 1000000).ToString("D6");

                string SqlCode = @"update   Contact set new_resetcode='@code'  where  ContactId='@AccountId'";

                SqlCode = SqlCode.Replace("@AccountId", AccountId);
                SqlCode = SqlCode.Replace("@code", r);
                int i = CRMAccessDB.Update(SqlCode);


                Sql=@"select new_username ,FullName from Contact  where  ContactId='@AccountId'";
                Sql = Sql.Replace("@AccountId", AccountId);
                DataTable dtnew=CRMAccessDB.SelectQ(Sql).Tables[0];
                return new account { fullname = dtnew.Rows[0]["FullName"].ToString(), username = dtnew.Rows[0]["new_username"].ToString(),accountid=AccountId.ToString(),password=Password };
            }



            return null;
        }



        [HttpGet]
        public bool CheckUserNameExist(string Username)
        {



            string sql = @" select fullname,new_IdNumer,mobilephone,emailaddress1,new_username,ContactId from Contact
where new_username='@username'";

            sql = sql.Replace("@username", Username);

            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];


            return dt.Rows.Count > 0;
        }


        [HttpGet]

        public bool CheckEmailExist(string Email)
        {



            string sql = @" select fullname,new_IdNumer,mobilephone,emailaddress1,new_username,ContactId from Contact
where EMailAddress1='@Email'";

            sql = sql.Replace("@Email", Email);

            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            return dt.Rows.Count > 0;

        }



        [HttpGet]

        public bool CheckIdNumberExist(string IdNumber)
        {



            string sql = @" select fullname,new_IdNumer,mobilephone,emailaddress1,new_username,ContactId from Contact
where new_IdNumer='@IdNumber'";

            sql = sql.Replace("@IdNumber", IdNumber);

            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            return dt.Rows.Count > 0;
        }

        [HttpGet]

        public bool CheckMobileExist(string MobileNumber)
        {



            string sql = @" select fullname,new_IdNumer,mobilephone,emailaddress1,new_username,ContactId from Contact
where MobilePhone='@MobileNumber'";

            sql = sql.Replace("@MobileNumber", MobileNumber);

            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];

            return dt.Rows.Count > 0;


        }




        public void Put()
        {
        }

        // DELETE api/account/5
        public void Delete(int id)
        {
        }
    }


    public class AccountModel
    {
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string IdNumber { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string CustomerId { get; set; }
        public string Gender { get; set; }

        public string Email { get; set; }

        //...other properties    
    }


    public class RegisterResult
    {
        public string CustomerId { get; set; }
        public List<Errors> Errors { get; set; }

    }


    public class Errors
    {

        public int Id { get; set; }
        public string Message { get; set; }

    }

    public class account
    {
        public string  username { get; set; }
        public string fullname { get; set; }
        public string accountid { get; set; }
        public string password { get; set; }

    }




}




    */

#endregion