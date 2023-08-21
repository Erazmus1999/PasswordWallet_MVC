using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PasswordWalletMVC.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "UserName required")]
        public string Login { get; set; }
        
       
        [Required(ErrorMessage = "Password required")]
        public string PasswordHash { get; set; }

       
        public string Salt { get; set; }
        public bool IsPasswordKeptAsHash { get; set; }

        public ICollection<Password> UserPasswords { get; set; }
    }
}
