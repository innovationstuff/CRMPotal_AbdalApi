using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class RecieptVoucherModel : BaseModel
    {
        [Required]
        public string Customerid { get; set; }
        [Required]
        public int paymenttype { get; set; }
        [Required]
        public string amount { get; set; }
        [Required]
        public string datatime { get; set; }
        [Required]
        public string vatrate { get; set; }
        [Required]
        public string contractid { get; set; }
        [Required]
        public string Contractnumber { get; set; }
        [Required]
        public string paymentcode { get; set; }
        [Required]
        public int who { get; set; }

        public string Voucherid { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceId { get; set; }
    }
}