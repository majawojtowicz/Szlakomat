namespace Szlakomat.Parties.Domain.Events;

public record PersonalDataUpdateSkipped(string PartyId, string Reason, DateTimeOffset OccurredAt) : IPartyEvent;
