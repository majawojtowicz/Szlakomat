using MediatR;
using Microsoft.AspNetCore.Mvc;
using Szlakomat.Parties.Application.AddRole;
using Szlakomat.Parties.Application.Addresses.AddAddress;
using Szlakomat.Parties.Application.Addresses.FindAddressesForParty;
using Szlakomat.Parties.Application.FindAllParties;
using Szlakomat.Parties.Application.FindPartiesByRole;
using Szlakomat.Parties.Application.FindPartyById;
using Szlakomat.Parties.Application.RegisterJdg;
using Szlakomat.Parties.Application.RegisterOrganisation;
using Szlakomat.Parties.Application.RegisterPerson;
using Szlakomat.Parties.Application.Relationships.AssignRelationship;
using Szlakomat.Parties.Application.Relationships.FindActiveRelationships;
using Szlakomat.Parties.Application.UpdatePersonalData;

namespace Szlakomat.Products.Api.Controllers;

[ApiController]
[Route("api/parties")]
[Produces("application/json")]
public class PartiesController(IMediator mediator) : ControllerBase
{
    
    [HttpPost("person")]
    public async Task<IActionResult> RegisterPerson([FromBody] RegisterPerson cmd)
    {
        var result = await mediator.Send(cmd);
        return result.IsSuccess()
            ? Created($"/api/parties/{result.SuccessValue}", new { id = result.SuccessValue })
            : BadRequest(new { error = result.FailureValue?.Reason });
    }

    [HttpPost("organisation")]
    public async Task<IActionResult> RegisterOrganisation([FromBody] RegisterOrganisation cmd)
    {
        var result = await mediator.Send(cmd);
        return result.IsSuccess()
            ? Created($"/api/parties/{result.SuccessValue}", new { id = result.SuccessValue })
            : BadRequest(new { error = result.FailureValue?.Reason });
    }

    [HttpPost("jdg")]
    public async Task<IActionResult> RegisterJdg([FromBody] RegisterJdg cmd)
    {
        var result = await mediator.Send(cmd);
        return result.IsSuccess()
            ? Created($"/api/parties/{result.SuccessValue}", new { id = result.SuccessValue })
            : BadRequest(new { error = result.FailureValue?.Reason });
    }

    [HttpGet]
    public async Task<IActionResult> All([FromQuery] string? filter = null)
        => Ok(await mediator.Send(new FindAllPartiesCriteria(filter)));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var view = await mediator.Send(new FindPartyByIdCriteria(id));
        return view is null ? NotFound() : Ok(view);
    }

    [HttpGet("by-role/{role}")]
    public async Task<IActionResult> ByRole(string role)
        => Ok(await mediator.Send(new FindPartiesByRoleCriteria(role)));

    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AddRole(string id, [FromBody] AddRoleRequest req)
    {
        var result = await mediator.Send(new AddRole(id, req.Role));
        return result.IsSuccess()
            ? Ok(result.SuccessValue)
            : BadRequest(new { error = result.FailureValue?.Reason });
    }

    [HttpPatch("{id}/personal-data")]
    public async Task<IActionResult> UpdatePersonalData(string id, [FromBody] UpdatePersonalDataRequest req)
    {
        var result = await mediator.Send(new UpdatePersonalData(id, req.FirstName, req.LastName));
        return result.IsSuccess()
            ? Ok(result.SuccessValue)
            : BadRequest(new { error = result.FailureValue?.Reason });
    }

    [HttpPost("{id}/addresses")]
    public async Task<IActionResult> AddAddress(string id, [FromBody] AddAddressRequest req)
    {
        var cmd = new AddPartyAddress(id, req.Name, req.Street, req.Building,
            req.City, req.PostalCode, req.Country, req.UseTypes);
        var result = await mediator.Send(cmd);
        return result.IsSuccess()
            ? Created($"/api/parties/{id}/addresses/{result.SuccessValue.AddressId}", result.SuccessValue)
            : BadRequest(new { error = result.FailureValue?.Reason });
    }

    [HttpGet("{id}/addresses")]
    public async Task<IActionResult> GetAddresses(string id)
        => Ok(await mediator.Send(new FindAddressesForPartyCriteria(id)));

    
    [HttpPost("relationships")]
    public async Task<IActionResult> AssignRelationship([FromBody] AssignPartyRelationship cmd)
    {
        var result = await mediator.Send(cmd);
        return result.IsSuccess()
            ? Created($"/api/parties/relationships/{result.SuccessValue.Id}", result.SuccessValue)
            : BadRequest(new { error = result.FailureValue?.Reason });
    }

    [HttpGet("{id}/relationships")]
    public async Task<IActionResult> GetRelationships(string id)
        => Ok(await mediator.Send(new FindActiveRelationshipsCriteria(id)));
}

public record AddRoleRequest(string Role);
public record UpdatePersonalDataRequest(string FirstName, string LastName);
public record AddAddressRequest(
    string? Name,
    string Street,
    string Building,
    string City,
    string PostalCode,
    string Country,
    IReadOnlyList<string> UseTypes
);