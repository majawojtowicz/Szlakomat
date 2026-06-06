namespace Szlakomat.Parties.Domain.Identifiers;

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
