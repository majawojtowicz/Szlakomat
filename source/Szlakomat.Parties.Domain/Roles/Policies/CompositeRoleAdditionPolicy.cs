using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Domain.Roles.Policies;

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
