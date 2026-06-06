using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Address;

public interface IAddress
{
    AddressId Id { get; }
    PartyId PartyId { get; }
    IReadOnlySet<AddressType> UseTypes { get; }
    IAddressDetails Details { get; }
}
