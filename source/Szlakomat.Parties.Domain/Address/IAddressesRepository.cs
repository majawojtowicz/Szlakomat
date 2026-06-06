using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Address;

internal interface IAddressesRepository
{
    Addresses? FindFor(PartyId partyId);
    void Save(Addresses addresses);
}
