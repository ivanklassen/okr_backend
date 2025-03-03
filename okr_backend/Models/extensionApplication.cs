﻿namespace okr_backend.Models
{
    public class extensionApplication
    {
        public Guid Id { get; set; }

        public Guid applicationId { get; set; }

        public DateTime extensionToDate { get; set; }

        public string? description { get; set; }

        public string? image { get; set; }

        public Status status { get; set; }
    }
}
