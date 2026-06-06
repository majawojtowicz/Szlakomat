using Szlakomat.Parties.Domain.Address.Failures;
using Szlakomat.Parties.Domain.Address.Policies;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Address;

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

    public Result<AddressDefinitionFailed, Addresses> AddOrUpdate(IAddress address)
    {
        Guard.IsNotNull(address);
        if (_addresses.ContainsKey(address.Id))
        {
            _addresses[address.Id] = address;
            Version = Version.Next();
            return Result<AddressDefinitionFailed, Addresses>.SuccessOf(this);
        }
        if (!_policy.IsAddressDefinitionAllowedFor(this, address))
        {
            return Result<AddressDefinitionFailed, Addresses>.FailureOf(
                AddressDefinitionFailed.DueToPolicyViolation(
                    address.Id.AsString(), PartyId.AsString(), _policy.ViolationReason));
        }
        _addresses[address.Id] = address;
        Version = Version.Next();
        return Result<AddressDefinitionFailed, Addresses>.SuccessOf(this);
    }

    public Result<AddressRemovalFailed, Addresses> Remove(AddressId addressId)
    {
        Guard.IsNotNull(addressId);
        _addresses.Remove(addressId);
        Version = Version.Next();
        return Result<AddressRemovalFailed, Addresses>.SuccessOf(this);
    }

    public IReadOnlyList<IAddress> ForParty(PartyId pid) =>
        _addresses.Values.Where(a => a.PartyId.Equals(pid)).ToList();

    public IReadOnlyList<IAddress> All() => _addresses.Values.ToList();

    public IAddress? FindById(AddressId id) =>
        _addresses.TryGetValue(id, out var a) ? a : null;
}
