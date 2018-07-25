using Microsoft.Xrm.Sdk;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class DomesticInvoiceManager : BaseManager, IDisposable
    {

        public DomesticInvoiceManager()
           : base(CrmEntityNamesMapping.DomesticInvoice, "new_indvpaymentid", "new_sabnumber")
        {

        }

        public void Dispose()
        {

        }

        public DomesticInvoice GetDomesticInvoiceDetails(string id, UserLanguage Lang)
        {
            // =================================================

            //string[] columns = { CrmGuidFieldName, CrmDisplayFieldName, "new_indvcontractid", "new_paymentduedate" , "new_fromdate", "new_todate",
            //    "new_custamount", "new_totalamountwithvat", "new_paymenttype" , "new_ispaid", "new_customer"
            //};
            //var entity = GetCrmEntity(id, columns);


            //if (entity == null) return null;

            //return new DomesticInvoice(entity);

            // ===================================================

            string optionSetGetValFn, otherLangOptionSetGetValFn;

            switch (Lang)
            {

                case UserLanguage.Arabic:
                    optionSetGetValFn = "dbo.getOptionSetDisplay";
                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplayen";
                    break;
                default:
                    optionSetGetValFn = "dbo.getOptionSetDisplayen";
                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplay";
                    break;

            }

            var query = String.Format(@" Select new_indvpaymentid , new_sabnumber , new_indvcontractid , new_paymentduedate, Convert(date, new_fromdate) as new_fromdate ,
                                                Convert(date, new_todate) as new_todate, new_custamount , 
                                               case when new_totalamountwithvat is null then (isnull(new_vatrate,0)*new_invoiceamount + new_invoiceamount) else new_totalamountwithvat end as new_totalamountwithvat, new_paymenttype,new_ispaid, new_customer, new_indvcontractidname , new_customername,
                                                Isnull({2}('new_paymenttype','{1}',new_paymenttype),{3}('new_paymenttype','{1}',new_paymenttype) ) as new_paymenttypename,
                                                contact.mobilephone, new_indvpayment.new_invoicenodays
                                         From new_indvpayment left outer join contact on contact.contactid =  new_indvpayment.new_customer
                                         Where new_indvpaymentid = '{0}'
                                       ", id , CrmEntityName, optionSetGetValFn, otherLangOptionSetGetValFn);
            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
            if (dt.Rows.Count == 0) return null;

            var invoice = new DomesticInvoice(dt.Rows[0]);
            return invoice;
        }

        public DomesticInvoice GetDomesticInvoiceDetails(string id, string user, UserLanguage Lang)
        {
            // to do :: filter by user also
            return GetDomesticInvoiceDetails(id, Lang);
        }

        public void UpdatePaymentStatus(string id, bool isPaid)
        {
            Entity invoice = new Entity(CrmEntityName);
            invoice[CrmGuidFieldName] = new Guid(id);
            invoice["new_ispaid"] = isPaid;
            GlobalCode.Service.Update(invoice);
        }

        public List<DomesticInvoice> GetDomesticInvoices(string userId, UserLanguage Lang)
        {
          
            string optionSetGetValFn, otherLangOptionSetGetValFn;

            switch (Lang)
            {

                case UserLanguage.Arabic:
                    optionSetGetValFn = "dbo.getOptionSetDisplay";
                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplayen";
                    break;
                default:
                    optionSetGetValFn = "dbo.getOptionSetDisplayen";
                    otherLangOptionSetGetValFn = "dbo.getOptionSetDisplay";
                    break;

            }

            var query = String.Format(@" Select new_indvpaymentid , new_sabnumber , new_indvcontractid , new_paymentduedate, new_fromdate , new_todate, new_custamount , 
                                                case when new_totalamountwithvat is null then (isnull(new_vatrate,0)*new_invoiceamount + new_invoiceamount) else new_totalamountwithvat end as new_totalamountwithvat, new_paymenttype,new_ispaid, new_customer, new_indvcontractidname , new_customername,
                                                Isnull({2}('new_paymenttype','{1}',new_paymenttype),{3}('new_paymenttype','{1}',new_paymenttype) ) as new_paymenttypename,
                                                contact.mobilephone
                                         From new_indvpayment left outer join contact on contact.contactid =  new_indvpayment.new_customer
                                         Where new_customer = '{0}'
                                       ", userId, CrmEntityName, optionSetGetValFn, otherLangOptionSetGetValFn);
            DataTable dt = CRMAccessDB.SelectQ(query).Tables[0];
            if (dt.Rows.Count == 0) return null;
            List<DomesticInvoice> invoices = new List<DomesticInvoice>();
            foreach (DataRow item in dt.Rows)
            {
                var invoice = new DomesticInvoice(item);
                invoices.Add(invoice);
            }
            
            return invoices;
        }

    }
}