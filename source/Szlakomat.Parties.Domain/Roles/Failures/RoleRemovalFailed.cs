namespace Szlakomat.Parties.Domain.Roles.Failures;

public sealed record RoleRemovalFailed(string PartyId, string Role, string Reason)
    : RoleOperationFailed(PartyId, Role, Reason)
{
    public static RoleRemovalFailed DueToLastRole(string partyId, string role) =>
        new(partyId, role, "CANNOT_REMOVE_LAST_ROLE");
}
