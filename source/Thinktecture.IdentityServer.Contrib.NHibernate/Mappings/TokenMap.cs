using FluentNHibernate.Mapping;
using NHibernate.Type;
using Thinktecture.IdentityServer.NH.Entities;
namespace Thinktecture.IdentityServer.NH.Mappings {
    public class TokenMap : ClassMap<Token> {
        public TokenMap() {
            Schema(NHibernateServiceOptions.Schema);
            CompositeId()
                .KeyProperty(x => x.Key, set => { })
                .KeyProperty(x => x.TokenType, set => { });
            Map(x => x.TokenType).CustomType<EnumStringType<TokenType>>();
        }
    }
}