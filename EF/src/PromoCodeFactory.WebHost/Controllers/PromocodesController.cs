using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController : ControllerBase
    {
        private readonly IRepository<PromoCode> promocodeRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Preference> preferenceRepository;
        private readonly IRepository<Employee> employeeRepository;

        public PromocodesController(IRepository<PromoCode> promocodeRepository,
            IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<Employee> employeeRepository)
        {
            this.promocodeRepository = promocodeRepository;
            this.customerRepository = customerRepository;
            this.preferenceRepository = preferenceRepository;
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var entities = await promocodeRepository.GetAllAsync();
            var responseList = entities.ConvertAll(x => new PromoCodeShortResponse(x));

            return Ok(responseList);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var preferences = await preferenceRepository.GetAllAsync(x => x.Name.Contains(request.Preference));
            if (preferences.Count == 0)
                return NotFound("Can't find any preference with the name " + request.Preference);
            else if (preferences.Count > 1)
                return BadRequest("There are more than one preference with the name " + request.Preference);
            var preference = preferences[0];

            var employees = await employeeRepository.GetAllAsync(x => (x.FirstName.Contains(request.PartnerName) || x.LastName.Contains(request.PartnerName))
                                                                   && x.Role.Name == "PartnerManager");
            if (employees.Count == 0)
                return NotFound("Can't find any manager with the name " + request.PartnerName);
            else if (employees.Count > 1)
                return BadRequest("There are more than one manager with the name " + request.PartnerName);
            var employee = employees[0];

            var customers = await customerRepository
                .GetAllAsync(x => x.Preferences.Any(x => x.Id == preference.Id));
            if (customers.Count == 0)
                return NotFound("Can't find any customer with the preference " + request.Preference);

            var promocodes = new List<PromoCode>();
            foreach (var customer in customers)
            {
                var promocode = new PromoCode
                {
                    ServiceInfo = request.ServiceInfo,
                    Code = request.PromoCode,
                    PartnerName = employee.FullName,
                    PartnerManager = employee,
                    Customer = customer,
                    BeginDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(preference.PromocodeValidityInDays),
                    Preference = preference
                };
                promocodes.Add(promocode);
            }
            await promocodeRepository.Create([.. promocodes]);

            employee.AppliedPromocodesCount += promocodes.Count;
            await employeeRepository.Update(employee);

            var pluralForm = promocodes.Count > 1 ? "s" : string.Empty;

            return Ok($"{promocodes.Count} promocode{pluralForm} have been created");
        }
    }
}