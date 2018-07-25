using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Settings
{
    public enum UserLanguage
    {
        Arabic = 0,
        English = 1
    }

    public enum AttachmentTypes
    {
        FinancialRequest = 100000008
    }

    public enum ServiceContractPerHourStatus
    {
        PaymentIsPendingConfirmation = 100000008
    }

    public enum UserAccountType
    {
        Admin = 0
    }

    public enum SectorsTypeEnum : byte
    {
        Business = 1,
        Individuals = 2,
        HeadOffice = 3
    }

    #region OptionSets

    public enum ReceiptVoucher_PaymentTypes
    {
        BankTransfer=2
    }

    public enum ReceiptVoucher_ReceiptFrom
    {
        IndividualCustomer = 4
    }


  

    #endregion
}