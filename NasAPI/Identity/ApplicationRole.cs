using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI.Identity
{
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
            ApplicationPages = new HashSet<ApplicationPage>();
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }
        public virtual ICollection<ApplicationPage> ApplicationPages { get; set; }
    }

}