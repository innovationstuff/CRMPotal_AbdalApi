using Microsoft.Xrm.Sdk;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class PaymentManager : BaseManager
    {

        #region Individual

        public bool PayDomesticInvoice(string id)
        {
            using (var mgr = new DomesticInvoiceManager())
            {
                mgr.UpdatePaymentStatus(id, true);
            }
            return true;
        }

        public string AddtDomesticInvoiceBankTransferAttachment(string id, string fileName)
        {
            Entity attachment = new Entity(CrmEntityNamesMapping.Attachment);
            attachment["new_attachmentid"] = new EntityReference(CrmEntityNamesMapping.DomesticInvoice, new Guid(id)); ;
            attachment["new_attachmentsid"] = Guid.NewGuid();
            attachment["new_attachmentype"] = new OptionSetValue((int)AttachmentTypes.FinancialRequest);
            attachment["new_name"] = fileName;

            var attachmentId = GlobalCode.Service.Create(attachment);
            return attachmentId.ToString();
        }


        #endregion
    }
}