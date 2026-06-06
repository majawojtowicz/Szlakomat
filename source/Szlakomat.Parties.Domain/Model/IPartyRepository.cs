using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Model;

internal interface IPartyRepository
{
    void Save(Party party);
    Party? FindById(PartyId id);
    Party? FindByIdValue(string idValue);
    IReadOnlyList<Party> FindAll();
    IReadOnlyList<Party> FindByRole(Role role);
    IReadOnlyList<Party> FindNaturalPersons();
    IReadOnlyList<Party> FindOrganisations();
}
