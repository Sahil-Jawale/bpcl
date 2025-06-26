using System.ComponentModel.DataAnnotations;

namespace GRCAuthServicesMVC.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Password { get; set; }    

        public string[] Roles { get; set; }

    }
}
