using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Facade;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Application.RegisterPerson;

internal sealed class RegisterPersonHandler
    : IRequestHandler<RegisterPerson, Result<PartyRelatedFailure, string>>
{
    private readonly IPartyRepository _repository;

    public RegisterPersonHandler(IPartyRepository repository) => _repository = repository;

    public Task<Result<PartyRelatedFailure, string>> Handle(RegisterPerson cmd, CancellationToken ct)
    {
        var roles = cmd.Roles.Select(PartyMapper.ToRole).ToHashSet();
        var ids = cmd.Identifiers.Select(i => PartyMapper.ToIdentifier(i.Type, i.Value))
                                 .ToHashSet<IRegisteredIdentifier>();

        var person = new Person(
            PartyId.Random(),
            PersonalData.Of(cmd.FirstName, cmd.LastName),
            roles, ids, Szlakomat.Parties.Domain.Common.Version.Initial());

        _repository.Save(person);
        return Task.FromResult(Result<PartyRelatedFailure, string>.SuccessOf(person.Id.AsString()));
    }
}
