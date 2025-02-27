using System.ComponentModel.DataAnnotations;

namespace okr_backend.Models
{
    public class RegistrationModel
    {

        [Required]
        [MinLength(1)]
        public string? fullName { get; set; }

        [Required]
        public DateTime birthDate { get; set; }

        [MinLength(1)]
        [Required]
        public string? email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(32)]
        public string? password { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(32)]
        public string? confirmPassword { get; set; }

    }
}
