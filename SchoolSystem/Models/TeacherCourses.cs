using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Models
{
    public class TeacherCourses
    {
        public int? Id { get; set; }
        public int? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
