using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientClaimMap : ClassMap<ClientClaim> {
        public ClientClaimMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Client);
        }
    }
}