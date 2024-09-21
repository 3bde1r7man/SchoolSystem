using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class StudentCourses
    {
        public int? Id { get; set; }
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        public int? Score { get; set; }
    }
}
