using MediatR;
using Szlakomat.Parties.Application.Relationships.Common;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Relationships;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Application.Relationships.AssignRelationship;

public record AssignPartyRelationship(
    string FromPartyId,
    string FromRole,
    string ToPartyId,
    string ToRole,
    string RelationshipName
) : IRequest<Result<PartyRelationshipRelatedFailure, PartyRelationshipView>>;

internal sealed class AssignPartyRelationshipHandler
    : IRequestHandler<AssignPartyRelationship, Result<PartyRelationshipRelatedFailure, PartyRelationshipView>>
{
    private readonly IPartyRelationshipRepository _repository;
    private readonly IPartyRepository _partyRepository;
    private readonly IPartyRelationshipDefiningPolicy _policy;

    public AssignPartyRelationshipHandler(
        IPartyRelationshipRepository repository,
        IPartyRepository partyRepository,
        IPartyRelationshipDefiningPolicy policy)
    {
        _repository = repository;
        _partyRepository = partyRepository;
        _policy = policy;
    }

    public Task<Result<PartyRelationshipRelatedFailure, PartyRelationshipView>> Handle(
        AssignPartyRelationship cmd, CancellationToken ct)
    {
        var fromId = PartyId.Of(cmd.FromPartyId);
        var toId = PartyId.Of(cmd.ToPartyId);

        var from = _partyRepository.FindById(fromId);
        var to = _partyRepository.FindById(toId);

        if (from is null)
            return Fail(PartyRelationshipRelatedFailure.RelationshipDefinitionFailed
                .DueToMissingParty(cmd.FromPartyId));
        if (to is null)
            return Fail(PartyRelationshipRelatedFailure.RelationshipDefinitionFailed
                .DueToMissingParty(cmd.ToPartyId));

        var fromRole = Role.Of(cmd.FromRole);
        var toRole = Role.Of(cmd.ToRole);
        var name = RelationshipName.Of(cmd.RelationshipName);

        if (!_policy.CanDefine(from, fromRole, to, toRole, name))
            return Fail(PartyRelationshipRelatedFailure.RelationshipDefinitionFailed
                .DueToPolicyViolation(cmd.FromPartyId, cmd.ToPartyId, _policy.ViolationReason));

        var rel = PartyRelationship.Of(fromId, fromRole, toId, toRole, name);
        _repository.Save(rel);
        return Task.FromResult(Result<PartyRelationshipRelatedFailure, PartyRelationshipView>.SuccessOf(
            RelationshipMapper.ToView(rel)));
    }

    private static Task<Result<PartyRelationshipRelatedFailure, PartyRelationshipView>> Fail(
        PartyRelationshipRelatedFailure f) =>
        Task.FromResult(Result<PartyRelationshipRelatedFailure, PartyRelationshipView>.FailureOf(f));
}
