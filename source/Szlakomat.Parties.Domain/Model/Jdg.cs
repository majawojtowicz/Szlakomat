using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Events;
using Szlakomat.Parties.Domain.Identifiers;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Domain.Model;

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
        if (!OwnerData.Equals(newData))
        {
            OwnerData = newData;
            RegisterEvent(new PersonalDataUpdated(
                Id.AsString(), newData.FirstName, newData.LastName, DateTimeOffset.UtcNow));
        }
        else
        {
            RegisterEvent(new PersonalDataUpdateSkipped(Id.AsString(), "NO_CHANGE", DateTimeOffset.UtcNow));
        }
        return Result<string, Jdg>.SuccessOf(this);
    }

    public string FullRegisteredName() =>
        $"{OwnerData.FullName()} prowadzacy dzialalnosc pod firma \"{BusinessName}\"";
}
