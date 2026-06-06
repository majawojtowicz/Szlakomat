namespace Szlakomat.Parties.Domain.Events;

public record RoleAdditionSkipped(string PartyId, string Role, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RoleAdditionSkipped DueToDuplication(string partyId, string role) =>
        new(partyId, role, "DUPLICATE", DateTimeOffset.UtcNow);
}
