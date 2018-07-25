using NasAPI.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;
using NasAPI.Helpers;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http.Results;
using NasAPI.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using NasAPI.Settings;
using System.Web.Http.Description;
using NasAPI.Controllers.API;
using NasAPI.Identity;
using NasAPI.Managers;

namespace NasAPI
{
    public class AuthRepository : BaseApiController, IDisposable
    {
        private AuthContext _ctx;

        private ApplicationUserManager _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager =new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(_ctx));
        }

        public async Task<ReturnData> RegisterUser(UserModel userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                PhoneNumber = userModel.UserName,
                Email = userModel.Email,
                Name = userModel.FirstName + " " + userModel.LastName,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                //ViewBag.Link = callbackUrl;
                //return View("DisplayEmail");

                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user.Id, userModel.UserName.RemoveWhiteSpace());

                string ToEmails = userModel.Email;
                string CCEmail = "";
                string subject = " كود التفعيل الخاص بك علي موقع أبدال ";
                string body = "<div style='direction:rtl;'>";

                body += "<p><b>";
                body += " عزيزي العميل ";
                body += userModel.Name;
                body += "</b></p><br />";

                body += "<p>";
                body += " تم فتح سجل خاص بك بنجاح على موقع ايدال التوظيف و الاستقدام و الخدملت العمالية تحت اسم المستخدم : ";
                body += userModel.UserName.RemoveWhiteSpace();
                body += "</p>";

                body += "<p>";
                body += " الرجاء اتمام تفعيل سجلك بادخال رقم التفعيل ";
                body += "<b>";
                body += code.ToString();
                body += "</b>";
                body += " على الرابط التالي ";
                body += "&nbsp;<a href='https://abdal.sa/ar/Manage/VerifyPhoneNumberByUserId?PhoneNumber=" + userModel.UserName.RemoveWhiteSpace() + "&userId=" + user.Id + "'>موقع أبدال</a>&nbsp;";
                body += " و من ثم اتمام المعلوملت الخاصة بك. ";
                body += "</p>";

                body += "<p>";
                body += " شكرا لاختياركم شركة ابدال ";
                body += "</p>";

                body += "<p>";
                body += " ابدال،،،راحة بال،،، ";
                body += "</p><br />";

                body += "<p><b>";
                body += " الرجاء تجاهل الاميل في حال انه وصلكم عن طريق الخطأ ";
                body += "</b></p>";
                body += "</div>";

                try
                {
                    MailSender.SendEmail02(ToEmails, CCEmail, subject, body, true, "");
                }
                catch (Exception ex)
                {
                }
                return new ReturnData()
                {
                    State = true,
                    Data = new VerifyRegisterCodeViewModel()
                    {
                        Code = code,
                        Password = userModel.Password,
                        PhoneNumber = userModel.UserName,
                        UserId = user.Id
                    }
                };
            }
            return new ReturnData()
            {
                State = false,
                Data = result.Errors.First().ToString()
            };
    }

  
        public async Task<ReturnData> VerifyCode(VerifyRegisterCodeViewModel model)
        {

            var isValidCode = await _userManager.VerifyChangePhoneNumberTokenAsync(model.UserId, model.Code, model.PhoneNumber);
            if (isValidCode)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {

                    var crmUserEntity = new Contact()
                    {
                        ContactId = "0",
                        MobilePhone = user.PhoneNumber,
                        FullName = user.Name,
                        Email = user.Email
                    };
                    try
                    {
                        var crmUser = new ContactManager().RegisterContactInPortal(crmUserEntity);
                        var result = await _userManager.ChangePhoneNumberAsync(model.UserId, model.PhoneNumber, model.Code);
                        if (result.Succeeded)
                        {
                            user.CrmUserId = crmUser.ContactId;
                            await _userManager.UpdateAsync(user);
                        }
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
                            return new ReturnData()
                            {
                                State = true,
                                Data = new { code = responseString , user = user } 
                            };
                        }
                    }
                    catch (Exception)
                    {
                        return new ReturnData()
                        {
                            State = false,
                            Data = "Failed to save in CRM"
                        };
                    }
                    
                    
                  
                }
            }

            return new ReturnData()
            {
                State = false,
                Data = "Invalid code"
            };
        }

        public async Task<ReturnData> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ReturnData()
                {
                    State = false,
                    Data = "User Not Found"
                };
            }

            var result = await _userManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return new ReturnData()
                {
                    State = true,
                    Data = "Password Changed"
                };
            }

            return new ReturnData()
            {
                State = false,
                Data = result
            };
        }

        public async Task<ReturnData> ForgotPassword(ForgotPasswordViewModel model, bool IsSMSEnabled, UserLanguage Lang)
        {
            model.PhoneNumber = model.PhoneNumber.RemoveWhiteSpace();
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return new ReturnData()
                {
                    State = false,
                    Data = "user does not exist or is not confirmed"
                };
            }

            // var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user.Id, model.PhoneNumber);

            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
            // ViewBag.Link = callbackUrl;

            ResetPasswordByPhoneViewModel resetModel = new ResetPasswordByPhoneViewModel()
            {
                PhoneNumber = model.PhoneNumber,
                UserId = user.Id
            };
          
            if (IsSMSEnabled)
            {
                await SendSMSAsync(new IdentityMessage
                {
                    Body = "   من فضلك استخدم الكود الأتى فى اعادة تعين  كلمة المرور  " + code,
                    Destination = model.PhoneNumber
                }, Lang);
            }
            return new ReturnData()
            {
                State = true,
                Data = new { code = code }
            };

        }

        public async Task<ReturnData> EditProfile(string UserId , ContactViewModel model,UserLanguage Lang)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return new ReturnData()
                {
                    State = false,
                    Data = "User not found"
                }; 
            }
            Contact c = new Contact()
            {
                ContactId = model.ContactId,
                MobilePhone = model.MobilePhone,
                FullName = model.FullName,
                Email = model.Email,
                CityId = model.CityId,
                GenderId = model.GenderId,
                IdNumber = model.IdNumber.ToString(),
                NationalityId= model.NationalityId,
            };
            model.MobilePhone = model.MobilePhone.RemoveWhiteSpace();
            try
            {

            // save to crm
            var result = new ContactManager().RegisterContactInPortal(c); 

           
                //update user in our db
                user.UserName = model.MobilePhone;
                user.Name = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.MobilePhone;
                await this._userManager.UpdateAsync(user);
                return new ReturnData()
                {
                    State = true,
                    Data = new { user = result }
                };
            
            }
            catch (Exception)
            {
                return new ReturnData()
                {
                    State = false,
                    Data = "Failed to save in CRM"
                };
            }
            return new ReturnData()
            {
                State = false,
                Data = "Not valid"
            };
        }

        public async Task<ReturnData> GetProfile(string UserId, UserLanguage Lang, string CrmUserId)
        {
            var currentUser = _userManager.FindById(UserId);
            var result = await GetResourceAsync<dynamic>("api/contact/" + CrmUserId,Lang);
            var model = result == null ? new ContactViewModel() : result.ToObject<ContactViewModel>();
            return new ReturnData()
            {
                State = true,
                Data = model
            };
        }

        public async Task<ReturnData> ResetPassword(ResetPasswordViewModel model)
        {
            model.PhoneNumber = model.PhoneNumber.RemoveWhiteSpace();
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return new ReturnData()
                {
                    State = false,
                    Data = " The user does not exist"
                };
            }

            if (!await _userManager.VerifyChangePhoneNumberTokenAsync(user.Id, model.Code, model.PhoneNumber))
            {
                return new ReturnData()
                {
                    State = false,
                    Data = " invalid code resend again"
                };
            }
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("NasAPI"); _userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("PasswordReset")); 
            var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await _userManager.ResetPasswordAsync(user.Id, code, model.Password);
            if (result.Succeeded)
            {
                if (!user.PhoneNumberConfirmed)
                {
                    user.PhoneNumberConfirmed = true;
                    await this._userManager.UpdateAsync(user);
                }
                return new ReturnData()
                {
                    State = true,
                    Data = "Done"
                };
            }
            return new ReturnData()
            {
                State = false,
                Data = "couldn't reset the password"
            };
        }

        public async Task<ReturnData> ReGenrateCode(RegenrateCodeViewModel model, bool IsSMSEnabled, UserLanguage Lang)
        {
            model.PhoneNumber = model.PhoneNumber.RemoveWhiteSpace();
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(model.UserId, model.PhoneNumber);

            if (string.IsNullOrEmpty(code))
                return new ReturnData()
                {
                    State = false,
                    Data = "غير قادر على اعادة توليد الرمز الجديد"
                };

            if (IsSMSEnabled)
            {
                await SendSMSAsync(new IdentityMessage
                {
                    Body = "   من فضلك استخدم الكود الأتى فى اعادة تعين  كلمة المرور  " + code,
                    Destination = model.PhoneNumber
                },Lang);
                return new ReturnData()
                {
                    State = true,
                    Data = new { code = code }
                };
            }
            return new ReturnData()
            {
                State = true,
                Data = new { code = code }
            };
        }

        public async Task<ReturnData> ForgotPasswordConfirmation(ResetPasswordByPhoneViewModel model)
        {
            model.PhoneNumber = model.PhoneNumber.RemoveWhiteSpace();
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return new ReturnData()
                {
                    State = false,
                    Data = "هذا المستخدم غير موجود"
                };
            }

            if (!await _userManager.VerifyChangePhoneNumberTokenAsync(user.Id, model.Code, model.PhoneNumber))
            {
                return new ReturnData()
                {
                    State = false,
                    Data = " غير قادر على التحقق من الكود"
                };
            }
            ResetPasswordViewModel resetModel = new ResetPasswordViewModel()
            {
                PhoneNumber = model.PhoneNumber,
                Code = model.Code,
            };
            return new ReturnData()
            {
                State = true,
                Data = resetModel
            };
        }

        public async Task<bool> SendSMSAsync(Microsoft.AspNet.Identity.IdentityMessage message, UserLanguage Lang)
        {
            string url = string.Format("api/SMS/Send?Message={0}&MobileNumber={1}", message.Body, message.Destination);
            var result = await GetResourceMessageAsync<string>(url,Lang);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var isSent = Convert.ToInt32(result.Result) == 1;
                return isSent;
            }
            return false;
        }
        public async Task<APIResponseModel<T>> GetResourceMessageAsync<T>(string url, UserLanguage Lang, params string[] paramters)
        {
            using (var client = new HttpClient())
            {
                string apiServiceUrl = SharedClass.ApiServerUrl + (Lang == UserLanguage.English ? "en" : "ar") + "/";

                //Passing service base url  
                client.BaseAddress = new Uri(apiServiceUrl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var urlParams = new StringBuilder();

                foreach (string param in paramters)
                {
                    urlParams.Append(param + "/");
                }

                var paramList = urlParams.ToString();
                var fullUrl = string.IsNullOrEmpty(paramList) ? url : url + paramList;

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                var response = await client.GetAsync(fullUrl);
                var result = new APIResponseModel<T>();
                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var responseContent = await response.Content.ReadAsAsync<T>();
                    result.Result = responseContent;
                    result.StatusCode = response.StatusCode;
                    return result;
                }
                result.Result = default(T);
                result.StatusCode = response.StatusCode;
                //result.StatusMessage = response.CreateApiException();
                return result;
            }
        }

        public async Task<ApplicationUser> GetUser(LoginModel userModel)
        {
            string userMobile = userModel.UserName.RemoveWhiteSpace();
            ApplicationUser user = _userManager.Users.FirstOrDefault(t => t.PhoneNumber == userMobile);
           
            return user;
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<T> GetResourceAsync<T>(string url, UserLanguage Lang, params string[] paramters)
        {
            using (var client = new HttpClient())
            {
                string apiServiceUrl = SharedClass.ApiServerUrl + (Lang == UserLanguage.English ? "en" : "ar") + "/";

                //Passing service base url  
                client.BaseAddress = new Uri(apiServiceUrl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var urlParams = new StringBuilder();

                foreach (string param in paramters)
                {
                    urlParams.Append(param + "/");
                }

                var paramList = urlParams.ToString();
                var fullUrl = string.IsNullOrEmpty(paramList) ? url : url + paramList;

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                var response = await client.GetAsync(fullUrl);

                //Checking the response is successful or not which is sent using HttpClient  
                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                return default(T);
            }
        }

        //public async Task<APIResponseModel<T>> PostResourceAsync<T>(string url, object content, UserLanguage Lang)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        string apiServiceUrl = SharedClass.ApiServerUrl + (Lang == UserLanguage.English ? "en" : "ar") + "/";

        //        client.BaseAddress = new Uri(apiServiceUrl);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //Model will be serialized automatically.
        //        var response = await client.PostAsJsonAsync(url, content);
        //        var result = new APIResponseModel<T>();

        //        if (response.IsSuccessStatusCode)
        //        {
        //            //ReadAsAsync permits to deserialize the response content
        //            var responseContent = await response.Content.ReadAsAsync<T>();
        //            result.Result = responseContent;
        //            result.StatusCode = response.StatusCode;
        //            return result;
        //        }
        //        result.Result = default(T);
        //        result.StatusCode = response.StatusCode;
        //        //result.StatusMessage = response.CreateApiException();
        //        return result;
        //    }
        //}
        
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}