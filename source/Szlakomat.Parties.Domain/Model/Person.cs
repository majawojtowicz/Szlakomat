using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Events;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Model;

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
