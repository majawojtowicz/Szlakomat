using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Relationships;

public record RelationshipName
{
    public string Value { get; }
    public RelationshipName(string? value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Value = value;
    }
    public static RelationshipName Of(string value) => new(value);
    public override string ToString() => Value;

    public static readonly RelationshipName Visited      = new("VISITED");
    public static readonly RelationshipName Reviewed     = new("REVIEWED");
    public static readonly RelationshipName Owns         = new("OWNS");
    public static readonly RelationshipName GuidesAt     = new("GUIDES_AT");
    public static readonly RelationshipName PartnersWith = new("PARTNERS_WITH");
    public static readonly RelationshipName Employs      = new("EMPLOYS");
}

public record PartyRelationshipId(Guid Value)
{
    public static PartyRelationshipId Random() => new(Guid.NewGuid());
    public static PartyRelationshipId Of(string v) => new(Guid.Parse(v));
    public string AsString() => Value.ToString();
}

public record PartyRelationship(
    PartyRelationshipId Id,
    PartyId FromPartyId, Role FromRole,
    PartyId ToPartyId,   Role ToRole,
    RelationshipName Name,
    DateOnly ValidFrom,
    DateOnly? ValidTo)
{
    public static PartyRelationship Of(PartyId from, Role fromRole, PartyId to, Role toRole, RelationshipName name)
    {
        Guard.IsNotNull(from);
        Guard.IsNotNull(to);
        Guard.IsNotNull(name);
        Guard.IsFalse(from.Equals(to), "A party cannot have a relationship with itself");
        return new(PartyRelationshipId.Random(), from, fromRole, to, toRole, name,
            DateOnly.FromDateTime(DateTime.Today), null);
    }

    public bool IsActive()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return today >= ValidFrom && (ValidTo is null || today <= ValidTo.Value);
    }

    public PartyRelationship Terminate(DateOnly date) => this with { ValidTo = date };

    public override string ToString() =>
        $"{FromPartyId.AsString()}[{FromRole.Name}] --[{Name.Value}]--> {ToPartyId.AsString()}[{ToRole.Name}]";
}

public abstract record PartyRelationshipRelatedFailure(string Reason)
{
    public sealed record RelationshipDefinitionFailed(string FromId, string ToId, string PolicyReason)
        : PartyRelationshipRelatedFailure($"Relationship blocked [{FromId} -> {ToId}]: {PolicyReason}")
    {
        public static RelationshipDefinitionFailed DueToPolicyViolation(string fromId, string toId, string reason) =>
            new(fromId, toId, reason);
        public static RelationshipDefinitionFailed DueToMissingParty(string partyId) =>
            new(partyId, "?", $"Party not found: {partyId}");
    }

    public sealed record RelationshipNotFound(string RelationshipId)
        : PartyRelationshipRelatedFailure($"Relationship not found: {RelationshipId}");
}

public interface IPartyRelationshipDefiningPolicy
{
    bool CanDefine(Party from, Role fromRole, Party to, Role toRole, RelationshipName name);
    string ViolationReason { get; }
}

public sealed class AllowAllPartyRelationshipDefiningPolicy : IPartyRelationshipDefiningPolicy
{
    public bool CanDefine(Party from, Role fromRole, Party to, Role toRole, RelationshipName name) => true;
    public string ViolationReason => "";
}

internal interface IPartyRelationshipRepository
{
    void Save(PartyRelationship relationship);
    PartyRelationship? FindById(PartyRelationshipId id);
    IReadOnlyList<PartyRelationship> FindFor(PartyId partyId);
    IReadOnlyList<PartyRelationship> FindActiveFor(PartyId partyId);
    IReadOnlyList<PartyRelationship> FindAll();
}
