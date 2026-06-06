using Szlakomat.Parties.Domain.Address;

namespace Szlakomat.Parties.Application.Addresses.Common;

public record PartyAddressView(
    string AddressId,
    string PartyId,
    IReadOnlyList<string> UseTypes,
    string? Name,
    string Street,
    string Building,
    string City,
    string PostalCode,
    string Country,
    string Display);

internal static class AddressMapper
{
    public static PartyAddressView ToView(IAddress address)
    {
        var d = (GeoAddressDetails)address.Details;
        return new PartyAddressView(
            address.Id.AsString(),
            address.PartyId.AsString(),
            address.UseTypes.Select(t => t.ToString().ToUpperInvariant()).ToList(),
            d.Name, d.Street, d.Building, d.City, d.PostalCode, d.Country,
            d.Display());
    }

    public static AddressType ToType(string s) => s.ToUpperInvariant() switch
    {
        "BILLING"    => AddressType.Billing,
        "CONTACT"    => AddressType.Contact,
        "REGISTERED" => AddressType.Registered,
        _ => throw new ArgumentException($"Unknown AddressType: {s}", nameof(s))
    };
}
