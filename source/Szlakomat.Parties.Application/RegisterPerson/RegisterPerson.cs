using MediatR;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Facade;

namespace Szlakomat.Parties.Application.RegisterPerson;

public record IdentifierInput(string Type, string Value);

public record RegisterPerson(
    string FirstName,
    string LastName,
    IReadOnlyList<string> Roles,
    IReadOnlyList<IdentifierInput> Identifiers
) : IRequest<Result<PartyRelatedFailure, string>>;
