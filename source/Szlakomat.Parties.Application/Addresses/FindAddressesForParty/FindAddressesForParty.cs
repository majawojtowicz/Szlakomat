using MediatR;
using Szlakomat.Parties.Application.Addresses.Common;
using Szlakomat.Parties.Domain.Address;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Application.Addresses.FindAddressesForParty;

public record FindAddressesForPartyCriteria(string PartyId) : IRequest<IReadOnlyList<PartyAddressView>>;

internal sealed class FindAddressesForPartyHandler
    : IRequestHandler<FindAddressesForPartyCriteria, IReadOnlyList<PartyAddressView>>
{
    private readonly IAddressesRepository _repository;
    public FindAddressesForPartyHandler(IAddressesRepository repository) => _repository = repository;

    public Task<IReadOnlyList<PartyAddressView>> Handle(FindAddressesForPartyCriteria q, CancellationToken ct)
    {
        var addresses = _repository.FindFor(PartyId.Of(q.PartyId));
        IReadOnlyList<PartyAddressView> views = addresses?.All().Select(AddressMapper.ToView).ToList() ?? [];
        return Task.FromResult(views);
    }
}
