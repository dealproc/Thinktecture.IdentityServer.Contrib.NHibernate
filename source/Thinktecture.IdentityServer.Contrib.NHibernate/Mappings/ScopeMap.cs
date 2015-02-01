using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ScopeMap : ClassMap<Scope> {
        public ScopeMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            HasMany(x => x.ScopeClaims).Cascade.All();

            Map(x => x.ClaimsRule);
            Map(x => x.Description);
            Map(x => x.DisplayName);
            Map(x => x.Emphasize);
            Map(x => x.Enabled);
            Map(x => x.IncludeAllClaimsForUser);
            Map(x => x.Name);
            Map(x => x.Required);
            Map(x => x.ShowInDiscoveryDocument);
            Map(x => x.Type);
        }
    }
}