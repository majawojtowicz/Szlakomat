namespace Szlakomat.Parties.Domain.Facade;

public sealed record InvalidPartyType(string PartyId, string ExpectedType)
    : PartyRelatedFailure($"Party {PartyId} is not of expected type {ExpectedType}");
