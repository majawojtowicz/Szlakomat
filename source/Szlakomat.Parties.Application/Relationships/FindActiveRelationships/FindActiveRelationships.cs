using MediatR;
using Szlakomat.Parties.Application.Relationships.Common;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Relationships;

namespace Szlakomat.Parties.Application.Relationships.FindActiveRelationships;

public record FindActiveRelationshipsCriteria(string PartyId)
    : IRequest<IReadOnlyList<PartyRelationshipView>>;

internal sealed class FindActiveRelationshipsHandler
    : IRequestHandler<FindActiveRelationshipsCriteria, IReadOnlyList<PartyRelationshipView>>
{
    private readonly IPartyRelationshipRepository _repository;
    public FindActiveRelationshipsHandler(IPartyRelationshipRepository repository) => _repository = repository;

    public Task<IReadOnlyList<PartyRelationshipView>> Handle(
        FindActiveRelationshipsCriteria q, CancellationToken ct)
    {
        IReadOnlyList<PartyRelationshipView> views =
            _repository.FindActiveFor(PartyId.Of(q.PartyId))
                .Select(RelationshipMapper.ToView).ToList();
        return Task.FromResult(views);
    }
}
