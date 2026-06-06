using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Application.FindPartyById;

public record FindPartyByIdCriteria(string PartyId) : IRequest<PartyView?>;

internal sealed class FindPartyByIdHandler : IRequestHandler<FindPartyByIdCriteria, PartyView?>
{
    private readonly IPartyRepository _repository;
    public FindPartyByIdHandler(IPartyRepository repository) => _repository = repository;

    public Task<PartyView?> Handle(FindPartyByIdCriteria query, CancellationToken ct)
    {
        var party = _repository.FindByIdValue(query.PartyId);
        return Task.FromResult(party is null ? null : PartyMapper.ToView(party));
    }
}
