using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientRedirectUriMap : ClassMap<ClientRedirectUri> {
        public ClientRedirectUriMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Client);
        }
    }
}