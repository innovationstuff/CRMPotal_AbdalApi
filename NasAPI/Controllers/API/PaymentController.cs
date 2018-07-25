using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Helpers;
using NasAPI.Managers;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/Payment")]
    public class PaymentController : BaseApiController
    {
        PaymentManager Manager;

        public PaymentController()
        {
            Manager = new PaymentManager();
        }

        public static string ToBase64(Stream PostedFile, string ContentType, bool DataUrl = false)
        {



            using (MemoryStream ms = new MemoryStream())
            {
                PostedFile.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();
                return (DataUrl ? "data:" + ContentType + ";base64," + Convert.ToBase64String(fileBytes, 0, (int)ms.Length) : Convert.ToBase64String(fileBytes, 0, (int)ms.Length));
            }


        }



        [HttpPost]
        [Route("AddPayment")]
        //this Action for car application for users payment 
        public decimal AddPayment(string carid, int paymenttype, string amount, string datatime, string contractid, string paymentcode)
        {




            string sql = @"select new_totalprice_def,new_HIndvContractId from new_HIndvContract where  new_ContractNumber='@contractid'";

            sql = sql.Replace("@contractid", contractid);
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];


            Entity Receipt = new Entity("new_receiptvoucher");

            Receipt["new_contracthourid"] = new EntityReference("new_hindvcontract", new Guid(dt.Rows[0][1].ToString()));
            Receipt["new_sourcecar"] = new EntityReference("new_carresource", new Guid(carid));



            if (paymenttype == 0)
            {
                Receipt["new_paymenttype"] = new OptionSetValue(6);
                Receipt["new_refrencenumber"] = paymentcode;
            }
            else
                Receipt["new_paymenttype"] = new OptionSetValue(3);

            Receipt["new_amount"] = new Money(decimal.Parse(dt.Rows[0][0].ToString()));
            Receipt["new_receiptdate"] = DateTime.ParseExact(datatime, "dd/MM/yyyy", null);



            Guid Id = GlobalCode.Service.Create(Receipt);


            string sqltotal = @"select isnull((select sum(new_amount) from new_receiptvoucher
where new_receiptvoucher.new_contracthourid='@id'),0) as paidamount from new_HIndvContract Contract ";

            sqltotal = sqltotal.Replace("@id", dt.Rows[0][1].ToString());
            DataTable dttable = CRMAccessDB.SelectQ(sqltotal).Tables[0];

            // new_ispaid
            if ((decimal.Parse(dt.Rows[0]["new_totalprice_def"].ToString()) - decimal.Parse(dttable.Rows[0][0].ToString())) <= 0)
            {
                Entity Contract = GlobalCode.Service.Retrieve("new_hindvcontract", new Guid(dt.Rows[0][1].ToString()), new ColumnSet(false));
                Contract["new_ispaid"] = true;
                Contract["new_contractconfirm"] = true;
                GlobalCode.Service.Update(Contract);

            }

            return (decimal.Parse(dt.Rows[0]["new_totalprice_def"].ToString()) - decimal.Parse(dttable.Rows[0][0].ToString()));



        }



        [HttpPost]
        [ResponseType(typeof(RecieptVoucherModel))]
        [Route("AddRecieptVoucher")]
        public HttpResponseMessage AddRecieptVoucher(RecieptVoucherModel Voucher)
        {
            Entity Receipt = new Entity("new_receiptvoucher");

            Receipt["new_contracthourid"] = new EntityReference("new_hindvcontract", new Guid(Voucher.contractid));
            Receipt["new_paymenttype"] = new OptionSetValue(2);
            Receipt["new_refrencenumber"] = Voucher.paymentcode;
            Receipt["new_amount"] = new Money(decimal.Parse(Voucher.amount));
            Receipt["new_vaterate"] = decimal.Parse(Voucher.vatrate);
            Receipt["new_vatamount"] = decimal.Parse(Voucher.amount) * decimal.Parse(Voucher.vatrate);
            Receipt["new_totalamountwithvat"] = new Money((decimal.Parse(Voucher.amount) * decimal.Parse(Voucher.vatrate)) + decimal.Parse(Voucher.amount));

            Receipt["new_receiptdate"] = DateTime.ParseExact(Voucher.datatime, "dd/MM/yyyy", null);
            Receipt["new_contactid"] = new EntityReference("contact", new Guid(Voucher.Customerid));
            Receipt["new_pointofreciept"] = new OptionSetValue(4);
            Receipt["new_source"] = new OptionSetValue(Voucher.who);

            if (Voucher.who == 2)
                Receipt["new_note"] = "تم انشاء السند عن طريق تحويل بنكى من الويب بورتال  للعقد رقم " + Voucher.Contractnumber;
            else
                Receipt["new_note"] = "تم انشاء السند عن طريق تحويل بنكى من  تطبيق الجوال  للعقد رقم  " + Voucher.Contractnumber;



            Guid Id = GlobalCode.Service.Create(Receipt);

            Entity Contract = GlobalCode.Service.Retrieve("new_hindvcontract", new Guid(Voucher.contractid), new ColumnSet(false));
            Contract["new_ispaid"] = true;
            Contract["new_contractconfirm"] = true;
            Contract["statuscode"] = new OptionSetValue(100000009);

            GlobalCode.Service.Update(Contract);

            Voucher.Voucherid = Id.ToString();
            return OkResponse(Voucher);



        }


        [HttpGet]
        [Route("SendVouchersToCRM")]
        //CRM Function that Connected to WorkFlow for creating Recieptvoucher with creating errors on transactions payment process with success transactions
        public string SendVouchersToCRM()
        {
            try
            {


                //1-Getting all RV that dident saved in CRM 
                string sql = @" select * from ReceiptVouchers where ReceiptVouchers.IsSaved=0";
                DataTable dt = CRMAccessDB.SelectQlabourdb(sql).Tables[0];
                if (dt.Rows.Count > 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Entity Receipt = new Entity("new_receiptvoucher");
                        Receipt["new_contracthourid"] = new EntityReference("new_hindvcontract", new Guid(dt.Rows[i]["ContractId"].ToString()));
                        Receipt["new_paymenttype"] = new OptionSetValue(2);
                        Receipt["new_refrencenumber"] = dt.Rows[i]["PaymentCode"].ToString();
                        Receipt["new_amount"] = new Money(decimal.Parse(dt.Rows[i]["Amount"].ToString()));
                        Receipt["new_vaterate"] = decimal.Parse(dt.Rows[i]["VatRate"].ToString());
                        Receipt["new_vatamount"] = decimal.Parse(dt.Rows[i]["Amount"].ToString()) * decimal.Parse(dt.Rows[i]["VatRate"].ToString());
                        Receipt["new_totalamountwithvat"] = new Money((decimal.Parse(dt.Rows[i]["Amount"].ToString()) * decimal.Parse(dt.Rows[i]["VatRate"].ToString())) + decimal.Parse(dt.Rows[i]["Amount"].ToString()));
                        Receipt["new_receiptdate"] = DateTime.Parse(dt.Rows[i]["Date"].ToString());
                        Receipt["new_contactid"] = new EntityReference("contact", new Guid(dt.Rows[i]["CustomerId"].ToString()));
                        Receipt["new_pointofreciept"] = new OptionSetValue(4);
                        Guid Id = GlobalCode.Service.Create(Receipt);
                        sql = @"update ReceiptVouchers set IsSaved=1 where Id=" + dt.Rows[i]["Id"].ToString();
                        CRMAccessDB.ExecuteNonQueryLaboursdb(sql);
                        Entity Contract = GlobalCode.Service.Retrieve("new_hindvcontract", new Guid(dt.Rows[i]["ContractId"].ToString()), new ColumnSet(false));
                        Contract["new_ispaid"] = true;
                        Contract["new_contractconfirm"] = true;
                        Contract["statuscode"] = new OptionSetValue(100000009);

                        GlobalCode.Service.Update(Contract);



                    }
            }
            catch (Exception e)
            {
                return "ok";
            }

            return "ok";
        }


        #region Individual 

        [HttpPost]
        [Route("Individual/UploadInvoiceBankFile/{0}")]
        public async Task<HttpResponseMessage> PostBankTransferStatementFile(string id)
        {

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = ConfigurationManager.AppSettings["UploadPath"].ToString();

            var provider = new MultipartFormDataStreamProvider(root);

            try
            {

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                var file = provider.FileData.FirstOrDefault();

                if (file == null)
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                if (String.IsNullOrEmpty(file.Headers.ContentDisposition.FileName))
                    throw new HttpResponseException(HttpStatusCode.Forbidden);

                string fileName = String.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(file.Headers.ContentDisposition.FileName).Replace(" ", "_").Replace("#", "")
                    , DateTime.Now.Ticks, Path.GetExtension(file.Headers.ContentDisposition.FileName));

                File.Move(file.LocalFileName, Path.Combine(root, fileName));


                var attachmentId = Manager.AddtDomesticInvoiceBankTransferAttachment(id, fileName);

                // update Domestic invoice to be reviewed by finance ::todo

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }




        [HttpPost]
        [ResponseType(typeof(RecieptVoucherModel))]
        [Route("Individual/AddRecieptVoucher")]
        public HttpResponseMessage AddRecieptVoucherForDomesticInvoice(RecieptVoucherModel Voucher)
        {

            using (var recieptVoucherMgr = new RecieptVoucherManager())
            {
                recieptVoucherMgr.CreateVoucherForDomesticInvoice(Voucher);
                Manager.PayDomesticInvoice(Voucher.InvoiceId);
                return OkResponse<RecieptVoucherModel>(Voucher);
            }
        }

        #endregion



        /////////// payment for mobile

        [HttpPost]
        public async Task<HttpResponseMessage> SystemicBankTransfer(string BankFileString, string BankFileName, string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiServiceUrl = SharedClass.ApiServerUrl + (Language == Settings.UserLanguage.English ? "en" : "ar") + "/api/HourlyContract/BankTransferStatementFile/" + id;

                    using (var content =
                        new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture)))
                    {
                        byte[] imageBytes = Convert.FromBase64String(BankFileString);
                        MemoryStream ms = new MemoryStream(imageBytes, 0,
                          imageBytes.Length);

                        content.Add(new StreamContent(ms), "BankFile", BankFileName);

                        using (
                           var message =
                               await client.PostAsync(apiServiceUrl, content))
                        {
                            var input = await message.Content.ReadAsStringAsync();
                            if (message.IsSuccessStatusCode)
                            {
                                var successMsg = new ResultMessageVM()
                                {
                                    Title = Language == Settings.UserLanguage.Arabic ? "تم رفع ملف التحويل البنكي بنجاح" : "Bank Transfer Statement was uploaded successfully!",
                                    Message = Language == Settings.UserLanguage.Arabic ? "تم رفع ملف التحويل البنكي بنجاح ... شكرا لكم" : "Bank Transfer Statement has been uploaded successfully... Thank you!",
                                    IsWithAutoRedirect = true,
                                    //UrlToRedirect = Url.Action("Details", "HourlyWorkers", new { id, lang = (Language == Settings.UserLanguage.Arabic ? "ar" : "en") }),
                                    RedirectTimeout = 10
                                };
                                return OkResponse<ReturnData>(new ReturnData() { State = true, Data = new { message = successMsg } });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            var failMsg = new ResultMessageVM()
            {
                Title = Language == Settings.UserLanguage.Arabic ? "لم يتم رفع ملف التحويل البنكي" : "Bank Transfer Statement was not uploaded!",
                Message = Language == Settings.UserLanguage.Arabic ? "لم يتم  رفع ملف التحويل البنكي بنجاح" : "Bank Transfer Statement was not uploaded!",
                IsWithAutoRedirect = false,
                //UrlToRedirect = Url.Action("Index", "Home", new { lang = Lang == Language.Arabic ? "ar" : "en" }),
                //RedirectTimeout = 10
            };
            return OkResponse<ReturnData>(new ReturnData() { State = false, Data = new { message = failMsg } });
        }

    }

    




}
