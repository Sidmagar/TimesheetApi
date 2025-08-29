using TimesheetApi.Models;

namespace TimesheetApi.Repositories
{
    public interface ITimesheetRepository
    {
        Task<IEnumerable<TimesheetEntry>> GetAllAsync();
        Task<TimesheetEntry> GetByIdAsync(int id);
        Task AddAsync(TimesheetEntry entry);
        Task UpdateAsync(TimesheetEntry entry);
        Task DeleteAsync(int id);
    }
}
