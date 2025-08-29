using TimesheetApi.Models;

namespace TimesheetApi.Services
{
    public interface ITimesheetService
    {
        Task<IEnumerable<TimesheetEntry>> GetAllTimesheetEntriesAsync();
        Task<TimesheetEntry> GetTimesheetEntryByIdAsync(int id);
        Task AddTimesheetEntryAsync(TimesheetEntry entry);
        Task<bool> UpdateTimesheetEntryAsync(int id, TimesheetEntry updatedEntry);
        Task<bool> DeleteTimesheetEntryAsync(int id);

    }
}
