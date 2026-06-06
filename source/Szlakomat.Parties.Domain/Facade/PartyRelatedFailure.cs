namespace Szlakomat.Parties.Domain.Facade;

public abstract record PartyRelatedFailure(string Reason)
{
    public sealed record PartyNotFound(string PartyId)
        : PartyRelatedFailure($"Party not found: {PartyId}");

    public sealed record PartyAlreadyExists(string PartyId)
        : PartyRelatedFailure($"Party already exists: {PartyId}");

    public sealed record RolePolicyViolation(string PartyId, string Role, string PolicyReason)
        : PartyRelatedFailure($"Role policy violation [{Role} -> {PartyId}]: {PolicyReason}");

    public sealed record InvalidPartyType(string PartyId, string ExpectedType)
        : PartyRelatedFailure($"Party {PartyId} is not of expected type {ExpectedType}");
}
