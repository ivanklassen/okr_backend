using System.ComponentModel.DataAnnotations;

namespace okr_backend.Models
{
    public class LoginModel
    {
        [Required]
        [MinLength(1)]
        public string? email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(32)]
        public string? password { get; set; }
    }
}
