namespace Szlakomat.Parties.Domain.Events;

public record PersonalDataUpdated(string PartyId, string FirstName, string LastName, DateTimeOffset OccurredAt)
    : IPublishedPartyEvent;
