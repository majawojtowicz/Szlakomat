namespace Szlakomat.Parties.Domain.Identifiers;

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
