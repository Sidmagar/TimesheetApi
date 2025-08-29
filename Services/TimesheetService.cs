using TimesheetApi.Models;
using TimesheetApi.Repositories;

namespace TimesheetApi.Services
{
    public class TimesheetService:ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public async Task<IEnumerable<TimesheetEntry>> GetAllTimesheetEntriesAsync()
        {
            return await _timesheetRepository.GetAllAsync();
        }

        public async Task<TimesheetEntry> GetTimesheetEntryByIdAsync(int id)
        {
            return await _timesheetRepository.GetByIdAsync(id);
        }

        public async Task AddTimesheetEntryAsync(TimesheetEntry entry)
        {
            await _timesheetRepository.AddAsync(entry);
        }

        public async Task<bool> UpdateTimesheetEntryAsync(int id, TimesheetEntry updatedEntry)
        {
            var existingEntry = await _timesheetRepository.GetByIdAsync(id);
            if (existingEntry == null) return false;

            existingEntry.Date = updatedEntry.Date;
            existingEntry.ProjectName = updatedEntry.ProjectName;
            existingEntry.HoursWorked = updatedEntry.HoursWorked;
            existingEntry.Description = updatedEntry.Description;

            await _timesheetRepository.UpdateAsync(existingEntry);
            return true;
        }

        public async Task<bool> DeleteTimesheetEntryAsync(int id)
        {
            var existingEntry = await _timesheetRepository.GetByIdAsync(id);
            if (existingEntry == null) return false;

            await _timesheetRepository.DeleteAsync(id);
            return true;
        }
    }
}
