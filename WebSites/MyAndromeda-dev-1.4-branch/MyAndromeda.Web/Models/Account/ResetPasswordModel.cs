using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyAndromeda.Web.Models.Account
{
    public class ResetPasswordModel
    {
        public string Code { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}