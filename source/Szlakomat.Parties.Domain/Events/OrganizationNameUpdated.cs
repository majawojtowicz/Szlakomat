namespace Szlakomat.Parties.Domain.Events;

public record OrganizationNameUpdated(string PartyId, string NewName, DateTimeOffset OccurredAt)
    : IPublishedPartyEvent;
