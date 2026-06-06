namespace Szlakomat.Parties.Domain.Events;

public record RoleRemoved(string PartyId, string Role, DateTimeOffset OccurredAt) : IPublishedPartyEvent;
