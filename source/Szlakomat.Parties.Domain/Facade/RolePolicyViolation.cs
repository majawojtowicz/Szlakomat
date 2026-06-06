namespace Szlakomat.Parties.Domain.Facade;

public sealed record RolePolicyViolation(string PartyId, string Role, string PolicyReason)
    : PartyRelatedFailure($"Role policy violation [{Role} -> {PartyId}]: {PolicyReason}");
