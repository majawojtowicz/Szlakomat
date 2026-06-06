namespace Szlakomat.Parties.Domain.Relationships;

public record RelationshipName
{
    public string Value { get; }
    public RelationshipName(string? value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        Value = value;
    }
    public static RelationshipName Of(string value) => new(value);
    public override string ToString() => Value;

    public static readonly RelationshipName Visited      = new("VISITED");
    public static readonly RelationshipName Reviewed     = new("REVIEWED");
    public static readonly RelationshipName Owns         = new("OWNS");
    public static readonly RelationshipName GuidesAt     = new("GUIDES_AT");
    public static readonly RelationshipName PartnersWith = new("PARTNERS_WITH");
    public static readonly RelationshipName Employs      = new("EMPLOYS");
}
