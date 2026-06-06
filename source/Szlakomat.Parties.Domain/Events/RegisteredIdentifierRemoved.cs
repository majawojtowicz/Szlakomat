namespace Szlakomat.Parties.Domain.Events;

public record RegisteredIdentifierRemoved(string PartyId, string Type, string Value, DateTimeOffset OccurredAt)
    : IPublishedPartyEvent;
