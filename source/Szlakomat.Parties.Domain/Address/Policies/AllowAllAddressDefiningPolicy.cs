namespace Szlakomat.Parties.Domain.Address.Policies;

public sealed class AllowAllAddressDefiningPolicy : IAddressDefiningPolicy
{
    public bool IsAddressDefinitionAllowedFor(Addresses a, IAddress c) => true;
    public string ViolationReason => "";
}
