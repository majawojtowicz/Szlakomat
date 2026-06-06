using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Application.Common;

public record IdentifierView(string Type, string Value);

public record PartyView(
    string PartyId,
    string DisplayName,
    string PartyType,       
    bool IsNaturalPerson,
    IReadOnlySet<string> Roles,
    IReadOnlyList<IdentifierView> Identifiers,
    string? FirstName,       
    string? LastName,        
    string? OrganizationName,
    string? LegalForm,       
    string? BusinessName,    
    string? Nip);            

internal static class PartyMapper
{
    public static PartyView ToView(Party party) => party switch
    {
        Person p => new PartyView(
            p.Id.AsString(), p.DisplayName(), "PERSON", true,
            p.Roles.Select(r => r.Name).ToHashSet(),
            MapIdentifiers(p.RegisteredIdentifiers),
            p.PersonalData.FirstName, p.PersonalData.LastName,
            null, null, null, null),

        Organisation o => new PartyView(
            o.Id.AsString(), o.DisplayName(), "ORGANISATION", false,
            o.Roles.Select(r => r.Name).ToHashSet(),
            MapIdentifiers(o.RegisteredIdentifiers),
            null, null,
            o.OrganizationName.Name, o.OrganizationName.LegalForm,
            null, null),

        Jdg j => new PartyView(
            j.Id.AsString(), j.DisplayName(), "JDG", true,
            j.Roles.Select(r => r.Name).ToHashSet(),
            MapIdentifiers(j.RegisteredIdentifiers),
            j.OwnerData.FirstName, j.OwnerData.LastName,
            null, null,
            j.BusinessName, j.Nip),

        _ => throw new InvalidOperationException($"Unknown party type: {party.GetType().Name}")
    };

    private static IReadOnlyList<IdentifierView> MapIdentifiers(IReadOnlySet<IRegisteredIdentifier> ids) =>
        ids.Select(i => new IdentifierView(i.Type(), i.AsString())).ToList();

    public static IRegisteredIdentifier ToIdentifier(string type, string value) => type.ToUpperInvariant() switch
    {
        "EMAIL" => new EmailAddress(value),
        "PHONE" => new PhoneNumber(value),
        "NIP"   => new TaxIdentifier(value),
        "PESEL" => new PersonalIdentificationNumber(value),
        _ => throw new ArgumentException($"Unknown identifier type: {type}", nameof(type))
    };

    public static Role ToRole(string name) => Role.Of(name);
}
