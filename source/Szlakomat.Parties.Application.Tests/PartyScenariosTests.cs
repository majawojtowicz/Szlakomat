using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Szlakomat.Parties.Application.AddRole;
using Szlakomat.Parties.Application.Addresses.AddAddress;
using Szlakomat.Parties.Application.FindAllParties;
using Szlakomat.Parties.Application.RegisterJdg;
using Szlakomat.Parties.Application.RegisterPerson;
using Szlakomat.Parties.Application.Relationships.AssignRelationship;
using Szlakomat.Parties.Infrastructure;

public class PartyScenariosTests
{
    private readonly IMediator _mediator;

    public PartyScenariosTests()
    {
        var services = new ServiceCollection().AddPartyModule().BuildServiceProvider();
        _mediator = services.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task Person_cannot_become_attraction_owner()
    {
        var jan = await _mediator.Send(new RegisterPerson("Jan", "Kowalski",
            ["VISITOR"], [new IdentifierInput("EMAIL", "jan@x.pl")]));

        var result = await _mediator.Send(new AddRole(jan.SuccessValue, "ATTRACTION_OWNER"));

        Assert.True(result.IsFailure());
        Assert.Contains("ATTRACTION_OWNER cannot be assigned to Person", result.FailureValue!.Reason);
    }

    [Fact]
    public async Task Jdg_can_be_attraction_owner()
    {
        var marek = await _mediator.Send(new RegisterJdg("Marek", "Wiśniewski",
            "Przewodnik Górski W.", "7771234567",
            ["TOUR_GUIDE", "ATTRACTION_OWNER"],
            [new IdentifierInput("NIP", "7771234567")]));

        Assert.True(marek.IsSuccess());
    }

    [Fact]
    public async Task Cannot_add_two_billing_addresses()
    {
        var org = await _mediator.Send(new RegisterPerson("Test", "Org",
            ["VISITOR"], []));
        var id = org.SuccessValue;

        var first = await _mediator.Send(new AddPartyAddress(id, null,
            "Krupówki", "10", "Zakopane", "34-500", "PL", ["BILLING"]));
        var second = await _mediator.Send(new AddPartyAddress(id, null,
            "Inna", "99", "Kraków", "30-001", "PL", ["BILLING"]));

        Assert.True(first.IsSuccess());
        Assert.True(second.IsFailure());
    }

    [Fact]
    public async Task Relationship_between_visitor_and_attraction_owner()
    {
        var jan = await _mediator.Send(new RegisterPerson("Jan", "K",
            ["VISITOR"], []));
        var tatromania = await _mediator.Send(new RegisterJdg("Anna", "Nowak",
            "Tatromania", "1234567890", ["ATTRACTION_OWNER"], []));

        var rel = await _mediator.Send(new AssignPartyRelationship(
            jan.SuccessValue, "VISITOR",
            tatromania.SuccessValue, "ATTRACTION_OWNER",
            "VISITED"));

        Assert.True(rel.IsSuccess());
    }
}