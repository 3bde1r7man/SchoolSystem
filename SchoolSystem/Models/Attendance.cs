namespace SchoolSystem.Models
{
    public class Attendance
    {
        public int? Id { get; set; }
        public Student? Student { get; set; }
        public Course? Course { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsPresent { get; set; }
        public string? Reason { get; set; } = "N/A";
    }
}
