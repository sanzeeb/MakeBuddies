using System.ComponentModel.DataAnnotations;

namespace MakeBuddies.API.Dtos
{
    public class UserCreateDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength=4 , ErrorMessage="Password length must be in 4 to 8 characters")]
        public string Password { get; set; }
    }
}