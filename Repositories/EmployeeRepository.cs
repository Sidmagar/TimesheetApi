using TimesheetApi.Data;
using TimesheetApi.Models;
using Microsoft.EntityFrameworkCore;


namespace TimesheetApi.Repositories
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly TimesheetDbContext _context;

        public EmployeeRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

    }
}
