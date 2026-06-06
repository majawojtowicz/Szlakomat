using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Facade;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Application.AddRole;

public record AddRole(string PartyId, string Role) : IRequest<Result<PartyRelatedFailure, PartyView>>;

internal sealed class AddRoleHandler : IRequestHandler<AddRole, Result<PartyRelatedFailure, PartyView>>
{
    private readonly IPartyRepository _repository;
    private readonly IRoleAdditionPolicy _policy;

    public AddRoleHandler(IPartyRepository repository, IRoleAdditionPolicy policy)
    {
        _repository = repository;
        _policy = policy;
    }

    public Task<Result<PartyRelatedFailure, PartyView>> Handle(AddRole cmd, CancellationToken ct)
    {
        var party = _repository.FindByIdValue(cmd.PartyId);
        if (party is null)
            return Task.FromResult(Result<PartyRelatedFailure, PartyView>.FailureOf(
                new PartyRelatedFailure.PartyNotFound(cmd.PartyId)));

        var role = Role.Of(cmd.Role);
        if (!_policy.Allows(party, role))
            return Task.FromResult(Result<PartyRelatedFailure, PartyView>.FailureOf(
                new PartyRelatedFailure.RolePolicyViolation(cmd.PartyId, role.Name, _policy.ViolationReason)));

        party.Add(role);
        _repository.Save(party);
        return Task.FromResult(Result<PartyRelatedFailure, PartyView>.SuccessOf(PartyMapper.ToView(party)));
    }
}
