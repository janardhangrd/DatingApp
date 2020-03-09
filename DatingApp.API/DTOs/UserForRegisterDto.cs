using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDto
    {
         [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 6, ErrorMessage = "Min Length should be 6 Characters")]
        public string Password { get; set; }
         [Required]
        public string Gender { get; set; }
         [Required]
        public string KnownAs { get; set; }
         [Required]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string City { get; set; }
         [Required]
        public string Country {get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}