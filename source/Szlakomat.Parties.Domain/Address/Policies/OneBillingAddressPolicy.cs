namespace Szlakomat.Parties.Domain.Address.Policies;

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
