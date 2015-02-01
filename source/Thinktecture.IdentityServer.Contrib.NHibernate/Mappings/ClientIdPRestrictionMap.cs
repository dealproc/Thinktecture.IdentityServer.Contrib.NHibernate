using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientIdPRestrictionMap : ClassMap<ClientIdPRestriction> {
        public ClientIdPRestrictionMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Client);

            Map(x => x.Provider);
        }
    }
}