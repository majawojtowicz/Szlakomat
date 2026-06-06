using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Events;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Model;

public abstract class Party
{
    private readonly HashSet<Role> _roles;
    private readonly HashSet<IRegisteredIdentifier> _identifiers;
    private readonly List<IPartyEvent> _domainEvents = new();

    protected Party(PartyId id, IEnumerable<Role> roles,
        IEnumerable<IRegisteredIdentifier> identifiers, Common.Version version)
    {
        Guard.IsNotNull(id);
        Guard.IsNotNull(roles);
        Guard.IsNotNull(identifiers);
        Guard.IsNotNull(version);
        Id = id;
        _roles = new HashSet<Role>(roles);
        _identifiers = new HashSet<IRegisteredIdentifier>(identifiers);
        Version = version;
    }

    public PartyId Id { get; }
    public Common.Version Version { get; }
    public IReadOnlySet<Role> Roles => _roles;
    public IReadOnlySet<IRegisteredIdentifier> RegisteredIdentifiers => _identifiers;

    public abstract bool IsNaturalPerson { get; }
    public abstract string DisplayName();

    public bool HasRole(Role role) => _roles.Contains(role);

    public Result<RoleOperationFailed.RoleAdditionFailed, Party> Add(Role role)
    {
        Guard.IsNotNull(role);
        if (_roles.Add(role))
            _domainEvents.Add(new RoleAdded(Id.AsString(), role.Name, DateTimeOffset.UtcNow));
        else
            _domainEvents.Add(RoleAdditionSkipped.DueToDuplication(Id.AsString(), role.Name));
        return Result<RoleOperationFailed.RoleAdditionFailed, Party>.SuccessOf(this);
    }

    public Result<RoleOperationFailed.RoleRemovalFailed, Party> Remove(Role role)
    {
        Guard.IsNotNull(role);
        if (_roles.Count <= 1 && _roles.Contains(role))
            return Result<RoleOperationFailed.RoleRemovalFailed, Party>.FailureOf(
                RoleOperationFailed.RoleRemovalFailed.DueToLastRole(Id.AsString(), role.Name));

        if (_roles.Remove(role))
            _domainEvents.Add(new RoleRemoved(Id.AsString(), role.Name, DateTimeOffset.UtcNow));
        else
            _domainEvents.Add(RoleRemovalSkipped.DueToMissingRole(Id.AsString(), role.Name));
        return Result<RoleOperationFailed.RoleRemovalFailed, Party>.SuccessOf(this);
    }

    public Result<string, Party> Add(IRegisteredIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        if (_identifiers.Add(identifier))
            _domainEvents.Add(new RegisteredIdentifierAdded(
                Id.AsString(), identifier.Type(), identifier.AsString(), DateTimeOffset.UtcNow));
        else
            _domainEvents.Add(RegisteredIdentifierAdditionSkipped.DueToDuplication(
                Id.AsString(), identifier.Type(), identifier.AsString()));
        return Result<string, Party>.SuccessOf(this);
    }

    public Result<string, Party> Remove(IRegisteredIdentifier identifier)
    {
        Guard.IsNotNull(identifier);
        if (_identifiers.Remove(identifier))
            _domainEvents.Add(new RegisteredIdentifierRemoved(
                Id.AsString(), identifier.Type(), identifier.AsString(), DateTimeOffset.UtcNow));
        else
            _domainEvents.Add(RegisteredIdentifierRemovalSkipped.DueToMissing(
                Id.AsString(), identifier.Type(), identifier.AsString()));
        return Result<string, Party>.SuccessOf(this);
    }

    protected void RegisterEvent(IPartyEvent ev)
    {
        Guard.IsNotNull(ev);
        _domainEvents.Add(ev);
    }

    public IReadOnlyList<IPartyEvent> Events => _domainEvents.AsReadOnly();

    public IReadOnlyList<IPublishedPartyEvent> PublishedEvents =>
        _domainEvents.OfType<IPublishedPartyEvent>().ToList().AsReadOnly();

    public void ClearEvents() => _domainEvents.Clear();

    public override bool Equals(object? obj) => obj is Party other && Id.Equals(other.Id);
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => $"{GetType().Name}[{Id.AsString()} | {DisplayName()}]";
}

public sealed class Person : Party
{
    public Person(PartyId id, PersonalData personalData,
        IEnumerable<Role> roles, IEnumerable<IRegisteredIdentifier> identifiers, Common.Version version)
        : base(id, roles, identifiers, version)
    {
        Guard.IsNotNull(personalData);
        PersonalData = personalData;
    }

    public PersonalData PersonalData { get; private set; }
    public override bool IsNaturalPerson => true;
    public override string DisplayName() => PersonalData.FullName();

    public Result<string, Person> Update(PersonalData newData)
    {
        Guard.IsNotNull(newData);
        if (!PersonalData.Equals(newData))
        {
            PersonalData = newData;
            RegisterEvent(new PersonalDataUpdated(
                Id.AsString(), newData.FirstName, newData.LastName, DateTimeOffset.UtcNow));
        }
        else
        {
            RegisterEvent(new PersonalDataUpdateSkipped(Id.AsString(), "NO_CHANGE", DateTimeOffset.UtcNow));
        }
        return Result<string, Person>.SuccessOf(this);
    }
}

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

public sealed class Jdg : Party
{
    public Jdg(PartyId id, PersonalData ownerData, string businessName, string nip,
        IEnumerable<Role> roles, IEnumerable<IRegisteredIdentifier> identifiers, Common.Version version)
        : base(id, roles, identifiers, version)
    {
        Guard.IsNotNull(ownerData);
        Guard.IsNotNullOrWhiteSpace(businessName);
        Guard.IsNotNullOrWhiteSpace(nip);
        OwnerData = ownerData;
        BusinessName = businessName;
        Nip = nip;
    }

    public PersonalData OwnerData { get; private set; }
    public string BusinessName { get; }
    public string Nip { get; }
    public override bool IsNaturalPerson => true;
    public override string DisplayName() => BusinessName;

    public Result<string, Jdg> UpdateOwnerData(PersonalData newData)
    {
        Guard.IsNotNull(newData);
        OwnerData = newData;
        return Result<string, Jdg>.SuccessOf(this);
    }

    public string FullRegisteredName() =>
        $"{OwnerData.FullName()} prowadzacy dzialalnosc pod firma \"{BusinessName}\"";
}
