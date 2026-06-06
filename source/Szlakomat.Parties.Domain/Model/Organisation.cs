using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Events;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Model;

public sealed class Organisation : Party
{
    public Organisation(PartyId id, OrganizationName organizationName,
        IEnumerable<Role> roles, IEnumerable<IRegisteredIdentifier> identifiers, Common.Version version)
        : base(id, roles, identifiers, version)
    {
        Guard.IsNotNull(organizationName);
        OrganizationName = organizationName;
    }

    public OrganizationName OrganizationName { get; private set; }
    public override bool IsNaturalPerson => false;
    public override string DisplayName() => OrganizationName.FullName();

    public Result<string, Organisation> Update(OrganizationName newName)
    {
        Guard.IsNotNull(newName);
        OrganizationName = newName;
        RegisterEvent(new OrganizationNameUpdated(Id.AsString(), newName.FullName(), DateTimeOffset.UtcNow));
        return Result<string, Organisation>.SuccessOf(this);
    }
}
