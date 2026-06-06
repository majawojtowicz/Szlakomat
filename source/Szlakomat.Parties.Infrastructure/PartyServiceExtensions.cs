using Microsoft.Extensions.DependencyInjection;
using Szlakomat.Parties.Application;
using Szlakomat.Parties.Domain.Address;
using Szlakomat.Parties.Domain.Model;
using Szlakomat.Parties.Domain.Relationships;
using Szlakomat.Parties.Domain.Roles;
using Szlakomat.Parties.Infrastructure.Addresses;
using Szlakomat.Parties.Infrastructure.Party;
using Szlakomat.Parties.Infrastructure.Relationships;

namespace Szlakomat.Parties.Infrastructure;

public static class PartyServiceExtensions
{
   
    public static IServiceCollection AddPartyModule(this IServiceCollection services)
    {
        services.AddSingleton<IPartyRepository, InMemoryPartyRepository>();
        services.AddSingleton<IAddressesRepository, InMemoryAddressesRepository>();
        services.AddSingleton<IPartyRelationshipRepository, InMemoryPartyRelationshipRepository>();

        services.AddSingleton<IRoleAdditionPolicy, NoAttractionOwnerForPersonPolicy>();
        services.AddSingleton<IPartyRelationshipDefiningPolicy, AllowAllPartyRelationshipDefiningPolicy>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<PartyModule>());

        return services;
    }
}
