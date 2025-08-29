using System.ComponentModel.DataAnnotations;

namespace TimesheetApi.Models
{
    public class TimesheetEntry
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } // Navigation property
        public DateTime Date { get; set; }
        public string ProjectName { get; set; }
        public double HoursWorked { get; set; }
        public string Description { get; set; }

    }
}
