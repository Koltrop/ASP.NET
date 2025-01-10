using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Preference> preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            this.customerRepository = customerRepository;
            this.preferenceRepository = preferenceRepository;
        }


        /// <summary>
        /// Получение списка всех клиентов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<CustomerShortResponse>>> GetCustomersAsync()
        {
            var entities = await customerRepository.GetAllAsync();
            var responseList = entities.ConvertAll(x => new CustomerShortResponse(x));

            return Ok(responseList);
        }

        /// <summary>
        /// Получение клиента по его идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            string[] includes = [nameof(Customer.Promocodes), nameof(Customer.Preferences)];
            var entity = await customerRepository.GetByIdAsync(id, includes);

            if (entity is null)
                return NotFound(id);

            var response = new CustomerResponse(entity);

            return Ok(response);
        }

        /// <summary>
        /// Создание нового клиента вместе с его предпочтениями
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var entity = await MapCustomer(request);
            await customerRepository.Create(entity);

            return Created();
        }

        /// <summary>
        /// Обновление данных клиента вместе с его предпочтениями
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var entity = await customerRepository.GetByIdAsync(id, [nameof(Customer.Preferences)]);

            if (entity is null)
                return NotFound(id);

            var mappedEntity = await MapCustomer(request, entity);
            await customerRepository.Update(mappedEntity);

            return Ok();
        }

        /// <summary>
        /// Удаление информации о клиенте
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var entity = await customerRepository.GetByIdAsync(id);

            if (entity is null)
                return NotFound(id);

            await customerRepository.Delete(entity);

            return Ok();
        }

        private async Task<Customer> MapCustomer(CreateOrEditCustomerRequest dto, Customer customer = null)
        {
            customer ??= new();

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Preferences = [];
            foreach (var preferenceId in dto.PreferenceIds)
            {
                var preference = await preferenceRepository.GetByIdAsync(preferenceId)
                    ?? throw new Exception("Can't find a preference with id " + preferenceId);

                customer.Preferences.Add(preference);
            }

            return customer;
        }
    }
}