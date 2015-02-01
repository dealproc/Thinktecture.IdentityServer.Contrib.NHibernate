using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ScopeClaimMap : ClassMap<ScopeClaim> {
        public ScopeClaimMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Scope);

            Map(x => x.AlwaysIncludeInIdToken);
            Map(x => x.Description);
            Map(x => x.Name);
        }
    }
}