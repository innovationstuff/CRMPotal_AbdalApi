using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NasAPI.Models;

namespace NasAPI.DisplayData
{
    public class StaticDisplayLists
    {
        public static IEnumerable<BaseOptionSet> HourlyContractStatusForProfile { get; private set; }
        public static IEnumerable<BaseOptionSet> HourlyContractStatusForProfile_Arabic { get; private set; }

        public static IEnumerable<BaseOptionSet> CustomerTicket_DalalProblemTypes_Arabic { get; private set; }
        public static IEnumerable<BaseOptionSet> CustomerTicket_DalalProblemTypes { get; private set; }

        public static IEnumerable<BaseOptionSet> CustomerTicket_IndividualProblemTypes_Arabic { get; private set; }
        public static IEnumerable<BaseOptionSet> CustomerTicket_IndividualProblemTypes { get; private set; }

        public static IEnumerable<BaseOptionSet> CustomerTicket_Status_Arabic { get; private set; }
        public static IEnumerable<BaseOptionSet> CustomerTicket_Status { get; private set; }

        public static IEnumerable<BaseOptionSet> HourlyContract_Status_Arabic { get; private set; }
        public static IEnumerable<BaseOptionSet> HourlyContract_Status { get; private set; }

        static StaticDisplayLists()
        {
            HourlyContractStatusForProfile = new List<BaseOptionSet>() { 
                new BaseOptionSet( 100000006, "New not paid contracts"),
                new BaseOptionSet( 100000004, "New waiting confirmation contracts"),
                new BaseOptionSet(100000005, "Running contracts"),
                new BaseOptionSet(100000002, "Paid contracts"),
                new BaseOptionSet( 100000000, "Finished contracts"),
                new BaseOptionSet( 100000007,"Canceled contracts")
            };

            HourlyContractStatusForProfile_Arabic = new List<BaseOptionSet>() { 
                new BaseOptionSet( 100000006, "عقود جديدة لم يتم دفعها"),
                new BaseOptionSet( 100000004, "عقود في انتظار التأكيد عليها"),
                new BaseOptionSet(100000005, "عقود جارية"),
                new BaseOptionSet(100000002, "عقود جديدة وتم دفعها"),
                new BaseOptionSet( 100000000, "عقود منتهية"),
                new BaseOptionSet( 100000007,"عقود ملغاة")
            };


            CustomerTicket_DalalProblemTypes = new List<BaseOptionSet>() { 
                new BaseOptionSet( 100000008, "Contract Cancellation Request"),
                new BaseOptionSet( 100000009, "Complain on Employee"),
                new BaseOptionSet( 100000010, "Update Contract"),
                new BaseOptionSet( 100000011, "Delete Contract"),
                new BaseOptionSet(100000005, "Others"),
            };

            CustomerTicket_DalalProblemTypes_Arabic = new List<BaseOptionSet>() { 
                 new BaseOptionSet( 100000008, "طلب الغاء عقد"),
                new BaseOptionSet( 100000009, "شكوى من العاملة"),
                new BaseOptionSet( 100000010, "تعديل عقد"),
                new BaseOptionSet( 100000011, "حذف عقد"),
                new BaseOptionSet(100000005, "أخرى"),
            };

            CustomerTicket_IndividualProblemTypes = new List<BaseOptionSet>() { 
                new BaseOptionSet( 100000007, "New Suggestion"),
                new BaseOptionSet( 100000006, "Ask / Enquiry"),
                new BaseOptionSet(100000000, "Escaped Employee"),
                new BaseOptionSet(100000001, "Employee refuse work"),
                new BaseOptionSet(100000002, "Employee didn’t get salary"),
                new BaseOptionSet(100000003, "ATM Card Problem"),
                new BaseOptionSet(100000004, "Medical Card Probem"),
                new BaseOptionSet( 100000010, "Update Contract"),
                new BaseOptionSet( 100000011, "Delete Contract"),
                new BaseOptionSet(100000005, "Others"),
            };

            CustomerTicket_IndividualProblemTypes_Arabic = new List<BaseOptionSet>() { 
                new BaseOptionSet( 100000007, "مقترح جديد"),
                new BaseOptionSet( 100000006, "سؤال / إستفسار"),
                new BaseOptionSet(100000000, "هروب عامل"),
                 new BaseOptionSet(100000001, "عامل رافض للعمل"),
                new BaseOptionSet(100000002, "عامل لم يستلم الراتب"),
                new BaseOptionSet(100000003, "مشكلة في بطاقة صراف العامل"),
                new BaseOptionSet(100000004, "مشكلة في الـامين الصحي للعامل"),
                new BaseOptionSet( 100000010, "تعديل عقد"),
                new BaseOptionSet( 100000011, "حذف عقد"),
                new BaseOptionSet(100000005, "أخرى"),
            };

            CustomerTicket_Status = new List<BaseOptionSet>() { 
                new BaseOptionSet( 100000000, "Pending"),
                new BaseOptionSet( 100000012, "Waiting to close"),
                new BaseOptionSet(100000008, "Canceled / Stopped"),
                new BaseOptionSet(100000009, "Finished Successfully"),
                new BaseOptionSet(100000010, "Sent to Finance"),
            };

            CustomerTicket_Status_Arabic = new List<BaseOptionSet>() { 
                new BaseOptionSet( 100000000, "جاري التعامل"),
                new BaseOptionSet( 100000012, "في انتظار الإغلاق"),
                new BaseOptionSet(100000008, "ملغاة / متوقفة"),
                new BaseOptionSet(100000009, "تم الانتهاء بنجاح"),
                new BaseOptionSet(100000010, "إرسال إلى مسئول الحساب"),
            };

            HourlyContract_Status = new List<BaseOptionSet>() { 
                new BaseOptionSet(0, "New"),
                new BaseOptionSet(1, "Started"),
                new BaseOptionSet(2, "Finished"),
                new BaseOptionSet(3, "Postponed"),
                new BaseOptionSet(4, "Cancelled"),
            };

            HourlyContract_Status_Arabic = new List<BaseOptionSet>() { 
                new BaseOptionSet(0, "جديد"),
                new BaseOptionSet(1, "بدأ"),
                new BaseOptionSet(2, "انتهي"),
                new BaseOptionSet(3, "مؤجل"),
                new BaseOptionSet(4, "ملغي"),
            };

        }
    }
}