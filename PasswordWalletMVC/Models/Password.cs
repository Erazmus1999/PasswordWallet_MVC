using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordWalletMVC.Models
{
    public class Password
    {
        public int PasswordId { get; set; }
        
        [Required(ErrorMessage = "Password required")]
        public string PasswordName { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string WebAddress { get; set; }

        public string Description { get; set; }

        public string Login { get; set; }

        //Foreign
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
