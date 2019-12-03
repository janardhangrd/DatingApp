using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisteration
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 6, ErrorMessage = "Min Length should be 6 Characters")]
        public string Password { get; set; }
    }
}