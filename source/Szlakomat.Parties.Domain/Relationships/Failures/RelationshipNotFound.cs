namespace Szlakomat.Parties.Domain.Relationships.Failures;

public sealed record RelationshipNotFound(string RelationshipId)
    : PartyRelationshipRelatedFailure($"Relationship not found: {RelationshipId}");
