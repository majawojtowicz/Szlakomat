namespace Szlakomat.Parties.Domain.Relationships;

public record PartyRelationshipId(Guid Value)
{
    public static PartyRelationshipId Random() => new(Guid.NewGuid());
    public static PartyRelationshipId Of(string v) => new(Guid.Parse(v));
    public string AsString() => Value.ToString();
}
