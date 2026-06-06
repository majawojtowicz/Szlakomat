namespace Szlakomat.Parties.Domain.Roles.Failures;

public abstract record RoleOperationFailed(string PartyId, string Role, string Reason);
