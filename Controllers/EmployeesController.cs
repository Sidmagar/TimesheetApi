using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimesheetApi.DTOs;
using TimesheetApi.Services;

namespace TimesheetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegistrationDto newEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _employeeService.RegisterEmployeeAsync(newEmployeeDto.FullName, newEmployeeDto.Email, newEmployeeDto.Password))
            {
                return Ok(new { message = "Registration successful" });
            }
            return BadRequest(new { message = "Email already exists" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmployeeLoginDto employeeCredentialsDto)
        {
            var employee = await _employeeService.AuthenticateEmployeeAsync(employeeCredentialsDto.Email, employeeCredentialsDto.Password);
            if (employee != null)
            {
                return Ok(new { message = "Login successful", employeeId = employee.Id });
            }
            return Unauthorized(new { message = "Invalid email or password" });
        }
    }
}
