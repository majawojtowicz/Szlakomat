using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Address;

public record AddressId(Guid Value)
{
    public static AddressId Random() => new(Guid.NewGuid());
    public static AddressId Of(Guid v) => new(v);
    public static AddressId Of(string v) => new(Guid.Parse(v));
    public string AsString() => Value.ToString();
    public override string ToString() => AsString();
}

public enum AddressType { Billing, Contact, Registered }

public interface IAddressDetails
{
    string Display();
}

public interface IAddress
{
    AddressId Id { get; }
    PartyId PartyId { get; }
    IReadOnlySet<AddressType> UseTypes { get; }
    IAddressDetails Details { get; }
}

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

    public record GeoAddressDetails(
        string? Name, string Street, string Building,
        string City, string PostalCode, string Country) : IAddressDetails
    {
        public string Display() =>
            (string.IsNullOrWhiteSpace(Name) ? "" : $"{Name}, ") +
            $"{Street} {Building}, {PostalCode} {City}, {Country}";
    }
}

public abstract record AddressRelatedFailure(string Reason)
{
    public sealed record AddressDefinitionFailed(string AddressId, string PartyId, string PolicyReason)
        : AddressRelatedFailure($"Address definition blocked [{AddressId}, party={PartyId}]: {PolicyReason}")
    {
        public static AddressDefinitionFailed DueToPolicyViolation(string addressId, string partyId, string reason) =>
            new(addressId, partyId, reason);
    }

    public sealed record AddressUpdateFailed(string AddressId, string Cause)
        : AddressRelatedFailure($"Address update failed [{AddressId}]: {Cause}");

    public sealed record AddressRemovalFailed(string AddressId, string Cause)
        : AddressRelatedFailure($"Address removal failed [{AddressId}]: {Cause}");
}

public interface IAddressDefiningPolicy
{
    bool IsAddressDefinitionAllowedFor(Addresses addresses, IAddress candidate);
    string ViolationReason { get; }
}

public sealed class AllowAllAddressDefiningPolicy : IAddressDefiningPolicy
{
    public bool IsAddressDefinitionAllowedFor(Addresses a, IAddress c) => true;
    public string ViolationReason => "";
}

public sealed class OneBillingAddressPolicy : IAddressDefiningPolicy
{
    public bool IsAddressDefinitionAllowedFor(Addresses addresses, IAddress candidate)
    {
        Guard.IsNotNull(addresses);
        Guard.IsNotNull(candidate);
        if (!candidate.UseTypes.Contains(AddressType.Billing)) return true;
        return !addresses.ForParty(candidate.PartyId).Any(a => a.UseTypes.Contains(AddressType.Billing));
    }
    public string ViolationReason => "A party may have at most one BILLING address";
}

public class Addresses
{
    private readonly Dictionary<AddressId, IAddress> _addresses;
    private readonly IAddressDefiningPolicy _policy;

    private Addresses(PartyId partyId, Dictionary<AddressId, IAddress> addresses,
        IAddressDefiningPolicy policy, Common.Version version)
    {
        PartyId = partyId;
        _addresses = addresses;
        _policy = policy;
        Version = version;
    }

    public static Addresses EmptyFor(PartyId partyId) =>
        new(partyId, new(), new OneBillingAddressPolicy(), Common.Version.Initial());

    public static Addresses EmptyFor(PartyId partyId, IAddressDefiningPolicy policy)
    {
        Guard.IsNotNull(partyId);
        Guard.IsNotNull(policy);
        return new(partyId, new(), policy, Common.Version.Initial());
    }

    public PartyId PartyId { get; }
    public Common.Version Version { get; private set; }

    public Result<AddressRelatedFailure.AddressDefinitionFailed, Addresses> AddOrUpdate(IAddress address)
    {
        Guard.IsNotNull(address);
        if (_addresses.ContainsKey(address.Id))
        {
            _addresses[address.Id] = address;
            Version = Version.Next();
            return Result<AddressRelatedFailure.AddressDefinitionFailed, Addresses>.SuccessOf(this);
        }
        if (!_policy.IsAddressDefinitionAllowedFor(this, address))
        {
            return Result<AddressRelatedFailure.AddressDefinitionFailed, Addresses>.FailureOf(
                AddressRelatedFailure.AddressDefinitionFailed.DueToPolicyViolation(
                    address.Id.AsString(), PartyId.AsString(), _policy.ViolationReason));
        }
        _addresses[address.Id] = address;
        Version = Version.Next();
        return Result<AddressRelatedFailure.AddressDefinitionFailed, Addresses>.SuccessOf(this);
    }

    public Result<AddressRelatedFailure.AddressRemovalFailed, Addresses> Remove(AddressId addressId)
    {
        Guard.IsNotNull(addressId);
        _addresses.Remove(addressId);
        Version = Version.Next();
        return Result<AddressRelatedFailure.AddressRemovalFailed, Addresses>.SuccessOf(this);
    }

    public IReadOnlyList<IAddress> ForParty(PartyId pid) =>
        _addresses.Values.Where(a => a.PartyId.Equals(pid)).ToList();

    public IReadOnlyList<IAddress> All() => _addresses.Values.ToList();

    public IAddress? FindById(AddressId id) =>
        _addresses.TryGetValue(id, out var a) ? a : null;
}

internal interface IAddressesRepository
{
    Addresses? FindFor(PartyId partyId);
    void Save(Addresses addresses);
}
