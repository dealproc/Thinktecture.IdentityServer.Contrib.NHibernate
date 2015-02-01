using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientScopeRestrictionMap : ClassMap<ClientScopeRestriction> {
        public ClientScopeRestrictionMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Client);

            Map(x => x.Scope);
        }
    }
}