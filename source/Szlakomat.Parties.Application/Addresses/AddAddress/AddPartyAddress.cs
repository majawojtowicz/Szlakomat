using MediatR;
using Szlakomat.Parties.Application.Addresses.Common;
using Szlakomat.Parties.Domain.Address;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Application.Addresses.AddAddress;

public record AddPartyAddress(
    string PartyId,
    string? Name,
    string Street,
    string Building,
    string City,
    string PostalCode,
    string Country,
    IReadOnlyList<string> UseTypes
) : IRequest<Result<AddressRelatedFailure, PartyAddressView>>;

internal sealed class AddPartyAddressHandler
    : IRequestHandler<AddPartyAddress, Result<AddressRelatedFailure, PartyAddressView>>
{
    private readonly IAddressesRepository _repository;
    private readonly IPartyRepository _partyRepository;

    public AddPartyAddressHandler(IAddressesRepository repository, IPartyRepository partyRepository)
    {
        _repository = repository;
        _partyRepository = partyRepository;
    }

    public Task<Result<AddressRelatedFailure, PartyAddressView>> Handle(AddPartyAddress cmd, CancellationToken ct)
    {
        var partyId = PartyId.Of(cmd.PartyId);
        if (_partyRepository.FindById(partyId) is null)
            return Task.FromResult(Result<AddressRelatedFailure, PartyAddressView>.FailureOf(
                AddressRelatedFailure.AddressDefinitionFailed.DueToPolicyViolation(
                    "?", cmd.PartyId, "Party not found")));

        var addresses = _repository.FindFor(partyId) ?? Szlakomat.Parties.Domain.Address.Addresses.EmptyFor(partyId);
        var useTypes = cmd.UseTypes.Select(AddressMapper.ToType);
        var address = GeoAddress.Of(partyId, cmd.Name, cmd.Street, cmd.Building,
            cmd.City, cmd.PostalCode, cmd.Country, useTypes);

        var result = addresses.AddOrUpdate(address);
        if (result.IsFailure())
            return Task.FromResult(Result<AddressRelatedFailure, PartyAddressView>.FailureOf(
                (AddressRelatedFailure)result.FailureValue));

        _repository.Save(result.SuccessValue);
        return Task.FromResult(Result<AddressRelatedFailure, PartyAddressView>.SuccessOf(
            AddressMapper.ToView(address)));
    }
}
