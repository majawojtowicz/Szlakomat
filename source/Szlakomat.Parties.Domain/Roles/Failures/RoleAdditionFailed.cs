namespace Szlakomat.Parties.Domain.Roles.Failures;

public sealed record RoleAdditionFailed(string PartyId, string Role, string Reason)
    : RoleOperationFailed(PartyId, Role, Reason)
{
    public static RoleAdditionFailed DueToPolicyViolation(string partyId, string role, string reason) =>
        new(partyId, role, reason);
}
