using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientPostLogoutRedirectUriMap : ClassMap<ClientPostLogoutRedirectUri> {
        public ClientPostLogoutRedirectUriMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Client);

            Map(x => x.Uri);
        }
    }
}