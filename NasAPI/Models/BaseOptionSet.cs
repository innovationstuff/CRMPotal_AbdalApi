using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class BaseOptionSet
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public BaseOptionSet()
        {

        }

        public BaseOptionSet(int key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}