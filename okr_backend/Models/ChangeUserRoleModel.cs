namespace okr_backend.Models
{
    public enum Role { Student, Teacher, Dean, Admin }
    public class ChangeUserRoleModel
    {
        public Role role { get; set; }
    }
}
