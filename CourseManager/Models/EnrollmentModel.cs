namespace CourseManager.Models
{
    internal class EnrollmentModel
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public bool IsComitted { get; set; }
    }
}
