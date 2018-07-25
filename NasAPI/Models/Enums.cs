using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Enums
{
    public enum DayShifts
    {
        Morning = 0, Evening = 1
    }

    public enum DayShiftsAR
    {
        صباحا = 0, مساءا = 1
    }

    public enum CustomerTicketSectorType
    {
        Individual = 2,
        Dalal = 4,
        Business = 1,
        Abdal = 3
    }

    public enum Who
    {
        CRM = 1,
        Web = 3,
        Mobile = 2
    }

    public enum RecordSource
    {
        CRM = 0,
        Mobile = 1,
        Web = 2
    }
}