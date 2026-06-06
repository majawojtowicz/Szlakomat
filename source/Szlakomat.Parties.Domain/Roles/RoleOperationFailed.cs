namespace Szlakomat.Parties.Domain.Roles;

public abstract record RoleOperationFailed(string PartyId, string Role, string Reason)
{
    public sealed record RoleAdditionFailed(string PartyId, string Role, string Reason)
        : RoleOperationFailed(PartyId, Role, Reason)
    {
        public static RoleAdditionFailed DueToPolicyViolation(string partyId, string role, string reason) =>
            new(partyId, role, reason);
    }

    public sealed record RoleRemovalFailed(string PartyId, string Role, string Reason)
        : RoleOperationFailed(PartyId, Role, Reason)
    {
        public static RoleRemovalFailed DueToLastRole(string partyId, string role) =>
            new(partyId, role, "CANNOT_REMOVE_LAST_ROLE");
    }
}
