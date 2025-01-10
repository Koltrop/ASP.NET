using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController(IRepository<Employee> employeeRepository) : ControllerBase
    {
        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync([FromBody] Employee employeeRequest)
        {
            var employee = new Employee
            {
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Roles = employeeRequest.Roles.Select(x => new Role
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount
            };

            var createdEmployee = await employeeRepository.CreateAsync(employee);

            var employeeModel = new EmployeeResponse
            {
                Id = createdEmployee.Id,
                Email = createdEmployee.Email,
                Roles = createdEmployee.Roles.Select(x => new RoleItemResponse
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = createdEmployee.FullName,
                AppliedPromocodesCount = createdEmployee.AppliedPromocodesCount
            };

            return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = employeeModel.Id }, employeeModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployeeAsync(Guid id, [FromBody] Employee employeeRequest)
        {
            var existingEmployee = await employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
                return NotFound();

            existingEmployee.FirstName = employeeRequest.FirstName;
            existingEmployee.LastName = employeeRequest.LastName;
            existingEmployee.Email = employeeRequest.Email;
            existingEmployee.Roles = employeeRequest.Roles.Select(x => new Role
            {
                Name = x.Name,
                Description = x.Description
            }).ToList();
            existingEmployee.AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount;

            var updatedEmployee = await employeeRepository.UpdateAsync(existingEmployee);

            var employeeModel = new EmployeeResponse
            {
                Id = updatedEmployee.Id,
                Email = updatedEmployee.Email,
                Roles = updatedEmployee.Roles.Select(x => new RoleItemResponse
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = updatedEmployee.FullName,
                AppliedPromocodesCount = updatedEmployee.AppliedPromocodesCount
            };

            return Ok(employeeModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployeeAsync(Guid id)
        {
            var existingEmployee = await employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
                return NotFound();

            await employeeRepository.DeleteAsync(id);

            return NoContent();
        }

    }
}