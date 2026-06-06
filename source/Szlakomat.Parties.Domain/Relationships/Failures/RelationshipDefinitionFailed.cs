namespace Szlakomat.Parties.Domain.Relationships.Failures;

public sealed record RelationshipDefinitionFailed(string FromId, string ToId, string PolicyReason)
    : PartyRelationshipRelatedFailure($"Relationship blocked [{FromId} -> {ToId}]: {PolicyReason}")
{
    public static RelationshipDefinitionFailed DueToPolicyViolation(string fromId, string toId, string reason) =>
        new(fromId, toId, reason);
    public static RelationshipDefinitionFailed DueToMissingParty(string partyId) =>
        new(partyId, "?", $"Party not found: {partyId}");
}
