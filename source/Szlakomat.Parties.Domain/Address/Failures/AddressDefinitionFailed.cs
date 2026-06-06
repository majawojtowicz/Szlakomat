namespace Szlakomat.Parties.Domain.Address.Failures;

public sealed record AddressDefinitionFailed(string AddressId, string PartyId, string PolicyReason)
    : AddressRelatedFailure($"Address definition blocked [{AddressId}, party={PartyId}]: {PolicyReason}")
{
    public static AddressDefinitionFailed DueToPolicyViolation(string addressId, string partyId, string reason) =>
        new(addressId, partyId, reason);
}
