using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Application.RegisterPerson;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Facade;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Application.RegisterOrganisation;

public record RegisterOrganisation(
    string Name,
    string? LegalForm,
    IReadOnlyList<string> Roles,
    IReadOnlyList<IdentifierInput> Identifiers
) : IRequest<Result<PartyRelatedFailure, string>>;

internal sealed class RegisterOrganisationHandler
    : IRequestHandler<RegisterOrganisation, Result<PartyRelatedFailure, string>>
{
    private readonly IPartyRepository _repository;
    public RegisterOrganisationHandler(IPartyRepository repository) => _repository = repository;

    public Task<Result<PartyRelatedFailure, string>> Handle(RegisterOrganisation cmd, CancellationToken ct)
    {
        var name = string.IsNullOrWhiteSpace(cmd.LegalForm)
            ? OrganizationName.Of(cmd.Name)
            : OrganizationName.Of(cmd.Name, cmd.LegalForm!);

        var roles = cmd.Roles.Select(PartyMapper.ToRole).ToHashSet();
        var ids = cmd.Identifiers.Select(i => PartyMapper.ToIdentifier(i.Type, i.Value))
                                 .ToHashSet<IRegisteredIdentifier>();

        var org = new Organisation(PartyId.Random(), name, roles, ids, Szlakomat.Parties.Domain.Common.Version.Initial());
        _repository.Save(org);
        return Task.FromResult(Result<PartyRelatedFailure, string>.SuccessOf(org.Id.AsString()));
    }
}
