using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chat_With_Polling_SignalR_DotNetCore_MySQL.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username", Prompt = "eg. Uchenna")]
        [StringLength(40, ErrorMessage = "Username must be atleast 5 and not more than 40", MinimumLength = 5)]
        public string UserName { get; set; }

        public string PassWord { get; set; }
        public string ChatPartner { get; set; }
    }
}
