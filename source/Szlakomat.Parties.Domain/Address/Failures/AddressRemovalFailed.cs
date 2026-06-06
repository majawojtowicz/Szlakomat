namespace Szlakomat.Parties.Domain.Address.Failures;

public sealed record AddressRemovalFailed(string AddressId, string Cause)
    : AddressRelatedFailure($"Address removal failed [{AddressId}]: {Cause}");
