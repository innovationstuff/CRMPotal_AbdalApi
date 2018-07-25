using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Helpers
{
    public class CrmColumnAttribute : Attribute
    {
        public string Name { get; protected set; }
        public CrmColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}