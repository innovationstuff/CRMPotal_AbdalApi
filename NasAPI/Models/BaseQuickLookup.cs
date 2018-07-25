using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class BaseQuickLookup
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public BaseQuickLookup()
        {
                
        }

        public BaseQuickLookup(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}