namespace Szlakomat.Parties.Domain.Address;

public record AddressId(Guid Value)
{
    public static AddressId Random() => new(Guid.NewGuid());
    public static AddressId Of(Guid v) => new(v);
    public static AddressId Of(string v) => new(Guid.Parse(v));
    public string AsString() => Value.ToString();
    public override string ToString() => AsString();
}
