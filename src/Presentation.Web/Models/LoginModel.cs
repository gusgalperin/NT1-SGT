using System.ComponentModel.DataAnnotations;

namespace Presentation.Web.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}