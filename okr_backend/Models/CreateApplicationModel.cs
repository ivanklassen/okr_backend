namespace okr_backend.Models
{
    public class CreateApplicationModel
    {
        public DateTime fromDate { get; set; }

        public DateTime toDate { get; set; }

        public string? description { get; set; }

        public string? image { get; set; }
    }
}
