namespace Szlakomat.Parties.Domain.Facade;

public sealed record PartyAlreadyExists(string PartyId)
    : PartyRelatedFailure($"Party already exists: {PartyId}");
