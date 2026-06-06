namespace Szlakomat.Parties.Domain.Address.Policies;

public interface IAddressDefiningPolicy
{
    bool IsAddressDefinitionAllowedFor(Addresses addresses, IAddress candidate);
    string ViolationReason { get; }
}
