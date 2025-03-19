using Pcf.Core.Integration;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Abstractions.Services;
public interface IPromocodeService
{
    Task GivePromoCodesToCustomersWithPreference(PromocodeDto dto);
}