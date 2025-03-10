namespace okr_backend.Models
{
    public enum Status { inProcess, Accepted, Rejected }
    public class Application
    {
        public Guid Id { get; set; }

        public DateTime fromDate { get; set; }

        public DateTime toDate { get; set; }

        public string? description { get; set; }

        public string? image { get; set; }

        public Status status { get; set; }

        public Guid userId { get; set; }

        public User user { get; set; }

        public List<extensionApplication> extensions { get; set; }


    }
}
