using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace NasAPI.Models
{
    public class Promotion
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? FreeVisitsFactor { get; set; }
        public decimal? Discount { get; set; }
        public decimal? FixedDiscount { get; set; }
        public string Available { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public Promotion()
        {

        }

        public Promotion(DataRow dataRow)
        {
            this.Id = dataRow.Table.Columns.Contains("new_promotionslistId") ? dataRow["new_promotionslistId"].ToString() : null;
            this.Name = dataRow.Table.Columns.Contains("new_name") ? dataRow["new_name"].ToString() : null;
            this.Code = dataRow.Table.Columns.Contains("new_code") ? dataRow["new_code"].ToString() : null;
            this.Description = dataRow.Table.Columns.Contains("new_description") ? dataRow["new_description"].ToString() : null;
            this.FreeVisitsFactor = (dataRow.Table.Columns.Contains("new_freevisits") && dataRow["new_freevisits"] != DBNull.Value) ? (int?) int.Parse(dataRow["new_freevisits"].ToString()) : null;
            this.Discount = (dataRow.Table.Columns.Contains("new_discount") && dataRow["new_discount"] != DBNull.Value) ? (decimal?) decimal.Parse(dataRow["new_discount"].ToString()) : null;
            this.FixedDiscount = (dataRow.Table.Columns.Contains("new_fixeddiscount") && dataRow["new_fixeddiscount"] != DBNull.Value) ? (decimal?) decimal.Parse(dataRow["new_fixeddiscount"].ToString()) : null;
            this.Available = dataRow.Table.Columns.Contains("new_availalbe") ? dataRow["new_availalbe"].ToString() : null;
            this.FromDate = dataRow.Table.Columns.Contains("new_fromdate") ? dataRow["new_fromdate"].ToString() : null;
            this.ToDate = dataRow.Table.Columns.Contains("new_todate") ? dataRow["new_todate"].ToString() : null;
        }
    }


}