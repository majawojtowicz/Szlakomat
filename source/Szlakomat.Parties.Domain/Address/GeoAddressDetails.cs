namespace Szlakomat.Parties.Domain.Address;

public record GeoAddressDetails(
    string? Name, string Street, string Building,
    string City, string PostalCode, string Country) : IAddressDetails
{
    public string Display() =>
        (string.IsNullOrWhiteSpace(Name) ? "" : $"{Name}, ") +
        $"{Street} {Building}, {PostalCode} {City}, {Country}";
}
