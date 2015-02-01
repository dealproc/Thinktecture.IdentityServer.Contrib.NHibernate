using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ClientGrantTypeRestrictionMap  : ClassMap<ClientGrantTypeRestriction>{
        public ClientGrantTypeRestrictionMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);
            References(x => x.Client);
        }
    }
}