using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using System.Threading.Tasks;
using System;
using Pcf.Administration.Core.Abstractions.Services;

namespace Pcf.Administration.Core.Services;

public sealed class PromocodeService(IRepository<Employee> repository) : IPromocodeService
{
    private readonly IRepository<Employee> _employeeRepository = repository;

    public async Task UpdateAppliedPromocodesAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id)
                     ?? throw new ArgumentNullException("Can't find an employee with id " + id);

        employee.AppliedPromocodesCount++;

        await _employeeRepository.UpdateAsync(employee);
    }
}
