using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimesheetApi.DTOs;
using TimesheetApi.Models;
using TimesheetApi.Services;

namespace TimesheetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetsController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;

        public TimesheetsController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _timesheetService.GetAllTimesheetEntriesAsync();
            return Ok(entries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _timesheetService.GetTimesheetEntryByIdAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TimesheetEntryDto newEntryDto)
        {
            var newEntry = new TimesheetEntry
            {
                EmployeeId = newEntryDto.EmployeeId,
                Date = newEntryDto.Date,
                ProjectName = newEntryDto.ProjectName,
                HoursWorked = newEntryDto.HoursWorked,
                Description = newEntryDto.Description
            };
            await _timesheetService.AddTimesheetEntryAsync(newEntry);
            return CreatedAtAction(nameof(GetById), new { id = newEntry.Id }, newEntry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TimesheetEntryDto updatedEntryDto)
        {
            var existingEntry = await _timesheetService.GetTimesheetEntryByIdAsync(id);
            if (existingEntry == null)
            {
                return NotFound();
            }

            existingEntry.Date = updatedEntryDto.Date;
            existingEntry.ProjectName = updatedEntryDto.ProjectName;
            existingEntry.HoursWorked = updatedEntryDto.HoursWorked;
            existingEntry.Description = updatedEntryDto.Description;

            if (!await _timesheetService.UpdateTimesheetEntryAsync(id, existingEntry))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _timesheetService.DeleteTimesheetEntryAsync(id))
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
