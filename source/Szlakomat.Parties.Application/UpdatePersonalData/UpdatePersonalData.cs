using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Domain.Common;
using Szlakomat.Parties.Domain.Facade;
using Szlakomat.Parties.Domain.Model;

namespace Szlakomat.Parties.Application.UpdatePersonalData;

public record UpdatePersonalData(string PartyId, string FirstName, string LastName)
    : IRequest<Result<PartyRelatedFailure, PartyView>>;

internal sealed class UpdatePersonalDataHandler
    : IRequestHandler<UpdatePersonalData, Result<PartyRelatedFailure, PartyView>>
{
    private readonly IPartyRepository _repository;
    public UpdatePersonalDataHandler(IPartyRepository repository) => _repository = repository;

    public Task<Result<PartyRelatedFailure, PartyView>> Handle(UpdatePersonalData cmd, CancellationToken ct)
    {
        var party = _repository.FindByIdValue(cmd.PartyId);
        if (party is not Person p)
            return Task.FromResult(Result<PartyRelatedFailure, PartyView>.FailureOf(
                party is null
                    ? new PartyRelatedFailure.PartyNotFound(cmd.PartyId)
                    : (PartyRelatedFailure)new PartyRelatedFailure.InvalidPartyType(cmd.PartyId, "PERSON")));

        p.Update(PersonalData.Of(cmd.FirstName, cmd.LastName));
        _repository.Save(p);
        return Task.FromResult(Result<PartyRelatedFailure, PartyView>.SuccessOf(PartyMapper.ToView(p)));
    }
}
