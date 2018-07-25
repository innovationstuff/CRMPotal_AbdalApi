using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class DomesticInvoice : BaseModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public int? InvoiceTypeKey { get; set; }
        public string InvoiceType { get; set; }
        public string ContractId { get; set; }
        public string Contract { get; set; }
        public string CustomerId { get; set; }
        public string Customer { get; set; }
        public string CustomerMobilePhone { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? CustomerAmount { get; set; }
        public decimal? TotalWithVat { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public bool IsPaid { get; set; }
        public int? InvoiceDaysCount { get; set; }

        public DomesticInvoice()
        {

        }

        public DomesticInvoice(Entity entity)
        {
            this.Id = (entity.Attributes.ContainsKey("new_indvpaymentid") && entity["new_indvpaymentid"] != null) ? entity["new_indvpaymentid"].ToString() : null;
            this.Number = (entity.Attributes.ContainsKey("new_sabnumber") && entity["new_sabnumber"] != null) ? entity["new_sabnumber"].ToString() : null;
            this.IsPaid = (entity.Attributes.ContainsKey("new_ispaid") && entity["new_ispaid"] != null) ? (bool)entity["new_ispaid"] : false;

            this.DueDate = (entity.Attributes.ContainsKey("new_paymentduedate") && entity["new_paymentduedate"] != null) ? (DateTime?)entity["new_paymentduedate"] : null;
            this.FromDate = (entity.Attributes.ContainsKey("new_fromdate") && entity["new_fromdate"] != null) ? (DateTime?)entity["new_fromdate"] : null;
            this.ToDate = (entity.Attributes.ContainsKey("new_todate") && entity["new_todate"] != null) ? (DateTime?)entity["new_todate"] : null;


            this.CustomerAmount = (entity.Attributes.ContainsKey("new_custamount") && entity["new_custamount"] != null) ? (decimal?)entity["new_custamount"] : null;
            this.TotalWithVat = (entity.Attributes.ContainsKey("new_totalamountwithvat") && entity["new_totalamountwithvat"] != null) ? (decimal?)entity["new_totalamountwithvat"] : null;
            this.InvoiceAmount = this.CustomerAmount;

            var contractEntity = (entity["new_indvcontractid"] as EntityReference);
            ContractId = contractEntity.Id.ToString();
            Contract = contractEntity.Name;

            var customerEntity = (entity["new_customer"] as EntityReference);
            CustomerId = contractEntity.Id.ToString();
            Customer = contractEntity.KeyAttributes["new_name"].ToString();

            //this.Customer = (entity.Attributes.ContainsKey("new_CustomerName") && entity["new_CustomerName"] != null) ? entity["new_CustomerName"].ToString() : null;


            var typeOption = (entity["new_paymenttype"] as OptionSetValue);
            InvoiceTypeKey = typeOption.Value;
        }

        public DomesticInvoice(DataRow dataRow)
        {


            Id = (dataRow.Table.Columns.Contains("new_indvpaymentid") && dataRow["new_indvpaymentid"] != DBNull.Value) ? dataRow["new_indvpaymentid"].ToString() : null;
            Number = (dataRow.Table.Columns.Contains("new_sabnumber") && dataRow["new_sabnumber"] != DBNull.Value) ? dataRow["new_sabnumber"].ToString() : null;
            IsPaid = (dataRow.Table.Columns.Contains("new_ispaid") && dataRow["new_ispaid"] != DBNull.Value) ? (bool)dataRow["new_ispaid"] : false;

            DueDate = (dataRow.Table.Columns.Contains("new_paymentduedate") && dataRow["new_paymentduedate"] != DBNull.Value) ? (DateTime?)dataRow["new_paymentduedate"] : null;
            FromDate = (dataRow.Table.Columns.Contains("new_fromdate") && dataRow["new_fromdate"] != DBNull.Value) ? (DateTime?)dataRow["new_fromdate"] : null;
            ToDate = (dataRow.Table.Columns.Contains("new_todate") && dataRow["new_todate"] != DBNull.Value) ? (DateTime?)dataRow["new_todate"] : null;


            CustomerAmount = (dataRow.Table.Columns.Contains("new_custamount") && dataRow["new_custamount"] != DBNull.Value) ? (decimal?)dataRow["new_custamount"] : null;
            TotalWithVat = (dataRow.Table.Columns.Contains("new_totalamountwithvat") && dataRow["new_totalamountwithvat"] != DBNull.Value) ? (decimal?)dataRow["new_totalamountwithvat"] : null;
            InvoiceAmount = this.CustomerAmount;

            ContractId = (dataRow.Table.Columns.Contains("new_indvcontractid") && dataRow["new_indvcontractid"] != DBNull.Value) ? dataRow["new_indvcontractid"].ToString() : null;
            Contract = (dataRow.Table.Columns.Contains("new_indvcontractidname") && dataRow["new_indvcontractidname"] != DBNull.Value) ? dataRow["new_indvcontractidname"].ToString() : null;
            CustomerId = (dataRow.Table.Columns.Contains("new_customer") && dataRow["new_customer"] != DBNull.Value) ? dataRow["new_customer"].ToString() : null;
            Customer = (dataRow.Table.Columns.Contains("new_customername") && dataRow["new_customername"] != DBNull.Value) ? dataRow["new_customername"].ToString() : null;
            CustomerMobilePhone = (dataRow.Table.Columns.Contains("mobilephone") && dataRow["mobilephone"] != DBNull.Value) ? dataRow["mobilephone"].ToString() : null;

            InvoiceTypeKey = (dataRow.Table.Columns.Contains("new_paymenttype") && dataRow["new_paymenttype"] != DBNull.Value) ? (int?)dataRow["new_paymenttype"] : null;
            InvoiceType = (dataRow.Table.Columns.Contains("new_paymenttypename") && dataRow["new_paymenttypename"] != DBNull.Value) ? dataRow["new_paymenttypename"].ToString() : null;

            InvoiceDaysCount = (dataRow.Table.Columns.Contains("new_invoicenodays") && dataRow["new_invoicenodays"] != DBNull.Value) ? (int?)dataRow["new_invoicenodays"] : null;
        }
    }
}