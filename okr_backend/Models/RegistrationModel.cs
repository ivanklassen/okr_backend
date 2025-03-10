using System.ComponentModel.DataAnnotations;

namespace okr_backend.Models
{
    public class RegistrationModel
    {

        [Required]
        public string? surname { get; set; }

        [Required]
        public string? name { get; set; }

        public string? patronymic { get; set; }

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
