namespace Szlakomat.Parties.Domain.Address.Failures;

public sealed record AddressUpdateFailed(string AddressId, string Cause)
    : AddressRelatedFailure($"Address update failed [{AddressId}]: {Cause}");
