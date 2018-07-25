using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class HourlyPricingCost
    {
        public decimal HourRate { get; set; }
        public decimal TotalPriceBeforeDiscount { get; set; }
        public decimal MonthelyPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalPriceWithVat { get; set; }
        public decimal NetPrice { get; set; }
        public decimal TotalPromotionDiscountAmount { get; set; }
        public decimal TotalPriceAfterPromotion { get; set; }

        public HourlyPricingCost()
        {

        }

        public HourlyPricingCost(DataRow dataRow)
        {

            this.NetPrice = this.TotalPriceWithVat = (dataRow.Table.Columns.Contains("new_finalprice") && dataRow["new_finalprice"] != DBNull.Value) ?decimal.Parse(dataRow["new_finalprice"].ToString()) : 0;
            this.VatRate = (dataRow.Table.Columns.Contains("new_vatrate") && dataRow["new_vatrate"] != DBNull.Value) ? decimal.Parse(dataRow["new_vatrate"].ToString()) : 0;
            this.VatAmount = (dataRow.Table.Columns.Contains("new_vatamount") && dataRow["new_vatamount"] != DBNull.Value) ? decimal.Parse(dataRow["new_vatamount"].ToString()) : 0;
            this.Discount = (dataRow.Table.Columns.Contains("new_discount_def") && dataRow["new_discount_def"] != DBNull.Value) ? decimal.Parse(dataRow["new_discount_def"].ToString()) : 0;
            this.TotalPriceBeforeDiscount = (dataRow.Table.Columns.Contains("totalPriceBeforeDiscount") && dataRow["totalPriceBeforeDiscount"] != DBNull.Value) ? decimal.Parse(dataRow["totalPriceBeforeDiscount"].ToString()) : 0;
            this.TotalPriceAfterDiscount = (dataRow.Table.Columns.Contains("new_totalprice_def") && dataRow["new_totalprice_def"] != DBNull.Value) ? decimal.Parse(dataRow["new_totalprice_def"].ToString()) : 0;
        }
    }
}