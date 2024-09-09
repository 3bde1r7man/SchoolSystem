namespace SchoolSystem.Models
{
    public class Course
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Credits { get; set; }
        public int? Grade { get; set; }
        public int? EnrollmentLimit { get; set; }
        public int? CurrentEnrollment { get; set; }

    }
}
