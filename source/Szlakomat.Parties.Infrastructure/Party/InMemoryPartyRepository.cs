using System.Collections.Concurrent;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Infrastructure.Party;

internal sealed class InMemoryPartyRepository : IPartyRepository
{
    private readonly ConcurrentDictionary<Guid, Domain.Model.Party> _storage = new();

    public void Save(Domain.Model.Party party) => _storage[party.Id.Value] = party;

    public Domain.Model.Party? FindById(PartyId id) =>
        _storage.TryGetValue(id.Value, out var p) ? p : null;

    public Domain.Model.Party? FindByIdValue(string idValue) =>
        Guid.TryParse(idValue, out var g) && _storage.TryGetValue(g, out var p) ? p : null;

    public IReadOnlyList<Domain.Model.Party> FindAll() => _storage.Values.ToList();

    public IReadOnlyList<Domain.Model.Party> FindByRole(Role role) =>
        _storage.Values.Where(p => p.HasRole(role)).ToList();

    public IReadOnlyList<Domain.Model.Party> FindNaturalPersons() =>
        _storage.Values.Where(p => p.IsNaturalPerson).ToList();

    public IReadOnlyList<Domain.Model.Party> FindOrganisations() =>
        _storage.Values.Where(p => !p.IsNaturalPerson).ToList();
}
