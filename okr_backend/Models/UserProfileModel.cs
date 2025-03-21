namespace okr_backend.Models
{
    public class UserProfileModel
    {
        public string? surname {  get; set; }

        public string? name { get; set; }

        public string? patronymic { get; set; }
        public string? email { get; set; }

        public bool isStudent { get; set; }

        public bool isTeacher { get; set; }

        public bool isDean { get; set; }

        public bool isAdmin { get; set; }

    }
}
