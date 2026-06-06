namespace Szlakomat.Parties.Domain.Model;

public record PersonalData
{
    public string FirstName { get; }
    public string LastName { get; }

    public PersonalData(string? firstName, string? lastName)
    {
        Guard.IsNotNullOrWhiteSpace(firstName);
        Guard.IsNotNullOrWhiteSpace(lastName);
        FirstName = firstName;
        LastName = lastName;
    }

    public static PersonalData Of(string firstName, string lastName) => new(firstName, lastName);

    public string FullName() => $"{FirstName} {LastName}";

    public override string ToString() => FullName();
}

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
