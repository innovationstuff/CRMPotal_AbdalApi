using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Inferstructures
{
    

    public class StaticLists
    {


         

        public static List<Status> contractstatus()
        {

            return new List<Status>()
            {
                // new Status(){StatusCode="100000006",name="عقود جديدة لم يتم دفعها"},
                // new Status(){StatusCode="100000004",name="عقود فى انتظار التاكيد عليها"},
                //  new Status(){StatusCode="10000005",name="عقود الجارية الان"},
                //new Status(){StatusCode="100000002",name="عقود قادمة وتم دفعها"},
               
                // new Status(){StatusCode="100StatusName00",name="عقود منتهية"},
            };



        }

    }
}