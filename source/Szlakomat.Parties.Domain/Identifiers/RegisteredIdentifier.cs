namespace Szlakomat.Parties.Domain.Identifiers;

public interface IRegisteredIdentifier
{
    string Type();
    string AsString();
}

public sealed record EmailAddress : IRegisteredIdentifier
{
    public string Value { get; }
    public EmailAddress(string? value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Guard.IsTrue(value.Contains('@'), "Email must contain '@'");
        Value = value;
    }
    public string Type() => "EMAIL";
    public string AsString() => Value;
    public override string ToString() => $"EMAIL:{Value}";
}

public sealed record PhoneNumber : IRegisteredIdentifier
{
    public string Value { get; }
    public PhoneNumber(string? value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Value = value;
    }
    public string Type() => "PHONE";
    public string AsString() => Value;
    public override string ToString() => $"PHONE:{Value}";
}

public sealed record TaxIdentifier : IRegisteredIdentifier
{
    public string Value { get; }
    public TaxIdentifier(string? value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Value = value;
    }
    public string Type() => "NIP";
    public string AsString() => Value;
    public override string ToString() => $"NIP:{Value}";
}

public sealed record PersonalIdentificationNumber : IRegisteredIdentifier
{
    public string Value { get; }
    public PersonalIdentificationNumber(string? value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Value = value;
    }
    public string Type() => "PESEL";
    public string AsString() => Value;
    public override string ToString() => $"PESEL:{Value}";
}
