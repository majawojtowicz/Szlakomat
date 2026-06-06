using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Application.RegisterPerson;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Facade;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Application.RegisterJdg;

public record RegisterJdg(
    string OwnerFirstName,
    string OwnerLastName,
    string BusinessName,
    string Nip,
    IReadOnlyList<string> Roles,
    IReadOnlyList<IdentifierInput> Identifiers
) : IRequest<Result<PartyRelatedFailure, string>>;

internal sealed class RegisterJdgHandler
    : IRequestHandler<RegisterJdg, Result<PartyRelatedFailure, string>>
{
    private readonly IPartyRepository _repository;
    public RegisterJdgHandler(IPartyRepository repository) => _repository = repository;

    public Task<Result<PartyRelatedFailure, string>> Handle(RegisterJdg cmd, CancellationToken ct)
    {
        var roles = cmd.Roles.Select(PartyMapper.ToRole).ToHashSet();
        var ids = cmd.Identifiers.Select(i => PartyMapper.ToIdentifier(i.Type, i.Value))
                                 .ToHashSet<IRegisteredIdentifier>();

        var jdg = new Jdg(
            PartyId.Random(),
            PersonalData.Of(cmd.OwnerFirstName, cmd.OwnerLastName),
            cmd.BusinessName, cmd.Nip,
            roles, ids, Szlakomat.Parties.Domain.Common.Version.Initial());

        _repository.Save(jdg);
        return Task.FromResult(Result<PartyRelatedFailure, string>.SuccessOf(jdg.Id.AsString()));
    }
}
