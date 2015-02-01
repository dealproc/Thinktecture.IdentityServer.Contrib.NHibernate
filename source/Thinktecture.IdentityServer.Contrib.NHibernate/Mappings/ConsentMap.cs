using FluentNHibernate.Mapping;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class ConsentMap : ClassMap<Consent>{
        public ConsentMap() {
            Schema(NHibernateServiceOptions.Schema);
            Id(x => x.Id);

            Map(x => x.ClientId);
            Map(x => x.Subject);
            Map(x => x.Scopes);
        }
    }
}