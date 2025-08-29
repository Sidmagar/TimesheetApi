using Microsoft.EntityFrameworkCore;
using TimesheetApi.Data;
using TimesheetApi.Models;

namespace TimesheetApi.Repositories
{
    public class TimesheetRepository:ITimesheetRepository
    {
        private readonly TimesheetDbContext _context;

        public TimesheetRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimesheetEntry>> GetAllAsync()
        {
            return await _context.TimesheetEntries.ToListAsync();
        }

        public async Task<TimesheetEntry> GetByIdAsync(int id)
        {
            return await _context.TimesheetEntries.FindAsync(id);
        }

        public async Task AddAsync(TimesheetEntry entry)
        {
            await _context.TimesheetEntries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TimesheetEntry updatedEntry)
        {
            _context.Entry(updatedEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entryToRemove = await _context.TimesheetEntries.FindAsync(id);
            if (entryToRemove != null)
            {
                _context.TimesheetEntries.Remove(entryToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
