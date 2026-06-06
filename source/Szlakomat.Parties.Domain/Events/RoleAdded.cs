namespace Szlakomat.Parties.Domain.Events;

public record RoleAdded(string PartyId, string Role, DateTimeOffset OccurredAt) : IPublishedPartyEvent;
