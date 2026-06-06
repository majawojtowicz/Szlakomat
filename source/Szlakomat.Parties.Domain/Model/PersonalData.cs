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
