using MediatR;
using Microsoft.AspNetCore.Mvc;
using Szlakomat.Parties.Application.RegisterPerson;
using Szlakomat.Parties.Application.FindAllParties;

[ApiController]
[Route("api/[controller]")]
public class PartiesController(IMediator mediator) : ControllerBase
{
    [HttpPost("person")]
    public async Task<IActionResult> RegisterPerson([FromBody] RegisterPerson cmd)
        => Ok(await mediator.Send(cmd));

    [HttpGet]
    public async Task<IActionResult> All()
        => Ok(await mediator.Send(new FindAllPartiesCriteria()));
}