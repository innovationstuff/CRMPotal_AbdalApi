using Microsoft.AspNet.Identity.EntityFramework;
using NasAPI.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NasAPI
{
    public class AuthContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public AuthContext()
            : base("AuthContext")
        {

        }
    }
}