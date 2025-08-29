using TimesheetApi.Models;
using TimesheetApi.Repositories;
using BCrypt.Net;

namespace TimesheetApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> RegisterEmployeeAsync(string fullName, string email, string password)
        {
            if (await _employeeRepository.GetByEmailAsync(email) != null)
            {
                return false;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var newEmployee = new Employee { FullName = fullName, Email = email, PasswordHash = passwordHash };
            await _employeeRepository.AddAsync(newEmployee);
            return true;
        }

        public async Task<Employee> AuthenticateEmployeeAsync(string email, string password)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);

            if (employee != null && BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash))
            {
                return employee;
            }
            return null;
        }
    }
}
