using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ScopeMap : ClassMap<Scope> {
        public ScopeMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            HasMany(x => x.ScopeClaims).Cascade.All();
        }
    }
}