using NasAPI.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class LoginModel
    {
        //  [Required(ErrorMessage = "phone")]
        [Required(ErrorMessageResourceName = "PhoneRequired", ErrorMessageResourceType = typeof(ValidationsResources))]
        [Display(Name = "phone")]
        public string UserName { get; set; }

        //  [Required]
        [Required(ErrorMessageResourceName = "Password", ErrorMessageResourceType = typeof(ValidationsResources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}