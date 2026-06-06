using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Events;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Roles;
using Szlakomat.Parties.Domain.Roles.Failures;

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

    public Result<RoleAdditionFailed, Party> Add(Role role)
    {
        Guard.IsNotNull(role);
        if (_roles.Add(role))
            _domainEvents.Add(new RoleAdded(Id.AsString(), role.Name, DateTimeOffset.UtcNow));
        else
            _domainEvents.Add(RoleAdditionSkipped.DueToDuplication(Id.AsString(), role.Name));
        return Result<RoleAdditionFailed, Party>.SuccessOf(this);
    }

    public Result<RoleRemovalFailed, Party> Remove(Role role)
    {
        Guard.IsNotNull(role);
        if (_roles.Count <= 1 && _roles.Contains(role))
            return Result<RoleRemovalFailed, Party>.FailureOf(
                RoleRemovalFailed.DueToLastRole(Id.AsString(), role.Name));

        if (_roles.Remove(role))
            _domainEvents.Add(new RoleRemoved(Id.AsString(), role.Name, DateTimeOffset.UtcNow));
        else
            _domainEvents.Add(RoleRemovalSkipped.DueToMissingRole(Id.AsString(), role.Name));
        return Result<RoleRemovalFailed, Party>.SuccessOf(this);
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
