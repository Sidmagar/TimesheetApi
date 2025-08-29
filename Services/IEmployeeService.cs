using TimesheetApi.Models;

namespace TimesheetApi.Services
{
    public interface IEmployeeService
    {
        Task<bool> RegisterEmployeeAsync(string fullName, string email, string password);
        Task<Employee> AuthenticateEmployeeAsync(string email, string password);

    }
}
