namespace Szlakomat.Parties.Domain.Model;

public record PartyId(Guid Value)
{
    public static PartyId Random() => new(Guid.NewGuid());
    public static PartyId Of(string value) => new(Guid.Parse(value));
    public static PartyId Of(Guid value) => new(value);
    public string AsString() => Value.ToString();
    public override string ToString() => AsString();
}
