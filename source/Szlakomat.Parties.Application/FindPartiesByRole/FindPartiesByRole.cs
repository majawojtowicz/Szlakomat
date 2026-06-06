using MediatR;
using Szlakomat.Parties.Application.Common;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Roles;

namespace Szlakomat.Parties.Application.FindPartiesByRole;

public record FindPartiesByRoleCriteria(string Role) : IRequest<IReadOnlyList<PartyView>>;

internal sealed class FindPartiesByRoleHandler
    : IRequestHandler<FindPartiesByRoleCriteria, IReadOnlyList<PartyView>>
{
    private readonly IPartyRepository _repository;
    public FindPartiesByRoleHandler(IPartyRepository repository) => _repository = repository;

    public Task<IReadOnlyList<PartyView>> Handle(FindPartiesByRoleCriteria query, CancellationToken ct)
    {
        IReadOnlyList<PartyView> views = _repository.FindByRole(Role.Of(query.Role))
            .Select(PartyMapper.ToView).ToList();
        return Task.FromResult(views);
    }
}
