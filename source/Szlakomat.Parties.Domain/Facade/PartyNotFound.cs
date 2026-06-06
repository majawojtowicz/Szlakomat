namespace Szlakomat.Parties.Domain.Facade;

public sealed record PartyNotFound(string PartyId)
    : PartyRelatedFailure($"Party not found: {PartyId}");
