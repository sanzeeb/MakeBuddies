using System.ComponentModel.DataAnnotations;

namespace MakeBuddies.API.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength=4)]
        public string Password { get; set; }
    }
}