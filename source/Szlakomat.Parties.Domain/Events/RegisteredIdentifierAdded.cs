namespace Szlakomat.Parties.Domain.Events;

public record RegisteredIdentifierAdded(string PartyId, string Type, string Value, DateTimeOffset OccurredAt)
    : IPublishedPartyEvent;
