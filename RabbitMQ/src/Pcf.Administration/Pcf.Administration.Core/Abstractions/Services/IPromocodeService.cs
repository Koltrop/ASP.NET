using System;
using System.Threading.Tasks;

namespace Pcf.Administration.Core.Abstractions.Services;

public interface IPromocodeService
{
    Task UpdateAppliedPromocodesAsync(Guid id);
}