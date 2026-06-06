namespace Szlakomat.Parties.Domain.Events;

public interface IPartyEvent
{
    DateTimeOffset OccurredAt { get; }
}

public interface IPublishedPartyEvent : IPartyEvent { }

public record RoleAdded(string PartyId, string Role, DateTimeOffset OccurredAt) : IPublishedPartyEvent;

public record RoleAdditionSkipped(string PartyId, string Role, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RoleAdditionSkipped DueToDuplication(string partyId, string role) =>
        new(partyId, role, "DUPLICATE", DateTimeOffset.UtcNow);
}

public record RoleRemoved(string PartyId, string Role, DateTimeOffset OccurredAt) : IPublishedPartyEvent;

public record RoleRemovalSkipped(string PartyId, string Role, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RoleRemovalSkipped DueToMissingRole(string partyId, string role) =>
        new(partyId, role, "NOT_FOUND", DateTimeOffset.UtcNow);
}

public record RegisteredIdentifierAdded(string PartyId, string Type, string Value, DateTimeOffset OccurredAt)
    : IPublishedPartyEvent;

public record RegisteredIdentifierAdditionSkipped(
    string PartyId, string Type, string Value, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RegisteredIdentifierAdditionSkipped DueToDuplication(string partyId, string type, string value) =>
        new(partyId, type, value, "DUPLICATE", DateTimeOffset.UtcNow);
}

public record RegisteredIdentifierRemoved(string PartyId, string Type, string Value, DateTimeOffset OccurredAt)
    : IPublishedPartyEvent;

public record RegisteredIdentifierRemovalSkipped(
    string PartyId, string Type, string Value, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RegisteredIdentifierRemovalSkipped DueToMissing(string partyId, string type, string value) =>
        new(partyId, type, value, "NOT_FOUND", DateTimeOffset.UtcNow);
}

public record PersonalDataUpdated(string PartyId, string FirstName, string LastName, DateTimeOffset OccurredAt)
    : IPublishedPartyEvent;

public record PersonalDataUpdateSkipped(string PartyId, string Reason, DateTimeOffset OccurredAt) : IPartyEvent;

public record OrganizationNameUpdated(string PartyId, string NewName, DateTimeOffset OccurredAt) : IPublishedPartyEvent;
