namespace Szlakomat.Parties.Domain.Identifiers;

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
