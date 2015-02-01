using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientSecretMap : ClassMap<ClientSecret> {
        public ClientSecretMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Client);
        }
    }
}