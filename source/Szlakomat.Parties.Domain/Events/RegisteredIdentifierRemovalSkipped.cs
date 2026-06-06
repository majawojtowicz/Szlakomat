namespace Szlakomat.Parties.Domain.Events;

public record RegisteredIdentifierRemovalSkipped(
    string PartyId, string Type, string Value, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RegisteredIdentifierRemovalSkipped DueToMissing(string partyId, string type, string value) =>
        new(partyId, type, value, "NOT_FOUND", DateTimeOffset.UtcNow);
}
