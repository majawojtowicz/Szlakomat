using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Address;

public sealed class GeoAddress : IAddress
{
    private GeoAddress(AddressId id, PartyId partyId,
        IEnumerable<AddressType> useTypes, GeoAddressDetails details)
    {
        Id = id;
        PartyId = partyId;
        UseTypes = new HashSet<AddressType>(useTypes);
        Details = details;
    }

    public static GeoAddress Of(PartyId partyId,
        string? name, string street, string building,
        string city, string postalCode, string country,
        IEnumerable<AddressType> useTypes)
    {
        Guard.IsNotNull(partyId);
        Guard.IsNotNullOrWhiteSpace(street);
        Guard.IsNotNullOrWhiteSpace(building);
        Guard.IsNotNullOrWhiteSpace(city);
        Guard.IsNotNullOrWhiteSpace(postalCode);
        Guard.IsNotNullOrWhiteSpace(country);
        Guard.IsNotNull(useTypes);
        return new(AddressId.Random(), partyId, useTypes,
            new GeoAddressDetails(name, street, building, city, postalCode, country));
    }

    public AddressId Id { get; }
    public PartyId PartyId { get; }
    public IReadOnlySet<AddressType> UseTypes { get; }
    public IAddressDetails Details { get; }

    public GeoAddress WithUseTypes(IEnumerable<AddressType> newUseTypes) =>
        new(Id, PartyId, newUseTypes, (GeoAddressDetails)Details);

    public override bool Equals(object? obj) => obj is GeoAddress g && Id.Equals(g.Id);
    public override int GetHashCode() => Id.GetHashCode();
}
