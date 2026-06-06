namespace Szlakomat.Parties.Domain.Model;

public record OrganizationName
{
    public string Name { get; }
    public string? LegalForm { get; }

    public OrganizationName(string? name, string? legalForm = null)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Name = name;
        LegalForm = legalForm;
    }

    public static OrganizationName Of(string name) => new(name);
    public static OrganizationName Of(string name, string legalForm) => new(name, legalForm);
    public string FullName() => LegalForm is not null ? $"{Name} {LegalForm}" : Name;
    public override string ToString() => FullName();
}
