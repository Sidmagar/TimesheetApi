using TimesheetApi.Models;

namespace TimesheetApi.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByEmailAsync(string email);
        Task AddAsync(Employee employee);
        Task<Employee> GetByIdAsync(int id);
    }
}
