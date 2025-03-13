namespace okr_backend.Models
{
    public class ApplicationModel
    {
        public Guid Id { get; set; }

        public Guid userId { get; set; }

        public DateTime fromDate { get; set; }

        public DateTime toDate { get; set; }

        public string? description { get; set; }

        public string? image { get; set; }

        public Status status { get; set; }

        public string? comment { get; set; }
    }
}
