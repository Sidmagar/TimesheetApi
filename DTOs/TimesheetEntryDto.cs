namespace TimesheetApi.DTOs
{
    public class TimesheetEntryDto
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string ProjectName { get; set; }
        public double HoursWorked { get; set; }
        public string Description { get; set; }
    }
}
