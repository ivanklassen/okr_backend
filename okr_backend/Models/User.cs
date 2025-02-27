﻿using System.ComponentModel.DataAnnotations;

namespace okr_backend.Models
{
    public class User
    {
        public Guid Id { get; set; }

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

        public bool isStudent { get; set; }

        public bool isTeacher { get; set; }

        public bool isDean { get; set; }

        public bool isAdmin { get; set; }

    }
}
