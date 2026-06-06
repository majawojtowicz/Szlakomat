namespace Szlakomat.Parties.Domain.Identifiers;

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
