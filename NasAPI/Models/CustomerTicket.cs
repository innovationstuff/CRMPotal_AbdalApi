using NasAPI.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NasAPI.Models
{
    public class CustomerTicket : BaseModel
    {
        public string ClientClosedCode { get; set; }
        //public string IdNumber { get; set; }
        public string TicketNumber { get; set; }
        public string ProblemDetails { get; set; }
        public string Id { get; set; }
        public string ContactId { get; set; }
        public string Contact { get; set; }

        public string ProblemTypeId { get; set; }
        public string ProblemType { get; set; }

        public string SectorTypeId { get; set; }
        public string SectorType { get; set; }

        public string StatusId { get; set; }
        public string Status { get; set; }

        public string ContractId { get; set; }
        public string Contract { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeText { get; set; }
        public  Who? who { get; set; }
        public CustomerTicket()
        {

        }

        public CustomerTicket(DataRow dataRow)
        {
            this.Id = dataRow.Table.Columns.Contains("new_csindvsectorId") ? dataRow["new_csindvsectorId"].ToString() : null;
            this.ClientClosedCode = dataRow.Table.Columns.Contains("new_closedno") ? dataRow["new_closedno"].ToString() : null;
            //this.IdNumber = dataRow.Table.Columns.Contains("new_idnumber") ? dataRow["new_idnumber"].ToString() : null;
            this.TicketNumber = dataRow.Table.Columns.Contains("new_name") ? dataRow["new_name"].ToString() : null;
            this.ProblemDetails = dataRow.Table.Columns.Contains("new_problemdetails") ? dataRow["new_problemdetails"].ToString() : null;

            this.Contact = dataRow.Table.Columns.Contains("new_contactName") ? dataRow["new_contactName"].ToString() : null;
            this.ProblemType = (dataRow.Table.Columns.Contains("new_problemcaseName")) ? dataRow["new_problemcaseName"].ToString() : null;
            this.SectorType = (dataRow.Table.Columns.Contains("new_contracttypeName")) ? dataRow["new_contracttypeName"].ToString() : null;
            this.Status = (dataRow.Table.Columns.Contains("statuscodeName")) ? dataRow["statuscodeName"].ToString() : null;
            


            this.ContactId = (dataRow.Table.Columns.Contains("ContactId")) ? dataRow["ContactId"].ToString() : null;
            this.ProblemTypeId = (dataRow.Table.Columns.Contains("new_problemcase")) ? dataRow["new_problemcase"].ToString() : null;
            this.SectorTypeId = (dataRow.Table.Columns.Contains("new_contracttype")) ? dataRow["new_contracttype"].ToString() : null;
            this.StatusId = (dataRow.Table.Columns.Contains("statuscode")) ? dataRow["statuscode"].ToString() : null;


            if (this.SectorTypeId == ((int)CustomerTicketSectorType.Individual).ToString())
                this.ContractId = (dataRow.Table.Columns.Contains("new_IndvContractId")) ? dataRow["new_IndvContractId"].ToString() : null;
            else if (this.SectorTypeId == ((int)CustomerTicketSectorType.Dalal).ToString())
                this.ContractId = (dataRow.Table.Columns.Contains("new_cshindivcontractid")) ? dataRow["new_cshindivcontractid"].ToString() : null;

            if (this.SectorTypeId == ((int)CustomerTicketSectorType.Individual).ToString())
                this.Contract = (dataRow.Table.Columns.Contains("new_indvcontractidName")) ? dataRow["new_indvcontractidName"].ToString() : null;
            else if (this.SectorTypeId == ((int)CustomerTicketSectorType.Dalal).ToString())
                this.Contract = (dataRow.Table.Columns.Contains("new_cshindivcontractidName")) ? dataRow["new_cshindivcontractidName"].ToString() : null;
        }
    }
}