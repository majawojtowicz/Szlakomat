using System.Collections.Concurrent;
using Szlakomat.Parties.Domain.Address;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Infrastructure.Addresses;

internal sealed class InMemoryAddressesRepository : IAddressesRepository
{
    private readonly ConcurrentDictionary<Guid, Domain.Address.Addresses> _store = new();

    public Domain.Address.Addresses? FindFor(PartyId partyId) =>
        _store.TryGetValue(partyId.Value, out var a) ? a : null;

    public void Save(Domain.Address.Addresses addresses) =>
        _store[addresses.PartyId.Value] = addresses;
}
