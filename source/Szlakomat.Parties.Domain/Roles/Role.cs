using Szlakomat.Parties.Domain.Model;

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

public interface IRoleAdditionPolicy
{
    bool Allows(Party party, Role role);
    string ViolationReason { get; }
}

public sealed class AllowAllRoleAdditionPolicy : IRoleAdditionPolicy
{
    public bool Allows(Party party, Role role) => true;
    public string ViolationReason => "";
}

public sealed class NoAttractionOwnerForPersonPolicy : IRoleAdditionPolicy
{
    public bool Allows(Party party, Role role)
    {
        Guard.IsNotNull(party);
        Guard.IsNotNull(role);
        return !(role == Role.AttractionOwner && party is Person);
    }
    public string ViolationReason =>
        "ATTRACTION_OWNER cannot be assigned to Person – register as JDG or Organisation instead";
}

public sealed class CompositeRoleAdditionPolicy : IRoleAdditionPolicy
{
    private readonly IRoleAdditionPolicy[] _policies;
    public CompositeRoleAdditionPolicy(params IRoleAdditionPolicy[] policies)
    {
        Guard.IsNotNull(policies);
        _policies = policies;
    }
    public bool Allows(Party party, Role role) => _policies.All(p => p.Allows(party, role));
    public string ViolationReason =>
        _policies.Select(p => p.ViolationReason).FirstOrDefault(r => r != "") ?? "";
}
