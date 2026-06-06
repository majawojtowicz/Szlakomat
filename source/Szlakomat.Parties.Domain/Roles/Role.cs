namespace Szlakomat.Parties.Domain.Roles;

public record Role
{
    public string Name { get; }
    public Role(string? name)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Name = name;
    }
    public static Role Of(string name) => new(name.ToUpperInvariant());

    public static readonly Role Visitor         = new("VISITOR");
    public static readonly Role Reviewer        = new("REVIEWER");
    public static readonly Role AttractionOwner = new("ATTRACTION_OWNER");
    public static readonly Role TourGuide       = new("TOUR_GUIDE");
    public static readonly Role Partner         = new("PARTNER");

    public override string ToString() => Name;
}
