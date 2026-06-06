namespace Szlakomat.Parties.Domain.Events;

public record RoleRemovalSkipped(string PartyId, string Role, string Reason, DateTimeOffset OccurredAt) : IPartyEvent
{
    public static RoleRemovalSkipped DueToMissingRole(string partyId, string role) =>
        new(partyId, role, "NOT_FOUND", DateTimeOffset.UtcNow);
}
