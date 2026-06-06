using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Application.FindAllParties;

public record FindAllPartiesCriteria(string? Filter = null) : IRequest<IReadOnlyList<PartyView>>;

internal sealed class FindAllPartiesHandler
    : IRequestHandler<FindAllPartiesCriteria, IReadOnlyList<PartyView>>
{
    private readonly IPartyRepository _repository;
    public FindAllPartiesHandler(IPartyRepository repository) => _repository = repository;

    public Task<IReadOnlyList<PartyView>> Handle(FindAllPartiesCriteria query, CancellationToken ct)
    {
        IReadOnlyList<Party> source = query.Filter?.ToUpperInvariant() switch
        {
            "NATURAL"      => _repository.FindNaturalPersons(),
            "ORGANISATION" => _repository.FindOrganisations(),
            _              => _repository.FindAll()
        };
        IReadOnlyList<PartyView> views = source.Select(PartyMapper.ToView).ToList();
        return Task.FromResult(views);
    }
}
