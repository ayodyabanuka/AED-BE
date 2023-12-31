﻿/**
 * @Author S.P Rupasinghe
 * @Created 10/3/2023
 * @Description Implement Employee API Controllers
 **/
using AED_BE.DTO.RequestDto;
using AED_BE.Models;
using AED_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace AED_BE.Controllers
{
        [Route("api/employee")] //Route
        [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly LoginService _loginService;

        public EmployeeController(EmployeeService employeeService, LoginService loginService) //constructor
        {
            _employeeService = employeeService;
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] EmployeeLoginRequest loginRequest) //Login
        {
            IActionResult response = Unauthorized();
            String token = await _loginService.EmployeeLogin(loginRequest);
            if (token != null)
            {
                response = Ok(new { access_token = token });
            }
            return response;

        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Post(Employee newEmployee) //Create Employee
        {
            await _employeeService.CreateAsync(newEmployee);
            return CreatedAtAction(nameof(Get), new { id = newEmployee.Id }, newEmployee);
        }

        [HttpGet]
        public async Task<List<Employee>> Get() //Get all employees
        {
            return await _employeeService.GetEmployeesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetOneEmployeeById(string id) //Get employee by ID
        {
            Employee employee = await _employeeService.GetEmployeeAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
    }
}
