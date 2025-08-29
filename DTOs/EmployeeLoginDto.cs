using System.ComponentModel.DataAnnotations;

namespace TimesheetApi.DTOs
{
    public class EmployeeLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
