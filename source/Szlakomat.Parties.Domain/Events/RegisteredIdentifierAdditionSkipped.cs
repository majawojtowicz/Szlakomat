namespace Szlakomat.Parties.Domain.Events;

public record RegisteredIdentifierAdditionSkipped(
    string PartyId, string Type, string Value, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RegisteredIdentifierAdditionSkipped DueToDuplication(string partyId, string type, string value) =>
        new(partyId, type, value, "DUPLICATE", DateTimeOffset.UtcNow);
}
