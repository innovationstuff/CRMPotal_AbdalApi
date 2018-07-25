using Microsoft.Xrm.Sdk;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class RecieptVoucherManager : BaseManager, IDisposable
    {
        public RecieptVoucherManager():base(CrmEntityNamesMapping.RecieptVoucher)
        {

        }

        public void Dispose()
        {
            
        }

        public RecieptVoucherModel CreateVoucherForDomesticInvoice(RecieptVoucherModel Voucher)
        {
            Entity Receipt = new Entity(CrmEntityName);

            Receipt["new_indv"] = new EntityReference(CrmEntityNamesMapping.IndividualContract, new Guid(Voucher.contractid));
            Receipt["new_paymenttype"] = new OptionSetValue((int)ReceiptVoucher_PaymentTypes.BankTransfer);
            Receipt["new_pointofreciept"] = new OptionSetValue((int)ReceiptVoucher_ReceiptFrom.IndividualCustomer);
            Receipt["new_refrencenumber"] = Voucher.paymentcode;


            Receipt["new_amount"] = new Money(decimal.Parse(Voucher.amount));
            Receipt["new_vaterate"] = decimal.Parse(Voucher.vatrate);
            Receipt["new_vatamount"] = decimal.Parse(Voucher.amount) * decimal.Parse(Voucher.vatrate);
            Receipt["new_totalamountwithvat"] = new Money((decimal.Parse(Voucher.amount) * decimal.Parse(Voucher.vatrate)) + decimal.Parse(Voucher.amount));

            Receipt["new_receiptdate"] = DateTime.ParseExact(Voucher.datatime, "dd/MM/yyyy", null);
            Receipt["new_contactid"] = new EntityReference(CrmEntityNamesMapping.Contact, new Guid(Voucher.Customerid));
            Receipt["new_source"] = new OptionSetValue(Voucher.who);

            if (Voucher.who == 2)
                Receipt["new_note"] =String.Format(" تم انشاء السند عن طريق تحويل بنكى- دفع اونلاين- من الويب بورتال لفاتورة أفراد رقم {0} وللعقد رقم {1} ", Voucher.InvoiceNumber,  Voucher.Contractnumber);
            else
                Receipt["new_note"] = String.Format(" تم انشاء السند عن طريق تحويل بنكى –دفع اونلاين-  من تطبيق الجوال لفاتورة أفراد رقم {0} وللعقد رقم {1} ", Voucher.InvoiceNumber, Voucher.Contractnumber);

            Guid Id = GlobalCode.Service.Create(Receipt);
            Voucher.Voucherid = Id.ToString();
            return Voucher;
        }
    }
}