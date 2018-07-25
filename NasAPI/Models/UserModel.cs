using NasAPI.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NasAPI.Models
{
    public class UserModel
    {
        //[Required(ErrorMessageResourceName = "RegisterPhoneIsRequired", ErrorMessageResourceType = typeof(ValidationsResources))]
        [Display(Name = "Mobile Phone")]
        //[RegularExpression(AppConstants.MobilePhoneRex, ErrorMessageResourceName = "RegisterPhoneIsNotValed", ErrorMessageResourceType = typeof(ValidationsResources))]
        public string UserName { get; set; }

        //[Display(Name = "Full name")]
        //[Required(ErrorMessageResourceName = "RegisterNameIsRequired", ErrorMessageResourceType = typeof(ValidationsResources))]
        public string Name { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessageResourceName = "RegisterFirstNameIsRequired", ErrorMessageResourceType = typeof(ValidationsResources))]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        [Required(ErrorMessageResourceName = "RegisterMiddleNameIsRequired", ErrorMessageResourceType = typeof(ValidationsResources))]
        public string MiddleName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessageResourceName = "RegisterLastNameIsRequired", ErrorMessageResourceType = typeof(ValidationsResources))]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "RegisterPasswordIsRequired", ErrorMessageResourceType = typeof(ValidationsResources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessageResourceName = "RegisterComparePassword", ErrorMessageResourceType = typeof(ValidationsResources))]
        public string ConfirmPassword { get; set; }
        public string CrmUserId { get; set; }

    }
}