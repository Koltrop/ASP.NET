using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Предпочтения клиентов
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PreferencesController : ControllerBase
{
    private readonly IRepository<Preference> repository;

    public PreferencesController(IRepository<Preference> repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Получение списка предпочтений
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<PreferenceResponse>>> GetAll()
    {
        var entities = await repository.GetAllAsync();
        var responseList = entities.ConvertAll(x => new PreferenceResponse(x));

        return Ok(responseList);
    }
}
